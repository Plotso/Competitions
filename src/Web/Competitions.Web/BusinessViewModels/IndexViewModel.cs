namespace Competitions.Web.BusinessViewModels
{
    using System.Collections.Generic;
    using Domain.BL.Models;

    public class IndexViewModel
    {
        public IEnumerable<TopRankingDTO> RankingByStatus { get; set; }
        public IEnumerable<TopRankingDTO> RankingBySport { get; set; }
        public IEnumerable<TopRankingDTO> RankingByLocation { get; set; }
    }
}