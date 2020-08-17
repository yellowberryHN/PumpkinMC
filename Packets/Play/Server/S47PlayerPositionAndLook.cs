using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PumpkinMC.Packets.Play.Server
{
    class S47PlayerPositionAndLook : Packet
    {
        private const byte packetId = 0x2F;

        public double x;
        public double y;
        public double z;

        public float yaw;
        public float pitch;

        public byte flags;

        public int teleportId;

        public override byte[] GetBytes()
        {
            return new byte[1]; // actually write this
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
