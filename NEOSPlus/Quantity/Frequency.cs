using System;
using QuantityX;

namespace NEOSPlus.Quantity
{
    public readonly struct Frequency : IQuantitySI<Frequency>, IQuantitySI, IQuantity<Frequency>, IQuantity, IComparable<Frequency>, IEquatable<Frequency>
    {
        public readonly double BaseValue;

        public static readonly Unit<Frequency> Hertz = new UnitSI<Frequency>(0, "", "");

        double IQuantity.BaseValue => BaseValue;

        public double SIPower => 1.0;

        public Unit<Frequency> DefaultUnit => Hertz;

        public Frequency(double baseValue = 0.0)
        {
            this = default(Frequency);
            BaseValue = baseValue;
        }

        public bool Equals(Frequency other)
        {
            return BaseValue == other.BaseValue;
        }

        public int CompareTo(Frequency other)
        {
            return BaseValue.CompareTo(other.BaseValue);
        }

        public string[] GetShortBaseNames()
        {
            return new string[] { "Hz" };
        }

        public string[] GetLongBaseNames()
        {
            return new string[] { "hertz" };
        }

        public IUnit[] GetCommonSIUnits()
        {
            return new IUnit[]
            {
                Hertz,
                SI<Frequency>.Kilo,
                SI<Frequency>.Mega,
                SI<Frequency>.Giga,
                SI<Frequency>.Tera
            };
        }

        public IUnit[] GetExludedSIUnits()
        {
            return new IUnit[]
            {
                SI<Frequency>.Yotta,
                SI<Frequency>.Zetta,
                SI<Frequency>.Exa,
                SI<Frequency>.Peta,
                SI<Frequency>.Deca,
                SI<Frequency>.Hecto,
                SI<Frequency>.Milli,
                SI<Frequency>.Centi,
                SI<Frequency>.Deci,
                SI<Frequency>.Yocto,
                SI<Frequency>.Zepto,
                SI<Frequency>.Atto,
                SI<Frequency>.Femto,
                SI<Frequency>.Pico,
                SI<Frequency>.Nano,
                SI<Frequency>.Micro,
            };
        }

        public Frequency New(double baseVal)
        {
            return new Frequency(baseVal);
        }

        public Frequency Add(Frequency q)
        {
            return new Frequency(BaseValue + q.BaseValue);
        }

        public Frequency Subtract(Frequency q)
        {
            return new Frequency(BaseValue - q.BaseValue);
        }

        public Frequency Multiply(double n)
        {
            return new Frequency(BaseValue * n);
        }

        public Frequency Divide(double n)
        {
            return new Frequency(BaseValue / n);
        }

        public Ratio Divide(Frequency q)
        {
            return new Ratio(BaseValue / q.BaseValue);
        }

        public static Frequency Parse(string str, Unit<Frequency> defaultUnit = null)
        {
            return Unit<Frequency>.Parse(str, defaultUnit);
        }

        public static bool TryParse(string str, out Frequency q, Unit<Frequency> defaultUnit = null)
        {
            return Unit<Frequency>.TryParse(str, out q, defaultUnit);
        }

        public static Frequency operator +(Frequency a, Frequency b)
        {
            return a.Add(b);
        }

        public static Frequency operator -(Frequency a, Frequency b)
        {
            return a.Subtract(b);
        }

        public static Frequency operator *(Frequency a, double n)
        {
            return a.Multiply(n);
        }

        public static Frequency operator /(Frequency a, double n)
        {
            return a.Divide(n);
        }

        public static Ratio operator /(Frequency a, Frequency b)
        {
            return a.Divide(b);
        }

        public static Frequency operator -(Frequency a)
        {
            return a.Multiply(-1.0);
        }

        public override string ToString()
        {
            return this.FormatAuto();
        }
    }
}
