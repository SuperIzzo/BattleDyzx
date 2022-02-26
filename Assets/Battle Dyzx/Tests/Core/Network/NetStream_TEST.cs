using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace BattleDyzx.Test
{
    public class NetStream_TEST
    {
        [Test]
        public void ReadPeekWriteByte()
        {
            NetStream stream = new NetStream(10);

            stream.WriteByte(42);
            stream.position = 0;

            byte b1 = stream.PeekByte();
            byte b2 = stream.ReadByte();

            Assert.AreEqual(42, b1);
            Assert.AreEqual(42, b2);
        }

        [Test]
        public void ReadWriteBytes()
        {
            NetStream stream = new NetStream(10);

            stream.WriteByte(42);
            stream.WriteByte(0);
            stream.WriteByte(255);
            stream.position = 0;

            byte b1 = stream.ReadByte();
            byte b2 = stream.ReadByte();
            byte b3 = stream.ReadByte();

            Assert.AreEqual(42, b1);
            Assert.AreEqual(0, b2);
            Assert.AreEqual(255, b3);
        }

        [Test]
        public void ReadWriteFloat()
        {
            NetStream stream = new NetStream(10);

            stream.WriteFloat(0.53f);
            stream.position = 0;

            float outFloat = stream.ReadFloat();

            Assert.AreEqual(0.53f, outFloat);
        }

        [Test(Description = "Network byte order is big-endian, NetStream must serialize in big-endian regardless host byte order")]
        public void NetStreamsAreBigEndian()
        {
            NetStream stream = new NetStream(10);

            stream.WriteUInt32(0x13FF008Au);
            stream.position = 0;

            byte b1 = stream.ReadByte();
            byte b2 = stream.ReadByte();
            byte b3 = stream.ReadByte();
            byte b4 = stream.ReadByte();

            Assert.AreEqual(0x13u, b1);
            Assert.AreEqual(0xFFu, b2);
            Assert.AreEqual(0x00u, b3);
            Assert.AreEqual(0x8Au, b4);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator NetStreamTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}