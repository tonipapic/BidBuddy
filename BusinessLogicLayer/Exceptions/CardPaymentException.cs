using System;

namespace BusinessLogicLayer.Exceptions {

    /// <summary>
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public class CardPaymentException : ApplicationException {

        public CardPaymentException(string message) : base(message) { }

    }
}
