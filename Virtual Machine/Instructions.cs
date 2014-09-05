using System;
using System.Collections.Generic;
using System.Text;

namespace Virtual_Machine
{
    public enum Instruction : byte
    {
        LD = 0x01,
        ST = 0x03,

        MERGE = 0x05,
        END = 0x06,
        JMP = 0x07,
        WAIT = 0x09,

        ADD = 0x0B,
        SUB = 0x0D,
        MUL = 0x0F,
        DIV = 0x11,

        IFAB = 0x13,
        IFXY = 0x15,
        LTAB = 0x17,
        LTXY = 0x19,
        GTAB = 0x1B,
        GTXY = 0x1D,

        MLD = 0x02,
        MST = 0x04,

        MJMP = 0x08,
        MWAIT = 0x0A,

        MADD = 0x0C,
        MSUB = 0x0E,
        MMUL = 0x10,
        MDIV = 0x12,

        MIFAB = 0x14,
        MIFXY = 0x16,
        MLTAB = 0x18,
        MLTXY = 0x1A,
        MGTAB = 0x1C,
        MGTXY = 0x1E
    }

    public struct dInstruction
    {
        public string mnemonic;
        public Instruction bytecode;
    }

    public static class InstructionList
    {
        private static dInstruction[] iList = {
                                          new dInstruction {mnemonic = "LD", bytecode = Instruction.LD},
                                          new dInstruction {mnemonic = "STA", bytecode = Instruction.ST},

                                          new dInstruction {mnemonic = "MERGE", bytecode = Instruction.MERGE},
                                          new dInstruction {mnemonic = "END", bytecode = Instruction.END},
                                          new dInstruction {mnemonic = "JMP", bytecode = Instruction.JMP},
                                          new dInstruction {mnemonic = "WAIT", bytecode = Instruction.WAIT},

                                          new dInstruction {mnemonic = "ADD", bytecode = Instruction.ADD},
                                          new dInstruction {mnemonic = "SUB", bytecode = Instruction.SUB},
                                          new dInstruction {mnemonic = "MUL", bytecode = Instruction.MUL},
                                          new dInstruction {mnemonic = "DIV", bytecode = Instruction.DIV},
                                          
                                          new dInstruction {mnemonic = "IFAB", bytecode = Instruction.IFAB},
                                          new dInstruction {mnemonic = "IFXY", bytecode = Instruction.IFXY},
                                          new dInstruction {mnemonic = "LTAB", bytecode = Instruction.LTAB},
                                          new dInstruction {mnemonic = "LTXY", bytecode = Instruction.LTXY},
                                          new dInstruction {mnemonic = "GTAB", bytecode = Instruction.GTAB},
                                          new dInstruction {mnemonic = "GTXY", bytecode = Instruction.GTXY}
                                      };
        public static Instruction GetInstruction(string Mnemonic, bool MemoryValue = false)
        {
            foreach (dInstruction i in iList)
            {
                if (i.mnemonic == Mnemonic) { if (MemoryValue) { return i.bytecode + 1; } else { return i.bytecode; } }
            }

            throw new ArgumentException("Error: The code \"" + Mnemonic + "\" is not a recognized operation code.");
        }
    }
}
