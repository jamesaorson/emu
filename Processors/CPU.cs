namespace Sharp6502.Processors
{
    public class CPU
    {
        #region Public

        #region Members
        public bool IsPoweredOn { get; private set; }
        #endregion

        #region Member Methods
        public void PowerUp()
        {
            if (IsPoweredOn)
            {
                return;
            }
            IsPoweredOn = true;
        }

        public void PowerDown()
        {
            if (!IsPoweredOn)
            {
                return;
            }
            IsPoweredOn = false;
        }
        #endregion

        #endregion
    }
}