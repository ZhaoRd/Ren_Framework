/*
namespace Zhaord.Web.Framework.Security
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    public class AdminValidateIpAddressAttribute : ActionFilterAttribute
    {

		public Lazy<IWebHelper> WebHelper { get; set; }
		public Lazy<SecuritySettings> SecuritySettings { get; set; }
		
		public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null || filterContext.HttpContext == null)
                return;

            HttpRequestBase request = filterContext.HttpContext.Request;
            if (request == null)
                return;

            //don't apply filter to child methods
            if (filterContext.IsChildAction)
                return;

            bool ok = false;
			var ipAddresses = this.SecuritySettings.Value.AdminAreaAllowedIpAddresses;
            if (ipAddresses != null && ipAddresses.Count > 0)
            {
				var webHelper = this.WebHelper.Value;
                foreach (string ip in ipAddresses)
                    if (ip.Equals(webHelper.GetCurrentIpAddress(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        ok = true;
                        break;
                    }
            }
            else
            {
                //no restrictions
                ok = true;
            }

            if (!ok)
            {
                //ensure that it's not 'Access denied' page
				var webHelper = this.WebHelper.Value;
                var thisPageUrl = webHelper.GetThisPageUrl(false);
                if (!thisPageUrl.StartsWith(string.Format("{0}admin/security/accessdenied", webHelper.GetStoreLocation()), StringComparison.InvariantCultureIgnoreCase))
                {
                    //redirect to 'Access denied' page
                    filterContext.Result = new RedirectResult(webHelper.GetStoreLocation() + "admin/security/accessdenied");
                }
            }
        }
    }
}
*/
