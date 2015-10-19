namespace Skymate
{
    public class PagingInput
    {
        /// <summary>
        /// Gets the page size.
        /// </summary>
        public int PageSize { get;private set; }

        /// <summary>
        /// Gets the page index.
        /// </summary>
        public int PageIndex { get;private set; }

        /// <summary>
        /// Prevents a default instance of the <see cref="PagingInput"/> class from being created.
        /// </summary>
        private PagingInput()
        {
            
        }

        /// <summary>
        /// The create paging input.
        /// </summary>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="pageIndex">
        /// The page index.
        /// </param>
        /// <returns>
        /// The <see cref="PagingInput"/>.
        /// </returns>
        public static PagingInput CreatePagingInput(int pageSize, int pageIndex)
        {
            return new PagingInput { PageSize = pageSize, PageIndex = pageIndex };
        }
    }
}