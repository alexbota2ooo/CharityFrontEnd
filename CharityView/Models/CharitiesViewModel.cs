using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CharityView.Models
{
    public class CharitiesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Mail { get; set; }
        public string? TargetGroup { get; set; }
        public string CauseId { get; set; }
        public string CauseName { get; set; }
        public string? Leadership { get; set; }
        public string? LeadershipDescription { get; set; }
        public string Website { get; set; }
        public string? Financials { get; set; }
        public string? AnnualReportLink { get; set; }
        public string? ImageUrl { get; set; }
        public bool Vetted { get; set; }
        public bool IsFeatured { get; set; }
        public decimal? Efficiency { get; set; }
        public decimal? SocialResponsibilityRating { get; set; }
        public int NumFavorites { get; set; }
        public string? DonationLink { get; set; }
        public bool IsFavorite { get; set; }
        public decimal? TotalIncome { get; set; }
        public decimal? Spendings { get; set; }
        public List<ImpactsViewModel> Impacts { get; set; } = new List<ImpactsViewModel>();
    }

    public class ImpactsViewModel
    {
        public int Id { get; set; }
        public string? Description { get; set; }
    }
}
