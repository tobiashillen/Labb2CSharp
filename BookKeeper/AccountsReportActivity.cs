
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
			List<string> reportList = getReport();
			accountsReportLV.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, reportList);
		}

		private List<string> getReport()
		{
			List<string> reportList = new List<string>();
			string report = "";

			//Incomeaccounts
			for (int i = 0; i < bkm.IncomeAccounts.Count; i++)
			{
				var currentAccountEntriesAmount = bkm.Entries.
													 Where(e => e.TypeAccount == bkm.IncomeAccounts[i].Number).
													 Select(e => e.Amount);

				report +=  bkm.IncomeAccounts[i].Name
						 + "("
						 + bkm.IncomeAccounts[i].Number
						 + ") (Totalt: "
						 + currentAccountEntriesAmount.Sum()
						 + " kr)\n";
				var currentAccountEntries = bkm.Entries.Where(e => e.TypeAccount == bkm.IncomeAccounts[i].Number);

				foreach (Entry e in currentAccountEntries)
				{
					report += e.Date.ToString("yyyy-MM-dd") + " - " + e.Description + ", " + e.Amount + " kr. \n";
				}
				reportList.Add(report);
				report = "";
			}


			//Expenseaccount
			for (int i = 0; i < bkm.ExpenseAccounts.Count; i++)
			{
				var currentAccountEntriesAmount = bkm.Entries.
													 Where(e => e.TypeAccount == bkm.ExpenseAccounts[i].Number).
													 Select(e => e.Amount);

				report +=  bkm.ExpenseAccounts[i].Name
						 + "("
						 + bkm.ExpenseAccounts[i].Number
						 + ") (Totalt: "
						 + currentAccountEntriesAmount.Sum() * -1
						 + " kr)\n";
				var currentAccountEntries = bkm.Entries.Where(e => e.TypeAccount == bkm.ExpenseAccounts[i].Number);

				foreach (Entry e in currentAccountEntries)
				{
					report += e.Date.ToString("yyyy-MM-dd") + " - " + e.Description + ", " + e.Amount * -1 + " kr. \n";
				}
				reportList.Add(report);
				report = "";
			}

			//MoneyAccounts
			for (int i = 0; i < bkm.MoneyAccounts.Count; i++)
			{
				var currentAccountEntriesPositiveAmount = bkm.Entries.
													 Where(e => e.MoneyAccount == bkm.MoneyAccounts[i].Number
														   && e.EntryType == 1).
													 Select(e => e.Amount);
				var currentAccountEntriesNegativeAmount = bkm.Entries.
													 Where(e => e.MoneyAccount == bkm.MoneyAccounts[i].Number
														   && e.EntryType == 2).
													 Select(e => e.Amount);

				report +=  bkm.MoneyAccounts[i].Name
						 + "("
						 + bkm.MoneyAccounts[i].Number
						 + ") (Totalt: "
						 + (currentAccountEntriesPositiveAmount.Sum() - currentAccountEntriesNegativeAmount.Sum())
						 + " kr)\n";
				var currentAccountEntries = bkm.Entries.Where(e => e.MoneyAccount == bkm.MoneyAccounts[i].Number);

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
					report += e.Date.ToString("yyyy-MM-dd") + " - " + e.Description + ", " + amount + " kr. \n";
				}
				reportList.Add(report);
				report = "";
			}
			return reportList;
		}
	}
}
