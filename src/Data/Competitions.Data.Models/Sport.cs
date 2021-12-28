namespace Competitions.Data.Models
{
    using Common.Models;

    public class Sport : BaseModel<int>
    {
        public string Name { get; set; }
        
        public bool IsVerified { get; set; }
    }
}