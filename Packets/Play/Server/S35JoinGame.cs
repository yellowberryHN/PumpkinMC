using PumpkinMC.Util;
using PumpkinMC.Util.Conversion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PumpkinMC.Packets.Play.Server
{
    class S35JoinGame : Packet
    {
        private const byte packetId = 0x23;

        public int entityId;
        public byte gamemode = 0x00;
        public int dimension = 0;
        public byte difficulty = 0x00;
        public byte maxPlayers;
        public string worldType = "default";
        public bool reducedDebugInfo = false;

        public S35JoinGame()
        {
        }

        public S35JoinGame(int entityId, byte gamemode, int dimension, byte difficulty, byte maxPlayers, string worldType)
        {
            this.entityId = entityId;
            this.gamemode = gamemode;
            this.dimension = dimension;
            this.difficulty = difficulty;
            this.maxPlayers = maxPlayers;
            this.worldType = worldType;
        }

        public override byte[] GetBytes()
        {
            MemoryStream ms = new MemoryStream();
            var bebc = new BigEndianBitConverter();

            ms.WriteByte(packetId); // Join Game
            ms.Write(bebc.GetBytes(entityId));
            ms.WriteByte(gamemode);
            ms.Write(bebc.GetBytes(dimension));
            ms.WriteByte(difficulty);
            ms.WriteByte(maxPlayers);
            MCString.Write(worldType, ms);
            ms.Write(bebc.GetBytes(reducedDebugInfo));

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
