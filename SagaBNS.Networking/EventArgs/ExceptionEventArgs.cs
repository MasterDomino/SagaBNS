using System;

namespace SagaBNS.Networking
{
    public class ExceptionEventArgs
    {
        #region Members

        public Exception Exception;

        #endregion

        #region Instantiation

        public ExceptionEventArgs(Exception ex) => Exception = ex;

        #endregion
    }
}