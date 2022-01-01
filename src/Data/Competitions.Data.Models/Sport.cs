namespace Competitions.Data.Models
{
    using Common.Models;

    public class Sport : BaseDeletableModel<int>
    {
        public string Name { get; set; }
        
        public bool IsVerified { get; set; }
    }
}