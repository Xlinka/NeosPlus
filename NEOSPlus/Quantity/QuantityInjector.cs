using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using BaseX;
using FrooxEngine;
using QuantityX;
//comments added by xlinka for my sanity so i could figure out what the hell is going on.
namespace NEOSPlus.Quantity
{
    internal static class QuantityInjector
    {
        // Injects the 'Data' type into the quantities array and updates the quantity cache
        internal static void Inject()
        {
            // Get the 'quantities' field using reflection from 'FrooxEngine.GenericTypes'
            var quantities =
                typeof(FrooxEngine.GenericTypes).GetField("quantities", BindingFlags.Static | BindingFlags.NonPublic);

            // Get all types in the 'NEOSPlus.Quantity' namespace that are value types
            var quantityTypes = typeof(QuantityInjector).Assembly.GetTypes()
                .Where(type => type.Namespace == "NEOSPlus.Quantity" && type.IsValueType);

            // Append all quantity types to the existing array of types
            var newArray = (quantities.GetValue(null) as Type[]).Concat(quantityTypes).ToArray();

            // Set the modified array back to the 'quantities' field
            quantities.SetValue(null, newArray);

            // Log the injected types
            foreach (var type in quantityTypes)
            {
                UniLog.Log($"Injected quantity type: {type.FullName}");
            }

            // Update the quantity cache
            UpdateQuantityCache();
        }

        // Updates the quantity cache with information about new quantity types
        internal static void UpdateQuantityCache()
        {
            // Get the 'unitCache' and 'unitNameCache' fields using reflection from 'QuantityX.QuantityX'
            var unitCache =
                typeof(QuantityX.QuantityX).GetField("unitCache", BindingFlags.Static | BindingFlags.NonPublic)
                    .GetValue(null) as Dictionary<Type, List<IUnit>>;

            var unitNameCache =
                typeof(QuantityX.QuantityX).GetField("unitNameCache", BindingFlags.Static | BindingFlags.NonPublic)
                    .GetValue(null) as Dictionary<Type, Dictionary<string, IUnit>>;

            // Get all types in the assembly containing the 'QuantityInjector' class
            Type[] types = typeof(QuantityInjector).Assembly.GetTypes();

            foreach (Type type in types)
            {
                // Check if the type is assignable to 'IQuantity' and is a value type
                if (!typeof(IQuantity).IsAssignableFrom(type) || !type.IsValueType)
                {
                    continue;
                }

                // Create an instance of the quantity type
                IQuantity quantity = (IQuantity)Activator.CreateInstance(type);

                // Create a list to store associated units
                List<IUnit> unitList = new List<IUnit>();
                unitCache.Add(type, unitList);

                bool isQuantitySI = false;

                // Get interfaces implemented by the quantity type
                Type[] interfaces = type.GetInterfaces();

                // Check if the quantity type implements a generic interface of 'IQuantitySI'
                foreach (Type interfaceType in interfaces)
                {
                    if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IQuantitySI<>))
                    {
                        isQuantitySI = true;
                        break;
                    }
                }

                BindingFlags bindingAttr = BindingFlags.Static | BindingFlags.Public;

                // Create a list to store fields
                List<FieldInfo[]> fieldLists = new List<FieldInfo[]> { type.GetFields(bindingAttr) };

                if (isQuantitySI)
                {
                    // If it's a quantitySI, add common SI units
                    IQuantitySI quantitySI = (IQuantitySI)quantity;
                    IUnit[] commonSIUnits = quantitySI.GetCommonSIUnits();

                    foreach (IUnit unit in commonSIUnits)
                    {
                        UnitGroup.Common.RegisterUnit(unit);
                        UnitGroup.CommonMetric.RegisterUnit(unit);
                    }

                    // Exclude specific SI units
                    commonSIUnits = quantitySI.GetExludedSIUnits();
                    foreach (IUnit unit2 in commonSIUnits)
                    {
                        UnitGroup.Metric.RemoveUnit(unit2);
                    }

                    Type siType = typeof(SI<>).MakeGenericType(type);
                    fieldLists.Add(siType.GetFields(bindingAttr));
                }

                foreach (FieldInfo[] fields in fieldLists)
                {
                    foreach (FieldInfo fieldInfo in fields)
                    {
                        if (typeof(IUnit).IsAssignableFrom(fieldInfo.FieldType))
                        {
                            // Get the unit and add it to the unit list
                            var unit = (IUnit)fieldInfo.GetValue(null);
                            unitList.Add(unit);
                        }
                    }
                }

                unitList.Sort();

                // Create a dictionary to store unit names
                Dictionary<string, IUnit> unitNameDictionary = new Dictionary<string, IUnit>();
                unitNameCache.Add(type, unitNameDictionary);

                foreach (IUnit unitItem in unitList)
                {
                    foreach (string unitName in unitItem.GetUnitNames())
                    {
                        unitNameDictionary.Add(unitName.Trim(), unitItem);
                    }
                }
            }
        }
    }
}
