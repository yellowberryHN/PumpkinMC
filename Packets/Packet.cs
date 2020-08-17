using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PumpkinMC.Util;

namespace PumpkinMC.Packets
{
    abstract class Packet
    {
        public Packet() { }

        public abstract void ReadPacket(Stream stream);

        public abstract void UpdateClient(GameClient gameClient);

        public abstract byte[] GetBytes();

        public void WritePacket(Stream stream)
        {
            byte[] buf = GetBytes();
            Console.WriteLine(BitConverter.ToString(buf));
            stream.Write(buf);
        }

        public abstract bool isValid();

        protected static byte[] AddPacketHeader(byte[] bytesIn, byte packetId)
        {
            Console.WriteLine("Adding packet length to packet of size {0} and type 0x{1}", bytesIn.Length, packetId.ToString("X2"));

            int lenSize = VarInt.GetSize(bytesIn.Length);
            byte[] bytes = new byte[lenSize + bytesIn.Length];
            Buffer.BlockCopy(VarInt.New(bytesIn.Length), 0, bytes, 0, lenSize);
            Buffer.BlockCopy(bytesIn, 0, bytes, lenSize, bytesIn.Length);

            return bytes;
        }
    }
}
