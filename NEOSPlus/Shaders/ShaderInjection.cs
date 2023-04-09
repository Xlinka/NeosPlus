using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudX.Shared;
using FrooxEngine;

namespace NEOSPlus.Shaders
{
    internal class ShaderInjection
    {
        // Any shader marked with archived are still technically usable but I don't want a orb for them as they are most likely obselete in some or another
        public static readonly Uri Hologram_Archived = new("neosdb:///d9e1620cc027cb009f4a994fe0e30136ed5dacd975c0f2fb562af55457b0a190.neoshader");
        public static readonly Uri NeosPlusTest = new("neosdb:///032da78c8a65dd71ca33cd8ed4ad7222682057b59f7afb3bbe582a42fdc0bbc9.neoshader");
        public static readonly Uri HologramV2 = new("neosdb:///55cbadb64068521f187d408bed05542af1365d4c056b2570b4cc0d6105657902.neoshader");
        public static readonly Uri MToon = new("neosdb:///f9db0509b5413ae1449ca912aedb660aac028d29415c74a7767daf4fafa4c764.neoshader");
        public static readonly Uri ParallaxOcclusion = new("neosdb:///2539719620e32ca7d0cadd510c8ee088500cb76fc9cb46bb03d5aa586303e451.neoshader");
        public static readonly Uri UnlitDisplacement = new("neosdb:///96648419238f3085bfb854cd5f0368af9f8ce0698fac791d7ff54089b9c17189.neoshader");
        public static readonly Uri WireFrame = new("neosdb:///53fa593a8d1689640a11ca8c6c8f212f85dbe97d9d0e983b19389b422f9b35f0.neoshader");
        private static readonly List<Uri> Shaders = new()
        {
            WireFrame,
            UnlitDisplacement,
            Hologram_Archived,
            NeosPlusTest,
            HologramV2,
            MToon,
            ParallaxOcclusion
        };

        private static async Task RegisterShader(Uri uri)
        {
            var signature = AssetUtil.ExtractSignature(uri);
            var shaderExists = await Engine.Current.LocalDB.ReadVariableAsync(signature, false);
            if (!shaderExists) await Engine.Current.LocalDB.WriteVariableAsync(signature, true);
        }

        // About time we got this working it took too long and too many hours of pain and suffering, well the first proper has been set up was the Hologram one at 3:29am on the 22/09/2022 and I am now tired good night -Panda
        // New note I think xLinka and I both clocked in a min of 13 hours each trying to get it to work but again thankfully we got it working -Panda
        // More hours are being poured into the bloody thing working cause they are hit or miss
        // So it turns out asset servers are realy fiddly to work with
        // More about the servers, I've had lows of 5 minutes upon uploading to have stuff work then up to multiple days to work :)

        public static void AppendShaders() => Task.WaitAll(Shaders.Select(shader => RegisterShader(shader)).ToArray());
    }
}