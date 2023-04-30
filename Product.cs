using System;

[Serializable]
public class Product
{
	public string Name { set; get; }
	public double Price { set; get; }
	public string Manufacturer { set; get; }

	public Product(string Name, double Price, string Manufacturer)
	{
		this.Name = Name;
		this.Price = Price;
		this.Manufacturer = Manufacturer;
	}
}
