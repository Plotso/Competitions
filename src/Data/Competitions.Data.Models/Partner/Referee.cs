namespace Competitions.Data.Models.Partner
{
    using Common.Models;
    using Common.Models.Interfaces;

    public class Referee : BaseDeletableModel<int>, ITestableEntity
    {
        public string Name { get; set; }
        
        public string Mobile { get; set; }
        
        public bool IsTestEntity { get; set; }
    }
}