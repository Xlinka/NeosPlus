namespace NEOSPlus.Components.Emulation.DMG.Emulator;

public interface IDMGMemoryController
{
    byte ReadByte(ushort addr);
    void WriteByte(ushort addr, byte value);
    ushort ReadWord(ushort addr);
    void WriteWord(ushort addr, ushort value);
}