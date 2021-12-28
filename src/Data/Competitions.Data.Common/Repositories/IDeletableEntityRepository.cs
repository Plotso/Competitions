﻿namespace Competitions.Data.Common.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;
    using Models.Interfaces;

    public interface IDeletableEntityRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IDeletableEntity
    {
        IQueryable<TEntity> AllWithDeleted();

        IQueryable<TEntity> AllAsNoTrackingWithDeleted();

        //Task<TEntity> GetByIdWithDeletedAsync(params object[] id); // ToDo: Maybe remove this

        void HardDelete(TEntity entity);

        void Undelete(TEntity entity);
    }
}