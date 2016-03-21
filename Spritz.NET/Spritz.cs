using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpritzDotNet
{
    public class Spritz
    {
        public static byte[] Encrypt(byte[] data, byte[] key)
        {
            return Encrypt(data, key, null);
        }

        public static byte[] Decrypt(byte[] data, byte[] key)
        {
            return Decrypt(data, key, null);
        }

        public static byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
        {
            var c = iv == null ? new SpritzCipher(key) : new SpritzCipher(key, iv);
            var ctxt = new byte[data.Length];

            var squeezed = c.Squeeze(data.Length);
            for (int i = 0; i < squeezed.Length; i++)
            {
                ctxt[i] = (byte)(data[i] + squeezed[i]);
            }
            return ctxt;
        }

        public static byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            var c = iv == null ? new SpritzCipher(key) : new SpritzCipher(key, iv);
            var ptxt = new byte[data.Length];

            var squeezed = c.Squeeze(data.Length);
            for (int i = 0; i < squeezed.Length; i++)
            {
                ptxt[i] = (byte)(data[i] - squeezed[i]);
            }
            return ptxt;
        }

        public static byte[] Hash(byte[] data, byte r)
        {
            var c = new SpritzCipher(data, r);
            return c.Squeeze(r);
        }
    }
}
