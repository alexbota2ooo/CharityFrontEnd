namespace CharityView.Models
{
    public class CharitiesFilterRequest
    {
        public int CauseId { get; set; }
        public string? Leadership { get; set; }
        public int? UserId { get; set; }
    }
}
