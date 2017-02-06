using System;
using System.Collections.Generic;
using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Model;

namespace BookKeeper
{
	public class CustomAdapter : BaseAdapter<Entry>
	{
		Activity context;
		List<Entry> list;

		public CustomAdapter(Activity context, List<Entry> list)
		{
			this.context = context;
			this.list = list;
		}

		public override int Count
		{
			get { return list.Count; }
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override Entry this[int index]
		{
			get { return list[index]; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView;

			if (view == null)
				view = context.LayoutInflater.Inflate(Resource.Layout.CustomListItem, parent, false);

			Entry item = this[position];
			view.FindViewById<TextView>(Resource.Id.tv_entry_date).Text = item.Date.ToString("yyyy-MM-dd");
			view.FindViewById<TextView>(Resource.Id.tv_entry_description).Text = item.Description;
			view.FindViewById<TextView>(Resource.Id.tv_entry_amount).Text = item.Amount.ToString() + " kr";
			if (item.EntryType == 1)
			{
				view.FindViewById<TextView>(Resource.Id.tv_entry_amount).SetTextColor(Color.Green);
			}
			else if (item.EntryType == 2)
			{
				view.FindViewById<TextView>(Resource.Id.tv_entry_amount).SetTextColor(Color.Red);
			}
			return view;
		}
	}
}
