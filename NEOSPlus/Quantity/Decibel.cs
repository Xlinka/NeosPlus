using System;
using QuantityX;

namespace NEOSPlus.Quantity
{
    public readonly struct Decibel : IQuantitySI<Decibel>, IQuantitySI, IQuantity<Decibel>, IQuantity, IComparable<Decibel>, IEquatable<Decibel>
    {
        public readonly double BaseValue;

        public static readonly Unit<Decibel> dB = new UnitSI<Decibel>(0, "", "");

        double IQuantity.BaseValue => BaseValue;

        public double SIPower => 1.0;

        public Unit<Decibel> DefaultUnit => dB;

        public Decibel(double baseValue = 0.0)
        {
            this = default(Decibel);
            BaseValue = baseValue;
        }

        public bool Equals(Decibel other)
        {
            return BaseValue == other.BaseValue;
        }

        public int CompareTo(Decibel other)
        {
            return BaseValue.CompareTo(other.BaseValue);
        }

        public string[] GetShortBaseNames()
        {
            return new string[] { "dB" };
        }

        public string[] GetLongBaseNames()
        {
            return new string[] { "decibels", "decibel" };
        }

        public IUnit[] GetCommonSIUnits()
        {
            return new IUnit[]
            {
                dB,
            };
        }

        public IUnit[] GetExludedSIUnits()
        {
            return new IUnit[]
            {
                SI<Decibel>.Yotta,
                SI<Decibel>.Zetta,
                SI<Decibel>.Exa,
                SI<Decibel>.Peta,
                SI<Decibel>.Tera,
                SI<Decibel>.Giga,
                SI<Decibel>.Mega,
                SI<Decibel>.Kilo,
                SI<Decibel>.Deca,
                SI<Decibel>.Hecto,
                SI<Decibel>.Milli,
                SI<Decibel>.Centi,
                SI<Decibel>.Deci,
                SI<Decibel>.Yocto,
                SI<Decibel>.Zepto,
                SI<Decibel>.Atto,
                SI<Decibel>.Femto,
                SI<Decibel>.Pico,
                SI<Decibel>.Nano,
                SI<Decibel>.Micro,
            };
        }

        public Decibel New(double baseVal)
        {
            return new Decibel(baseVal);
        }

        public Decibel Add(Decibel q)
        {
            return new Decibel(BaseValue + q.BaseValue);
        }

        public Decibel Subtract(Decibel q)
        {
            return new Decibel(BaseValue - q.BaseValue);
        }

        public Decibel Multiply(double n)
        {
            return new Decibel(BaseValue * n);
        }

        public Decibel Divide(double n)
        {
            return new Decibel(BaseValue / n);
        }

        public Ratio Divide(Decibel q)
        {
            return new Ratio(BaseValue / q.BaseValue);
        }

        public static Decibel Parse(string str, Unit<Decibel> defaultUnit = null)
        {
            return Unit<Decibel>.Parse(str, defaultUnit);
        }

        public static bool TryParse(string str, out Decibel q, Unit<Decibel> defaultUnit = null)
        {
            return Unit<Decibel>.TryParse(str, out q, defaultUnit);
        }

        public static Decibel operator +(Decibel a, Decibel b)
        {
            return a.Add(b);
        }

        public static Decibel operator -(Decibel a, Decibel b)
        {
            return a.Subtract(b);
        }

        public static Decibel operator *(Decibel a, double n)
        {
            return a.Multiply(n);
        }

        public static Decibel operator /(Decibel a, double n)
        {
            return a.Divide(n);
        }

        public static Ratio operator /(Decibel a, Decibel b)
        {
            return a.Divide(b);
        }

        public static Decibel operator -(Decibel a)
        {
            return a.Multiply(-1.0);
        }

        public override string ToString()
        {
            return this.FormatAuto();
        }
    }
}
