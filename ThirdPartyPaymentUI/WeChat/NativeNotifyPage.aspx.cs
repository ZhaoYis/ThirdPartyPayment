using System;
using WeCahtPayment.Bussiness;

namespace ThirdPartyPaymentUI.WeChat
{
    public partial class NativeNotifyPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            NativeNotify nativeNatify = new NativeNotify(this);
            nativeNatify.ProcessNotify();
        }
    }
}