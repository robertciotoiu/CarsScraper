using DocumentFormat.OpenXml;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

public class Car
{
	public string Brand { get; set; }
	public string Model { get; set; }
	public string Category { get; set; }
	// a car may be released 5 years ago, and a new model is not yet available
	public string Year { get; set; }
/*	public Dimensions CarDimensions { get; set; }*/
	// in liters
	public int BootSpace { get; set; }
	public int Seats { get; set; }
	public List<string> EngineType { get; set; }
	public PriceRange Price { get; set; }
	public CarImages ImageNames { get; set; }

/*    public Car(string brand, string model, string category, string year, Dimensions carDimensions, int bootSpace, int seats, List<string> engineType)
    {
        Brand = brand;
        Model = model;
        Category = category;
        Year = year;
        CarDimensions = carDimensions;
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
    }*/

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

	/*	public enum CarCategory
		{
			City,
			Small,
			Compact,
			Family,
			Executive,
			Luxury,
			Sport,
			Estate,
			MPV,
			SmallSUV,
			CompactSUV,
			MidSizeSUV,
			LargeSUV,//or 4x4
			PickUp,
			PassengerVan
		}*/
	/*
		internal static class Motorizations
		{
			public static readonly string Petrol = "petrol";
			public static readonly string PetrolHybrid = "petrol hybrid";
			public static readonly string PetrolMildHybrid = "petrol mild hybrid";
			public static readonly string PetrolPlugInHybrid = "petrol plug-in hybrid";
			public static readonly string Diesel = "diesel";
			public static readonly string DieselHybrid = "diesel hybrid";
			public static readonly string DieselMildHybrid = "diesel mild hybrid";
			public static readonly string DieselPlugInHybrid = "diesel plug-in hybrid";
			public static readonly string Electric = "electric";
			public static readonly string Hydrogen = "hydrogen";
			public static readonly string LPG = "lpg";

			// Or you could initialize in static constructor
			static Motorizations()
			{
				//row = string.Format("String{0}", 4);
			}
		}*/

	/*public enum Motorizations
    {
		[Description("petrol")]
		Petrol,
		[Description("petrol hybrid")]
		PetrolHybrid,
		[Description("petrol mild hybrid")]
		PetrolMildHybrid,
		[Description("petrol plug-in hybrid")]
		PetrolPlugInHybrid,
		[Description("diesel")]
		Diesel,
		[Description("diesel hybrid")]
		DieselHybrid,
		[Description("diesel mild hybrid")]
		DieselMildHybrid,
		[Description("diesel plug-in hybrid")]
		DieselPlugInHybrid,
		[Description("electric")]
		Electric,
		[Description("hydrogen")]
		Hydrogen,
		[Description("lpg")]
		LPG//or autogas
    }*/

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

	public override string ToString()
	{
		return JsonConvert.SerializeObject(this);
	}

    public class CarImages
    {
		public string DimensionsImage { get; set; }
		public string BootSpaceImage { get; set; }
		public string DashboardImage {	get; set; }
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
}
