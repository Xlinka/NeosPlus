using QuantityX;

namespace NEOSPlus.Quantity
{
    public readonly struct Resistance : IQuantitySI<Resistance>, IQuantitySI, IQuantity<Resistance>, IQuantity, IComparable<Resistance>, IEquatable<Resistance>
    {
        public readonly double BaseValue;

        // Define resistance units (ohms)
        public static readonly Unit<Resistance> Ohm = new UnitSI<Resistance>(1, "Ω", "ohm");

        double IQuantity.BaseValue => BaseValue;

        public double SIPower => 1.0;

        // Default unit for resistance
        public Unit<Resistance> DefaultUnit => Ohm;

        public Resistance(double baseValue = 0.0)
        {
            this = default(Resistance);
            BaseValue = baseValue;
        }

        public bool Equals(Resistance other)
        {
            return BaseValue == other.BaseValue;
        }

        public int CompareTo(Resistance other)
        {
            return BaseValue.CompareTo(other.BaseValue);
        }

        public string[] GetShortBaseNames()
        {
            return new string[] { "Ω", "ohm" };
        }

        public string[] GetLongBaseNames()
        {
            return new string[] { "ohms" };
        }

        public IUnit[] GetCommonSIUnits()
        {
            return new IUnit[]
            {
               SI<Data>.Ohm
            };
        }

        public IUnit[] GetExcludedSIUnits()
        {
            return new IUnit[]
            {
                SI<Data>.Pascal,
			    SI<Data>.Atmosphere,
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

        public Resistance New(double baseVal)
        {
            return new Resistance(baseVal);
        }

        public Resistance Add(Resistance q)
        {
            return new Resistance(BaseValue + q.BaseValue);
        }

        public Resistance Subtract(Resistance q)
        {
            return new Resistance(BaseValue - q.BaseValue);
        }

        public Resistance Multiply(double n)
        {
            return new Resistance(BaseValue * n);
        }

        public Resistance Multiply(Resistance a, Ratio r)
        {
            return a * r.BaseValue;
        }

        public Resistance Multiply(Ratio r, Resistance a)
        {
            return a * r.BaseValue;
        }

        public Resistance Divide(double n)
        {
            return new Resistance(BaseValue / n);
        }

        public Ratio Divide(Resistance q)
        {
            return new Ratio(BaseValue / q.BaseValue);
        }

        public static Resistance Parse(string str, Unit<Resistance> defaultUnit = null)
        {
            return Unit<Resistance>.Parse(str, defaultUnit);
        }

        public static bool TryParse(string str, out Resistance q, Unit<Resistance> defaultUnit = null)
        {
            return Unit<Resistance>.TryParse(str, out q, defaultUnit);
        }

        public static Resistance operator +(Resistance a, Resistance b)
        {
            return a.Add(b);
        }

        public static Resistance operator -(Resistance a, Resistance b)
        {
            return a.Subtract(b);
        }

        public static Resistance operator *(Resistance a, double n)
        {
            return a.Multiply(n);
        }

        public static Resistance operator /(Resistance a, double n)
        {
            return a.Divide(n);
        }

        public static Ratio operator /(Resistance a, Resistance b)
        {
            return a.Divide(b);
        }

        public static Resistance operator -(Resistance a)
        {
            return a.Multiply(-1.0);
        }

        public override string ToString()
        {
            return this.FormatAuto();
        }
    }
}
