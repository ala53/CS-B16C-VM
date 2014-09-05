using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Virtual_Machine
{
    /*
     * What is it?
     * A 16 bit RISC emulator, executes bytecode in a virtual machine - no permanent storage, just ram
     */
    /*
     * References: http://www.codeproject.com/KB/recipes/B32Machine1/VMCS.pdf
     * http://c.learncodethehardway.org/book/introduction.html
     */

    /*
Specifications
16 bit CPU
64K RAM (60K usable)
5KHz – 5MHz clock speed
Possible 2^17 byte permanent storage (2x64K)
Memory	
0x0000-0xFF9F addressable
0x0000-0xF7FF 			– General Purpose memory
0xF800-0xF7CF 			– Video memory (80x25 screen) 
0xF7D0- 0xFF9F 			– Video memory metadata
-	3 bit RGB foreground – 1 bit intensity
-	3 bit RGB background – 1 bit intensity
0xFFA0 – 0xFFFF 		– Register assignments (for LOAD to/from register)

Registers
‘A’ register – 8 bits 							ID 0xFFA0
‘B’ register – 8 bits 							ID 0xFFA1
‘D’ register – 16 bits concatenates ‘A’ and ‘B’	ID 0xFFA2
‘X’ register – 16 bits 							ID 0xFFA3
‘Y’ register – 16 bits 							ID 0xFFA4

File Header
First 16 bits are 0xB16C
Bits 16-32 are the memory offset to copy the program to
Bits 32-48 are the memory offset to start execution at – absolute in memory



Functions
Mnemonic	What it does	Bytecode mapping
LDA 0xAA	Load ‘A’ Register	0x01
LDX 0xAAAA	Load ‘X’ Register	0x02
STA 0xADDR	Store ‘A’ at the specified memory location	0x03
END	End the program, required as the last line	0x04
JMP 0xADDR	Jumps execution to the specified memory address	0x05
		

Example Program
Start: //The program label
 LDA #65 //Load the A register with the ASCII value of ‘A’
 LDX $0xF7FF //Load the X Register with the top left of the screen
 STA ,X //STORE A into the memory address provided by register X
 JMP Start //Restart program
 END //and end
Bytecode

0xB16C 			//Header
0x0000 			//Copy address
0x0000 			//Start address
0x01 0x41 			//LDA
0x02 0xF7FF 		//LDX
0x03 0xFFA3 		//STA
0x05 0x0006 		//JMP
0x04 				//END

*/
    class Compiler
    {
        private ushort startAddr = 0x0000;
        private string[] assembler; //the input code
        private string outName = "";
        public Compiler(string filename, string outputFilename, string initAddress)
        {
            assembler = System.IO.File.ReadAllLines(filename);
            outName = outputFilename;
            startAddr = Convert.ToUInt16(initAddress, 16);
        }
    }
}

