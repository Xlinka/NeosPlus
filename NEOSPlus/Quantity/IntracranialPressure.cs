using QuantityX;
using System;
//IF YOUR USING THIS VALUE YOUR A MEDICAL STUDENT OR WEIRD - Linka.
namespace NEOSPlus.Quantity
{
    public readonly struct IntracranialPressure : IQuantitySI<IntracranialPressure>, IQuantitySI, IQuantity<IntracranialPressure>, IQuantity, IComparable<IntracranialPressure>, IEquatable<IntracranialPressure>
    {
        public readonly double BaseValue;

        // Define the unit for Intracranial Pressure (ICP) as millimeters of mercury (mmHg)
        public static readonly Unit<IntracranialPressure> MillimetersOfMercury = new UnitSI<IntracranialPressure>(1, "mmHg", "millimeters of mercury");

        // Define the unit for ICP in kilopascals (kPa)
        public static readonly Unit<IntracranialPressure> Kilopascals = new UnitSI<IntracranialPressure>(0.133322, "kPa", "kilopascals");

        // Define the unit for ICP in pounds per square inch (psi)
        public static readonly Unit<IntracranialPressure> PoundsPerSquareInch = new UnitSI<IntracranialPressure>(0.0193368, "psi", "pounds per square inch");

        // Define the unit for ICP in centimeters of water (cmH2O)
        public static readonly Unit<IntracranialPressure> CentimetersOfWater = new UnitSI<IntracranialPressure>(13.5951, "cmH2O", "centimeters of water");

        double IQuantity.BaseValue => BaseValue;

        public double SIPower => 1.0;

        // Default unit for Intracranial Pressure
        public Unit<IntracranialPressure> DefaultUnit => MillimetersOfMercury;

        public IntracranialPressure(double baseValue = 0.0)
        {
            this = default(IntracranialPressure);
            BaseValue = baseValue;
        }

        public bool Equals(IntracranialPressure other)
        {
            return BaseValue == other.BaseValue;
        }

        public int CompareTo(IntracranialPressure other)
        {
            return BaseValue.CompareTo(other.BaseValue);
        }

        public string[] GetShortBaseNames()
        {
            return new string[] { "mmHg", "kPa", "psi", "cmH2O" };
        }

        public string[] GetLongBaseNames()
        {
            return new string[] { "millimeters of mercury", "kilopascals", "pounds per square inch", "centimeters of water" };
        }

        public IUnit[] GetCommonSIUnits()
        {
            return new IUnit[]
            {
                SI<Data>.MillimetersOfMercury,  // Include mmHg as a common unit for ICP
                SI<Data>.Kilopascals,
                SI<Data>.PoundsPerSquareInch,
                SI<Data>.CentimetersOfWater
            };
        }

        public IUnit[] GetExcludedSIUnits()
        {
            return new IUnit[]
            {
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
			    Byte,            };
        }

        public IntracranialPressure New(double baseVal)
        {
            return new IntracranialPressure(baseVal);
        }

        public IntracranialPressure Add(IntracranialPressure q)
        {
            return new IntracranialPressure(BaseValue + q.BaseValue);
        }

        public IntracranialPressure Subtract(IntracranialPressure q)
        {
            return new IntracranialPressure(BaseValue - q.BaseValue);
        }

        public IntracranialPressure Multiply(double n)
        {
            return new IntracranialPressure(BaseValue * n);
        }

        public IntracranialPressure Multiply(IntracranialPressure a, Ratio r)
        {
            return a * r.BaseValue;
        }

        public IntracranialPressure Multiply(Ratio r, IntracranialPressure a)
        {
            return a * r.BaseValue;
        }

        public IntracranialPressure Divide(double n)
        {
            return new IntracranialPressure(BaseValue / n);
        }

        public Ratio Divide(IntracranialPressure q)
        {
            return new Ratio(BaseValue / q.BaseValue);
        }

        public static IntracranialPressure Parse(string str, Unit<IntracranialPressure> defaultUnit = null)
        {
            return Unit<IntracranialPressure>.Parse(str, defaultUnit);
        }

        public static bool TryParse(string str, out IntracranialPressure q, Unit<IntracranialPressure> defaultUnit = null)
        {
            return Unit<IntracranialPressure>.TryParse(str, out q, defaultUnit);
        }

        public static IntracranialPressure operator +(IntracranialPressure a, IntracranialPressure b)
        {
            return a.Add(b);
        }

        public static IntracranialPressure operator -(IntracranialPressure a, IntracranialPressure b)
        {
            return a.Subtract(b);
        }

        public static IntracranialPressure operator *(IntracranialPressure a, double n)
        {
            return a.Multiply(n);
        }

        public static IntracranialPressure operator /(IntracranialPressure a, double n)
        {
            return a.Divide(n);
        }

        public static Ratio operator /(IntracranialPressure a, IntracranialPressure b)
        {
            return a.Divide(b);
        }

        public static IntracranialPressure operator -(IntracranialPressure a)
        {
            return a.Multiply(-1.0);
        }

        public override string ToString()
        {
            return this.FormatAuto();
        }

        // Conversion methods
        public double ToKilopascals()
        {
            return BaseValue * 0.133322;
        }

        public double ToPoundsPerSquareInch()
        {
            return BaseValue * 0.0193368;
        }

        public double ToCentimetersOfWater()
        {
            return BaseValue * 13.5951;
        }

    }
}
