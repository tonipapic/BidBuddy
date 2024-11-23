namespace EntitiesLayer.Models {

    /// <summary>
    /// Stores information needed for virman payment.
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public class VirmanPaymentInfo {

        public string ReceiverName { get; set; }
        public string ReceiverIBAN { get; set; }

        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }

    }
}
