using System;
using SQLite;
//using SQLite;
namespace Model
{
	public class Entry
	{
		[PrimaryKey, AutoIncrement, Column("_id")]
		int Id { get; set; }
		public DateTime Date { get; set; }
		public string Description { get; set; }
		public int Type { get; set; }
		public int Account { get; set; }
		public int Amount { get; set; }
		public int TaxRate { get; set; }




		public Entry() { }

		public Entry(DateTime date, string description, int type, int account, int amount, int taxRate)
		{
			Date = date;
			Description = description;
			Type = type;
			Account = account;
			Amount = amount;
			TaxRate = taxRate;
		}

		public override string ToString()
		{
			return string.Format("[Entry: Date={0}, Description={1}, Type={2}, Account={3}, Amount={4}, Vat={5}]", Date, Description, Type, Account, Amount, TaxRate);
		}
	}
}
