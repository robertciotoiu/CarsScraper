using DocumentFormat.OpenXml;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace CarsScraper.Model
{
    public class Car
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Category { get; set; }
        // a car may be released 5 years ago, and a new model is not yet available
        public string Year { get; set; }
        // in liters
        public int BootSpace { get; set; }
        public int Seats { get; set; }
        public List<string> EngineType { get; set; }
        public PriceRange Price { get; set; }
        public CarImages ImageNames { get; set; }

        public Car(string brand, string model, string category, string year, int bootSpace, int seats, List<string> engineType)
        {
            Brand = brand;
            Model = model;
            Category = category;
            Year = year;
            BootSpace = bootSpace;
            Seats = seats;
            EngineType = engineType;

            var imagePrefix = $"{Brand}_{Model}_";

            ImageNames = new CarImages(
                imagePrefix + "dimensions",
                imagePrefix + "bootspace",
                imagePrefix + "dashboard",
                imagePrefix + "interior",
                imagePrefix + "other1",
                imagePrefix + "other2",
                imagePrefix + "other3"
                );
        }

        public class Dimensions
        {
            // in millimeters
            public int Width { get; set; }
            public int Height { get; set; }
            public int Length { get; set; }
        }

        public class PriceRange
        {
            public int MinimumPrice { get; set; }
            public int MaximumPrice { get; set; }
            // Optional field
            public int AveragePrice { get; set; }
        }

        public class CarImages
        {
            public string DimensionsImage { get; set; }
            public string BootSpaceImage { get; set; }
            public string DashboardImage { get; set; }
            public string InteriorImage { get; set; }

            public string OtherImage1 { get; set; }
            public string OtherImage2 { get; set; }
            public string OtherImage3 { get; set; }

            public CarImages(string dimensionsImage, string bootSpaceImage, string dashboardImage, string interiorImage, string otherImage1, string otherImage2, string otherImage3)
            {
                DimensionsImage = dimensionsImage;
                BootSpaceImage = bootSpaceImage;
                DashboardImage = dashboardImage;
                InteriorImage = interiorImage;
                OtherImage1 = otherImage1;
                OtherImage2 = otherImage2;
                OtherImage3 = otherImage3;
            }
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}