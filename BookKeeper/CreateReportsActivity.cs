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

namespace BookKeeper
{
	[Activity(Label = "CreateReportsActivity")]
	public class CreateReportsActivity : Activity
	{
		Button accountReportsBtn;
		Button taxReportBtn;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_create_reports);

			accountReportsBtn = FindViewById<Button>(Resource.Id.btn_account_reports);
			taxReportBtn = FindViewById<Button>(Resource.Id.btn_tax_report);
		}
	}
}
