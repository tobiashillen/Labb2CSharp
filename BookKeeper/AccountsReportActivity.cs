
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Model;

namespace BookKeeper
{
	[Activity(Label = "AccountsReportActivity")]
	public class AccountsReportActivity : Activity
	{
		BookKeeperManager bkm;
		ListView accountsReportLV;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_accounts_report);
			SetTitle(Resource.String.title_accounts_report_activity);
			bkm = BookKeeperManager.Instance;
			accountsReportLV = FindViewById<ListView>(Resource.Id.lv_accountsReport);

			List<string> reportList = GetTypeAccountReport(bkm.IncomeAccounts);
			reportList.AddRange(GetTypeAccountReport(bkm.ExpenseAccounts));
			reportList.AddRange(GetMoneyAccountReport());
			accountsReportLV.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, reportList);

		}

		//Generates a list of strings containing information about the type account passed to this method.
		private List<string> GetTypeAccountReport(List<Account> list)
		{
			List<string> typeAccountReportList = new List<string>();

			//Iterates list of type accounts
			for (int i = 0; i < list.Count; i++)
			{
				//Declare an empty string representing current money account.
				string report = "";

				//Gets total amount for current account.
				int total = bkm.Entries.
				                      Where(e => e.TypeAccount == list[i].Number).
				               Select(e => e.Amount).Sum();

				//Adds all necessary information about the current account to the string.
				report += String.Format("{0} ({1}) (Totalt: {2} kr)\n", 
				                        list[i].Name, list[i].Number, total);

				//Gets all entries connected to the current acount.
				var currentAccountEntries = bkm.Entries.Where(e => e.TypeAccount == list[i].Number);


				//Iterates entries, adding all necessary information about that entry to the report string.
				foreach (Entry e in currentAccountEntries)
				{
					report += e.Date.ToString("yyyy-MM-dd") + " - " + e.Description + ", " + e.Amount + " kr. \n";
				}

				//Adding string with all account information and transactions of the current account to return list.
				typeAccountReportList.Add(report);
			}
			return typeAccountReportList;
		}

		//Generates a list of strings containing information about all money account.
		private List<string> GetMoneyAccountReport()
		{
			List<string> moneyAccountReportList = new List<string>();

			//Iterates all money accounts to compose a string per account.
			for (int i = 0; i < bkm.MoneyAccounts.Count; i++)
			{
				//Declare an empty string representing current money account.
				string report = "";

				//Gets the total amount from all income transactions connected to current money account.
				int totalIncomeAmount = bkm.Entries.
									  Where(e => e.MoneyAccount == bkm.MoneyAccounts[i].Number
										  && e.EntryType == 1).
									  Select(e => e.Amount).Sum();
				
				//Gets the total amount from all expense transactions connected to current money account. 
				//Result in a negative value.
				int totalExpenseAmount = bkm.Entries.
											Where(e => e.MoneyAccount == bkm.MoneyAccounts[i].Number
												  && e.EntryType == 2).
											Select(e => e.Amount * -1).Sum();
				
				//Adds all necessary information about the current account to the string.
				report += String.Format("{0} ({1}) (Totalt: {2} kr)\n",
										bkm.MoneyAccounts[i].Name, bkm.MoneyAccounts[i].Number,
										(totalIncomeAmount + totalExpenseAmount));

				//Gets all entries connected to the current acount.
				var currentAccountEntries = bkm.Entries.Where(e => e.MoneyAccount == bkm.MoneyAccounts[i].Number);

				//Iterates entries, setting positive or negative value according to EntryType 
				//and adding all necessary information about that entry to the report string.
				foreach (Entry e in currentAccountEntries)
				{
					int amount = 0;
					if (e.EntryType == 1)
					{
						amount = e.Amount;
					}
					else if (e.EntryType == 2)
					{
						amount = e.Amount * -1;
					}
					report += String.Format("{0} - {1}, {2} kr. \n",
											e.Date.ToString("yyyy-MM-dd"), e.Description, amount);
				}

				//Adding string with all account information and transactions of the current account to return list.
				moneyAccountReportList.Add(report);
			}

			return moneyAccountReportList;
		}
	}
}
