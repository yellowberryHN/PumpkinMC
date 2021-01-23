using PumpkinMC.Util;
using PumpkinMC.Util.Conversion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PumpkinMC.Packets.Play.Server
{
    class S65UpdateHealth : Packet
    {
        private const byte packetId = 0x41;

        public float health;
        public int food;
        public float saturation;

        public override byte[] GetBytes()
        {
            var bebc = new BigEndianBitConverter();
            var ms = new MemoryStream();

            ms.WriteByte(packetId);
            ms.Write(bebc.GetBytes(health));
            VarInt.Write(food, ms);
            ms.Write(bebc.GetBytes(saturation));

            return AddPacketHeader(ms.ToArray(), packetId);

        }

        public override bool isValid()
        {
            throw new NotImplementedException();
        }

        public override void ReadPacket(Stream stream)
        {
            throw new NotImplementedException();
        }

        public override void UpdateClient(GameClient gameClient)
        {
            throw new NotImplementedException();
        }
    }
}
