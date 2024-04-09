using API.Data;
using API.Entities;
using API.Entities.Users;
using API.Helpers.Params;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly DataContext _dataContext;

        public ReportRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public void CreateReport(Report report)
        {
            _dataContext.Add(report);
        }

        public void UpdateReportStatus(Report report)
        {
            _dataContext.Entry(report).State = EntityState.Modified;
        }

        public async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Report>> GetListReports(ReportParams reportParams)
        {
            var query = _dataContext.Reports
                .OrderByDescending(r => r.Id)
                .ThenByDescending(r => r.ReportTime)
                .AsQueryable();

            if(!string.IsNullOrWhiteSpace(reportParams.ObjectType))
            {
                query.Where(r => r.ObjectType == reportParams.ObjectType);
            }
            if(!string.IsNullOrWhiteSpace(reportParams.Status))
            {
                query.Where(r => r.Status == reportParams.Status);
            }

            return await query.ToListAsync();
        }

        public async Task<Report> GetReport(int reportId)
        {
            return await _dataContext.Reports
                .SingleOrDefaultAsync(r => r.Id == reportId);
        }
    }
}
