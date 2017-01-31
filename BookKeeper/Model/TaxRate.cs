using System;
namespace Model
{
	public class TaxRate
	{
		public double Rate { get; private set; }

		public TaxRate(double rate)
		{
			Rate = rate;
		}

		public override string ToString()
		{
			return string.Format("{0} %", Rate);
		}
	}
}
