using SWP391.Project.Models.Dtos.Pagination;

namespace SWP391.Project.Models.Dtos.Account
{
    public class FilterAccountDto : PaginationRequestDto
    {
        public string SearchKey { get; set; } = string.Empty;
    }
}