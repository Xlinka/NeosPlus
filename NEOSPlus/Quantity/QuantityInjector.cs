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
                typeof(GenericTypes).GetField("quantities", BindingFlags.Static | BindingFlags.NonPublic);

            // Get all types in the 'NEOSPlus.Quantity' namespace that are value types
            var quantityTypes = typeof(QuantityInjector).Assembly.GetTypes()
                .Where(type => type.Namespace == "NEOSPlus.Quantity" && type.IsValueType).ToArray();

            // Append all quantity types to the existing array of types
            var newArray = (quantities.GetValue(null) as Type[]).Concat(quantityTypes).ToArray();

            // Set the modified array back to the 'quantities' field
            quantities.SetValue(null, newArray);

            // Log the injected types
            foreach (var type in quantityTypes) UniLog.Log($"Injected quantity type: {type.FullName}");

            // Update the quantity cache
            UpdateQuantityCache();
        }

        // Updates the quantity cache with information about new quantity types
        internal static void UpdateQuantityCache()
        {
            // Get the 'unitCache' and 'unitNameCache' fields using reflection from 'QuantityX.QuantityX'
            var unitCache =
                typeof(QuantityX.QuantityX).GetField("unitCache", BindingFlags.Static | BindingFlags.NonPublic)!
                    .GetValue(null) as Dictionary<Type, List<IUnit>>;

            var unitNameCache =
                typeof(QuantityX.QuantityX).GetField("unitNameCache", BindingFlags.Static | BindingFlags.NonPublic)!
                    .GetValue(null) as Dictionary<Type, Dictionary<string, IUnit>>;

            // Get all types in the assembly containing the 'QuantityInjector' class
            var types = typeof(QuantityInjector).Assembly.GetTypes();

            foreach (var type in types)
            {
                // Check if the type is assignable to 'IQuantity' and is a value type
                if (!typeof(IQuantity).IsAssignableFrom(type) || !type.IsValueType)
                    continue;

                // Create an instance of the quantity type
                var quantity = (IQuantity)Activator.CreateInstance(type);

                // Create a list to store associated units
                var unitList = new List<IUnit>();
                unitCache!.Add(type, unitList);

                // Get interfaces implemented by the quantity type
                var interfaces = type.GetInterfaces();

                // Check if the quantity type implements a generic interface of 'IQuantitySI'
                var isQuantitySi = interfaces.Any(interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IQuantitySI<>));

                const BindingFlags bindingAttr = BindingFlags.Static | BindingFlags.Public;

                // Create a list to store fields
                var fieldLists = new List<FieldInfo[]> { type.GetFields(bindingAttr) };

                if (isQuantitySi)
                {
                    // If it's a quantitySI, add common SI units
                    var quantitySi = (IQuantitySI)quantity;
                    var commonSiUnits = quantitySi.GetCommonSIUnits();

                    foreach (var unit in commonSiUnits)
                    {
                        UnitGroup.Common.RegisterUnit(unit);
                        UnitGroup.CommonMetric.RegisterUnit(unit);
                    }

                    // Exclude specific SI units
                    commonSiUnits = quantitySi.GetExludedSIUnits();
                    foreach (var unit2 in commonSiUnits) UnitGroup.Metric.RemoveUnit(unit2);

                    var siType = typeof(SI<>).MakeGenericType(type);
                    fieldLists.Add(siType.GetFields(bindingAttr));
                }

                unitList.AddRange(from fields in fieldLists
                    from fieldInfo in fields
                    where typeof(IUnit).IsAssignableFrom(fieldInfo.FieldType)
                    select (IUnit) fieldInfo.GetValue(null));

                unitList.Sort();

                // Create a dictionary to store unit names
                var unitNameDictionary = new Dictionary<string, IUnit>();
                unitNameCache!.Add(type, unitNameDictionary);

                foreach (var unitItem in unitList)
                {
                    foreach (var unitName in unitItem.GetUnitNames())
                    {
                        unitNameDictionary.Add(unitName.Trim(), unitItem);
                    }
                }
            }
        }
    }
}
