using System;
using System.Web.UI;
using WeCahtPayment.Util;

namespace WeCahtPayment.Bussiness
{
    /// <summary>
    /// 扫码支付模式一回调处理类
    /// 接收微信支付后台发送的扫码结果，调用统一下单接口并将下单结果返回给微信支付后台
    /// </summary>
    public class NativeNotify : Notify
    {
        public NativeNotify(Page page) : base(page)
        {

        }

        /// <summary>
        /// 处理通知
        /// </summary>
        public override void ProcessNotify()
        {
            WxPayData notifyData = GetNotifyData();

            //检查openid和product_id是否返回
            if (!notifyData.IsSet("openid") || !notifyData.IsSet("product_id"))
            {
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "回调数据异常");
                LogHelper.Info(this.GetType().ToString(), "The data WeChat post is error : " + res.ToXml());
                page.Response.Write(res.ToXml());
                page.Response.End();
            }

            //调统一下单接口，获得下单结果
            string openid = notifyData.GetValue("openid").ToString();
            string product_id = notifyData.GetValue("product_id").ToString();
            WxPayData unifiedOrderResult = new WxPayData();
            try
            {
                unifiedOrderResult = UnifiedOrder(openid, product_id);
            }
            catch (Exception ex)//若在调统一下单接口时抛异常，立即返回结果给微信支付后台
            {
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "统一下单失败");
                LogHelper.Error(this.GetType().ToString(), "UnifiedOrder failure : " + res.ToXml() + "  异常信息：" + ex.Message);
                page.Response.Write(res.ToXml());
                page.Response.End();
            }

            //若下单失败，则立即返回结果给微信支付后台
            if (!unifiedOrderResult.IsSet("appid") || !unifiedOrderResult.IsSet("mch_id") || !unifiedOrderResult.IsSet("prepay_id"))
            {
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "统一下单失败");
                LogHelper.Error(this.GetType().ToString(), "UnifiedOrder failure : " + res.ToXml());
                page.Response.Write(res.ToXml());
                page.Response.End();
            }

            //统一下单成功,则返回成功结果给微信支付后台
            WxPayData data = new WxPayData();
            data.SetValue("return_code", "SUCCESS");
            data.SetValue("return_msg", "OK");
            data.SetValue("appid", WxPayConfig.AppId);
            data.SetValue("mch_id", WxPayConfig.MchId);
            data.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
            data.SetValue("prepay_id", unifiedOrderResult.GetValue("prepay_id"));
            data.SetValue("result_code", "SUCCESS");
            data.SetValue("err_code_des", "OK");
            data.SetValue("sign", data.MakeSign());

            LogHelper.Info(this.GetType().ToString(), "UnifiedOrder success , send data to WeChat : " + data.ToXml());
            page.Response.Write(data.ToXml());
            page.Response.End();
        }

        /// <summary>
        /// 统一下单
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        private WxPayData UnifiedOrder(string openId, string productId)
        {
            //TODO 根据productId获取产品信息

            //统一下单
            WxPayData req = new WxPayData();
            req.SetValue("body", "广东雅达电子股份有限公司");
            req.SetValue("attach", "附加信息,用于后台或者存入数据库,做自己的判断");
            req.SetValue("out_trade_no", WxPayApi.GenerateOutTradeNo());
            req.SetValue("total_fee", 1);
            req.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            req.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            req.SetValue("goods_tag", "商品的备忘,可以自定义");
            req.SetValue("trade_type", "NATIVE");
            req.SetValue("openid", openId);
            req.SetValue("product_id", productId);
            WxPayData result = WxPayApi.UnifiedOrder(req);
            return result;
        }
    }
}