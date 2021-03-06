namespace Skymate.Application.Dto
{
    using System;
    using System.Collections.Generic;

    using Skymate.Application;

    /// <summary>
    /// This class can be used to return a paged list from an <see cref="IApplicationService"/> method.
    /// </summary>
    /// <typeparam name="T">Type of the items in the <see cref="IApListResultDto{T} list</typeparam>
    [Serializable]
    public class PagedResultOutput<T> : PagedResultDto<T>, IOutputDto
    {
        /// <summary>
        /// Creates a new <see cref="PagedResultOutput{T}"/> object.
        /// </summary>
        public PagedResultOutput()
        {

        }

        /// <summary>
        /// Creates a new <see cref="PagedResultOutput{T}"/> object.
        /// </summary>
        /// <param name="totalCount">Total count of Items</param>
        /// <param name="items">List of items in current page</param>
        public PagedResultOutput(int totalCount, IReadOnlyList<T> items)
            : base(totalCount, items)
        {

        }
    }
}