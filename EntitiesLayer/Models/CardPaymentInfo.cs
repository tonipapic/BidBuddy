namespace EntitiesLayer.Models {

    /// <summary>
    /// Stores information needed for card payment.
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public class CardPaymentInfo {

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public string CardNumber { get; set; }
        public string ExpireDate { get; set; }
        public int CVV { get; set; }

    }
}
