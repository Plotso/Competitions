namespace Competitions.Data.Models.Partner
{
    using Common.Models;
    using Common.Models.Interfaces;

    public class Partner : BaseDeletableModel<int>, ITestableEntity
    {
        public string Name { get; set; }
        
        public string Email { get; set; }
        
        public string Mobile { get; set; }
        
        public string Website { get; set; }
        
        public bool IsTestEntity { get; set; }
    }
}