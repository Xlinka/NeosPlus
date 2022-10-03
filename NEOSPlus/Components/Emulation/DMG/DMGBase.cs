using FrooxEngine;
using NEOSPlus.Components.Emulation.DMG.Emulator;

//implementation is based on information from many different open source gameboy emulators, including
//https://github.com/BluestormDNA/ProjectDMG,
//https://github.com/GoldenCrystal/CrystalBoy
//https://github.com/kasp315b/GBEmulator
//https://github.com/LIJI32/SameBoy
//implementation is based on code from ProjectDMG

namespace FrooxEngine.Emulation.DMG;

public class DMG : Component
{
    public readonly SyncRef<User> SimulatingUser;
    
}