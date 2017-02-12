
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

		private List<string> GetTypeAccountReport(List<Account> list)
		{
			List<string> typeAccountReportList = new List<string>();
			for (int i = 0; i < list.Count; i++)
			{
				string report = "";
				var currentAccountEntriesAmount = bkm.Entries.
				                                     Where(e => e.TypeAccount == list[i].Number).
													 Select(e => e.Amount);

				report += list[i].Name
						 + "("
						 + list[i].Number
						 + ") (Totalt: "
						 + currentAccountEntriesAmount.Sum()
						 + " kr)\n";
				var currentAccountEntries = bkm.Entries.Where(e => e.TypeAccount == list[i].Number);

				foreach (Entry e in currentAccountEntries)
				{
					report += e.Date.ToString("yyyy-MM-dd") + " - " + e.Description + ", " + e.Amount + " kr. \n";
				}
				typeAccountReportList.Add(report);
			}
			return typeAccountReportList;
		}

		private List<string> GetMoneyAccountReport()
		{
			List<string> moneyAccountReportList = new List<string>();
			for (int i = 0; i < bkm.MoneyAccounts.Count; i++)
			{
				string report = "";
				var currentAccountEntriesPositiveAmount = bkm.Entries.
													 Where(e => e.MoneyAccount == bkm.MoneyAccounts[i].Number
														   && e.EntryType == 1).
													 Select(e => e.Amount);
				var currentAccountEntriesNegativeAmount = bkm.Entries.
													 Where(e => e.MoneyAccount == bkm.MoneyAccounts[i].Number
														   && e.EntryType == 2).
													 Select(e => e.Amount);

				report += bkm.MoneyAccounts[i].Name
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
				moneyAccountReportList.Add(report);
				report = "";
			}
			return moneyAccountReportList;
		}
	}
}
