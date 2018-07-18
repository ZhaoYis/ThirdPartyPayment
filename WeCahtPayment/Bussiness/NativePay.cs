using System;
using System.Collections.Generic;
using WeCahtPayment.Util;

namespace WeCahtPayment.Bussiness
{
    public class NativePay
    {
        /// <summary>
        /// 生成扫描支付模式一URL
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns>模式一URL</returns>
        public string GetPrePayUrl(string productId)
        {
            LogHelper.Info(this.GetType().ToString(), "Native pay mode 1 url is producing...");

            WxPayData data = new WxPayData();
            data.SetValue("appid", WxPayConfig.AppId);//公众帐号id
            data.SetValue("mch_id", WxPayConfig.MchId);//商户号
            data.SetValue("time_stamp", WxPayApi.GenerateTimeStamp());//时间戳
            data.SetValue("nonce_str", WxPayApi.GenerateNonceStr());//随机字符串
            data.SetValue("product_id", productId);//商品ID
            data.SetValue("sign", data.MakeSign());//签名
            string str = ToUrlParams(data.GetValues());//转换为URL串
            string url = "weixin://wxpay/bizpayurl?" + str;
            //weixin://wxpay/bizpayurl?appid=wx426b3015555a46be&mch_id=1900009851&nonce_str=lz2gmz19sbeteufbg2onmbt9sabv0kef&product_id=123456789&time_stamp=1531917401&sign=69C6AEFFF6943AADCF84C4FA372BE60D
            LogHelper.Info(this.GetType().ToString(), "Get native pay mode 1 url : " + url);
            return url;
        }

        /// <summary>
        /// 生成直接支付url，支付url有效期为2小时,模式二
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="body">商品描述</param>
        /// <param name="attach">附加数据</param>
        /// <param name="totalFee">总金额</param>
        /// <param name="goodsTag">商品标记</param>
        /// <returns>模式二URL</returns>
        public string GetPayUrl(string productId, string body, string attach, string totalFee, string goodsTag)
        {
            LogHelper.Info(this.GetType().ToString(), "Native pay mode 2 url is producing...");

            WxPayData data = new WxPayData();
            data.SetValue("body", body);//商品描述
            data.SetValue("attach", attach);//附加数据
            data.SetValue("out_trade_no", WxPayApi.GenerateOutTradeNo());//随机字符串
            data.SetValue("total_fee", totalFee);//总金额
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
            data.SetValue("goods_tag", goodsTag);//商品标记
            data.SetValue("trade_type", "NATIVE");//交易类型
            data.SetValue("product_id", productId);//商品ID

            WxPayData result = WxPayApi.UnifiedOrder(data);//调用统一下单接口
            string url = result.GetValue("code_url").ToString();//获得统一下单接口返回的二维码链接

            LogHelper.Info(this.GetType().ToString(), "Get native pay mode 2 url : " + url);
            return url;
        }
        
        /// <summary>
        /// 参数数组转换为url格式
        /// </summary>
        /// <param name="map">参数名与参数值的映射表</param>
        /// <returns>URL字符串</returns>
        private string ToUrlParams(SortedDictionary<string, object> map)
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in map)
            {
                buff += pair.Key + "=" + pair.Value + "&";
            }
            buff = buff.Trim('&');
            return buff;
        }
    }
}