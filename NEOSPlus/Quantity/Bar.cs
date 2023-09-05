using QuantityX;

namespace NEOSPlus.Quantity
{
    public readonly struct Bar : IQuantitySI<Bar>, IQuantitySI, IQuantity<Bar>, IQuantity, IComparable<Bar>, IEquatable<Bar>
    {
        public readonly double BaseValue;

        // Define pressure units for Bar
        public static readonly Unit<Bar> Bar = new UnitSI<Bar>(1, "bar", "bar");
        public static readonly Unit<Bar> Millibar = new UnitSI<Bar>(0.001, "mbar", "millibar");
        public static readonly Unit<Bar> Microbar = new UnitSI<Bar>(0.000001, "µbar", "microbar");
        public static readonly Unit<Bar> Nanobar = new UnitSI<Bar>(0.000000001, "nbar", "nanobar");

        double IQuantity.BaseValue => BaseValue;

        public double SIPower => 1.0;

        // Default unit for Bar
        public Unit<Bar> DefaultUnit => Bar;

        public Bar(double baseValue = 0.0)
        {
            this = default(Bar);
            BaseValue = baseValue;
        }

        public bool Equals(Bar other)
        {
            return BaseValue == other.BaseValue;
        }

        public int CompareTo(Bar other)
        {
            return BaseValue.CompareTo(other.BaseValue);
        }

        public string[] GetShortBaseNames()
        {
            return new string[] { "bar", "mbar", "µbar", "nbar" };
        }

        public string[] GetLongBaseNames()
        {
            return new string[] { "bars", "millibars", "microbars", "nanobars" };
        }

        public IUnit[] GetCommonSIUnits()
        {
            return new IUnit[]
            {
                SI<Bar>.Bar,
                SI<Bar>.Millibar,
                SI<Bar>.Microbar,
                SI<Bar>.Nanobar,
            };
        }

        public IUnit[] GetExcludedSIUnits()
        {
            return new IUnit[]
            {
                SI<Data>.MillimetersOfMercury,  
                SI<Data>.Kilopascals,
                SI<Data>.PoundsPerSquareInch,
                SI<Data>.CentimetersOfWater
                SI<Data>.Lumen  
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

        public Bar New(double baseVal)
        {
            return new Bar(baseVal);
        }

        public Bar Add(Bar q)
        {
            return new Bar(BaseValue + q.BaseValue);
        }

        public Bar Subtract(Bar q)
        {
            return new Bar(BaseValue - q.BaseValue);
        }

        public Bar Multiply(double n)
        {
            return new Bar(BaseValue * n);
        }

        public Bar Multiply(Bar a, Ratio r)
        {
            return a * r.BaseValue;
        }

        public Bar Multiply(Ratio r, Bar a)
        {
            return a * r.BaseValue;
        }

        public Bar Divide(double n)
        {
            return new Bar(BaseValue / n);
        }

        public Ratio Divide(Bar q)
        {
            return new Ratio(BaseValue / q.BaseValue);
        }

        public static Bar Parse(string str, Unit<Bar> defaultUnit = null)
        {
            return Unit<Bar>.Parse(str, defaultUnit);
        }

        public static bool TryParse(string str, out Bar q, Unit<Bar> defaultUnit = null)
        {
            return Unit<Bar>.TryParse(str, out q, defaultUnit);
        }

        public static Bar operator +(Bar a, Bar b)
        {
            return a.Add(b);
        }

        public static Bar operator -(Bar a, Bar b)
        {
            return a.Subtract(b);
        }

        public static Bar operator *(Bar a, double n)
        {
            return a.Multiply(n);
        }

        public static Bar operator /(Bar a, double n)
        {
            return a.Divide(n);
        }

        public static Ratio operator /(Bar a, Bar b)
        {
            return a.Divide(b);
        }

        public static Bar operator -(Bar a)
        {
            return a.Multiply(-1.0);
        }

        public override string ToString()
        {
            return this.FormatAuto();
        }
    }
}
