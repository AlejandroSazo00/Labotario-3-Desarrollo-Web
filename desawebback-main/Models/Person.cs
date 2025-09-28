namespace HelloApi.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int? CreatedBy { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        public int? UpdatedBy { get; set; } = 0;
        public DateTime? UpdatedAt { get; set; }
        public List<Order> Orders { get; set; } = [];
    }
}
