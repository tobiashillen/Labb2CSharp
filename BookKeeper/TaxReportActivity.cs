
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
	[Activity(Label = "TaxReportActivity")]
	public class TaxReportActivity : Activity
	{

		TextView totalTaxPaidTV;
		BookKeeperManager bkm;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_tax_report);

			totalTaxPaidTV = FindViewById<TextView>(Resource.Id.tv_total_tax_paid);
			bkm = BookKeeperManager.Instance;
			totalTaxPaidTV.Text = bkm.GetTaxReport();

		}
	}
}
