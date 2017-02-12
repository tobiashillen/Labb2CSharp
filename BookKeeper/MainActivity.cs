using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;

namespace BookKeeper
{
	[Activity(Label = "BookKeeper", MainLauncher = true)]
	public class MainActivity : Activity
	{
		Button newEntryBtn;
		Button entryListBtn;
		Button createReportsBtn;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_main);

			newEntryBtn = FindViewById<Button>(Resource.Id.btn_new_entry);
			entryListBtn = FindViewById<Button>(Resource.Id.btn_entry_list);
			createReportsBtn = FindViewById<Button>(Resource.Id.btn_create_reports);

			newEntryBtn.Click += delegate
			{
				Intent i = new Intent(this, typeof(EntryActivity));
				StartActivity(i);
			};

			entryListBtn.Click += delegate
			{
				Intent i = new Intent(this, typeof(EntryListActivity));
				StartActivity(i);
			};

			createReportsBtn.Click += delegate 
			{
				Intent i = new Intent(this, typeof(CreateReportsActivity));
				StartActivity(i);
			};
		}


	}
}

