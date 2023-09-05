using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using BaseX;
using FrooxEngine;
using QuantityX;

namespace NEOSPlus.Quantity;

internal static class QuantityInjector
{
    internal static void Inject()
    {
        var quantities =
            typeof(FrooxEngine.GenericTypes).GetField("quantities", BindingFlags.Static | BindingFlags.NonPublic);
        var newArray = (quantities.GetValue(null) as Type[]).Append(typeof(Data)).ToArray();
        quantities.SetValue(null, newArray);
        
        UpdateQuantityCache();
    }

    internal static void UpdateQuantityCache()
    {
        var unitCache = 
            typeof(QuantityX.QuantityX).GetField("unitCache", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<Type, List<IUnit>>;
        var unitNameCache =
            typeof(QuantityX.QuantityX).GetField("unitNameCache", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<Type, Dictionary<string, IUnit>>;
        Type[] types = typeof(QuantityInjector).Assembly.GetTypes();
        
        foreach (Type type in types)
        {
            if (!typeof(IQuantity).IsAssignableFrom(type) || !type.IsValueType)
            {
                continue;
            }
            IQuantity quantity = (IQuantity)Activator.CreateInstance(type);
            List<IUnit> list = new List<IUnit>();
            unitCache.Add(type, list);
            bool flag = false;
            Type[] interfaces = type.GetInterfaces();
            foreach (Type type2 in interfaces)
            {
                if (type2.IsGenericType && type2.GetGenericTypeDefinition() == typeof(IQuantitySI<>))
                {
                    flag = true;
                    break;
                }
            }
            BindingFlags bindingAttr = BindingFlags.Static | BindingFlags.Public;
            List<FieldInfo[]> list2 = new List<FieldInfo[]> { type.GetFields(bindingAttr) };
            if (flag)
            {
                IQuantitySI quantitySI = (IQuantitySI)quantity;
                IUnit[] commonSIUnits = quantitySI.GetCommonSIUnits();
                foreach (IUnit unit in commonSIUnits)
                {
                    UnitGroup.Common.RegisterUnit(unit);
                    UnitGroup.CommonMetric.RegisterUnit(unit);
                }
                commonSIUnits = quantitySI.GetExludedSIUnits();
                foreach (IUnit unit2 in commonSIUnits)
                {
                    UnitGroup.Metric.RemoveUnit(unit2);
                }
                Type type3 = typeof(SI<>).MakeGenericType(type);
                list2.Add(type3.GetFields(bindingAttr));
            }
            foreach (FieldInfo[] item in list2)
            {
                foreach (FieldInfo fieldInfo in item)
                {
                    if (typeof(IUnit).IsAssignableFrom(fieldInfo.FieldType))
                    {
                        var unit = (IUnit)fieldInfo.GetValue(null);
                        list.Add(unit);
                    }
                }
            }
            list.Sort();
            Dictionary<string, IUnit> dictionary = new Dictionary<string, IUnit>();
            unitNameCache.Add(type, dictionary);
            foreach (IUnit item2 in list)
            {
                foreach (string unitName in item2.GetUnitNames())
                {
                    dictionary.Add(unitName.Trim(), item2);
                }
            }
        }
    }
}