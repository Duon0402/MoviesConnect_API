using API.DTOs.Reports;
using API.Entities;
using API.Extentions;
using API.Helpers.Params;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class ReportController : BaseApiController
    {
        private readonly IReportRepository _reportRepository;

        public ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [HttpPost("CreateReport")]
        public async Task<ActionResult> CreateReport(ReportCreateDto reportCreateDto)
        {
            if (reportCreateDto == null) return BadRequest();
            var report = new Report
            {
                ObjectId = reportCreateDto.ObjectId,  // la movieId neu la ObjectType rating
                ObjectId2 = reportCreateDto.ObjectId2, // la userId ObjectType neu la rating
                Content = reportCreateDto.Content,
                ObjectType = reportCreateDto.ObjectType,
                ReporterId = User.GetUserId(),
                Status = "Unprocessed"
            };
            _reportRepository.CreateReport(report);
            return Ok(report);
        }

        [Authorize(Policy = "RequireModeratorRole")]
        [HttpGet("GetReport/{reportId}")]
        public async Task<ActionResult<ReportDto>> GetReport(int reportId)
        {
            var report = await _reportRepository.GetReport(reportId);
            if (report == null) return NotFound();
            return Ok(report);
        }

        [Authorize(Policy = "RequireModeratorRole")]
        [HttpGet("GetListReports")]
        public async Task<ActionResult<IEnumerable<ReportDto>>> GetListReports([FromQuery] ReportParams reportParams)
        {
            var reports = await _reportRepository.GetListReports(reportParams);
            return Ok(reports);
        }

        [Authorize(Policy = "RequireModeratorRole")]
        [HttpPut("UpdateStatusReport/{reportId}")]
        public async Task<ActionResult> UpdateStatusReport(int reportId,[FromBody] ReportUpdateDto reportUpdateDto)
        {
            var report = await _reportRepository.GetReport(reportId);
            if (report == null) return NotFound();

            report.Status = reportUpdateDto.Status;
            report.ReportTime = DateTime.Now;
            report.HandlerId = User.GetUserId();

            if (await _reportRepository.Save()) return Ok(report);
            return BadRequest("Failed to update status report");
        }
    }
}
