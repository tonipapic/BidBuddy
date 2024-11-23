namespace EntitiesLayer.Entities {
    public partial class AuctionState {

        public static readonly int Active = 1;
        public static readonly int Finished = 2;
        public static readonly int NotSold = 3;
        public static readonly int PaymentProcessing = 4;
        public static readonly int InDelivery = 5;
        public static readonly int Sold = 6;

    }
}
