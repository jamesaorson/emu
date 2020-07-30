using System;

namespace Sharp6502.Buses
{
    public static class AddressBus
    {
        #region Public

        #region Static Fields
        public static UInt16 Bus { get; set; }
        #endregion

        #endregion

        #region Internal

        #region Static Methods
        internal static void Initialize()
        {
            Bus = 0x0000;
        }
        #endregion

        #endregion

        static AddressBus()
        {
            Initialize();
        }
    }
}