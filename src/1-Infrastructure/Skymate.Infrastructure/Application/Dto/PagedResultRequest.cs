using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skymate.Application.Dto
{
    public class PagedResultRequest:IPagedResultRequest
    {
        public int MaxResultCount { get; set; }

        public int SkipCount { get; set; }

        public PagedResultRequest(int skip,int maxResultCount)
        {
            this.SkipCount = skip;
            this.MaxResultCount = maxResultCount;
        }

    }
}
