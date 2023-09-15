using System;
using QuantityX;

namespace NEOSPlus.Quantity
{
    public readonly struct MagneticField : IQuantitySI<MagneticField>, IQuantitySI, IQuantity<MagneticField>, IQuantity, IComparable<MagneticField>, IEquatable<MagneticField>
    {
        public readonly double BaseValue;

        public static readonly Unit<MagneticField> Gauss = new UnitSI<MagneticField>(0, "", "");
        public static readonly Unit<MagneticField> Tesla = new Unit<MagneticField>(1e4, new UnitGroup[1] { UnitGroup.Common }, new string[1] { "T" }, new string[1] { " tesla" });

        double IQuantity.BaseValue => BaseValue;

        public double SIPower => 1.0;

        public Unit<MagneticField> DefaultUnit => Gauss;

        public MagneticField(double baseValue = 0.0)
        {
            this = default(MagneticField);
            BaseValue = baseValue;
        }

        public bool Equals(MagneticField other)
        {
            return BaseValue == other.BaseValue;
        }

        public int CompareTo(MagneticField other)
        {
            return BaseValue.CompareTo(other.BaseValue);
        }

        public string[] GetShortBaseNames()
        {
            return new string[] { "G" };
        }

        public string[] GetLongBaseNames()
        {
            return new string[] { "gauss" };
        }

        public IUnit[] GetCommonSIUnits()
        {
            return new IUnit[]
            {
                Gauss,
                Tesla
            };
        }

        public IUnit[] GetExludedSIUnits()
        {
            return new IUnit[]
            {
            };
        }

        public MagneticField New(double baseVal)
        {
            return new MagneticField(baseVal);
        }

        public MagneticField Add(MagneticField q)
        {
            return new MagneticField(BaseValue + q.BaseValue);
        }

        public MagneticField Subtract(MagneticField q)
        {
            return new MagneticField(BaseValue - q.BaseValue);
        }

        public MagneticField Multiply(double n)
        {
            return new MagneticField(BaseValue * n);
        }

        public MagneticField Divide(double n)
        {
            return new MagneticField(BaseValue / n);
        }

        public Ratio Divide(MagneticField q)
        {
            return new Ratio(BaseValue / q.BaseValue);
        }

        public static MagneticField Parse(string str, Unit<MagneticField> defaultUnit = null)
        {
            return Unit<MagneticField>.Parse(str, defaultUnit);
        }

        public static bool TryParse(string str, out MagneticField q, Unit<MagneticField> defaultUnit = null)
        {
            return Unit<MagneticField>.TryParse(str, out q, defaultUnit);
        }

        public static MagneticField operator +(MagneticField a, MagneticField b)
        {
            return a.Add(b);
        }

        public static MagneticField operator -(MagneticField a, MagneticField b)
        {
            return a.Subtract(b);
        }

        public static MagneticField operator *(MagneticField a, double n)
        {
            return a.Multiply(n);
        }

        public static MagneticField operator /(MagneticField a, double n)
        {
            return a.Divide(n);
        }

        public static Ratio operator /(MagneticField a, MagneticField b)
        {
            return a.Divide(b);
        }

        public static MagneticField operator -(MagneticField a)
        {
            return a.Multiply(-1.0);
        }

        public override string ToString()
        {
            return this.FormatAuto();
        }
    }
}
