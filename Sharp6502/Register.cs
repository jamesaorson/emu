namespace Sharp6502
{
    public struct Register
    {
        #region Public

        #region Members
        public byte Value { get; set; }
        #endregion

        #region Member Methods
        public void Clear()
        {
            Value = 0x00;
        }
        #endregion

        #endregion
    }
}