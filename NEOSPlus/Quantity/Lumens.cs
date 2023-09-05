using System;
using QuantityX;

namespace NEOSPlus.Quantity
{
    public readonly struct LuminousFlux : IQuantitySI<LuminousFlux>, IQuantitySI, IQuantity<LuminousFlux>, IQuantity, IComparable<LuminousFlux>, IEquatable<LuminousFlux>
    {
        public readonly double BaseValue;

        public static readonly Unit<LuminousFlux> Lumen = new UnitSI<LuminousFlux>(0, "", "");

        double IQuantity.BaseValue => BaseValue;

        public double SIPower => 1.0;

        public Unit<LuminousFlux> DefaultUnit => Lumen;

        public LuminousFlux(double baseValue = 0.0)
        {
            this = default(LuminousFlux);
            BaseValue = baseValue;
        }

        public bool Equals(LuminousFlux other)
        {
            return BaseValue == other.BaseValue;
        }

        public int CompareTo(LuminousFlux other)
        {
            return BaseValue.CompareTo(other.BaseValue);
        }

        public string[] GetShortBaseNames()
        {
            return new string[] { "lm" };
        }

        public string[] GetLongBaseNames()
        {
            return new string[] { "lumens", "lumen" };
        }

        public IUnit[] GetCommonSIUnits()
        {
            return new IUnit[]
            {
                SI<LuminousFlux>.Mega,
                SI<LuminousFlux>.Kilo,
                Lumen,
                SI<LuminousFlux>.Milli,
                SI<LuminousFlux>.Micro
            };
        }


        public IUnit[] GetExludedSIUnits()
        {
            return new IUnit[]
            {
                SI<LuminousFlux>.Yotta,
                SI<LuminousFlux>.Zetta,
                SI<LuminousFlux>.Exa,
                SI<LuminousFlux>.Peta,
                SI<LuminousFlux>.Tera,
                SI<LuminousFlux>.Giga,
                SI<LuminousFlux>.Deca,
                SI<LuminousFlux>.Hecto,
                SI<LuminousFlux>.Centi,
                SI<LuminousFlux>.Deci,
                SI<LuminousFlux>.Nano,
                SI<LuminousFlux>.Pico,
                SI<LuminousFlux>.Femto,
                SI<LuminousFlux>.Atto,
                SI<LuminousFlux>.Zepto,
                SI<LuminousFlux>.Yocto
            };
        }


        public LuminousFlux New(double baseVal)
        {
            return new LuminousFlux(baseVal);
        }

        public LuminousFlux Add(LuminousFlux q)
        {
            return new LuminousFlux(BaseValue + q.BaseValue);
        }

        public LuminousFlux Subtract(LuminousFlux q)
        {
            return new LuminousFlux(BaseValue - q.BaseValue);
        }

        public LuminousFlux Multiply(double n)
        {
            return new LuminousFlux(BaseValue * n);
        }

        public LuminousFlux Divide(double n)
        {
            return new LuminousFlux(BaseValue / n);
        }

        public Ratio Divide(LuminousFlux q)
        {
            return new Ratio(BaseValue / q.BaseValue);
        }

        public static LuminousFlux Parse(string str, Unit<LuminousFlux> defaultUnit = null)
        {
            return Unit<LuminousFlux>.Parse(str, defaultUnit);
        }

        public static bool TryParse(string str, out LuminousFlux q, Unit<LuminousFlux> defaultUnit = null)
        {
            return Unit<LuminousFlux>.TryParse(str, out q, defaultUnit);
        }

        public static LuminousFlux operator +(LuminousFlux a, LuminousFlux b)
        {
            return a.Add(b);
        }

        public static LuminousFlux operator -(LuminousFlux a, LuminousFlux b)
        {
            return a.Subtract(b);
        }

        public static LuminousFlux operator *(LuminousFlux a, double n)
        {
            return a.Multiply(n);
        }

        public static LuminousFlux operator /(LuminousFlux a, double n)
        {
            return a.Divide(n);
        }

        public static Ratio operator /(LuminousFlux a, LuminousFlux b)
        {
            return a.Divide(b);
        }

        public static LuminousFlux operator -(LuminousFlux a)
        {
            return a.Multiply(-1.0);
        }

        public override string ToString()
        {
            return this.FormatAuto();
        }
    }
}
