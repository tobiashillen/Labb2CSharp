using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;

namespace BookKeeper
{
	[Activity(Label = "BookKeeper", MainLauncher = true)]
	public class MainActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.activity_main);

			Button newEntryBtn = FindViewById<Button>(Resource.Id.btn_new_entry);

			newEntryBtn.Click += delegate
			{
				Intent i = new Intent(this, typeof(NewEntryActivity));
				StartActivity(i);
			};
		}


	}
}

