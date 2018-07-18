using System;
using WeCahtPayment.Util;

namespace ThirdPartyPaymentUI.WeChat
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LogHelper.Info(this.GetType().ToString(), "page load");
        }
    }
}