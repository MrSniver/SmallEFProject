namespace WorkerManagementApi.Application.Common.Pagination
{
    public static class PagedResultExtensions
    {
        public static PagedResult<T> GetPaged<T>(this IQueryable<T> query, int page, int pageSize = 20) where T : class
        {
            var result = new PagedResult<T>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            if (result.CurrentPage > result.PageCount)
            {
                result.CurrentPage = result.PageCount;
                page = result.PageCount;
            }

            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }

        public static PagedResult<T> GetPaged<T>(this IEnumerable<T> query, int page, int pageSize = 20) where T : class
        {
            var result = new PagedResult<T>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            if (result.CurrentPage > result.PageCount)
            {
                result.CurrentPage = result.PageCount;
                page = result.PageCount;
            }

            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }

        public static PagedResult<T> GetPaged<T>(this ICollection<T> query, int page, int pageSize = 20) where T : class
        {
            var result = new PagedResult<T>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            if (result.CurrentPage > result.PageCount)
            {
                result.CurrentPage = result.PageCount;
                page = result.PageCount;
            }

            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }
    }
}
