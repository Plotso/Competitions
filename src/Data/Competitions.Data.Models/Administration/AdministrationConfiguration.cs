namespace Competitions.Data.Models.Administration
{
    using System;
    using Common.Models;

    public class AdministrationConfiguration : BaseModel<string>
    {
        public AdministrationConfiguration()
        {
            Id = Guid.NewGuid().ToString();
        }

        public bool IsFrozen { get; set; }
    }
}