using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PumpkinMC.Packets.Play.Server
{
    class S13ServerDifficulty : Packet
    {
        private const byte packetId = 0x0D;

        public uint difficulty;

        public S13ServerDifficulty(uint difficulty)
        {
            this.difficulty = difficulty;
        }

        public override byte[] GetBytes()
        {
            MemoryStream ms = new MemoryStream();

            ms.WriteByte(packetId); // Server Difficulty
            ms.WriteByte((byte)difficulty);

            return AddPacketHeader(ms.ToArray(), packetId);
        }

        public override void ReadPacket(Stream stream)
        {
            throw new NotImplementedException();
        }

        public override void UpdateClient(GameClient gameClient)
        {
            throw new NotImplementedException();
        }

        public override bool isValid()
        {
            throw new NotImplementedException();
        }
    }
}
