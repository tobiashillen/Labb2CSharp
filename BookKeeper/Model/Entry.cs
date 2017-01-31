using System;
//using SQLite;
namespace Model
{
	public class Entry
	{
//		[PrimaryKey, AutoIncrement]
		int Id { get; set; }
		public DateTime Date { get; set; }
		public string Description { get; set; }
		public Account Type { get; set; }
		public Account Account { get; set; }
		public int Amount { get; set; }
		public TaxRate Vat { get; set; }

		public Entry(DateTime date, string description, Account type, Account account, int amount, TaxRate vat)
		{
			Date = date;
			Description = description;
			Type = type;
			Account = account;
			Amount = amount;
			Vat = vat;
		}

		public override string ToString()
		{
			return string.Format("[Entry: Date={0}, Description={1}, Type={2}, Account={3}, Amount={4}, Vat={5}]", Date, Description, Type, Account, Amount, Vat);
		}
	}
}
