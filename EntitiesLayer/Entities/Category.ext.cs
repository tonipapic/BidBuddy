namespace EntitiesLayer.Entities {
    public partial class Category {

        public static readonly int UnknownCategory = 0;

        public bool HasSubCategories { get => SubCategories.Count != 0; }

        public override string ToString() {
            return $"Id={CategoryId}, Name={Name}, Parent={ParentId?.ToString() ?? "NULL"}";
        }

    }
}
