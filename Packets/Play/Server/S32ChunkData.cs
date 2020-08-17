using System;
using System.Collections.Generic;
using System.Text;

namespace PumpkinMC.Packets.Play.Server
{
    class S32ChunkData
    {
        public int chunkX;
        public int chunkZ;
        public bool isNew; // Ground-Up Continuous
        public int chunkMask;
        public int dataSize;
        public byte[] data;
        public int blockEntitiesSize;
        public byte[] blockEntities;
    }
}
