using System;

namespace SagaBNS.Core.Cryptography
{
    public class RC4
    {
        #region Members

        private readonly byte[] _s;
        private uint _i;
        private uint _j;

        #endregion

        #region Instantiation

        public RC4(byte[] key)
        {
            _s = new byte[256];
            _j = 0;
            _i = 0;

            for (int index = 0; index < 256; index++)
            {
                _s[index] = (byte)index;
            }

            int index1 = 0;
            int s_index = 0;
            int v12 = 256;
            byte v4 = 0;

            do
            {
                if (index1 >= key.Length)
                {
                    throw new ApplicationException("index1 < bytes");
                }

                byte v8 = _s[s_index];
                v4 = (byte)((v4 + key[index1++] + _s[s_index]) & 0xFF);
                _s[s_index] = _s[v4];
                _s[v4] = v8;

                if (index1 == key.Length)
                {
                    index1 = 0;
                }

                s_index++;
            } while (v12-- != 1);
        }

        #endregion

        #region Methods

        public byte[] Encrypt(byte[] input)
        {
            byte[] output = new byte[input.Length];

            int index = 0;

            do
            {
                _i = (_i + 1) & 0xFF;
                _j = (_j + _s[_i]) & 0xFF;

                //swap
                byte swap = _s[_i];
                _s[_i] = _s[_j];
                _s[_j] = swap;

                // xor
                output[index] = (byte)((input[index]) ^ _s[(_s[_j] + _s[_i]) & 0xFF]);

                // i put index++ here instead and we dont have to do -1
                ++index;
            }
            while (index != input.Length); // + 1 or - 1?

            return output;
        }

        public void EncryptBuffer(byte[] buffer, long offset, long count)
        {
            for (int index = 0; index < count; index++)
            {
                long bufferIndex = index + offset;

                _i = (_i + 1) & 0xFF;
                _j = (_j + _s[_i]) & 0xFF;

                // swap
                byte swap = _s[_i];
                _s[_i] = _s[_j];
                _s[_j] = swap;

                // xor
                buffer[bufferIndex] = (byte)((buffer[bufferIndex]) ^ _s[(_s[_j] + _s[_i]) & 0xFF]);
            }
        }

        #endregion
    }
}