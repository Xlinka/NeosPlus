using System;
using System.Collections.Generic;
using System.Linq;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Collections;
using FrooxEngine.UIX;

namespace NEOSPlus
{
    public static class NodeExtensions
    {
        public static void GenerateListButtons(this UIBuilder ui, ButtonEventHandler plus, ButtonEventHandler minus)
        {
            var uIBuilder = ui;
            uIBuilder.Panel();
            uIBuilder.HorizontalFooter(32f, out var footer, out var _);
            var uIBuilder2 = new UIBuilder(footer);
            uIBuilder2.HorizontalLayout(4f);
            LocaleString text = "+";
            var tint = color.White;
            uIBuilder2.Button(in text, in tint, plus);
            text = "-";
            tint = color.White;
            uIBuilder2.Button(in text, in tint, minus);
        }
        public static Type CollectionsOverload(NodeTypes connectingTypes, string inputName, Type genericTypeDefinition,
            Type makeType)
        {
            var input = connectingTypes.inputs[inputName];
            if (input == null) return null;
            var enumerableGeneric =
                input.GetInterfaces().FirstOrDefault(i =>
                        i.IsGenericType && i.GetGenericTypeDefinition() == genericTypeDefinition)
                    ?.GetGenericArguments()[0];
            return enumerableGeneric == null ? null : makeType.MakeGenericType(enumerableGeneric, input);
        }
        public static Type CollectionsSyncOverload(NodeTypes connectingTypes, string inputName, Type genericTypeDefinition, Type makeType, Type syncMakeType)
        {
            var input = connectingTypes.inputs[inputName];
            if (input == null) return null;
            if (CollectionsHelperList.HelperMapping.ContainsKey(input.GetGenericTypeDefinition()))
            {
                var enumerableGeneric = input.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)).GenericTypeArguments[0];
                return enumerableGeneric == null ? null : syncMakeType.MakeGenericType(enumerableGeneric, input);
            }
            else
            {
                var enumerableGeneric =
                    input.GetInterfaces().FirstOrDefault(i =>
                            i.IsGenericType && i.GetGenericTypeDefinition() == genericTypeDefinition)
                        ?.GetGenericArguments()[0];
                return enumerableGeneric == null ? null : makeType.MakeGenericType(enumerableGeneric, input);
            }
        }
    }
}