using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PumpkinMC.Util
{
    class VarInt
    {
        public static int GetSize(int value)
        {
            for (int i = 1; i < 5; ++i)
            {
                if ((value & -1 << i * 7) == 0) return i;
            }

            return 5;
        }

        public static int Read(Stream stream)
        {
            int counter = 0;
            int result = 0;
            byte read;
            do
            {
                read = (byte)stream.ReadByte();
                int value = (read & 0b01111111);
                result |= (value << (7 * counter));

                counter++;
                if (counter > 5)
                {
                    throw new IndexOutOfRangeException("VarInt is too big");
                }
            } while ((read & 0b10000000) != 0);

            return result;
        }

        public static void Write(int value, Stream stream)
        {
            do
            {
                byte temp = (byte)(value & 0b01111111);
                value = (int)((uint)value >> 7);
                if (value != 0)
                {
                    temp |= 0b10000000;
                }
                stream.WriteByte(temp);
            } while (value != 0);
        }

        public static byte[] New(int value)
        {
            byte[] buf = new byte[GetSize(value)];
            int i = 0;
            do
            {
                byte temp = (byte)(value & 0b01111111);
                value = (int)((uint)value >> 7);
                if (value != 0)
                {
                    temp |= 0b10000000;
                }
                buf[i] = temp;
                i++;
            } while (value != 0);
            return buf;
        }
    }
}
