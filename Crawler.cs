using CarsScraper.Model;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Text.RegularExpressions;

namespace CarsScraper
{
    internal class Crawler
    {
        //Crawler for automobiledimension.com

        // we need:
        // user-agent
        string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.102 Safari/537.36";

        List<string> WebsiteCategories = new List<string>
        {
            "city-cars.php",
            "small-cars.php",
            "compact-cars.php",
            "family-cars.php",
            "executive-cars.php",
            "luxury-cars.php",
            "sports-cars.php",
            "estate-cars.php",
            "mpv-cars.php",
            "small-suv.php",
            "compact-suv.php",
            "mid-size-suv.php",
            "large-suv-4x4-cars.php",
            "pick-up-vehicles.php",
            "passenger-vans.php"
        };
        // Headers
        // wait-function(to parse responsible) --- Random Intervals

        public string URL { get; set; }
        public string SaveFolder { get; }

        private HtmlDocument doc;
        private HtmlWeb web = new HtmlWeb();

        public Crawler(string url = "https://www.automobiledimension.com/", string saveFolder = @"E:\HackathonMHP\ScrappedData")
        {
            URL = url;
            SaveFolder = saveFolder;
            web.UserAgent = UserAgent;
        }

        internal List<Car> RetrieveCarsByCategory()
        {
            List<Car> cars = new();
            //int i = 0, j = 0;

            try
            {

                foreach (var cat in WebsiteCategories)
                {
                    var carsUrlFromCategory = new List<string>();

                    Console.WriteLine($"Starting to RetrieveCarsFromCategory: {URL + cat}");

                    carsUrlFromCategory = RetrieveCarsFromCategory(URL + cat);

                    Console.WriteLine($"Result to RetrieveCarsFromCategory: {carsUrlFromCategory}");

                    foreach (var carUrl in carsUrlFromCategory)
                    {
                        Console.WriteLine($"Starting to RetrieveCar: {carUrl}");

                        var car = RetrieveCar(carUrl, cat);
                        cars.Add(car);

                        Console.WriteLine($"Result to RetrieveCarsFromCategory: {car}");

/*                        if (i == 3)
                            break;
                        i++;*/
                    }
/*                    if (j == 3)
                        break;
                    j++;*/
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception occured: {e.Message}");
            }
            finally
            {
                var carsAsJson = JsonConvert.SerializeObject(cars, Formatting.Indented);

                Console.WriteLine($"Final result: {carsAsJson}");

                File.WriteAllText(Path.Combine(SaveFolder, "cars.json"), carsAsJson);
            }
            return cars;
        }

        /**
         * Access category link
         * Extracts all car links
         * returns list of links
         */
        private List<string> RetrieveCarsFromCategory(string catUrl)
        {
            // class="unit"
            // each unit has a <a href> child. Extract the link
            List<string> carsUrls = new List<string>();

            doc = web.Load(catUrl);
            NextPageWait();

            IEnumerable<HtmlNode> nodes = doc.DocumentNode.Descendants(0).Where(n => n.HasClass("unit"));
            foreach (var node in nodes)
            {
                carsUrls.Add(node.ChildNodes.Where(child => child.Attributes["href"] != null).First().Attributes["href"].Value);
            }

            return carsUrls;
        }

        /**
         * Access car link
         * Extracts data into an local object Car
         * Download images to an input folder
         * returns the Car object
         */
        private Car RetrieveCar(string carUrl, string category)
        {
            doc = web.Load(URL + carUrl[1..]);
            NextPageWait();
            List<string> carsUrls = new List<string>();

            // We need:

            carUrl = carUrl.Replace("/model/", "");
            var brand = carUrl.Substring(0, carUrl.IndexOf('/')).ToLower();
            var model = carUrl.Replace(brand + "/", "");

            var filePrefix = $"{brand}_{model}_";
            category = category[..category.IndexOf(".")];


            var title = doc.DocumentNode.Descendants(0).Where(n => n.HasClass("titol")).First().InnerText;
            // Old regex: @"\d+"
            var year = Regex.Match(title, @"(\d+)(?!.*\d)").Value;

            var interiorFigures = doc.DocumentNode.Descendants(0).Where(n => n.HasClass("interior-figure"));

            var dimensionsImage = interiorFigures.First().Descendants(1).First().Descendants(0).First().GetAttributeValue("src", "not-found");

            var bootspaceImage = interiorFigures.ElementAt(1).Descendants(1).ElementAt(0).GetAttributeValue("src", "not-found");

            var bootspaceResult = Regex.Match(interiorFigures.ElementAt(1).InnerText, @"\d+").Value;

            if (!int.TryParse(bootspaceResult, out int bootspace))// extract only the number
                bootspace = -1;

            var dashboardImage = interiorFigures.ElementAt(2).Descendants(1).ElementAt(0).GetAttributeValue("src", "not-found");

            var interiorImage = interiorFigures.ElementAt(3).Descendants(1).ElementAt(0).GetAttributeValue("src", "not-found");

            var seatsResult = Regex.Match(interiorFigures.ElementAt(3).InnerText, @"\d+").Value;

            if (!int.TryParse(seatsResult, out int seats))// extract only the number
                seats = -1;

            var interiorText = doc.DocumentNode.Descendants(0).Where(n => n.HasClass("interior-text"));

            var interiorTextString = interiorText.First().InnerText.ToLower();

            var utilEngineS = interiorTextString[interiorTextString.IndexOf("motorization:")..].ToLower();

            var engines = utilEngineS[..utilEngineS.IndexOf(".")]
                .Replace("motorization: ", "")
                .Split(new[] { ",", "and" }, StringSplitOptions.None)
                .Select(s => s.Trim())
                .ToList();

            var interiorExtra = doc.DocumentNode.Descendants(0).Where(n => n.HasClass("interior-extra"));

            if (interiorExtra.Any())
            {
                var interiorExtra1 = interiorExtra.First().ChildNodes.First().GetAttributeValue("src", "not-found");
                var interiorExtra2 = interiorExtra.First().ChildNodes.ElementAt(1).GetAttributeValue("src", "not-found");
                var interiorExtra3 = interiorExtra.First().ChildNodes.ElementAt(2).GetAttributeValue("src", "not-found");

                DownloadCarImage(filePrefix + "other1", interiorExtra1);
                DownloadCarImage(filePrefix + "other2", interiorExtra2);
                DownloadCarImage(filePrefix + "other3", interiorExtra3);
            }

            DownloadCarImage(filePrefix + "dimensions", dimensionsImage);
            DownloadCarImage(filePrefix + "bootspace", bootspaceImage);
            DownloadCarImage(filePrefix + "dashboard", dashboardImage);
            DownloadCarImage(filePrefix + "interior", interiorImage);

            Car car = new Car(
                brand,
                model,
                category,
                year,
                bootspace,
                seats,
                engines
                );

            //class: interior-extra
            return car;
        }

        private void DownloadCarImage(string filename, string imageName)
        {
            try
            {
                WebClient client = new WebClient();
                Stream stream = client.OpenRead(URL + imageName);
                Bitmap bitmap; bitmap = new Bitmap(stream);

                if (bitmap != null)
                {
                    bitmap.Save(Path.Combine(SaveFolder, "images", filename + ".jpeg"), ImageFormat.Jpeg);
                }

                stream.Flush();
                stream.Close();
                client.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occured for image: {imageName} \n{ex.Message}");
            }

            NextPageWaitFAST();
        }

        // Use this after each page Load for a responsible scrapping
        private static void NextPageWait()
        {
            Random random = new Random();
            var rndWaitingTime = random.Next(1500,3500);
            Thread.Sleep(rndWaitingTime);
        }

        private static void NextPageWaitFAST()
        {
            Random random = new Random();
            var rndWaitingTime = random.Next(500, 1000);
            Thread.Sleep(rndWaitingTime);
        }
    }
}
