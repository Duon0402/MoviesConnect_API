namespace API.DTOs.Admin
{
    public class StatisticsDto
    {
        public int TotalMovies { get; set; }
        public int TotalUsers { get; set; }
        public int TotalReports { get; set; }
        public int TotalReportUnprocesseds {  get; set; }
        public int TotalReportProcesseds { get; set; }
        public int TotalGenres { get; set; }
        public int TotalCertifications { get; set; }
    }
}
