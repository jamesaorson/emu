using System;

namespace Sharp6502
{
    public static class Memory
    {
        #region Public

        #region Members
        public static byte[] Data { get; private set; }
        #endregion

        #endregion

        static Memory()
        {
            Data = new byte[(int)Math.Pow(2, 16)];
        }
    }
}