using QuantityX;

namespace NEOSPlus.Quantity
{
    public readonly struct LuminousFlux : IQuantitySI<LuminousFlux>, IQuantitySI, IQuantity<LuminousFlux>, IQuantity, IComparable<LuminousFlux>, IEquatable<LuminousFlux>
    {
        public readonly double BaseValue;

        // Define luminous flux unit (lumen)
        public static readonly Unit<LuminousFlux> Lumen = new UnitSI<LuminousFlux>(1, "lm", "lumen");

        double IQuantity.BaseValue => BaseValue;

        public double SIPower => 1.0;

        // Default unit for luminous flux
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
            return new string[] { "lm", "lumen" };
        }

        public string[] GetLongBaseNames()
        {
            return new string[] { "lumens" };
        }

        public IUnit[] GetCommonSIUnits()
        {
            return new IUnit[]
            {
                Lumen  // Include lumens as a common unit for luminous flux
            };
        }

        public IUnit[] GetExcludedSIUnits()
        {
            return new IUnit[]
            {
                SI<Data>.Deca,
                SI<Data>.Hecto,
                SI<Data>.Milli,
                SI<Data>.Centi,
                SI<Data>.Deci,
                SI<Data>.Yocto,
                SI<Data>.Zepto,
                SI<Data>.Atto,
                SI<Data>.Femto,
                SI<Data>.Pico,
                SI<Data>.Nano,
                SI<Data>.Micro,
                SI<Data>.Yotta,
                SI<Data>.Zetta,
                SI<Data>.Exa,
                SI<Data>.Peta,
                SI<Data>.Tera,
                SI<Data>.Giga,
                SI<Data>.Mega,
                SI<Data>.Kilo,
			    Byte,
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

        public LuminousFlux Multiply(LuminousFlux a, Ratio r)
        {
            return a * r.BaseValue;
        }

        public LuminousFlux Multiply(Ratio r, LuminousFlux a)
        {
            return a * r.BaseValue;
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
