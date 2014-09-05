using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Virtual_Machine
{

    static class StringTools
    {

        public static bool StartsWithWhitespace(this string str)
        {
            return char.IsWhiteSpace(str, 0);
        }
        public static bool EndsWithColon(this string str)
        {
            return str[str.Length - 1] == ':';
        }
    }
}
