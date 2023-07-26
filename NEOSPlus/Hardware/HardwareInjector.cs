using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseX;
using FrooxEngine;
using NEOSPlus.Hardware;

namespace NEOSPlus.Hardware
{
    internal class HardwareInjector
    {
        private static readonly List<Type> HardwareClasses = new()
        {
            typeof(ButtplugIntegration),
            // Add other hardware classes as needed
        };

        private static async Task RegisterHardwareClass(Type hardwareClass)
        {
            UniLog.Log($"Initializing hardware class: {hardwareClass.Name}");

            // Check the type of the hardwareClass and call its initialization method accordingly
            if (hardwareClass == typeof(ButtplugIntegration))
            {
                ButtplugIntegration.InitializeButtplugConnector();
            }

            await Task.Delay(10); // Simulate a potential async operation for registering the hardware class
        }

        public static void InitializeHardwareClasses() => Task.WaitAll(HardwareClasses.Select(hardwareClass => RegisterHardwareClass(hardwareClass)).ToArray());
    }
}
