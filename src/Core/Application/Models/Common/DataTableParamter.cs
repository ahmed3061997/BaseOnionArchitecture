namespace Application.Models.Common
{
    public class DataTableParameters
    {
        public string Search { get; set; }
        public string Draw { get; set; }
        public string Order { get; set; }
        public string OrderDir { get; set; }
        public int StartRec { get; set; }
        public int PageSize { get; set; }
    }
}
