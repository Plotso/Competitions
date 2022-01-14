namespace Competitions.Domain.BL.Models
{
    using System;

    public class TopRankingDTO
    {
        /// <summary>
        /// The name of the specified ranking item
        /// </summary>
        public string GroupName { get; set; }
        
        /// <summary>
        /// The points the specified ranking item has according to the criteria
        /// </summary>
        public int Points { get; set; }
        
        /// <summary>
        /// The criteria based on which items are ranked
        /// </summary>
        public RankingCriteria Criteria { get; set; }
    }
}