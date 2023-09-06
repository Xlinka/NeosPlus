using System;
using QuantityX;

namespace NEOSPlus.Quantity
{
    public readonly struct Capacitance : IQuantitySI<Capacitance>, IQuantitySI, IQuantity<Capacitance>, IQuantity, IComparable<Capacitance>, IEquatable<Capacitance>
    {
        public readonly double BaseValue;

        public static readonly Unit<Capacitance> Farad = new UnitSI<Capacitance>(0, "F", "farad");

        double IQuantity.BaseValue => BaseValue;

        public double SIPower => 1.0;

        public Unit<Capacitance> DefaultUnit => Farad;

        public Capacitance(double baseValue = 0.0)
        {
            this = default(Capacitance);
            BaseValue = baseValue;
        }

        public bool Equals(Capacitance other)
        {
            return BaseValue == other.BaseValue;
        }

        public int CompareTo(Capacitance other)
        {
            return BaseValue.CompareTo(other.BaseValue);
        }

        public string[] GetShortBaseNames()
        {
            return new string[] { "F" };
        }

        public string[] GetLongBaseNames()
        {
            return new string[] { "farads", "farad" };
        }

        public IUnit[] GetCommonSIUnits()
        {
            return new IUnit[]
            {
                SI<Capacitance>.Pico,
                SI<Capacitance>.Nano,
                SI<Capacitance>.Micro,
                SI<Capacitance>.Milli,
                Farad,
                SI<Capacitance>.Kilo,
                SI<Capacitance>.Mega,
                SI<Capacitance>.Giga
            };
        }

        public IUnit[] GetExludedSIUnits()
        {
            return new IUnit[]
            {
                SI<Capacitance>.Yotta,
                SI<Capacitance>.Zetta,
                SI<Capacitance>.Exa,
                SI<Capacitance>.Peta,
                SI<Capacitance>.Tera,
                SI<Capacitance>.Deca,
                SI<Capacitance>.Hecto,
                SI<Capacitance>.Centi,
                SI<Capacitance>.Deci,
                SI<Capacitance>.Yocto,
                SI<Capacitance>.Zepto,
                SI<Capacitance>.Atto,
                SI<Capacitance>.Femto
            };
        }

        public Capacitance New(double baseVal)
        {
            return new Capacitance(baseVal);
        }

        public Capacitance Add(Capacitance q)
        {
            return new Capacitance(BaseValue + q.BaseValue);
        }

        public Capacitance Subtract(Capacitance q)
        {
            return new Capacitance(BaseValue - q.BaseValue);
        }

        public Capacitance Multiply(double n)
        {
            return new Capacitance(BaseValue * n);
        }

        public Capacitance Divide(double n)
        {
            return new Capacitance(BaseValue / n);
        }

        public Ratio Divide(Capacitance q)
        {
            return new Ratio(BaseValue / q.BaseValue);
        }

        public static Capacitance operator +(Capacitance a, Capacitance b)
        {
            return a.Add(b);
        }

        public static Capacitance operator -(Capacitance a, Capacitance b)
        {
            return a.Subtract(b);
        }

        public static Capacitance operator *(Capacitance a, double n)
        {
            return a.Multiply(n);
        }

        public static Capacitance operator /(Capacitance a, double n)
        {
            return a.Divide(n);
        }

        public static Ratio operator /(Capacitance a, Capacitance b)
        {
            return a.Divide(b);
        }

        public static Capacitance operator -(Capacitance a)
        {
            return a.Multiply(-1.0);
        }

        public override string ToString()
        {
            return this.FormatAuto();
        }
    }
}

