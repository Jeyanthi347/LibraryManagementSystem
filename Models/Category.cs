namespace LibraryManagementSystem.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string? Description { get; set; }

        // Default constructor
        public Category()
        {
            CategoryName = string.Empty;
            Description = null;
        }

        // Parameterized constructor
        public Category(int categoryId, string categoryName, string? description)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            Description = description;
        }
    }
}