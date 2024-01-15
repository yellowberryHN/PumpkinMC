using System.IO;
using PumpkinMC.Util;

namespace PumpkinMC.Packets.Play.Server
{
    class S14TabComplete : Packet
    {
        private const byte packetId = 0x0E;
        public string message;

        public S14TabComplete(string message)
        {
            this.message = message;
        }

        public override byte[] GetBytes()
        {
            MemoryStream ms = new MemoryStream();

            ms.WriteByte(packetId); // Tab-Complete (from Server)
            
            VarInt.Write(2, ms);
            ms.Write(MCString.New("Yellowberry"));
            ms.Write(MCString.New("TabTest"));

            return AddPacketHeader(ms.ToArray(), packetId);
        }
        
        public override void ReadPacket(Stream stream)
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateClient(GameClient gameClient)
        {
            throw new System.NotImplementedException();
        }

        public override bool isValid()
        {
            throw new System.NotImplementedException();
        }
    }
}

