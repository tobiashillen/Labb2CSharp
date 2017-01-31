using System;
using System.Collections.Generic;
//using SQLite;

namespace Model
{
	public class BookKeeperManager
	{
		private static BookKeeperManager instance;

		public List<Entry> Entries { get; private set; }
		public List<Account> IncomeAccounts { get; private set; }
		public List<Account> ExpenseAccounts { get; private set; }
		public List<Account> MoneyAccounts { get; private set; }
		public List<TaxRate> TaxRates { get; private set; }

		private static string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + @"\database.db";

		private BookKeeperManager() 
		{
			Entries = new List<Entry>();
			IncomeAccounts = new List<Account>() { new Account("Försäljning av tjänster", 3040, 1),
				new Account("Varuförsäljning", 3050, 1),
				new Account("Hyresintäkter", 3911, 1),
				new Account("Provisionsintäkter", 3921, 1)};
			ExpenseAccounts = new List<Account>(){ new Account("Varuinköp", 4010, 2),
				new Account("Lokalhyra", 5010, 2),
				new Account("Städning och renhållning", 5060, 2),
				new Account("Leasing av datorer", 5252, 2)};
			MoneyAccounts = new List<Account>(){ new Account("Kassa", 1910, 3),
				new Account("Checkräkningskonto", 1930, 3)};
			TaxRates = new List<TaxRate>(){ new TaxRate(6), new TaxRate(12), new TaxRate(24)};
		}

		public static BookKeeperManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new BookKeeperManager();
//					SQLiteConnection db = new SQLiteConnection(path);
//					db.CreateTable
				}
				return instance;
			}
		}
		public void AddEntry(Entry e)
		{
			Entries.Add(e);
//			SQLiteConnection db = new SQLiteConnection(path);
		}
	}
}