﻿using API.Entities;
using API.Helpers.Params;

namespace API.Interfaces
{
    public interface IReportRepository
    {
        void CreateReport(Report report);
        void UpdateReportStatus(Report report);
        Task<bool> Save();
        Task<Report> GetReport(int reportId);
        Task<IEnumerable<Report>> GetListReports(ReportParams reportParams);
    }
}
