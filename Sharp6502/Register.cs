namespace Sharp6502
{
    public class Register
    {
        #region Public

        #region Members
        public byte Value;
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