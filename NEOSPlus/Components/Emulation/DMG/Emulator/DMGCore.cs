using System;

namespace NEOSPlus.Components.Emulation.DMG.Emulator;

//this part is very much based on ProjectDMG

internal enum DMGRegisters
{
    AF,
    BC,
    DE,
    HL
}

public partial class DMGCore
{
    public IDMGMemoryController Memory;
    private ushort PC;
    private ushort SP;
    private byte A, B, C, D, E, F, H, L;

    private ushort AF
    {
        get => (ushort) (A << 8 | F);
        set
        {
            A = (byte) (value >> 8);
            F = (byte) (value & 0xF0);
        }
    }

    private ushort BC
    {
        get => (ushort) (B << 8 | C);
        set
        {
            B = (byte) (value >> 8);
            C = (byte) value;
        }
    }

    private ushort DE
    {
        get => (ushort) (D << 8 | E);
        set
        {
            D = (byte) (value >> 8);
            E = (byte) value;
        }
    }

    private ushort HL
    {
        get => (ushort) (H << 8 | L);
        set
        {
            H = (byte) (value >> 8);
            L = (byte) value;
        }
    }

    private void SetRegister(DMGRegisters register, ushort value)
    {
        switch (register)
        {
            case DMGRegisters.AF:
                AF = value;
                break;
            case DMGRegisters.BC:
                BC = value;
                break;
            case DMGRegisters.DE:
                DE = value;
                break;
            case DMGRegisters.HL:
                HL = value;
                break;
        }
    }

    private ushort GetRegister(DMGRegisters register) =>
        register switch
        {
            DMGRegisters.AF => AF,
            DMGRegisters.BC => BC,
            DMGRegisters.DE => DE,
            DMGRegisters.HL => HL,
        };

    private bool FlagZ
    {
        get => (F & 0x80) != 0;
        set => F = value ? (byte) (F | 0x80) : (byte) (F & ~0x80);
    }

    private bool FlagN
    {
        get => (F & 0x40) != 0;
        set => F = value ? (byte) (F | 0x40) : (byte) (F & ~0x40);
    }

    private bool FlagH
    {
        get => (F & 0x20) != 0;
        set => F = value ? (byte) (F | 0x20) : (byte) (F & ~0x20);
    }

    private bool FlagC
    {
        get => (F & 0x10) != 0;
        set => F = value ? (byte) (F | 0x10) : (byte) (F & ~0x10);
    }

    private bool _ime;
    private bool _imeEnabler;
    private bool _halted;
    private bool _haltBug;
    private int _cycleCounter;

    public void Tick()
    {
        var op = Memory.ReadByte(PC++);
        if (_haltBug)
        {
            PC--;
            _haltBug = false;
        }
        _cycleCounter = 0;
        DoOpcode(op);
    }
    //use code generation so my fingers stop hurting
    partial void DoOpcode(byte op);
    private void Nop()
    {
    }

    private void LD(out byte a, byte b)
    {
        a = b;
    }

    private void LD(out byte a, ushort b)
    {
        a = Memory.ReadByte(b);
        PC++;
    }

    private void LD(ushort a, byte b)
    {
        Memory.WriteByte(a, b);
    }

    private void LD(DMGRegisters a, ushort b)
    {
        SetRegister(a, Memory.ReadWord(b));
        PC += 2;
    }

    private void RotateFeedCarry(byte magic)
    {
        F = 0;
        FlagC = (A & magic) != 0;
        A = (byte) ((A << 1) | (A >> 7));
    }

    private void Rotate(byte magic1, byte magic2)
    {
        var prevC = FlagC;
        F = 0;
        FlagC = (A & magic1) != 0;
        A = (byte)((A << 1) | (prevC ? magic2 : 0));
    }
    
    //these are directly taken from ProjectDMG and cleaned up
    private void INC(ref byte a)
    {
        a++;
        SetFlagZ(a);
        FlagN = false;
        SetFlagH(a, 1);
    }
    private void DEC(ref byte a)
    {
        a--;
        SetFlagZ(a);
        FlagN = true;
        SetFlagHSub(a, 1);
    }
    private void ADD(byte b) 
    {
        var result = A + b;
        SetFlagZ(result);
        FlagN = false;
        SetFlagH(A, b);
        SetFlagC(result);
        A = (byte)result;
    }
    private void ADC(byte b) 
    {
        var carry = FlagC ? 1 : 0;
        var result = A + b + carry;
        SetFlagZ(result);
        FlagN = false;
        if (FlagC) SetFlagHCarry(A, b);
        else SetFlagH(A, b);
        SetFlagC(result);
        A = (byte)result;
    }
    private void SUB(byte b) 
    {
        var result = A - b;
        SetFlagZ(result);
        FlagN = true;
        SetFlagHSub(A, b);
        SetFlagC(result);
        A = (byte)result;
    }
    private void SBC(byte b) 
    {
        var carry = FlagC ? 1 : 0;
        var result = A - b - carry;
        SetFlagZ(result);
        FlagN = true;
        if (FlagC) SetFlagHSubCarry(A, b);
        else SetFlagHSub(A, b);
        SetFlagC(result);
        A = (byte)result;
    }
    private void AND(byte b) 
    {
        A = (byte)(A & b);
        SetFlagZ(A);
        FlagN = false;
        FlagH = true;
        FlagC = false;
    }
    private void XOR(byte b) 
    {
        A = (byte)(A ^ b);
        SetFlagZ(A);
        FlagN = false;
        FlagH = false;
        FlagC = false;
    }
    private void OR(byte b) 
    {
        A = (byte)(A | b);
        SetFlagZ(A);
        FlagN = false;
        FlagH = false;
        FlagC = false;
    }
    private void CP(byte b) 
    {
        var result = A - b;
        SetFlagZ(result);
        FlagN = true;
        SetFlagHSub(A, b);
        SetFlagC(result);
    }
    private void DAD(ushort w) 
    {
        var result = HL + w;
        FlagN = false;
        SetFlagH(HL, w);
        FlagC = result >> 16 != 0;
        HL = (ushort)result;
    }
    private void SetFlagZ(int b) => FlagZ = (byte) b == 0;
    private void SetFlagC(int i) => FlagC = i >> 8 != 0;
    private void SetFlagH(byte b1, byte b2) => FlagH = (b1 & 0xF) + (b2 & 0xF) > 0xF;
    private void SetFlagH(ushort w1, ushort w2) => FlagH = (w1 & 0xFFF) + (w2 & 0xFFF) > 0xFFF;
    private void SetFlagHCarry(byte b1, byte b2) => FlagH = (b1 & 0xF) + (b2 & 0xF) >= 0xF;
    private void SetFlagHSub(byte b1, byte b2) => FlagH = (b1 & 0xF) < (b2 & 0xF);

    private void SetFlagHSubCarry(byte b1, byte b2)
    {
        var carry = FlagC ? 1 : 0;
        FlagH = (b1 & 0xF) < ((b2 & 0xF) + carry);
    }
}