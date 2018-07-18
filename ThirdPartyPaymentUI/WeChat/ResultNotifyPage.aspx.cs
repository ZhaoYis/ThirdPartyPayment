using System;
using WeCahtPayment.Bussiness;

namespace ThirdPartyPaymentUI.WeChat
{
    public partial class ResultNotifyPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ResultNotify resultNotify = new ResultNotify(this);
            resultNotify.ProcessNotify();
        }
    }
}