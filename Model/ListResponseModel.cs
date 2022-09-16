namespace VerficationApi.Model
{
    public class ListResponseModel
    {
        public int X_totalcount { get; set; }
        public int x_pagesize { get; set; }
        public int x_totalpages { get; set; }
        public List<ResponseModel>? ResponseModel { get; set; }
    }
}
