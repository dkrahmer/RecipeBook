﻿using Newtonsoft.Json;
using Rationals;
using System;
using System.Text.RegularExpressions;

namespace Common.Structs
{
	[JsonConverter(typeof(AmountJsonConverter))]
	public struct Amount
	{
		private decimal? _decimal;
		private Rational? _fraction;

		private const int DIGITS_CONVERT_TO_FRACTION_THRESHOLD = 3;
		private const decimal FRACTION_TOLERANCE = 0.01m;

		public bool IsDecimal => _decimal.HasValue;

		public bool IsFraction => _fraction.HasValue;

		public bool IsEmpty => !IsDecimal && !IsFraction;

		public Amount(decimal value) : this()
		{
			_decimal = decimal.Parse(value.ToString("G29"));
		}

		public Amount(Rational value) : this()
		{
			_fraction = value;
		}

		public Amount(string value) : this()
		{
			if (!TryParse(value, out Amount amount))
				throw new ArgumentException($"String '{value}' could not be parsed into an Amount.");

			_decimal = amount._decimal;
			_fraction = amount._fraction;
		}

		public Amount ToDecimalAmount(int decimals = 2)
		{
			if (IsDecimal)
				return this;

			return new Amount() { _decimal = decimal.Round((decimal)_fraction.Value.Numerator / (decimal)_fraction.Value.Denominator, 2) };
		}

		public Amount ToFractionAmount()
		{
			if (IsFraction)
				return this;

			return new Amount() { _fraction = Rational.ParseDecimal(_decimal.ToString(), FRACTION_TOLERANCE) };
		}

		public static Amount Parse(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentException("Invalid Amount value. Cannot be null or empty.");

			if (value.Contains("/"))
			{
				// this is a fraction. Format it the way the Rationals library likes it
				Regex regex = new Regex("[ ]{2,}");
				value = regex.Replace(value.Replace("\t", " "), " ").Replace(" ", " + ");

				return new Amount() { _fraction = Rational.Parse(value) };
			}
			else
			{
				var amount = new Amount() { _decimal = decimal.Parse(decimal.Parse(value).ToString("G29")) };
				int digitsAfterDecimal = BitConverter.GetBytes(decimal.GetBits(amount._decimal.Value)[3])[2];
				if (digitsAfterDecimal >= DIGITS_CONVERT_TO_FRACTION_THRESHOLD)
					amount = amount.ToFractionAmount();

				return amount;
			}
		}

		public static bool TryParse(string value, out Amount amount)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				amount = new Amount();
				return false;
			}

			if (value.Contains("/"))
			{
				// this is a fraction. Format it the way the Rationals library likes it
				Regex regex = new Regex("[ ]{2,}");
				value = regex.Replace(value.Replace("\t", " "), " ").Replace(" ", " + ");

				if (Rational.TryParse(value, out Rational fraction))
				{
					amount = new Amount() { _fraction = fraction };
					return true;
				}
			}
			else
			{
				if (decimal.TryParse(value, out decimal number))
				{
					amount = new Amount() { _decimal = decimal.Parse(number.ToString("G29")) };
					int digitsAfterDecimal = BitConverter.GetBytes(decimal.GetBits(amount._decimal.Value)[3])[2];
					if (digitsAfterDecimal >= DIGITS_CONVERT_TO_FRACTION_THRESHOLD)
						amount = amount.ToFractionAmount();
					return true;
				}
			}

			amount = new Amount(0m);
			return false;
		}

		public static Amount operator +(Amount x, Amount y)
		{
			if (x.IsEmpty || y.IsEmpty)
				return new Amount();

			if (x.IsDecimal && y.IsDecimal)
				return new Amount(x._decimal.Value + y._decimal.Value);

			if (x.IsDecimal)
				return new Amount(Rational.ParseDecimal(x._decimal.ToString(), FRACTION_TOLERANCE) + y._fraction.Value);

			if (y.IsDecimal)
				return new Amount(x._fraction.Value + Rational.ParseDecimal(y._decimal.ToString(), FRACTION_TOLERANCE));

			return new Amount(x._fraction.Value + y._fraction.Value);
		}

		public static Amount operator -(Amount x, Amount y)
		{
			if (x.IsEmpty || y.IsEmpty)
				return new Amount();

			if (x.IsDecimal && y.IsDecimal)
				return new Amount(x._decimal.Value - y._decimal.Value);

			if (x.IsDecimal)
				return new Amount(Rational.ParseDecimal(x._decimal.ToString(), FRACTION_TOLERANCE) - y._fraction.Value);

			if (y.IsDecimal)
				return new Amount(x._fraction.Value - Rational.ParseDecimal(y._decimal.ToString(), FRACTION_TOLERANCE));

			return new Amount(x._fraction.Value - y._fraction.Value);
		}

		public static Amount operator *(Amount x, Amount y)
		{
			if (x.IsEmpty || y.IsEmpty)
				return new Amount();

			if (x.IsDecimal && y.IsDecimal)
				return new Amount(x._decimal.Value * y._decimal.Value);

			if (x.IsDecimal)
				return new Amount(Rational.ParseDecimal(x._decimal.ToString(), FRACTION_TOLERANCE) * y._fraction.Value);

			if (y.IsDecimal)
				return new Amount(x._fraction.Value * Rational.ParseDecimal(y._decimal.ToString(), FRACTION_TOLERANCE));

			return new Amount(x._fraction.Value * y._fraction.Value);
		}

		public static Amount operator /(Amount x, Amount y)
		{
			if (x.IsEmpty || y.IsEmpty)
				return new Amount();

			if (x.IsDecimal && y.IsDecimal)
				return new Amount(x._decimal.Value / y._decimal.Value);

			if (x.IsDecimal)
				return new Amount(Rational.ParseDecimal(x._decimal.ToString(), FRACTION_TOLERANCE) / y._fraction.Value);

			if (y.IsDecimal)
				return new Amount(x._fraction.Value / Rational.ParseDecimal(y._decimal.ToString(), FRACTION_TOLERANCE));

			return new Amount(x._fraction.Value / y._fraction.Value);
		}

		public override string ToString()
		{
			return _decimal?.ToString("G29") ?? _fraction?.ToString("W").Replace(" + ", " ") ?? "";
		}
	}
}