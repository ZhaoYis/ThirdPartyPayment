using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using ThoughtWorks.QRCode.Codec;

namespace ThirdPartyPaymentUI.WeChat
{
    public partial class MakeQRCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["data"]))
            {
                string str = Request.QueryString["data"];

                //初始化二维码生成工具
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder
                {
                    QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE,
                    QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M,
                    QRCodeVersion = 0,
                    QRCodeScale = 4
                };

                //将字符串生成二维码图片
                Bitmap image = qrCodeEncoder.Encode(str, Encoding.Default);

                //保存为PNG到内存流  
                MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.Png);

                //输出二维码图片
                Response.BinaryWrite(ms.GetBuffer());
                Response.End();
            }
        }
    }
}