using PumpkinMC.Util;
using PumpkinMC.Util.Conversion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PumpkinMC.Packets.Play.Server
{
    class S12BossBar : Packet
    {
        private const byte packetId = 0x0C;

        public enum BossBarAction
        {
            Add,
            Remove,
            UpdateHealth,
            UpdateTitle,
            UpdateStyle,
            UpdateFlags
        }

        public enum BossBarColor
        {
            Pink,
            Blue,
            Red,
            Green,
            Yellow,
            Purple,
            White
        }

        public enum BossBarDivider
        {
            None,
            Six,
            Ten,
            Twelve,
            Twenty
        }

        [Flags]
        public enum BossBarFlags
        {
            None,
            DarkSky = 1,
            DragonBar = 2,
        }

        public Guid uuid;
        public BossBarAction action;
        public MCJsonString title;
        public float health;
        public BossBarColor color;
        public BossBarDivider divider;
        public BossBarFlags flags;

        public override byte[] GetBytes()
        {
            var bebc = new BigEndianBitConverter();
            var ms = new MemoryStream();

            ms.WriteByte(packetId);
            ms.Write(uuid.ToByteArray());
            ms.WriteByte((byte)action);

            switch (action)
            {
                case BossBarAction.Add:
                    ms.Write(MCString.New(title.ToString()));
                    ms.Write(bebc.GetBytes(health));
                    VarInt.Write((byte)color, ms);
                    VarInt.Write((byte)divider, ms);
                    ms.WriteByte((byte)flags);
                    break;
                case BossBarAction.Remove:
                    // no args, removes bar
                    break;
                case BossBarAction.UpdateHealth:
                    ms.Write(bebc.GetBytes(health));
                    break;
                case BossBarAction.UpdateTitle:
                    ms.Write(MCString.New(title.ToString()));
                    break;
                case BossBarAction.UpdateStyle:
                    VarInt.Write((byte)color, ms);
                    VarInt.Write((byte)divider, ms);
                    break;
                case BossBarAction.UpdateFlags:
                    ms.WriteByte((byte)flags);
                    break;
                default:
                    break;
            }

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
