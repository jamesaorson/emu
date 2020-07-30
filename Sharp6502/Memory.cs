using System;

namespace Sharp6502
{
    public static class Memory
    {
        #region Public

        #region Static Fields
        public static byte[] Data { get; private set; }
        #endregion

        #region Static Methods
        public static void LoadProgram(byte[] data)
        {
            Initialize();
            for (var i = 0; i < UInt16.MaxValue; i++)
            {
                if (i >= data.Length)
                {
                    break;
                }
                Data[i] = data[i];
            }
        }
        #endregion

        #endregion

        #region Private

        #region Static Methods
        private static void Initialize()
        {
            Data = new byte[UInt16.MaxValue];
        }
        #endregion

        #endregion

        static Memory()
        {
            Initialize();
        }
    }
}