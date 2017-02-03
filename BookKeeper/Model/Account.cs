using System;
using SQLite;

namespace Model
{
	public class Account
	{
		[PrimaryKey]
		public int Number { get; private set; }
		public string Name { get; private set; }

		// 1 = IncomeAccounts, 2 = ExpenseAccounts, 3 = MoneyAccounts
		public int AccountType { get; set; }

		public Account() { }

		public Account(string name, int number, int accountType)
		{
			Name = name;
			Number = number;
			AccountType = accountType;
		}

		public override string ToString()
		{
			return string.Format("{0}, {1}", Name, Number );
		}
	}
}
