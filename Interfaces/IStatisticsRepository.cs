using API.DTOs.Admin;

namespace API.Interfaces
{
    public interface IStatisticsRepository
    {
        Task<StatisticsDto> GetStatistics();
    }
}
