using System;
using System.IO;
using PumpkinMC.Packets.Play.Server;
using PumpkinMC.Util;

namespace PumpkinMC.Packets.Play.Client
{
    class C01TabComplete : Packet
    {
        public override void ReadPacket(Stream stream)
        {
            byte[] messageBytes = new byte[VarInt.Read(stream)];
            stream.Read(messageBytes, 0, messageBytes.Length);
            string message = System.Text.Encoding.UTF8.GetString(messageBytes);
            bool assumeCommand = stream.ReadByte() == 1;
            bool hasPosition = stream.ReadByte() == 1;
            if (hasPosition)
            {
                // TODO: reading positions not implemented
                stream.Read(new byte[8], 0, 8);
            }
            Console.WriteLine("[0x01] Got tab complete request: \"{0}\"", message);
            
            var tabPacket = new S14TabComplete(message);
            tabPacket.WritePacket(stream);
        }

        public override void UpdateClient(GameClient gameClient)
        {
            throw new System.NotImplementedException();
        }

        public override byte[] GetBytes()
        {
            throw new System.NotImplementedException();
        }

        public override bool isValid()
        {
            throw new System.NotImplementedException();
        }
    }   
}