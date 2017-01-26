using System;
namespace Model
{
	public class Entry
	{
		bool income;
		DateTime date;
		string description;
		int amount;
		int type;
		int account;
		double vat;

		public bool Income
		{
			get
			{
				return income;
			}
			set
			{
				income = value;
			}
		}

		public DateTime Date
		{
			get
			{
				return date;
			}
			set
			{
				date = value;
			}
		}

		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				description = value;
			}
		}

		public int Type
		{
			get
			{
				return type;
			}
			set
			{
				type = value;
			}
		}

		public int Account
		{
			get
			{
				return account;
			}
			set
			{
				account = value;
			}
		}

		public int Amount
		{
			get
			{
				return amount;
			}
			set
			{
				amount = value;
			}
		}

		public double Vat
		{
			get
			{
				return vat;
			}
			set
			{
				vat = value;
			}
		}

		public Entry(bool income, DateTime date, string description, int type, int account, int amount, double vat)
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
