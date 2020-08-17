using PumpkinMC.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PumpkinMC.Packets.Login.Server
{
    class S02LoginSuccess : Packet
    {
        private const byte packetId = 0x02;

        public string uuid;
        public string username;

        public S02LoginSuccess() { }

        public S02LoginSuccess(string username)
        {
            this.uuid = Guid.NewGuid().ToString().ToUpper();
            this.username = username;
        }

        public override bool isValid()
        {
            return username != null && username != String.Empty
                && uuid != null && uuid != String.Empty;
        }

        public override void UpdateClient(GameClient gameClient)
        {
            gameClient.uuid = uuid;
            gameClient.state = GameState.PLAY;
        }

        public override byte[] GetBytes()
        {
            MemoryStream ms = new MemoryStream();

            ms.WriteByte(packetId); // Login Success
            MCString.Write(uuid, ms);
            MCString.Write(username, ms);

            return AddPacketHeader(ms.ToArray(), packetId);
        }

        public override void ReadPacket(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
