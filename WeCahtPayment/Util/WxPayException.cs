using System;

namespace WeCahtPayment.Util
{
    public class WxPayException : Exception 
    {
        /// <inheritdoc />
        /// <summary>
        /// 自定义异常
        /// </summary>
        /// <param name="msg">异常信息</param>
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}