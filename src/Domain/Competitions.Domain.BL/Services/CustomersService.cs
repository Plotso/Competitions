namespace Competitions.Domain.BL.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Common.Repositories;
    using Data.Models;
    using Data.Models.Customer;
    using Interfaces;
    using Mapping.Mapping.Single;
    using Microsoft.Extensions.Logging;

    public class CustomersService : ICustomersService
    {
        private readonly IDeletableEntityRepository<Customer> _customersRepository;
        private readonly IDeletableEntityRepository<Participant> _participantsRepository;
        private readonly IDeletableEntityRepository<Organiser> _organiserRepository;
        private readonly IRepository<ApplicationUser> _usersRepository;
        private readonly ILogger<CustomersService> _logger;

        public CustomersService(
            IDeletableEntityRepository<Customer> customersRepository,
            IDeletableEntityRepository<Participant> participantsRepository,
            IDeletableEntityRepository<Organiser> organiserRepository, 
            IRepository<ApplicationUser> usersRepository,
            ILogger<CustomersService> logger)
        {
            _customersRepository = customersRepository;
            _participantsRepository = participantsRepository;
            _organiserRepository = organiserRepository;
            _usersRepository = usersRepository;
            _logger = logger;
        }
        
        public async Task CreateInternalCustomer(string applicationUserId)
        {
            var user = _usersRepository.All().FirstOrDefault(u => u.Id == applicationUserId);
            if (user == null)
            {
                _logger.LogError("Creation of internal customer failed due to missing user or invalid user id.");
                throw new ArgumentException("User not found");
            }

            var customer = new Customer
            {
                ApplicationUser = user,
                Email = user.Email
            };
            await _customersRepository.AddAsync(customer);
            await _customersRepository.SaveChangesAsync();
            customer = _customersRepository.All().FirstOrDefault(c => c.ApplicationUserId == applicationUserId);

            var participant = new Participant
            {
                Customer = customer
            };
            await _participantsRepository.AddAsync(participant);
            await _participantsRepository.SaveChangesAsync();

            var organiser = new Organiser()
            {
                Customer = customer
            };
            await _organiserRepository.AddAsync(organiser);
            await _organiserRepository.SaveChangesAsync();
        }

        public string GetOrganiserId(string applicationUserId)
        {
            var user = _usersRepository.All().FirstOrDefault(u => u.Id == applicationUserId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }
            var customer = _customersRepository.All().FirstOrDefault(c => c.ApplicationUserId == applicationUserId);
            if (customer == null)
            {
                _logger.LogCritical("No customer mapped to User: {applicationUserId}", applicationUserId);
                throw new ArgumentException("No customer is mapped to the respective user");
            }

            var organiser = _organiserRepository.All().FirstOrDefault(o => o.CustomerId == customer.Id);
            if (organiser == null)
            {
                _logger.LogCritical("No organiser mapped to Customer: {customerId}", customer.Id);
                throw new ArgumentException("No organiser profile is mapped to the respective customer");
            }

            return organiser.Id;
        }

        public string GetParticipantId(string applicationUserId)
        {
            var user = _usersRepository.All().FirstOrDefault(u => u.Id == applicationUserId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }
            var customer = _customersRepository.All().FirstOrDefault(c => c.ApplicationUserId == applicationUserId);
            if (customer == null)
            {
                _logger.LogCritical("No customer mapped to User: {applicationUserId}", applicationUserId);
                throw new ArgumentException("No customer is mapped to the respective user");
            }

            var participant = _participantsRepository.All().FirstOrDefault(o => o.CustomerId == customer.Id);
            if (participant == null)
            {
                _logger.LogCritical("No participant mapped to Customer: {customerId}", customer.Id);
                throw new ArgumentException("No participant profile is mapped to the respective customer");
            }

            return participant.Id;
        }

        public string GetCustomerId(string applicationUserId)
        {
            var user = _usersRepository.All().FirstOrDefault(u => u.Id == applicationUserId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }
            var customer = _customersRepository.All().FirstOrDefault(c => c.ApplicationUserId == applicationUserId);
            if (customer == null)
            {
                _logger.LogCritical("No customer mapped to User: {applicationUserId}", applicationUserId);
                throw new ArgumentException("No customer is mapped to the respective user");
            }

            return customer.Id;
        }

        public T GetParticipant<T>(string participantId) 
            => _participantsRepository.All().Where(p => p.Id == participantId).To<T>().FirstOrDefault();

        public T GetOrganiserByParticipantId<T>(string participantId) 
            => _participantsRepository.All().Where(p => p.Id == participantId).Select(p => p.Customer.Organiser).To<T>().FirstOrDefault();
    }
}