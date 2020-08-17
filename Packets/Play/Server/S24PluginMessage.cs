using PumpkinMC.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PumpkinMC.Packets.Play.Server
{
    class S24PluginMessage : Packet
    {
        private const byte packetId = 0x18;

        public string pluginChannel;
        public byte[] data;

        public S24PluginMessage() { }

        public S24PluginMessage(string pluginChannel)
        {
            this.pluginChannel = pluginChannel;
        }

        public override byte[] GetBytes()
        {
            MemoryStream ms = new MemoryStream();

            ms.WriteByte(packetId); // Plugin Message (to Client)
            MCString.Write(pluginChannel, ms);
            ms.Write(data);

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
