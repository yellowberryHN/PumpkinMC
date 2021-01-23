using PumpkinMC.Util;
using PumpkinMC.Util.Conversion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PumpkinMC.Packets.Play.Server
{
    class S19OpenWindow : Packet
    {
        private const byte packetId = 0x13;

        public byte windowId;
        public string windowType;
        public MCJsonString windowTitle;
        public byte slots;
        public int entityId = -1;

        public S19OpenWindow(byte windowId, string windowType, MCJsonString windowTitle, byte slots)
        {
            this.windowId = windowId;
            this.windowType = windowType;
            this.windowTitle = windowTitle;
            this.slots = slots;
        }

        public override byte[] GetBytes()
        {
            var bebc = new BigEndianBitConverter();
            var ms = new MemoryStream();

            ms.WriteByte(packetId); // Open Window
            ms.WriteByte(windowId);
            ms.Write(MCString.New(windowType));
            ms.Write(MCString.New(windowTitle.ToString()));
            ms.WriteByte(slots);
            if(entityId>-1) ms.Write(bebc.GetBytes(entityId));

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
