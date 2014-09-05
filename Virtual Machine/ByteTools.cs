using System;
using System.Collections.Generic;
using System.Text;

namespace Virtual_Machine
{
    public static class ByteTools
    {
        public static ushort Combine(this byte b1, byte b2)
        {
            ushort combined = (ushort)((b1 << 8) + b2);
            return combined;
        }

    }
}
