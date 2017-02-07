using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace Model
{
	public class BookKeeperManager
	{
		static BookKeeperManager instance;
		static string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + @"\database.db";


		public List<Entry> Entries
		{
			get
			{
				SQLiteConnection db = new SQLiteConnection(path);
				return db.Table<Entry>().ToList();
			}

		}

		public List<Account> IncomeAccounts
		{
			get
			{
				SQLiteConnection db = new SQLiteConnection(path);
				return db.Table<Account>().Where(acc => acc.AccountType == 1).ToList();
			}

		}

		public List<Account> ExpenseAccounts
		{
			get
			{
				SQLiteConnection db = new SQLiteConnection(path);
				return db.Table<Account>().Where(acc => acc.AccountType == 2).ToList();
			}

		}

		public List<Account> MoneyAccounts
		{
			get
			{
				SQLiteConnection db = new SQLiteConnection(path);
				return db.Table<Account>().Where(acc => acc.AccountType == 3).ToList();
			}

		}

		public List<TaxRate> TaxRates
		{
			get
			{
				SQLiteConnection db = new SQLiteConnection(path);
				return db.Table<TaxRate>().ToList();
			}

		}



		BookKeeperManager() 
		{
			SQLiteConnection db = new SQLiteConnection(path);
			db.CreateTable<Entry>();
			db.CreateTable<Account>();
			db.CreateTable<TaxRate>();

			if (IncomeAccounts.Count == 0)
			{
				db.Insert(new Account("Försäljning av tjänster", 3040, 1));
				db.Insert(new Account("Varuförsäljning", 3050, 1));
				db.Insert(new Account("Hyresintäkter", 3911, 1));
    			db.Insert(new Account("Provisionsintäkter", 3921, 1));
			}


			if (ExpenseAccounts.Count == 0)
			{
				db.Insert(new Account("Varuinköp", 4010, 2));
				db.Insert(new Account("Lokalhyra", 5010, 2));
				db.Insert(new Account("Städning och renhållning", 5060, 2));
				db.Insert(new Account("Leasing av datorer", 5252, 2));
			}

			if (MoneyAccounts.Count == 0)
			{
				db.Insert(new Account("Kassa", 1910, 3));
				db.Insert(new Account("Checkräkningskonto", 1930, 3));
			}

			if (TaxRates.Count == 0)
			{
				db.Insert(new TaxRate(0.06));
				db.Insert(new TaxRate(0.12));
				db.Insert(new TaxRate(0.25));
			}
		}

		public static BookKeeperManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new BookKeeperManager();
				}
				return instance;
			}
		}

		public void AddEntry(Entry e)
		{
			Entries.Add(e);
			SQLiteConnection db = new SQLiteConnection(path);
			db.Insert(e);
		}

		public string GetTaxReport()
		{
			string taxReport = "";
			double paidTaxTotal = 0;

			foreach (Entry e in Entries)
			{
				double rate = TaxRates[e.TaxRate-1].Rate;
				double paidTaxForEntry = (rate * e.Amount);

				if (e.EntryType == 2)
				{
					paidTaxForEntry = paidTaxForEntry * -1;

				}
				paidTaxTotal += paidTaxForEntry;
				taxReport += e.Date.ToShortDateString() +
							  " | " + e.Description +
							  " | Belopp: " + e.Amount +
							  "kr | Moms: " + paidTaxForEntry + " kr\n";
			}

			return taxReport + "\n Total moms: " + paidTaxTotal + " kr";
		}
	}
}