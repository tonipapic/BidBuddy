using System;

namespace BusinessLogicLayer.Exceptions {

    /// <summary>
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public class InvalidInputException : ApplicationException {

        public InvalidInputException(string message) : base(message) {}

    }
}
