
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

	[Activity(Label = "EntryListActivity")]
	public class EntryListActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_entry_list);

			// Temp list for testing.
			List<Entry> list = new List<Entry>();
			list.Add(new Entry(true, new DateTime(2017, 01, 08), "Husköp", 1000, 2000, 1000000, 15.00));
			list.Add(new Entry(true, new DateTime(2017, 02, 01), "Datorköp", 1000, 2000, 15439, 15.00));
			list.Add(new Entry(true, new DateTime(2017, 02, 04), "Köpte mat", 1000, 2000, 700, 15.00));
			list.Add(new Entry(true, new DateTime(2016, 12, 20), "Besiktning", 1000, 2000, 1200, 15.00));
			list.Add(new Entry(true, new DateTime(2016, 12, 15), "Allmänt", 1000, 2000, 350, 15.00));
			list.Add(new Entry(true, new DateTime(2016, 11, 02), "Busskort", 1000, 2000, 1050, 15.00));
			list.Add(new Entry(true, new DateTime(2016, 10, 29), "Annan typ av utgift", 1000, 2000, 120, 15.00));
			list.Add(new Entry(true, new DateTime(2016, 10, 28), "Transport", 1000, 2000, 8020, 15.00));
			list.Add(new Entry(true, new DateTime(2016, 08, 09), "Skogsavgift", 1000, 2000, 4500, 15.00));

			//TODO Fix how date/time is displayed. Do we need the OnListItemClick? 

			// Initalizing ListView with custom adapter.
			ListView listView = FindViewById<ListView>(Resource.Id.lv_entries);
			listView.ItemClick += OnListItemClick;
			listView.Adapter = new CustomAdapter(this, list);

		}

		void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			
		}
	}
}
