using System;
using QuantityX;

namespace NEOSPlus.Quantity
{
    public readonly struct Pressure : IQuantitySI<Pressure>, IQuantitySI, IQuantity<Pressure>, IQuantity, IComparable<Pressure>, IEquatable<Pressure>
    {
        public readonly double BaseValue;

        public static readonly Unit<Pressure> Pascal = new UnitSI<Pressure>(0, "", "");
        public static readonly Unit<Pressure> Bar = new Unit<Pressure>(1e5, new UnitGroup[1] { UnitGroup.Common }, new string[1] { "bar" }, new string[1] { "bar" });

        double IQuantity.BaseValue => BaseValue;

        public double SIPower => 1.0;

        public Unit<Pressure> DefaultUnit => Pascal;

        public Pressure(double baseValue = 0.0)
        {
            this = default(Pressure);
            BaseValue = baseValue;
        }

        public bool Equals(Pressure other)
        {
            return BaseValue == other.BaseValue;
        }

        public int CompareTo(Pressure other)
        {
            return BaseValue.CompareTo(other.BaseValue);
        }

        public string[] GetShortBaseNames()
        {
            return new string[] { "Pa" };
        }

        public string[] GetLongBaseNames()
        {
            return new string[] { "pascals", "pascal" };
        }

        public IUnit[] GetCommonSIUnits()
        {
            return new IUnit[]
            {
                SI<Pressure>.Kilo,
                SI<Pressure>.Mega,
                SI<Pressure>.Giga,
                Bar
            };
        }

        public IUnit[] GetExludedSIUnits()
        {
            return new IUnit[]
            {
                SI<Pressure>.Yotta,
                SI<Pressure>.Zetta,
                SI<Pressure>.Exa,
                SI<Pressure>.Peta,
                SI<Pressure>.Tera,
                SI<Pressure>.Deca,
                SI<Pressure>.Hecto,
                SI<Pressure>.Centi,
                SI<Pressure>.Deci,
                SI<Pressure>.Milli,
                SI<Pressure>.Micro,
                SI<Pressure>.Nano,
                SI<Pressure>.Pico,
                SI<Pressure>.Femto,
                SI<Pressure>.Atto,
                SI<Pressure>.Zepto,
                SI<Pressure>.Yocto
            };
        }

        public Pressure New(double baseVal)
        {
            return new Pressure(baseVal);
        }

        public Pressure Add(Pressure q)
        {
            return new Pressure(BaseValue + q.BaseValue);
        }

        public Pressure Subtract(Pressure q)
        {
            return new Pressure(BaseValue - q.BaseValue);
        }

        public Pressure Multiply(double n)
        {
            return new Pressure(BaseValue * n);
        }

        public Pressure Divide(double n)
        {
            return new Pressure(BaseValue / n);
        }

        public Ratio Divide(Pressure q)
        {
            return new Ratio(BaseValue / q.BaseValue);
        }

        public static Pressure Parse(string str, Unit<Pressure> defaultUnit = null)
        {
            return Unit<Pressure>.Parse(str, defaultUnit);
        }

        public static bool TryParse(string str, out Pressure q, Unit<Pressure> defaultUnit = null)
        {
            return Unit<Pressure>.TryParse(str, out q, defaultUnit);
        }

        public static Pressure operator +(Pressure a, Pressure b)
        {
            return a.Add(b);
        }

        public static Pressure operator -(Pressure a, Pressure b)
        {
            return a.Subtract(b);
        }

        public static Pressure operator *(Pressure a, double n)
        {
            return a.Multiply(n);
        }

        public static Pressure operator /(Pressure a, double n)
        {
            return a.Divide(n);
        }

        public static Ratio operator /(Pressure a, Pressure b)
        {
            return a.Divide(b);
        }

        public static Pressure operator -(Pressure a)
        {
            return a.Multiply(-1.0);
        }

        public override string ToString()
        {
            return this.FormatAuto();
        }
    }
}
