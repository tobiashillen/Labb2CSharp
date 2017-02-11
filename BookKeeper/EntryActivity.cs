
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
	[Activity(Label = "EntryActivity")]
	public class EntryActivity : Activity
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
		TextView amountExclTaxValueTV;
		EditText amountET;
		Spinner taxRateSpinner;
		Button addEntryBtn;

		bool isEditMode;
		Entry editEntry;
		int editEntryId;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_entry);

			bkm = BookKeeperManager.Instance;


			isEditMode = Intent.GetBooleanExtra("EditMode", false);
			if (isEditMode)
			{
				editEntryId = Intent.GetIntExtra("Entry", -1);
				editEntry = bkm.Entries[editEntryId];
			}

			incomeRadioBtn = FindViewById<RadioButton>(Resource.Id.rb_income);
			expenseRadioBtn = FindViewById<RadioButton>(Resource.Id.rb_expense);
			dateTV = FindViewById<TextView>(Resource.Id.tv_date_result);
			dateBtn = FindViewById<Button>(Resource.Id.btn_date);
			descriptionET = FindViewById<EditText>(Resource.Id.ed_description);
			typeSpinner = FindViewById<Spinner>(Resource.Id.spinner_type);
			accountSpinner = FindViewById<Spinner>(Resource.Id.spinner_account);
			amountExclTaxValueTV = FindViewById<TextView>(Resource.Id.tv_amount_excl_tax_value);
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

			// Setting up DateTime
			date = DateTime.Now;
			dateTV.Text = date.ToString("yyyy-MM-dd");
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
			//Setting up spinner for TaxRate
			setUpTaxRateSpinner(bkm.TaxRates, taxRateSpinner);

			//Setting up amount excl tax
			amountET.TextChanged += delegate
			{
				setExclTax();
			};

			taxRateSpinner.ItemSelected += delegate 
			{
				setExclTax();
			};

			// Adding entry
			addEntryBtn.Click += delegate
			{
				if (incomeRadioBtn.Checked
					&& !descriptionET.Text.Equals("")
					&& !amountET.Text.Equals("")
					&& !isEditMode)
				{

					Entry e = new Entry(date,
										descriptionET.Text,
										bkm.IncomeAccounts[typeSpinner.SelectedItemPosition].Number,
										bkm.MoneyAccounts[accountSpinner.SelectedItemPosition].Number,
										Int32.Parse(amountET.Text),
										bkm.TaxRates[taxRateSpinner.SelectedItemPosition].Id,
										1);
					bkm.AddEntry(e);
					Finish();
				}
				else if (expenseRadioBtn.Checked
						 && !descriptionET.Equals("")
						 && !amountET.Text.Equals("")
						 && !isEditMode)
				{
					Entry e = new Entry(date,
										descriptionET.Text,
										bkm.ExpenseAccounts[typeSpinner.SelectedItemPosition].Number,
										bkm.MoneyAccounts[accountSpinner.SelectedItemPosition].Number,
										Int32.Parse(amountET.Text),
										bkm.TaxRates[taxRateSpinner.SelectedItemPosition].Id,
										2);
					bkm.AddEntry(e);
					Finish();
				}
				else if (incomeRadioBtn.Checked
						 && !descriptionET.Equals("")
						 && !amountET.Text.Equals("")
						 && isEditMode)
				{
					editEntry.Date = date;
					editEntry.Description = descriptionET.Text;
					editEntry.TypeAccount = bkm.IncomeAccounts[typeSpinner.SelectedItemPosition].Number;
					editEntry.MoneyAccount = bkm.MoneyAccounts[accountSpinner.SelectedItemPosition].Number;
					editEntry.Amount = Int32.Parse(amountET.Text);
					editEntry.TaxRate = bkm.TaxRates[taxRateSpinner.SelectedItemPosition].Id;
					editEntry.EntryType = 1;

					bkm.EditEntry(editEntry);
					Finish();
				}
				else if (expenseRadioBtn.Checked 
				         && !descriptionET.Equals("") 
				         && !amountET.Text.Equals("") 
				         && isEditMode)
				{
					editEntry.Date = date;
					editEntry.Description = descriptionET.Text;
					editEntry.TypeAccount = bkm.ExpenseAccounts[typeSpinner.SelectedItemPosition].Number;
					editEntry.MoneyAccount = bkm.MoneyAccounts[accountSpinner.SelectedItemPosition].Number;
					editEntry.Amount = Int32.Parse(amountET.Text);
					editEntry.TaxRate = bkm.TaxRates[taxRateSpinner.SelectedItemPosition].Id;
					editEntry.EntryType = 2;

					bkm.EditEntry(editEntry);
					Finish();
				}
				else
				{
					Toast.MakeText(this, "Var god fyll i alla fält.", ToastLength.Short).Show();
				}
			};

			//Setting up edit mode if true
			if (isEditMode)
			{
				setUpEditModeView();
			}

		}

		private void setExclTax()
		{
			if (amountET.Text.Equals(""))
			{
				amountExclTaxValueTV.Text = "-";
			}
			else
			{
				double value = Double.Parse(amountET.Text)
									 * (1 - bkm.TaxRates[taxRateSpinner.SelectedItemPosition].Rate);
				amountExclTaxValueTV.Text = value.ToString();
			}
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

		//Fills all fields with info from current Entry and changes button to edit mode.
		private void setUpEditModeView()
		{

			if (editEntry.EntryType == 1)
			{
				incomeRadioBtn.Checked = true;
				setUpAccountSpinner(bkm.IncomeAccounts, typeSpinner);
				setSelectionForAccountSpinner(bkm.IncomeAccounts, editEntry.TypeAccount, typeSpinner);
			}
			else if (editEntry.EntryType == 2)
			{
				expenseRadioBtn.Checked = true;
				setUpAccountSpinner(bkm.ExpenseAccounts, typeSpinner);
				setSelectionForAccountSpinner(bkm.ExpenseAccounts, editEntry.TypeAccount, typeSpinner);
			}

			date = editEntry.Date;
			dateTV.Text = date.ToString("yyyy-MM-dd");
			descriptionET.Text = editEntry.Description;
			setSelectionForAccountSpinner(bkm.MoneyAccounts, editEntry.MoneyAccount, accountSpinner);
			amountET.Text = editEntry.Amount.ToString();
			taxRateSpinner.SetSelection(editEntry.TaxRate-1);
			setExclTax();

			addEntryBtn.Text = "Ändra händelse";
		}

		private void setSelectionForAccountSpinner(List<Account> list, int accountNumber, Spinner spinner)
		{
			int listId = -1;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Number == accountNumber)
				{
					listId = i;
				}
			}

			if (listId > -1)
			{
				spinner.SetSelection(listId);
			}
		}
	}










	//DatePickerFragment-Class
	public class DatePickerFragment : DialogFragment,
									  DatePickerDialog.IOnDateSetListener
	{
		public static readonly string TAG = "TAG:" + typeof(DatePickerFragment).Name.ToUpper();
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
			DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
			_dateSelectedHandler(selectedDate);
		}
	}
}