using System;
using System.Collections.Generic;
using System.Text;

namespace PumpkinMC.Packets.Play.Client
{
    class C07ClickWindow
    {
        private const byte packetId = 0x07;

        public int windowId;
        public short slot;
        public int button;
        public short actionNum;
        public int mode;
        public byte[] item;

        public C07ClickWindow()
        {
        }
    }
}
