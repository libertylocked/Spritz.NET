using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spritz_Tests
{
    struct TestCase
    {
        public readonly byte[] Key;
        public readonly byte[] IV;
        public readonly byte[] Output;

        public TestCase(string key, byte[] output)
            : this(Encoding.ASCII.GetBytes(key), null, output)
        {
        }

        public TestCase(string key, string output)
            : this(key, null, output)
        {
        }

        public TestCase(string key, byte[] iv, string output)
            : this(Encoding.ASCII.GetBytes(key), iv, Encoding.ASCII.GetBytes(output))
        {
        }

        public TestCase(byte[] key, byte[] iv, byte[] output)
        {
            Key = key;
            IV = iv;
            Output = output;
        }
    }
}
