using PumpkinMC.Util;
using PumpkinMC.Util.Conversion;
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

        public PPALFlags flags = 0;

        public int teleportId;

        public S47PlayerPositionAndLook(double x, double y, double z, float yaw, float pitch)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.yaw = yaw;
            this.pitch = pitch;
        }

        [Flags]
        public enum PPALFlags
        {
            None,
            X = 0x01,
            Y = 0x02,
            Z = 0x04,
            YRot = 0x08,
            XRot = 0x10
        }

        public override byte[] GetBytes()
        {
            var bebc = new BigEndianBitConverter();
            var ms = new MemoryStream();

            ms.WriteByte(packetId);

            // X Y Z
            ms.Write(bebc.GetBytes(x));
            ms.Write(bebc.GetBytes(y));
            ms.Write(bebc.GetBytes(z));

            // Yaw and Pitch
            ms.Write(bebc.GetBytes(yaw));
            ms.Write(bebc.GetBytes(pitch));

            // Flags
            ms.WriteByte((byte)flags);

            // Teleport ID (Write)
            VarInt.Write(teleportId, ms);

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
