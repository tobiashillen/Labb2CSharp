using System;
using SQLite;
//using SQLite;
namespace Model
{
	public class Entry
	{
		[PrimaryKey, AutoIncrement, Column("_id")]
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public string Description { get; set; }
		public int TypeAccount { get; set; }
		public int MoneyAccount { get; set; }
		public int Amount { get; set; }
		public int TaxRate { get; set; }
		//1 = income, 2 = expense.
		public int EntryType { get; set; }

		public Entry() { }

		public Entry(DateTime date, string description, int type, int account, int amount, int taxRate, int entryType)
		{
			Date = date;
			Description = description;
			TypeAccount = type;
			MoneyAccount = account;
			Amount = amount;
			TaxRate = taxRate;
			EntryType = entryType;
		}


		public override string ToString()
		{
			return string.Format("[Entry: Date={0}, Description={1}, TypeAccount={2}, MoneyAccount={3}, Amount={4}, TaxRate={5}, EntryType={6}]", 
			                     Date, Description, TypeAccount, MoneyAccount, Amount, TaxRate, EntryType);
		}
	}
}
