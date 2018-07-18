using System;
using System.Web;
using WeCahtPayment.Bussiness;
using WeCahtPayment.Util;

namespace ThirdPartyPaymentUI.WeChat
{
    public partial class NativePayPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LogHelper.Info(this.GetType().ToString(), "page load");

            NativePay nativePay = new NativePay();

            //生成扫码支付模式一url
            string url1 = nativePay.GetPrePayUrl("123456789");

            //生成扫码支付模式二url
            //TODO 根据productId获取产品信息
            //string url2 = nativePay.GetPayUrl("123456789", "牛肉干", "", "1000", "meet");

            //将url生成二维码图片
            Image1.ImageUrl = "MakeQRCode.aspx?data=" + HttpUtility.UrlEncode(url1);
            //Image2.ImageUrl = "MakeQRCode.aspx?data=" + HttpUtility.UrlEncode(url2);
        }
    }
}