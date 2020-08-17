using PumpkinMC.Util.Conversion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace PumpkinMC.Util
{
    class MCString
    {
        public static string Read(Stream stream)
        {
            int len = VarInt.Read(stream);
            byte[] buf = new byte[len];
            stream.Read(buf, 0, buf.Length);
            return System.Text.Encoding.UTF8.GetString(buf);
        }

        public static byte[] New(string str)
        {
            byte[] len = VarInt.New(str.Length);

            byte[] ret = new byte[len.Length + str.Length];

            Buffer.BlockCopy(len, 0, ret, 0, len.Length);

            byte[] strBytes = System.Text.Encoding.UTF8.GetBytes(str, 0, str.Length);
            Buffer.BlockCopy(strBytes, 0, ret, len.Length, strBytes.Length);

            return ret;
        }

        public static void Write(string str, Stream stream)
        {
            stream.Write(New(str));
        }
    }

    class MCPosition
    {
        public static ulong New(int x, int y, int z)
        {
            return (ulong)(((x & 0x3FFFFFF) << 38) | ((y & 0xFFF) << 26) | (z & 0x3FFFFFF)); ;
        }

        public static byte[] GetBytes(int x, int y, int z)
        {
            var bebc = new BigEndianBitConverter();
            ulong pos = New(x,y,z);

            return bebc.GetBytes(pos);
        }

        public static byte[] GetBytes(ulong pos)
        {
            var bebc = new BigEndianBitConverter();
            return bebc.GetBytes(pos);
        }

        public static void Write(int x, int y, int z, Stream stream)
        {
            stream.Write(GetBytes(x,y,z));
        }

        public static void Write(ulong pos, Stream stream)
        {
            stream.Write(GetBytes(pos));
        }
    }
}
