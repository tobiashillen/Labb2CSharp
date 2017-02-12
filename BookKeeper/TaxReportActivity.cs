
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

		ListView taxReportLV;
		BookKeeperManager bkm;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_tax_report);
			SetTitle(Resource.String.title_tax_report_activity);
			taxReportLV = FindViewById<ListView>(Resource.Id.lv_taxReport);
			bkm = BookKeeperManager.Instance;
			string[] taxReportList = bkm.GetTaxReport().Split('*');
			taxReportLV.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, taxReportList);
		}
	}
}
