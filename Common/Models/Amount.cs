namespace Common.Models
{
	public class Amount
	{
		public decimal Decimal { get; protected set; }
		public string Fraction { get; protected set; }

		/*
		public bool TryParse(string value, out Amount amount)
		{
			amount = new Amount();
			if (value.Contains("/"))
			{
				// this is a fraction
			}
			else
			{

			}
		}
		*/

		public enum AmountTypeEnum
		{
			Fraction,
			Decimal
		}

		public AmountTypeEnum AmountType;

		public override string ToString()
		{
			switch (AmountType)
			{
				case AmountTypeEnum.Decimal:
					return "";

				case AmountTypeEnum.Fraction:
					return "";

				default:
					return null;
			}
		}
	}
}