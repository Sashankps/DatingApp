namespace API.Helpers
{
    public class PaginationParams
    {

        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;

        //giving the option to the user to select max number of items to display in one page. By default 10 
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}