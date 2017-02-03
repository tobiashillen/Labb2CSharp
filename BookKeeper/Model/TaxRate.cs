using System;
using SQLite;

namespace Model
{
	public class TaxRate
	{
		[PrimaryKey, AutoIncrement, Column("_id")]
		public int Id { get; private set; }
		public double Rate { get; private set; }

		public TaxRate() { }

		public TaxRate(double rate)
		{
			Rate = rate;
		}

		// Overrides ToString in procent format.
		public override string ToString()
		{
			return string.Format("{0} %", Rate*100);
		}
	}
}
