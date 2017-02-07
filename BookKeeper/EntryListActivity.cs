﻿
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

		BookKeeperManager bkm;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_entry_list);

			//TODO Do we need the OnListItemClick? 

			bkm = BookKeeperManager.Instance;

			// Initalizing ListView with custom adapter.
			ListView listView = FindViewById<ListView>(Resource.Id.lv_entries);
			listView.ItemClick += OnListItemClick;
			listView.Adapter = new CustomAdapter(this, bkm.Entries);
		}

		void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			Intent i = new Intent(this, typeof(EntryActivity));
			StartActivity(i);
		}
	}
}
