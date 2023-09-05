using QuantityX;

namespace NEOSPlus.Quantity
{
    public readonly struct Data : IQuantitySI<Data>, IQuantitySI, IQuantity<Data>, IQuantity, IComparable<Data>, IEquatable<Data>
    {
        public readonly double BaseValue;

        // Define pressure units
        public static readonly Unit<Data> Pascal = new UnitSI<Data>(1, "Pa", "pascal");
        public static readonly Unit<Data> Hectopascal = new UnitSI<Data>(100, "hPa", "hectopascal");
        public static readonly Unit<Data> Kilopascal = new UnitSI<Data>(1000, "kPa", "kilopascal");
        public static readonly Unit<Data> Megapascal = new UnitSI<Data>(1000000, "MPa", "megapascal");
        public static readonly Unit<Data> Gigapascal = new UnitSI<Data>(1000000000, "GPa", "gigapascal");

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
            return new string[] { "Pa", "hPa", "kPa", "MPa", "GPa" };
        }

        public string[] GetLongBaseNames()
        {
            return new string[] { "pascals", "hectopascals", "kilopascals", "megapascals", "gigapascals" };
        }

        public IUnit[] GetCommonSIUnits()
        {
            return new IUnit[]
            {
                SI<Data>.Pascal,
                SI<Data>.Hectopascal,
                SI<Data>.Kilopascal,
                SI<Data>.Megapascal,
                SI<Data>.Gigapascal,
            };
        }

        public IUnit[] GetExcludedSIUnits()
        {
            return new IUnit[]
            {
                SI<Data>.Lumen,
                SI<Bar>.Bar,
                SI<Bar>.Millibar,
                SI<Bar>.Microbar,
                SI<Bar>.Nanobar,
                SI<Data>.Deca,
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
                SI<Data>.Mega,
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
