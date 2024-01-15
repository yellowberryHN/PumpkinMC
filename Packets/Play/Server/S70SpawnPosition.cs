using PumpkinMC.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PumpkinMC.Packets.Play.Server
{
    class S70SpawnPosition : Packet
    {
        private const byte packetId = 0x46;

        public ulong position;

        public S70SpawnPosition() { }

        public S70SpawnPosition(ulong position)
        {
            this.position = position;
        }

        public override byte[] GetBytes()
        {
            MemoryStream ms = new MemoryStream();

            ms.WriteByte(packetId); // Spawn Position
            MCPosition.Write(position, ms); // why is this too long.

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
