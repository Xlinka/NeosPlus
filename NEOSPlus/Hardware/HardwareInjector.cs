using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseX;
using CloudX.Shared;
using FrooxEngine;


namespace NEOSPlus.Hardware
{
    internal class HardwareInjector
    {
        private static readonly List<Type> HardwareClasses = new()
        {
            //typeof(XboxOneController),
        };

        private static async Task RegisterHardwareClass(Type hardwareClass)
        {
            UniLog.Log($"Initializing hardware class: {hardwareClass.Name}");
            await Task.Delay(10); // Simulate a potential async operation for registering the hardware class
        }

        public static void InitializeHardwareClasses() => Task.WaitAll(HardwareClasses.Select(hardwareClass => RegisterHardwareClass(hardwareClass)).ToArray());
    }
}
