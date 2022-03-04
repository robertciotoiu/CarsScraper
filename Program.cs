// See https://aka.ms/new-console-template for more information
using CarsScraper;

Console.WriteLine("Hello, World!");

Crawler crawler = new Crawler();

var extracted_cars = crawler.RetrieveCarsByCategory();


