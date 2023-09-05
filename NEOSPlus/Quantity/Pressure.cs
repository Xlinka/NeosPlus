using QuantityX;

namespace NEOSPlus.Quantity
{
    public readonly struct Data : IQuantitySI<Data>, IQuantitySI, IQuantity<Data>, IQuantity, IComparable<Data>, IEquatable<Data>
    {
        public readonly double BaseValue;

        // Define pressure units
        public static readonly Unit<Data> Pascal = new UnitSI<Data>(1, "Pa", "pascal");
        public static readonly Unit<Data> Atmosphere = new Unit<Data>(101325, "atm", "atmosphere");
        public static readonly Unit<Data> Bar = new UnitSI<Data>(100000, "bar", "bar");
        public static readonly Unit<Data> Millibar = new UnitSI<Data>(100, "mbar", "millibar");
        public static readonly Unit<Data> Microbar = new UnitSI<Data>(0.1, "µbar", "microbar");
        public static readonly Unit<Data> Nanobar = new UnitSI<Data>(0.0001, "nbar", "nanobar");

        double IQuantity.BaseValue => BaseValue;

        public double SIPower => 1.0;

        // Default unit for pressure
        public Unit<Data> DefaultUnit => Pascal;

        public Data(double baseValue = 0.0)
        {
            this = default(Data);
            BaseValue = baseValue;
        }

        public bool Equals(Data other)
        {
            return BaseValue == other.BaseValue;
        }

        public int CompareTo(Data other)
        {
            return BaseValue.CompareTo(other.BaseValue);
        }

        public string[] GetShortBaseNames()
        {
            return new string[] { "Pa", "atm", "bar", "mbar", "µbar", "nbar" };
        }

        public string[] GetLongBaseNames()
        {
            return new string[] { "pascals", "atmospheres", "bars", "millibars", "microbars", "nanobars" };
        }

        public IUnit[] GetCommonSIUnits()
        {
            return new IUnit[]
            {
                SI<Data>.Pascal,
                SI<Data>.Atmosphere,
                SI<Data>.Bar,
                SI<Data>.Millibar,
                SI<Data>.Microbar,
                SI<Data>.Nanobar,
            };
        }

        public IUnit[] GetExcludedSIUnits()
        {
            return new IUnit[]
            {
                SI<Data>.Lumen,
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

        public Data New(double baseVal)
        {
            return new Data(baseVal);
        }

        public Data Add(Data q)
        {
            return new Data(BaseValue + q.BaseValue);
        }

        public Data Subtract(Data q)
        {
            return new Data(BaseValue - q.BaseValue);
        }

        public Data Multiply(double n)
        {
            return new Data(BaseValue * n);
        }

        public Data Multiply(Data a, Ratio r)
        {
            return a * r.BaseValue;
        }

        public Data Multiply(Ratio r, Data a)
        {
            return a * r.BaseValue;
        }

        public Data Divide(double n)
        {
            return new Data(BaseValue / n);
        }

        public Ratio Divide(Data q)
        {
            return new Ratio(BaseValue / q.BaseValue);
        }

        public static Data Parse(string str, Unit<Data> defaultUnit = null)
        {
            return Unit<Data>.Parse(str, defaultUnit);
        }

        public static bool TryParse(string str, out Data q, Unit<Data> defaultUnit = null)
        {
            return Unit<Data>.TryParse(str, out q, defaultUnit);
        }

        public static Data operator +(Data a, Data b)
        {
            return a.Add(b);
        }

        public static Data operator -(Data a, Data b)
        {
            return a.Subtract(b);
        }

        public static Data operator *(Data a, double n)
        {
            return a.Multiply(n);
        }

        public static Data operator /(Data a, double n)
        {
            return a.Divide(n);
        }

        public static Ratio operator /(Data a, Data b)
        {
            return a.Divide(b);
        }

        public static Data operator -(Data a)
        {
            return a.Multiply(-1.0);
        }

        public override string ToString()
        {
            return this.FormatAuto();
        }
    }
}
