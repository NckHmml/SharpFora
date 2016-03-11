using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpFora.Security
{
    public static class Base32
    {
        const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        const int InSize = 8;
        const int OutSize = 5;

        public static string ToString(byte[] data)
        {
            char[] result = new char[(data.Length * InSize + 4) / OutSize];
            byte cur, next;

            for (int x = 0, i = 0, index = 0, digit; x < data.Length; i++)
            {
                cur = data[x];
                if (index > (InSize - OutSize))
                {
                    if (x + 1 < data.Length)
                        next = data[x + 1];
                    else
                        next = 0;

                    digit = cur & (0xFF >> index);
                    index += OutSize;
                    index %= InSize;
                    digit <<= index;
                    digit |= next >> (InSize - index);
                    x++;
                }
                else
                {
                    index += OutSize;
                    digit = (cur >> (InSize - index)) & 0x1F;
                    index %= InSize;
                    if (index == 0)
                        x++;
                }
                result[i] = Chars[digit];
            }
            return new string(result);
        }
    }
}
