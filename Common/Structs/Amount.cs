using Newtonsoft.Json;
using Rationals;
using System;
using System.Collections.Generic;
using System.Linq;
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

		public Amount(Amount value) : this()
		{
			_decimal = value._decimal;
			_fraction = value._fraction;
		}

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

		public decimal ToDecimal(int decimals = 2)
		{
			if (IsDecimal)
				return decimal.Round(_decimal.Value, decimals, MidpointRounding.AwayFromZero);

			return decimal.Round((decimal) _fraction.Value.Numerator / (decimal) _fraction.Value.Denominator, decimals, MidpointRounding.AwayFromZero);
		}

		public Rational ToRational()
		{
			if (IsFraction)
				return _fraction.Value;

			return Rational.ParseDecimal(_decimal.ToString(), FRACTION_TOLERANCE);
		}

		public Amount ToDecimalAmount(int decimals = 2)
		{
			if (IsDecimal)
				return this;

			return new Amount() { _decimal = ToDecimal(decimals) };
		}

		public Amount ToFractionAmount()
		{
			if (IsFraction)
				return this;

			return new Amount() { _fraction = ToRational() };
		}

		public static Amount Parse(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentException("Invalid Amount value. Cannot be null or empty.");

			value = GetNormalizedValue(value);

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

		public static readonly Dictionary<string, string> FractionMap = new Dictionary<string, string>()
		{
			// Fraction characters
			{ "\x2044", "/"},		// Fraction Slash
			{ "\x215F", "1/"},
			{ "\x00BC", "1/4"},
			{ "\x00BD", "1/2"},
			{ "\x00BE", "3/4"},
			{ "\x2150", "1/7"},
			{ "\x2151", "1/9"},
			{ "\x2152", "1/10"},
			{ "\x2153", "1/3"},
			{ "\x2154", "2/3"},
			{ "\x2155", "1/5"},
			{ "\x2156", "2/5"},
			{ "\x2157", "3/5"},
			{ "\x2158", "4/5"},
			{ "\x2159", "1/6"},
			{ "\x215A", "5/6"},
			{ "\x215B", "1/8"},
			{ "\x215C", "3/8"},
			{ "\x215D", "5/8"},
			{ "\x215E", "7/8"},
			{ "\x2189", "0"}		// Char: "0/8"
		};

		private static string GetNormalizedValue(string value)
		{
			string output = FractionMap.Aggregate(value, (current, map) => current.Replace(map.Key, map.Value));

			return output;
		}

		public static bool TryParse(string value, out Amount amount)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				amount = new Amount();
				return false;
			}

			value = GetNormalizedValue(value);

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

		public static bool operator <=(Amount x, Amount y)
		{
			if (x.IsEmpty && y.IsEmpty)
				return true;

			if (x.IsEmpty || y.IsEmpty)
				return false;

			if (x.IsDecimal && y.IsDecimal)
				return x._decimal.Value <= y._decimal.Value;

			if (x.IsDecimal)
				return Rational.ParseDecimal(x._decimal.ToString(), FRACTION_TOLERANCE) <= y._fraction.Value;

			if (y.IsDecimal)
				return x._fraction.Value <= Rational.ParseDecimal(y._decimal.ToString(), FRACTION_TOLERANCE);

			return x._fraction.Value <= y._fraction.Value;
		}

		public static bool operator >=(Amount x, Amount y)
		{
			if (x.IsEmpty && y.IsEmpty)
				return true;

			if (x.IsEmpty || y.IsEmpty)
				return false;

			if (x.IsDecimal && y.IsDecimal)
				return x._decimal.Value >= y._decimal.Value;

			if (x.IsDecimal)
				return Rational.ParseDecimal(x._decimal.ToString(), FRACTION_TOLERANCE) >= y._fraction.Value;

			if (y.IsDecimal)
				return x._fraction.Value >= Rational.ParseDecimal(y._decimal.ToString(), FRACTION_TOLERANCE);

			return x._fraction.Value >= y._fraction.Value;
		}

		public static bool operator ==(Amount x, Amount y)
		{
			if (x.IsEmpty && y.IsEmpty)
				return true;

			if (x.IsEmpty || y.IsEmpty)
				return false;

			if (x.IsDecimal && y.IsDecimal)
				return x._decimal.Value == y._decimal.Value;

			if (x.IsDecimal)
				return Rational.ParseDecimal(x._decimal.ToString(), FRACTION_TOLERANCE) == y._fraction.Value;

			if (y.IsDecimal)
				return x._fraction.Value == Rational.ParseDecimal(y._decimal.ToString(), FRACTION_TOLERANCE);

			return x._fraction.Value == y._fraction.Value;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is Amount))
				return false;

			return (Amount) obj == this;
		}

		public override int GetHashCode()
		{
			if (IsDecimal)
				return _decimal.GetHashCode();

			if (IsFraction)
				return _fraction.GetHashCode();

			return base.GetHashCode();
		}

		public static bool operator <(Amount x, Amount y)
		{
			return !(x >= y);
		}

		public static bool operator >(Amount x, Amount y)
		{
			return !(x <= y);
		}

		public static bool operator !=(Amount x, Amount y)
		{
			return !(x == y);
		}

		/// <summary>
		/// Converts to string.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return ToString(decimalFormat: "G29", fractionFormat: "W");
		}

		/// <summary>
		/// Converts to string.
		/// </summary>
		/// <param name="decimalFormat">Use decimal type format.</param>
		/// <param name="fractionFormat">F for normal fraction, C for canonical fraction, W for whole+fractional.</param>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public string ToString(string decimalFormat = "G29", string fractionFormat = "W")
		{
			return _decimal?.ToString(decimalFormat) ?? _fraction?.ToString(fractionFormat).Replace(" + ", " ") ?? "";
		}
	}
}