using System;
namespace Model
{
	public class Account
	{
		private string AccountName { get; set; }
		private int AccountNumber { get; set; }
		public Account(string accountName, int accountNumber)
		{
			AccountName = accountName;
			AccountNumber = accountNumber;
		}
	}
}
