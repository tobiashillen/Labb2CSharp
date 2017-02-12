
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

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_entry);
			bkm = BookKeeperManager.Instance;

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

			SetUpSpinner(bkm.IncomeAccounts, typeSpinner);
			SetUpSpinner(bkm.MoneyAccounts, accountSpinner);
			SetUpSpinner(bkm.TaxRates, taxRateSpinner);

			isEditMode = Intent.GetBooleanExtra("EditMode", false);

			if (isEditMode)
			{
				SetTitle(Resource.String.title_edit_entry_activity);
				editEntry = bkm.Entries[Intent.GetIntExtra("Entry", -1)];
				SetUpEditModeView();
			}
			else
			{
				SetTitle(Resource.String.title_new_entry_activity);
				date = DateTime.Now;
				dateTV.Text = date.ToString("yyyy-MM-dd");
			}

			//--- Delegates for all actions in view ---

			incomeRadioBtn.Click += delegate
			{
				SetUpSpinner(bkm.IncomeAccounts, typeSpinner);
			};

			expenseRadioBtn.Click += delegate
			{
				SetUpSpinner(bkm.ExpenseAccounts, typeSpinner);
			};

			dateBtn.Click += delegate 
			{
				DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
														 {
															date = time;
															dateTV.Text = date.ToString("yyyy-MM-dd");
														 });
				frag.Show(FragmentManager, DatePickerFragment.TAG);
			};

			amountET.TextChanged += delegate
			{
				SetExclTax();
			};

			taxRateSpinner.ItemSelected += delegate 
			{
				SetExclTax();
			};

			addEntryBtn.Click += delegate
			{
				if (!descriptionET.Text.Equals("")
					&& !amountET.Text.Equals(""))
				{
					if (incomeRadioBtn.Checked
					   && !isEditMode)
					{
						AddNewEntry(bkm.IncomeAccounts, 1);
					}
					else if (expenseRadioBtn.Checked
							 && !isEditMode)
					{
						AddNewEntry(bkm.ExpenseAccounts, 2);
					}
					else if (incomeRadioBtn.Checked
							 && isEditMode)
					{
						EditOldEntry(bkm.IncomeAccounts, 1);
					}
					else if (expenseRadioBtn.Checked
							 && isEditMode)
					{
						EditOldEntry(bkm.ExpenseAccounts, 2);
					}
					Finish();
				}
				else
				{
					Toast.MakeText(this, "Var god fyll i alla fält.", ToastLength.Short).Show();
				}
			};
		}

		//Adds new entry via BookKeeperManager.
		private void AddNewEntry(List<Account> typeAcount, int entryType)
		{
			bkm.AddEntry(new Entry(date,
										descriptionET.Text,
			                       		typeAcount[typeSpinner.SelectedItemPosition].Number,
										bkm.MoneyAccounts[accountSpinner.SelectedItemPosition].Number,
										Int32.Parse(amountET.Text),
										bkm.TaxRates[taxRateSpinner.SelectedItemPosition].Id,
			                       		entryType));
		}

		//Edits old entry via BookKeeperManager.
		private void EditOldEntry(List<Account> typeAccount, int entryType)
		{
			editEntry.Date = date;
			editEntry.Description = descriptionET.Text;
			editEntry.TypeAccount = typeAccount[typeSpinner.SelectedItemPosition].Number;
			editEntry.MoneyAccount = bkm.MoneyAccounts[accountSpinner.SelectedItemPosition].Number;
			editEntry.Amount = Int32.Parse(amountET.Text);
			editEntry.TaxRate = bkm.TaxRates[taxRateSpinner.SelectedItemPosition].Id;
			editEntry.EntryType = entryType;

			bkm.EditEntry(editEntry);
		}

		//Sets the tax excluding amount according to incl. amount and taxrate. 
		private void SetExclTax()
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

		//Populates chosen spinner with chosen generic list<T>.
		private void SetUpSpinner<T>(List<T> list, Spinner spinner)
		{
			ArrayAdapter adapter = new ArrayAdapter<T>(this, Android.Resource.Layout.SimpleSpinnerItem, list);
			adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinner.Adapter = adapter;
		}

		//Changes all view objects according to EditEntry.
		private void SetUpEditModeView()
		{
			if (editEntry.EntryType == 1)
			{
				incomeRadioBtn.Checked = true;
				SetUpSpinner(bkm.IncomeAccounts, typeSpinner);
				SetSelectionForAccountSpinner(bkm.IncomeAccounts, editEntry.TypeAccount, typeSpinner);
			}
			else if (editEntry.EntryType == 2)
			{
				expenseRadioBtn.Checked = true;
				SetUpSpinner(bkm.ExpenseAccounts, typeSpinner);
				SetSelectionForAccountSpinner(bkm.ExpenseAccounts, editEntry.TypeAccount, typeSpinner);
			}

			date = editEntry.Date;
			dateTV.Text = date.ToString("yyyy-MM-dd");
			descriptionET.Text = editEntry.Description;
			SetSelectionForAccountSpinner(bkm.MoneyAccounts, editEntry.MoneyAccount, accountSpinner);
			amountET.Text = editEntry.Amount.ToString();
			taxRateSpinner.SetSelection(editEntry.TaxRate-1);
			SetExclTax();
			addEntryBtn.Text = GetString(Resource.String.btn_edit_entry);
		}

		//Sets spinner value according to chosen account number.
		private void SetSelectionForAccountSpinner(List<Account> accountList, int accountNumber, Spinner spinner)
		{
			int listId = -1;
			for (int i = 0; i < accountList.Count; i++)
			{
				if (accountList[i].Number == accountNumber)
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