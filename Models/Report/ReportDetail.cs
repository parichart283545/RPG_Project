namespace RPG_Project.Models.Report
{
    public class ReportDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ReportHeaderId { get; set; }
        public ReportHeader ReportHeader { get; set; }
    }
}