using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Virtual_Machine
{
    class Program
    {
        private static byte[] ops = { 
                                        0x01, 0xA7, 0xF0, 0x00, //LD Y 0xF7FF
                                        0x01, 0xA1, 0x64,       //LD A 0x64
                                        0x01, 0xA2, 0xAE,       //LD B 0xAE
                                        0x04, 0xA1, 0xFF, 0xA7, //ST A 0xF7FF
                                        0x01, 0xA7, 0xBA, 0xBE, //LD Y 0xBABE
                                        0x01, 0xA1, 0x41,       //LD A 0x41
                                        0x01, 0xA5, 0xF0, 0x00, //LD X 0xF7FF
                                        0x01, 0xA7, 0xA3, 0x23, //LD Y 0xA323
                                        0x01, 0xA2, 0xCC,       //LD B 0xCC
                                        0x04, 0xA1, 0xFF, 0xA5, //ST A ,X
                                        0x01, 0xA5, 0xA1, 0x3E, //LD X 0xA13E
                                        0x07, 0x00, 0x00,       //JMP 0x0000
                                        0x06                    //END
                                    };
        static void Main(string[] args)
        {
            // if (args[1] == "-c") //Compiler Mode
            //{
            //   Compiler c = new Compiler(args[2], args[3], args[4]);
            //}
            VirtualMachine vm = new VirtualMachine(ops, 0, 0);
            vm.runVM();
            Console.WriteLine("Press any key to exit");
            Console.Read();
        }
    }
}
