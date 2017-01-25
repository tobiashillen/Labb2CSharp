using System;
using System.Collections.Generic;

namespace Model
{
	public class BookKeeperManager
	{
		private static BookKeeperManager instance;

		private List<Entry> AllEntrys { get; set; }
		private List<Account> IncomeAccounts { get; set; }
		private List<Account> ExpenseAccounts { get; set; }
		private List<Account> MoneyAccounts { get; set; }
		private List<TaxRate> TaxRates { get; set; }

		private BookKeeperManager() { }

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
			AllEntrys.Add(e);
		}
	}
}