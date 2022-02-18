using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


namespace BattleDyzx.Test
{
    public class NetPacketId_TEST
    {
        // A Test behaves as an ordinary method
        [Test]
        public void CompareInNormalRange()
        {
            /*
            byte a = 180;
            byte b = 56;

            byte d1 = (byte)(a - b);
            byte d2 = (byte)(b - a);

            bool aIsBigger = d1 < d2;

            Assert.AreEqual(aIsBigger, false);
            */

            NetPacketId idA = new NetPacketId(50);
            NetPacketId idB = new NetPacketId(10);

            Assert.True(idA > idB, "PacketId 50 > 10 failed");
            Assert.False(idA < idB, "PacketId !(50 < 10) failed");

            Assert.True(idB < idA, "PacketId 10 < 50 failed");
            Assert.False(idB > idA, "PacketId !(10 > 50) failed");
        }

        [Test]
        public void CompareInNegativeRange()
        {
            NetPacketId idA = new NetPacketId(50);
            NetPacketId idB = new NetPacketId(UInt32.MaxValue - 9);

            Assert.True(idA > idB, "PacketId 50 > -10 failed");
            Assert.False(idA < idB, "PacketId !(50 < -10) failed");

            Assert.True(idB < idA, "PacketId -10 < 50 failed");
            Assert.False(idB > idA, "PacketId !(-10 > 50) failed");
        }

        [Test]
        public void CompareInExtremeRange()
        {
            NetPacketId idA = new NetPacketId(10);
            NetPacketId idB = new NetPacketId(UInt32.MaxValue / 2 + 12);
            NetPacketId idC = new NetPacketId(UInt32.MaxValue / 2 + 11);
            NetPacketId idD = new NetPacketId(UInt32.MaxValue / 2 + 10);

            Assert.True(idA > idB, "PacketId 10 > h+12 failed");
            Assert.False(idB > idA, "PacketId !(h+12 > 10) failed");

            Assert.True(idB < idA, "PacketId h+12 < 10 failed");
            Assert.False(idA < idB, "PacketId !(10 < h+12) failed");
            
            // Comparison on the very opposite end is false both ways
            Assert.False(idA < idC, "PacketId !(10 < h+11) failed");
            Assert.False(idC < idA, "PacketId !(h+11 < 10) failed");

            Assert.False(idC > idA, "PacketId !(h+11 > 10) failed");
            Assert.False(idA > idC, "PacketId !(10 > h+11) failed");

            Assert.True(idA < idD, "PacketId (10 < h+10) failed");
            Assert.False(idD < idA, "PacketId !(h+10 < 10) failed");

            Assert.True(idD > idA, "PacketId !(h+10 > 10) failed");
            Assert.False(idA > idD, "PacketId !(10 > h+10) failed");
        }
    }
}