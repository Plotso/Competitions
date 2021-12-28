namespace Competitions.Domain.BL.Mapping
{
    using AutoMapper;

    public interface IMapExplicitly
    {
        void CreateMappings(IProfileExpression configuration);
    }
}