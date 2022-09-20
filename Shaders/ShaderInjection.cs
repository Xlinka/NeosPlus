using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseX;
using CloudX.Shared;
using FrooxEngine;

namespace NEOSPlus.Shaders
{
    internal class ShaderInjection
    {
        public static readonly Uri FragmentHologram =
            new Uri("neosdb:///af2c3da741e0b1913dd145978731462ea9ff2da3eb8ff3283872adcc82e27b66.neoshader");

        private static async Task RegisterShader(Uri uri)
        {
            string signature = AssetUtil.ExtractSignature(uri);
            var shaderExists = await Engine.Current.LocalDB.ReadVariableAsync(signature, false);
            if (!shaderExists) await Engine.Current.LocalDB.WriteVariableAsync(signature, true);
        }

        public static void AppendShaders()
        {
            List<Uri> shaders = new List<Uri>();
            shaders.Add(FragmentHologram);
            UniLog.Log($"Added {FragmentHologram} to the LocalDB");

            List<Task> tasks = new List<Task>();

            foreach (var shader in shaders)
            {
                tasks.Add(RegisterShader(shader));
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}