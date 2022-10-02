using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseX;
using CloudX.Shared;
using FrooxEngine;

namespace NEOSPlus.Shaders
{
    internal class ShaderInjection
    {
        // Both of these are made by the wonderful and sleep deprived upside down man LeCloutPanda
        public static readonly Uri NeosPlusTest =
            new Uri("neosdb:///032da78c8a65dd71ca33cd8ed4ad7222682057b59f7afb3bbe582a42fdc0bbc9.neoshader");

        public static readonly Uri FragmentHologram =
            new Uri("neosdb:///d9e1620cc027cb009f4a994fe0e30136ed5dacd975c0f2fb562af55457b0a190.neoshader");

        private static readonly List<Uri> Shaders = new()
        {
            NeosPlusTest,
            FragmentHologram
        };
        private static async Task RegisterShader(Uri uri)
        {
            var signature = AssetUtil.ExtractSignature(uri);
            var shaderExists = await Engine.Current.LocalDB.ReadVariableAsync(signature, false);
            if (!shaderExists) await Engine.Current.LocalDB.WriteVariableAsync(signature, true);
        }

        // About time we got this working it took too long and too many hours of pain and suffering, well the first proper has been set up was the Hologram one at 3:29am on the 22/09/2022 and I am now tired good night -Panda
        // New note I think xLinka and I both clocked in a min of 13 hours each trying to get it to work but again thankfully we got it working -Panda

        public static void AppendShaders() => Task.WaitAll(Shaders.Select(shader => RegisterShader(shader)).ToArray());
    }
}