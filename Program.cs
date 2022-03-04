// See https://aka.ms/new-console-template for more information
using CarsScraper;
using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");


Crawler crawler = new Crawler();

/*var bootspaceResult = Regex.Match("No int: 25 ", @"\d+").Value;

if (!int.TryParse(bootspaceResult, out int bootspace))// extract only the number
    bootspace = 0;

Console.WriteLine(bootspace); */

var extracted_cars = crawler.RetrieveCarsByCategory();


