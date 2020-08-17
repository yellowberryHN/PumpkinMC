using PumpkinMC.Util;
using PumpkinMC.Util.Conversion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PumpkinMC.Packets.Handshake.Client
{
    class C00Handshake : Packet
    {
        private const byte packetId = 0x00;

        public int protocol;
        public string host;
        public ushort port;
        public GameState requestState;

        public C00Handshake() { }

        public C00Handshake(int protocol, string host, ushort port, GameState requestState)
        {
            this.protocol = protocol;
            this.host = host;
            this.port = port;
            this.requestState = requestState;
        }

        public override void ReadPacket(Stream stream)
        {
            var bebc = new BigEndianBitConverter();

            protocol = VarInt.Read(stream);
            host = MCString.Read(stream);
            byte[] buf = new byte[2];
            stream.Read(buf, 0, buf.Length);
            port = bebc.ToUInt16(buf, 0);
            requestState = (GameState)stream.ReadByte();
        }

        public static void GetBytes(Stream stream)
        {
            //byte[] bytes = new byte[];
        }

        public override void UpdateClient(GameClient gameClient)
        {
            gameClient.protocol = protocol;
            gameClient.connectHost = host;
            gameClient.connectPort = port;
            gameClient.state = requestState;
        }

        public override bool isValid()
        {
            return protocol != 0 &&
                host != null &&
                port != 0;
        }

        public override byte[] GetBytes()
        {
            throw new NotImplementedException();
        }
    }
}
