namespace CharityView.Models
{
    public class VettingCriteriaViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public virtual List<VettingDetailsViewModel> VettingDetailsResponse { get; set; } = new List<VettingDetailsViewModel>();
    }

    public class VettingDetailsViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
