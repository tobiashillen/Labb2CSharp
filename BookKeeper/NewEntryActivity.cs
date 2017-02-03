
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
	[Activity(Label = "NewEntryActivity")]
	public class NewEntryActivity : Activity
	{
		RadioButton incomeRadioBtn;
		RadioButton expenseRadioBtn;
		TextView dateTV;
		Button dateBtn;
		EditText descriptionET;
		BookKeeperManager bkm;
		DateTime date;
		Spinner typeSpinner;
		Spinner accountSpinner;
		EditText amountET;
		Spinner taxRateSpinner;
		Button addEntryBtn;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_new_entry);

			bkm = BookKeeperManager.Instance;
			incomeRadioBtn = FindViewById<RadioButton>(Resource.Id.rb_income);
			expenseRadioBtn = FindViewById<RadioButton>(Resource.Id.rb_expense);
			dateTV = FindViewById<TextView>(Resource.Id.tv_date_result);
			dateBtn = FindViewById<Button>(Resource.Id.btn_date);
			descriptionET = FindViewById<EditText>(Resource.Id.ed_description);
			typeSpinner = FindViewById<Spinner>(Resource.Id.spinner_type);
			accountSpinner = FindViewById<Spinner>(Resource.Id.spinner_account);
			amountET = FindViewById<EditText>(Resource.Id.ed_amount);
			taxRateSpinner = FindViewById<Spinner>(Resource.Id.spinner_tax_rate);
			addEntryBtn = FindViewById<Button>(Resource.Id.btn_add_entry);


			// Setting up RadioGroup
			incomeRadioBtn.Click += delegate
			{
				setUpAccountSpinner(bkm.IncomeAccounts, typeSpinner);
			};
			expenseRadioBtn.Click += delegate
			{
				setUpAccountSpinner(bkm.ExpenseAccounts, typeSpinner);
			};

			// Set DateTime
			dateBtn.Click += delegate 
			{
				DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
														 {
															 date = time;
															dateTV.Text = date.ToString("yyyy-MM-dd");
														 });
				frag.Show(FragmentManager, DatePickerFragment.TAG);
			};

			// Setting spinner for type-accounts
			setUpAccountSpinner(bkm.IncomeAccounts, typeSpinner);
			// Setting up spinner for money-account
			setUpAccountSpinner(bkm.MoneyAccounts, accountSpinner);
			//Setting up spinner for VAT
			setUpTaxRateSpinner(bkm.TaxRates, taxRateSpinner);

			// Adding entry
			addEntryBtn.Click += delegate
			{
				
				if (incomeRadioBtn.Checked)
				{
					
					Entry e = new Entry(date,
										descriptionET.Text,
					                    bkm.IncomeAccounts[typeSpinner.SelectedItemPosition].Number,
					                    bkm.MoneyAccounts[accountSpinner.SelectedItemPosition].Number,
										Int32.Parse(amountET.Text),
					                    bkm.TaxRates[taxRateSpinner.SelectedItemPosition].Id);
					bkm.AddEntry(e);

				}
				else if (expenseRadioBtn.Checked)
				{
					Entry e = new Entry(date,
										descriptionET.Text,
					                    bkm.IncomeAccounts[typeSpinner.SelectedItemPosition].Number,
					                    bkm.MoneyAccounts[accountSpinner.SelectedItemPosition].Number,
										Int32.Parse(amountET.Text),
					                    bkm.TaxRates[taxRateSpinner.SelectedItemPosition].Id);
					bkm.AddEntry(e);
				}
				Finish();
			};
		}
		// Needed?
		private void TypeSpinnerItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			
		}

		private void setUpAccountSpinner(List<Account> list, Spinner spinner)
		{
			ArrayAdapter adapter = new ArrayAdapter<Account>(this, Android.Resource.Layout.SimpleSpinnerItem, list);
			spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(TypeSpinnerItemSelected);
			adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinner.Adapter = adapter;
		}

		private void setUpTaxRateSpinner(List<TaxRate> list, Spinner spinner)
		{
			ArrayAdapter adapter = new ArrayAdapter<TaxRate>(this, Android.Resource.Layout.SimpleSpinnerItem, list);
			spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(TypeSpinnerItemSelected);
			adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinner.Adapter = adapter;
		}
	}

	//DatePickerFragment-Class
	public class DatePickerFragment : DialogFragment,
									  DatePickerDialog.IOnDateSetListener
	{
		// TAG can be any string of your choice.
		public static readonly string TAG = "X:" + typeof(DatePickerFragment).Name.ToUpper();

		// Initialize this value to prevent NullReferenceExceptions.
		Action<DateTime> _dateSelectedHandler = delegate { };

		public static DatePickerFragment NewInstance(Action<DateTime> onDateSelected)
		{
			DatePickerFragment frag = new DatePickerFragment();
			frag._dateSelectedHandler = onDateSelected;
			return frag;
		}

		public override Dialog OnCreateDialog(Bundle savedInstanceState)
		{
			DateTime currently = DateTime.Now;
			DatePickerDialog dialog = new DatePickerDialog(Activity,
														   this,
														   currently.Year,
														   currently.Month,
														   currently.Day);
			return dialog;
		}

		public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
		{
			// Note: monthOfYear is a value between 0 and 11, not 1 and 12!
			DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
			_dateSelectedHandler(selectedDate);
		}
	}
}