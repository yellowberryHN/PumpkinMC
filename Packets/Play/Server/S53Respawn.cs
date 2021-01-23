using PumpkinMC.Util;
using PumpkinMC.Util.Conversion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PumpkinMC.Packets.Play.Server
{
    class S53Respawn : Packet
    {
        private const byte packetId = 0x35;

        public override byte[] GetBytes()
        {
            var bebc = new BigEndianBitConverter();
            MemoryStream ms = new MemoryStream();

            ms.WriteByte(packetId); // Spawn Position
            ms.Write(bebc.GetBytes(0));
            ms.WriteByte(0);
            ms.WriteByte(0);
            ms.Write(MCString.New("default"));

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
