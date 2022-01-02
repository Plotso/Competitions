namespace Competitions.Domain.Mapping.Mapping
{
    using AutoMapper;

    public interface IMapExplicitly
    {
        void CreateMappings(IProfileExpression configuration);
    }
}