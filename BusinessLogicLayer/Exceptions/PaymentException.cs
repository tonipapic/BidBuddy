using System;

namespace BusinessLogicLayer.Exceptions {

    /// <summary>
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public class PaymentException : ApplicationException {

        public PaymentException(string message) : base(message) { }

    }
}
