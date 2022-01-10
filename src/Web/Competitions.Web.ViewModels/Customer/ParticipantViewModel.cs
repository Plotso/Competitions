﻿namespace Competitions.Web.ViewModels.Customer
{
    using System.Collections.Generic;
    using Data.Models.Customer;
    using Domain.Mapping.Mapping;
    using Rating;

    public class ParticipantViewModel : IMapFrom<Participant>
    {
        public CustomerViewModel Customer { get; set; }
    }
}