using System;
using QuantityX;

namespace NEOSPlus.Quantity
{
    public readonly struct Joule : IQuantitySI<Joule>, IQuantitySI, IQuantity<Joule>, IQuantity, IComparable<Joule>, IEquatable<Joule>
    {
        public readonly double BaseValue;

        public static readonly Unit<Joule> Joules = new UnitSI<Joule>(0, "J", "joule");

        double IQuantity.BaseValue => BaseValue;

        public double SIPower => 1.0;

        public Unit<Joule> DefaultUnit => Joules;

        public Joule(double baseValue = 0.0)
        {
            this = default(Joule);
            BaseValue = baseValue;
        }

        public bool Equals(Joule other)
        {
            return BaseValue == other.BaseValue;
        }

        public int CompareTo(Joule other)
        {
            return BaseValue.CompareTo(other.BaseValue);
        }

        public string[] GetShortBaseNames()
        {
            return new string[] { "J" };
        }

        public string[] GetLongBaseNames()
        {
            return new string[] { "joules", "joule" };
        }

        public IUnit[] GetCommonSIUnits()
        {
            return new IUnit[]
            {
                SI<Joule>.Pico,
                SI<Joule>.Nano,
                SI<Joule>.Micro,
                SI<Joule>.Milli,
                Joules,
                SI<Joule>.Kilo,
                SI<Joule>.Mega,
                SI<Joule>.Giga,
                SI<Joule>.Tera,
                SI<Joule>.Peta
            };
        }

        public IUnit[] GetExludedSIUnits()
        {
            return new IUnit[]
            {
                SI<Joule>.Yotta,
                SI<Joule>.Zetta,
                SI<Joule>.Exa,
                SI<Joule>.Deca,
                SI<Joule>.Hecto,
                SI<Joule>.Centi,
                SI<Joule>.Deci,
                SI<Joule>.Yocto,
                SI<Joule>.Zepto,
                SI<Joule>.Atto,
                SI<Joule>.Femto
            };
        }

        public Joule New(double baseVal)
        {
            return new Joule(baseVal);
        }

        public Joule Add(Joule q)
        {
            return new Joule(BaseValue + q.BaseValue);
        }

        public Joule Subtract(Joule q)
        {
            return new Joule(BaseValue - q.BaseValue);
        }

        public Joule Multiply(double n)
        {
            return new Joule(BaseValue * n);
        }

        public Joule Divide(double n)
        {
            return new Joule(BaseValue / n);
        }

        public Ratio Divide(Joule q)
        {
            return new Ratio(BaseValue / q.BaseValue);
        }

        public static Joule operator +(Joule a, Joule b)
        {
            return a.Add(b);
        }

        public static Joule operator -(Joule a, Joule b)
        {
            return a.Subtract(b);
        }

        public static Joule operator *(Joule a, double n)
        {
            return a.Multiply(n);
        }

        public static Joule operator /(Joule a, double n)
        {
            return a.Divide(n);
        }

        public static Ratio operator /(Joule a, Joule b)
        {
            return a.Divide(b);
        }

        public static Joule operator -(Joule a)
        {
            return a.Multiply(-1.0);
        }

        public override string ToString()
        {
            return this.FormatAuto();
        }
    }
}

