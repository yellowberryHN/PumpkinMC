using PumpkinMC.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PumpkinMC.Packets.Login.Client
{
    class C00LoginStart : Packet
    {
        private const byte packetId = 0x00;

        public string username;
        public C00LoginStart() { }

        public override byte[] GetBytes()
        {
            throw new NotImplementedException();
        }

        public override bool isValid()
        {
            return username != null && username != String.Empty;
        }

        public override void UpdateClient(GameClient gameClient)
        {
            gameClient.username = username;
        }

        public override void ReadPacket(Stream stream)
        {
            username = MCString.Read(stream);
        }
    }
}
