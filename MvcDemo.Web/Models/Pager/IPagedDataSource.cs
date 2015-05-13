namespace MvcDemo.Web.Services
{
    public interface IPagedDataSource
    {
        int PageIndex
        {
            get;
        }

        int PageSize
        {
            get;
        }

        int TotalItemCount
        {
            get;
        }
    }
}