using System.Runtime.CompilerServices;
using BaseX;
using Buttplug;
using FrooxEngine;
using FrooxEngine.LogiX;

namespace NEOSPlus.Hardware
{
    public static class ButtplugIntegration
    {
        // Create the ButtplugEmbeddedConnectorOptions instance here
        public static ButtplugEmbeddedConnectorOptions connector = new ButtplugEmbeddedConnectorOptions();
        
        public static void InitializeButtplugConnector()
        {
            UniLog.Log("Initializing ButtplugEmbeddedConnectorOptions...");
            // You can perform any additional setup for the ButtplugEmbeddedConnectorOptions here, if needed.
        }
    }
}
