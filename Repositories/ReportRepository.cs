using API.Data;
using API.DTOs.Reports;
using API.Entities;
using API.Entities.Users;
using API.Helpers.Params;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public ReportRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
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

        public async Task<IEnumerable<ReportDto>> GetListReports(ReportParams reportParams)
        {
            var query = _dataContext.Reports
                .OrderByDescending(r => r.Id)
                .ThenByDescending(r => r.ReportTime)
                .ProjectTo<ReportDto>(_mapper.ConfigurationProvider)
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
