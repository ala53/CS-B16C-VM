using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Virtual_Machine
{
    enum RegistersShort : byte
    {
        A = 0xA1,
        B = 0xA2,
        D = 0xA3,
        X = 0xA5,
        Y = 0xA7
    }
    enum RegistersLong : ushort
    {
        A = 0xFFA1,
        B = 0xFFA2,
        D = 0xFFA3,
        X = 0xFFA5,
        Y = 0xFFA7
    }
}
