using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Virtual_Machine
{
    public class VirtualMachine
    {
        private byte NextOP
        {
            get
            {
                OpIndex++;
                return machine.getByte((ushort)(OpIndex - 1));
            }
        }

        private ushort NextShort
        {
            get
            {
                var m_val = machine.getShort(OpIndex);
                OpIndex += 2;
                return m_val;
            }
        }

        private ushort OpIndex = 0;
        private Hardware machine = new Hardware();
        public VirtualMachine(byte[] opCode, ushort copyOffset, ushort startOffset)
        {
            ushort i = copyOffset;
            foreach (byte b in opCode)
            {
                machine.setByte(i, b);
                i++;
            }

            OpIndex = (ushort)(copyOffset + startOffset);
        }
        public bool keepRunning = true;
        public void runVM()
        {
            //25,603 ms for 100,000,000 million cycles
            //3905 cycles / ms
            //~3.9 million CPU cycles /s
            //~4Mhz CPU -- IF 1 cycle per clock
            //~100 mhz with a more realistic IPC
            while (keepRunning)
            {
                //Run the VM Here
                processNextOpCode();
                Console.Clear();
                Console.WriteLine("A " + machine.RegisterA);
                Console.WriteLine("B " + machine.RegisterB);
                Console.WriteLine("D " + machine.RegisterD);
                Console.WriteLine("X " + machine.RegisterX);
                Console.WriteLine("Y " + machine.RegisterY);
                Console.WriteLine("Screen Top Left: " + machine.getByte(0xF000));
                accurateWait(TimeSpan.FromMilliseconds(50));
            }
        }

        public void processNextOpCode()
        {
            Instruction opID = (Instruction)NextOP;

            //Load
            if (opID == Instruction.LD)
            {
                InterpretLD();
            }

            if (opID == Instruction.MLD)
            {
                InterpretMLD();
            }

            //Store
            if (opID == Instruction.ST)
            {
                InterpretST();
            }
            if (opID == Instruction.MST)
            {
                InterpretMST();
            }

            //Merge
            if (opID == Instruction.MERGE)
            {
                InterpretMerge();
            }

            //End
            if (opID == Instruction.END)
            {
                //We're done...
                keepRunning = false;
            }

            //Jumps
            if (opID == Instruction.JMP)
            {
                OpIndex = NextShort;
            }
            if (opID == Instruction.MJMP)
            {
                OpIndex = machine.getShort(NextShort);
            }

            //Wait operates
            if (opID == Instruction.WAIT)
            {
                accurateWait(TimeSpan.FromMilliseconds(NextShort));
            }
            if (opID == Instruction.MWAIT)
            {
               accurateWait(TimeSpan.FromMilliseconds(machine.getShort(NextShort)));
            }

            //Adds
            if (opID == Instruction.ADD)
            {
                InterpretADD();
            }
            if (opID == Instruction.MADD)
            {
                InterpretMADD();
            }

            //Subtracts
            if (opID == Instruction.SUB)
            {
                InterpretSUB();
            }
            if (opID == Instruction.MADD)
            {
                InterpretMSUB();
            }

            //Multilies
            if (opID == Instruction.MUL)
            {
                InterpretMUL();
            }
            if (opID == Instruction.MMUL)
            {
                InterpretMMUL();
            }

            //Divides
            if (opID == Instruction.DIV)
            {
                InterpretDIV();
            }
            if (opID == Instruction.MDIV)
            {
                InterpretMDIV();
            }

            //If values
            if (opID == Instruction.IFAB)
            {
                InterpretIFAB();
            }
            if (opID == Instruction.MIFAB)
            {
                InterpretMIFAB();
            }
            if (opID == Instruction.IFXY)
            {
                InterpretIFXY();
            }
            if (opID == Instruction.MIFXY)
            {
                InterpretMIFXY();
            }

            //Less than
            if (opID == Instruction.LTAB)
            {
                InterpretLTAB();
            }
            if (opID == Instruction.MLTAB)
            {
                InterpretMLTAB();
            }
            if (opID == Instruction.LTXY)
            {
                InterpretLTXY();
            }
            if (opID == Instruction.MLTXY)
            {
                InterpretMLTXY();
            }

            //Greater than
            if (opID == Instruction.GTAB)
            {
                InterpretGTAB();
            }
            if (opID == Instruction.MGTAB)
            {
                InterpretMGTAB();
            }
            if (opID == Instruction.GTXY)
            {
                InterpretGTXY();
            }
            if (opID == Instruction.MGTXY)
            {
                InterpretMGTXY();
            }
        }

        public void InterpretLD()
        {
            //Register number
            RegistersShort register = (RegistersShort)NextOP;

            if (register == RegistersShort.A)
            {
                machine.RegisterA = NextOP;
            }

            if (register == RegistersShort.B)
            {
                machine.RegisterB = NextOP;
            }

            if (register == RegistersShort.D)
            {
                machine.RegisterD = NextShort;
            }

            if (register == RegistersShort.X)
            {
                machine.RegisterX = NextShort;
            }

            if (register == RegistersShort.Y)
            {
                machine.RegisterY = NextShort;
            }
        }
        public void InterpretMLD()
        {
            //Register number
            RegistersShort register = (RegistersShort)NextOP;
            ushort addr = NextShort;
            if (register == RegistersShort.A)
            {
                machine.RegisterA = machine.getByte(addr);
            }

            if (register == RegistersShort.B)
            {
                machine.RegisterB = machine.getByte(addr);
            }

            if (register == RegistersShort.D)
            {
                machine.RegisterD = machine.getShort(addr);
            }

            if (register == RegistersShort.X)
            {
                machine.RegisterX = machine.getShort(addr);
            }

            if (register == RegistersShort.Y)
            {
                machine.RegisterY = machine.getShort(addr);
            }
        }
        public void InterpretST()
        {
            //Register number
            RegistersShort register = (RegistersShort)NextOP;

            if (register == RegistersShort.A)
            {
                machine.setByte(NextShort, machine.RegisterA);
            }
            if (register == RegistersShort.B)
            {
                machine.setByte(NextShort, machine.RegisterB);
            }
            if (register == RegistersShort.D)
            {
                machine.setShort(NextShort, machine.RegisterD);
            }
            if (register == RegistersShort.X)
            {
                machine.setShort(NextShort, machine.RegisterX);
            }
            if (register == RegistersShort.Y)
            {
                machine.setShort(NextShort, machine.RegisterY);
            }

        }
        public void InterpretMST()
        {
            //Register number
            RegistersShort register = (RegistersShort)NextOP;
            if (register == RegistersShort.A)
            {
                var val = machine.getShort(NextShort);
                machine.setByte(val, machine.RegisterA);
            }
            if (register == RegistersShort.B)
            {
                machine.setByte(machine.getShort(NextShort), machine.RegisterB);
            }
            if (register == RegistersShort.D)
            {
                machine.setShort(machine.getShort(NextShort), machine.RegisterD);
            }
            if (register == RegistersShort.X)
            {
                machine.setShort(machine.getShort(NextShort), machine.RegisterX);
            }
            if (register == RegistersShort.Y)
            {
                machine.setShort(machine.getShort(NextShort), machine.RegisterY);
            }

        }
        public void InterpretMerge()
        {
            byte[] arr = { machine.RegisterA, machine.RegisterB };
            machine.RegisterD = BitConverter.ToUInt16(arr, 0);
        }
        public void InterpretADD()
        {
            RegistersShort register = (RegistersShort)NextOP;

            if (register == RegistersShort.A)
            {
                machine.RegisterA += NextOP;
            }
            if (register == RegistersShort.B)
            {
                machine.RegisterB += NextOP;
            }
            if (register == RegistersShort.D)
            {
                machine.RegisterD += NextShort;
            }
            if (register == RegistersShort.X)
            {
                machine.RegisterX += NextShort;
            }
            if (register == RegistersShort.Y)
            {
                machine.RegisterY += NextShort;
            }
        }
        public void InterpretMADD()
        {
            RegistersShort register = (RegistersShort)NextOP;

            if (register == RegistersShort.A)
            {
                machine.RegisterA += machine.getByte(NextShort);
            }
            if (register == RegistersShort.B)
            {
                machine.RegisterB += machine.getByte(NextShort);
            }
            if (register == RegistersShort.D)
            {
                machine.RegisterD += machine.getShort(NextShort);
            }
            if (register == RegistersShort.X)
            {
                machine.RegisterX += machine.getShort(NextShort);
            }
            if (register == RegistersShort.Y)
            {
                machine.RegisterY += machine.getShort(NextShort);
            }
        }
        public void InterpretSUB()
        {
            RegistersShort register = (RegistersShort)NextOP;

            if (register == RegistersShort.A)
            {
                machine.RegisterA -= NextOP;
            }
            if (register == RegistersShort.B)
            {
                machine.RegisterB -= NextOP;
            }
            if (register == RegistersShort.D)
            {
                machine.RegisterD -= NextShort;
            }
            if (register == RegistersShort.X)
            {
                machine.RegisterX -= NextShort;
            }
            if (register == RegistersShort.Y)
            {
                machine.RegisterY -= NextShort;
            }
        }
        public void InterpretMSUB()
        {
            RegistersShort register = (RegistersShort)NextOP;

            if (register == RegistersShort.A)
            {
                machine.RegisterA -= machine.getByte(NextShort);
            }
            if (register == RegistersShort.B)
            {
                machine.RegisterB -= machine.getByte(NextShort);
            }
            if (register == RegistersShort.D)
            {
                machine.RegisterD -= machine.getShort(NextShort);
            }
            if (register == RegistersShort.X)
            {
                machine.RegisterX -= machine.getShort(NextShort);
            }
            if (register == RegistersShort.Y)
            {
                machine.RegisterY -= machine.getShort(NextShort);
            }
        }
        public void InterpretMUL()
        {
            RegistersShort register = (RegistersShort)NextOP;

            if (register == RegistersShort.A)
            {
                machine.RegisterA *= NextOP;
            }
            if (register == RegistersShort.B)
            {
                machine.RegisterB *= NextOP;
            }
            if (register == RegistersShort.D)
            {
                machine.RegisterD *= NextShort;
            }
            if (register == RegistersShort.X)
            {
                machine.RegisterX *= NextShort;
            }
            if (register == RegistersShort.Y)
            {
                machine.RegisterY *= NextShort;
            }
        }
        public void InterpretMMUL()
        {
            RegistersShort register = (RegistersShort)NextOP;

            if (register == RegistersShort.A)
            {
                machine.RegisterA *= machine.getByte(NextShort);
            }
            if (register == RegistersShort.B)
            {
                machine.RegisterB *= machine.getByte(NextShort);
            }
            if (register == RegistersShort.D)
            {
                machine.RegisterD *= machine.getShort(NextShort);
            }
            if (register == RegistersShort.X)
            {
                machine.RegisterX *= machine.getShort(NextShort);
            }
            if (register == RegistersShort.Y)
            {
                machine.RegisterY *= machine.getShort(NextShort);
            }
        }
        public void InterpretDIV()
        {
            RegistersShort register = (RegistersShort)NextOP;

            if (register == RegistersShort.A)
            {
                machine.RegisterA /= NextOP;
            }
            if (register == RegistersShort.B)
            {
                machine.RegisterB /= NextOP;
            }
            if (register == RegistersShort.D)
            {
                machine.RegisterD /= NextShort;
            }
            if (register == RegistersShort.X)
            {
                machine.RegisterX /= NextShort;
            }
            if (register == RegistersShort.Y)
            {
                machine.RegisterY /= NextShort;
            }
        }
        public void InterpretMDIV()
        {
            RegistersShort register = (RegistersShort)NextOP;

            if (register == RegistersShort.A)
            {
                machine.RegisterA /= machine.getByte(NextShort);
            }
            if (register == RegistersShort.B)
            {
                machine.RegisterB /= machine.getByte(NextShort);
            }
            if (register == RegistersShort.D)
            {
                machine.RegisterD /= machine.getShort(NextShort);
            }
            if (register == RegistersShort.X)
            {
                machine.RegisterX /= machine.getShort(NextShort);
            }
            if (register == RegistersShort.Y)
            {
                machine.RegisterY /= machine.getShort(NextShort);
            }
        }
        public void InterpretIFAB()
        {
            if (machine.RegisterA == machine.RegisterB)
            {
                OpIndex = NextShort;
            }
        }
        public void InterpretMIFAB()
        {
            if (machine.RegisterA == machine.RegisterB)
            {
                OpIndex = machine.getShort(NextShort);
            }
        }
        public void InterpretIFXY()
        {
            if (machine.RegisterX == machine.RegisterY)
            {
                OpIndex = NextShort;
            }
        }
        public void InterpretMIFXY()
        {
            if (machine.RegisterX == machine.RegisterY)
            {
                OpIndex = machine.getShort(NextShort);
            }
        }
        public void InterpretLTAB()
        {
            if (machine.RegisterA < machine.RegisterB)
            {
                OpIndex = NextShort;
            }
        }
        public void InterpretMLTAB()
        {
            if (machine.RegisterA < machine.RegisterB)
            {
                OpIndex = machine.getShort(NextShort);
            }
        }
        public void InterpretLTXY()
        {
            if (machine.RegisterX < machine.RegisterY)
            {
                OpIndex = NextShort;
            }
        }
        public void InterpretMLTXY()
        {
            if (machine.RegisterX < machine.RegisterY)
            {
                OpIndex = machine.getShort(NextShort);
            }
        }
        public void InterpretGTAB()
        {
            if (machine.RegisterA > machine.RegisterB)
            {
                OpIndex = NextShort;
            }
        }
        public void InterpretMGTAB()
        {
            if (machine.RegisterA > machine.RegisterB)
            {
                OpIndex = machine.getShort(NextShort);
            }
        }
        public void InterpretGTXY()
        {
            if (machine.RegisterX > machine.RegisterY)
            {
                OpIndex = NextShort;
            }
        }
        public void InterpretMGTXY()
        {
            if (machine.RegisterX > machine.RegisterY)
            {
                OpIndex = machine.getShort(NextShort);
            }
        }


        private void accurateWait(TimeSpan time)
        {
            var sw = Stopwatch.StartNew();

            if (time.TotalMilliseconds >= 100)
                Thread.Sleep(time - TimeSpan.FromMilliseconds(50));

            while (sw.ElapsedTicks < time.Ticks) ;
        }
    }

    public class Hardware
    {
        //Change to 24 bit addressing soon?
        private byte[] memory = new byte[61440]; //Because of the special function bytes, subtract 96
        private byte[] video = new byte[4000]; //Video ram
        const ushort VRamStart = 0xF000;
        //private byte[] permStor = new byte[65536];  //For future use, a permanent storage disk, 
        //from which byte 0x0000 - 0x0001 is a pointer to the OS Code
        //this will eventually use 32 bit addressing & be a 4MB disk

        //Memory helpers
        public byte getByte(ushort addr)
        {
            if (addr >= VRamStart - 1)
            {
                //video ram
                if ((addr >= VRamStart) && (addr < (ushort)RegistersLong.A))
                {
                    return video[addr - VRamStart];
                }
                //Its a register
                if (addr == (ushort)RegistersLong.A) { return RegisterA; }
                if (addr == (ushort)RegistersLong.B) { return RegisterB; }
                if (addr == (ushort)RegistersLong.D) { return BitConverter.GetBytes(RegisterD)[1]; }
                if (addr == ((ushort)RegistersLong.D + 1)) { return BitConverter.GetBytes(RegisterD)[0]; }
                if (addr == (ushort)RegistersLong.X) { return BitConverter.GetBytes(RegisterX)[1]; }
                if (addr == ((ushort)RegistersLong.X + 1)) { return BitConverter.GetBytes(RegisterX)[0]; }
                if (addr == (ushort)RegistersLong.Y) { return BitConverter.GetBytes(RegisterY)[1]; }
                if (addr == ((ushort)RegistersLong.Y + 1)) { return BitConverter.GetBytes(RegisterY)[0]; }

                if (addr == 0xFFFF) { return LastKey; } //special case, the last keyboard key press

                return memory[addr]; //If not...somehow
            }
            else
            {
                return memory[addr];
            }
        }
        public ushort getShort(ushort addr)
        {
            byte[] arr = { getByte((ushort)(addr + 1)), getByte(addr) };
            return BitConverter.ToUInt16(arr, 0);
        }
        public void setByte(ushort addr, byte value)
        {
            if (addr > 0xEFFF)
            {
                //video ram
                if ((addr >= VRamStart) && (addr < (ushort)RegistersLong.A))
                {
                    video[addr - VRamStart] = value;
                }
                //Its a register
                if (addr == (ushort)RegistersLong.A) { RegisterA = value; }
                if (addr == (ushort)RegistersLong.B) { RegisterB = value; }
                if (addr == (ushort)RegistersLong.D)
                {
                    byte[] val = { value, BitConverter.GetBytes(RegisterD)[0] };
                    RegisterD = BitConverter.ToUInt16(val, 0);
                }
                if (addr == ((ushort)RegistersLong.D + 1))
                {
                    byte[] val = { BitConverter.GetBytes(RegisterD)[1], value };
                    RegisterD = BitConverter.ToUInt16(val, 0);
                }
                if (addr == (ushort)RegistersLong.X)
                {
                    byte[] val = { value, BitConverter.GetBytes(RegisterX)[0] };
                    RegisterX = BitConverter.ToUInt16(val, 0);
                }
                if (addr == ((ushort)RegistersLong.X + 1))
                {
                    byte[] val = { BitConverter.GetBytes(RegisterX)[1], value };
                    RegisterX = BitConverter.ToUInt16(val, 0);
                }
                if (addr == (ushort)RegistersLong.Y)
                {
                    byte[] val = { value, BitConverter.GetBytes(RegisterY)[0] };
                    RegisterY = BitConverter.ToUInt16(val, 0);
                }
                if (addr == ((ushort)RegistersLong.Y + 1))
                {
                    byte[] val = { BitConverter.GetBytes(RegisterY)[1], value };
                    RegisterY = BitConverter.ToUInt16(val, 0);
                }
            }
            else
            {
                memory[addr] = value;
            }
        }
        public void setShort(ushort addr, ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            setByte(addr, bytes[1]);
            setByte((ushort)(addr + 1), bytes[0]);
        }

        //Special function
        public byte LastKey = 0;
        //Registers
        public byte RegisterA = 0;
        public byte RegisterB = 0;
        public ushort RegisterD = 0;
        public ushort RegisterX = 0;
        public ushort RegisterY = 0;
        //public uint RegisterZ = 0; //The eventual 32 bit register
        public Hardware()
        {
            for (int i = 0; i == memory.Count(); i++)
            {
                memory[i] = 0;
            }
            //vram
            for (int i = 0; i == (video.Count() / 2); i++)
            {
                video[i] = 0x20; //Foreground text is whitespace
            }
            for (int i = (video.Count() / 2); i == video.Count(); i++)
            {
                video[i] = 0xF0; //color is WHITE (F) on BLACK (0)
            }
        }
    }
}
