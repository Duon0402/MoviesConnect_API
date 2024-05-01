using API.Data;
using API.DTOs.Admin;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly DataContext _dataContext;

        public StatisticsRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<StatisticsDto> GetStatistics()
        {
            var statistics = new StatisticsDto();

            statistics.TotalActors = await _dataContext.Actors
                .Where(a => a.IsDeleted == false).CountAsync();
            statistics.TotalDirectors = await _dataContext.Directors
                .Where(d => d.IsDeleted == false).CountAsync();
            statistics.TotalUsers = await _dataContext.Users.CountAsync();
            statistics.TotalMovies = await _dataContext.Movies
                .Where(m => m.IsDeleted == false)
                .CountAsync();
            statistics.TotalGenres = await _dataContext.Genres
                .Where(g => g.IsDeleted == false)
                .CountAsync();
            statistics.TotalCertifications = await _dataContext.Certifications
                .Where(c => c.IsDeleted == false)
                .CountAsync();
            statistics.TotalReports = await _dataContext.Reports.CountAsync();
            statistics.TotalReportProcesseds = await _dataContext.Reports
                .Where(r => r.Status.Equals("Processed"))
                .CountAsync();
            statistics.TotalReportUnprocesseds = await _dataContext.Reports
                .Where(r => r.Status.Equals("Unprocessed"))
                .CountAsync();

            return statistics;
        }

    }
}
