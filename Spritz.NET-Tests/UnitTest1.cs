using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpritzDotNet;

namespace Spritz_Tests
{
    [TestClass]
    public class UnitTest1
    {
        //[TestMethod]
        //public void TestOutput()
        //{
        //    var tests = new TestCase[]
        //    {
        //        new TestCase("ABC", 
        //            new byte[] 
        //            { 0x77, 0x9a, 0x8e, 0x01, 0xf9, 0xe9, 0xcb, 0xc0 }),
        //        new TestCase("spam", 
        //            new byte[] 
        //            { 0xf0, 0x60, 0x9a, 0x1d, 0xf1, 0x43, 0xce, 0xbf }),
        //        new TestCase("arcfour", 
        //            new byte[] 
        //            { 0x1a, 0xfa, 0x8b, 0x5e, 0xe3, 0x37, 0xdb, 0xc7 }),
        //    };

        //    foreach (var tc in tests)
        //    {
        //        SpritzCipher c = new SpritzCipher(tc.Key);

        //        for (int i = 0; i < tc.Output.Length; i++)
        //        {
        //            var v = c.Drip();
        //            Assert.AreEqual(v, tc.Output[i]);
        //        }
        //    }
        //}

        [TestMethod]
        public void TestHash()
        {
            var tests = new TestCase[]
            {
                new TestCase("ABC", 
                    new byte[] 
                    { 0x02, 0x8f, 0xa2, 0xb4, 0x8b, 0x93, 0x4a, 0x18 }),
                new TestCase("spam", 
                    new byte[] 
                    { 0xac, 0xbb, 0xa0, 0x81, 0x3f, 0x30, 0x0d, 0x3a }),
                new TestCase("arcfour", 
                    new byte[] 
                    { 0xff, 0x8c, 0xf2, 0x68, 0x09, 0x4c, 0x87, 0xb9 }),
            };

            foreach (var tc in tests)
            {
                var h = Spritz.Hash(tc.Key, 32);

                for (int i = 0; i < tc.Output.Length; i++)
                {
                    Assert.AreEqual(tc.Output[i], h[i]);
                }
            }
        }

        [TestMethod]
        public void TestRoundtrip()
        {
            var tests = new TestCase[]
            {
                new TestCase("I am a key",
                    "Hello, World!"),
                new TestCase("ABC",  
                    "The quick brown fox jumps over the lazy dog"),
                new TestCase("spam",  
                    "A returning noise skips into the arcade"),
                new TestCase("arcfour",  
                    "The postage postulates a chaos"),
                new TestCase("lizard",  
                    "The glue bounces"),
            };

            foreach (var tc in tests)
            {
                var key = tc.Key;
                var data = tc.Output;

                var ctxt = Spritz.Encrypt(data, key);
                var ptxt = Spritz.Decrypt(ctxt, key);

                Console.WriteLine("Plaintext: " + ByteArrayToString(data));
                Console.WriteLine("Key: " + ByteArrayToString(key));
                Console.WriteLine("Ciphertext: " + ByteArrayToString(ctxt));
                Console.WriteLine();
                CollectionAssert.AreEqual(data, ptxt);
            }
        }

        [TestMethod]
        public void TestRoundtripWithIV()
        {
            var tests = new TestCase[]
            {
                new TestCase("ABC", 
                    new byte[] { 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f },
                    "The quick brown fox jumps over the lazy dog"),
                new TestCase("spam", 
                    new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7 },
                    "A returning noise skips into the arcade"),
                new TestCase("arcfour",
                    new byte[] { 0xff, 0xf0, 0xef, 0xe0, 0xdf, 0xd0, 0xcf, 0xc0 },
                    "The postage postulates a chaos"),
            };

            foreach (var tc in tests)
            {
                var key = tc.Key;
                var data = tc.Output;
                var iv = tc.IV;

                var ctxt = Spritz.Encrypt(data, key, iv);
                var ptxt = Spritz.Decrypt(ctxt, key, iv);

                CollectionAssert.AreEqual(data, ptxt);
            }
        }

        static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
