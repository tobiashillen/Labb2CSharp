using System;
namespace Model
{
	public class Entry
	{
		private bool Income { get; set; }
		private int Date { get; set; }
		private string Description { get; set; }
		private int Type { get; set; }
		private int Account { get; set; }
		private int Amount { get; set; }
		private double Vat { get; set; }

		public Entry(bool income, int date, string description, int type, int account, int amount, double vat)
		{
			Income = income;
			Date = date;
			Description = description;
			Type = type;
			Account = account;
			Amount = amount;
			Vat = vat;
		}
	}
}
