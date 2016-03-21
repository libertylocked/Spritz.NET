using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpritzDotNet
{
    class SpritzCipher
    {
        const int N = 256;

        byte i, j, k;
        byte z;
        byte a;
        byte w;
        byte[] s;

        #region Constructors
        public SpritzCipher()
        {
            InitializeState();
        }

        public SpritzCipher(byte[] key)
        {
            InitializeState();
            Absorb(key);
        }

        public SpritzCipher(byte[] key, byte[] iv)
        {
            InitializeState();
            Absorb(key);
            AbsorbStop();
            Absorb(iv);
        }

        public SpritzCipher(byte[] key, byte bits)
        {
            InitializeState();
            Absorb(key);
            AbsorbStop();
            AbsorbByte(bits);
        }
        #endregion

        #region Public methods
        public byte[] Squeeze(int r)
        {
            if (a > 0)
            {
                Shuffle();
            }

            var p = new byte[r];

            for (int v = 0; v < r; v++)
            {
                p[v] = Drip();
            }

            return p;
        }
        #endregion

        #region Private methods
        void InitializeState()
        {
            i = j = k = z = a = 0;
            w = 1;
            s = new byte[N];
            for (int v = 0; v < N; v++)
            {
                s[v] = (byte)v;
            }
        }

        void Absorb(byte[] I)
        {
            foreach (byte b in I)
            {
                AbsorbByte(b);
            }
        }

        void AbsorbStop()
        {
            if (a == N / 2)
            {
                Shuffle();
            }
            a++;
        }

        void AbsorbByte(byte b)
        {
            AbsorbNibble((byte)(b & 0x0f));
            AbsorbNibble((byte)(b >> 4));
        }

        void AbsorbNibble(byte x)
        {
            if (a == N / 2)
            {
                Shuffle();
            }
            Swap(s, a, N / 2 + x);
            a++;
        }

        void Shuffle()
        {
            Whip(2 * N);
            Crush();
            Whip(2 * N);
            Crush();
            Whip(2 * N);
            a = 0;
        }

        void Whip(int r)
        {
            for (int v = 0; v < r; v++)
            {
                Update();
            }
            w += 2;
        }

        void Crush()
        {
            for (int v = 0; v < N / 2; v++)
            {
                if (s[v] > s[N - 1 - v])
                {
                    Swap(s, v, N - 1 - v);
                }
            }
        }

        byte Drip()
        {
            if (a > 0)
            {
                Shuffle();
            }
            Update();
            return Output();
        }

        void Update()
        {
            i = (byte)(i + w);
            j = (byte)(k + s[(byte)(j + s[i])]);
            k = (byte)(i + k + s[j]);
            Swap(s, i, j);
        }

        byte Output()
        {
            z = s[(byte)(j + s[(byte)(i + s[(byte)(z + k)])])];
            return z;
        }
        #endregion

        #region Private static methods
        static void Swap(byte[] arr, int a, int b)
        {
            byte tmp = arr[a];
            arr[a] = arr[b];
            arr[b] = tmp;
        }
        #endregion
    }
}
