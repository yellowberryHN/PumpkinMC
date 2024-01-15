using System.IO;
using PumpkinMC.Util;

namespace PumpkinMC.Packets.Play.Server
{
    class S07Statistics : Packet
    {
        private const byte packetId = 0x07;

        
        // https://gist.github.com/Alvin-LB/8d0d13db00b3c00fd0e822a562025eff
        public override byte[] GetBytes()
        {
            MemoryStream ms = new MemoryStream();

            ms.WriteByte(packetId); // Statistics
            VarInt.Write(1, ms);
            ms.Write(MCString.New("stat.jump"));
            VarInt.Write(1337, ms);
            
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