namespace SWP391.Project.Models
{
    public class ResponseModel<DataModel>
    {
        public string? ErrorCode { get; set; } = default;
        public DataModel? Data { get; set; } = default;
        public bool Success { get; set; }
    }
}