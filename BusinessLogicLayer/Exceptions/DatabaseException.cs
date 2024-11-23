using System;

namespace BusinessLogicLayer.Exceptions {

    /// <summary>
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public class DatabaseException : ApplicationException {

        public DatabaseException(string message) : base(message) { }

    }
}
