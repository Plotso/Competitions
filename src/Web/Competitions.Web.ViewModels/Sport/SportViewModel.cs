﻿namespace Competitions.Web.ViewModels.Sport
{
    using Data.Models;
    using Domain.BL.Mapping;

    public class SportViewModel : IMapFrom<Sport>
    {
        public string Name { get; set; }
    }
}