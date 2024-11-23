namespace EntitiesLayer.Entities {
    public partial class User {

        public string FullName { get => $"{FirstName} {LastName}"; }
        public string ReasonMessage()
        {
            if (string.IsNullOrEmpty(BanMessage)) return "-";
            return BanMessage;
        }

        public int ReviewCount()
        {
            return Reviews.Count;
        }
    }
}
