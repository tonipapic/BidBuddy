using System;

namespace BusinessLogicLayer.Exceptions {

    /// <summary>
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public class HUB3PaymentException : ApplicationException {

        public HUB3PaymentException(string message) : base(message) { }

    }
}
