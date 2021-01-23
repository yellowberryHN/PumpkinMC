using System;
using System.Collections.Generic;
using System.Text;

namespace PumpkinMC.Util
{
    class ChunkData
    {
        //private void WriteChunkSection(ChunkSection section, Buffer buf)
        //{
        //    ChunkPalette palette = section.palette;
        //    byte bitsPerBlock = 13;

        //    WriteByte(bitsPerBlock);
        //    palette.Write(buf);

        //    int dataLength = (16 * 16 * 16) * bitsPerBlock / 64; // See tips section for an explanation of this calculation
        //    UInt64[] data = new UInt64[dataLength];

        //    // A bitmask that contains bitsPerBlock set bits
        //    uint individualValueMask = (uint)((1 << bitsPerBlock) - 1);

        //    for (int y = 0; y < SECTION_HEIGHT; y++)
        //    {
        //        for (int z = 0; z < SECTION_WIDTH; z++)
        //        {
        //            for (int x = 0; x < SECTION_WIDTH; x++)
        //            {
        //                int blockNumber = (((blockY * SECTION_HEIGHT) + blockZ) * SECTION_WIDTH) + blockX;
        //                int startLong = (blockNumber * bitsPerBlock) / 64;
        //                int startOffset = (blockNumber * bitsPerBlock) % 64;
        //                int endLong = ((blockNumber + 1) * bitsPerBlock - 1) / 64;

        //                BlockState state = section.GetState(x, y, z);

        //                UInt64 value = palette.IdForState(state);
        //                value &= individualValueMask;

        //                data[startLong] |= (Value << startOffset);

        //                if (startLong != endLong)
        //                {
        //                    data[endLong] = (value >> (64 - startOffset));
        //                }
        //            }
        //        }
        //    }

        //    WriteVarInt(dataLength);
        //    WriteLongArray(data);

        //    for (int y = 0; y < SECTION_HEIGHT; y++)
        //    {
        //        for (int z = 0; z < SECTION_WIDTH; z++)
        //        {
        //            for (int x = 0; x < SECTION_WIDTH; x += 2)
        //            {
        //                // Note: x += 2 above; we read 2 values along x each time
        //                byte value = section.GetBlockLight(x, y, z) | (section.GetBlockLight(x + 1, y, z) << 4);
        //                WriteByte(data, value);
        //            }
        //        }
        //    }

        //    if (currentDimension.HasSkylight())
        //    { // IE, current dimension is overworld / 0
        //        for (int y = 0; y < SECTION_HEIGHT; y++)
        //        {
        //            for (int z = 0; z < SECTION_WIDTH; z++)
        //            {
        //                for (int x = 0; x < SECTION_WIDTH; x += 2)
        //                {
        //                    // Note: x += 2 above; we read 2 values along x each time
        //                    byte value = section.GetSkyLight(x, y, z) | (section.GetSkyLight(x + 1, y, z) << 4);
        //                    WriteByte(data, value);
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
