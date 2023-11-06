namespace CharityView.Models
{
    public class CausesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }

        public int CharityCount { get; set; } = 0;
    }
}
