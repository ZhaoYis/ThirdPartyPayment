using WeCahtPayment.Util;

namespace WeCahtPayment.Bussiness
{
    /// <summary>
    /// 订单查询
    /// </summary>
    public static class OrderQuery
    {
        /// <summary>
        /// 订单查询完整业务流程逻辑
        /// </summary>
        /// <param name="transactionId">微信订单号（优先使用）</param>
        /// <param name="outTradeNo">商户订单号</param>
        /// <returns>订单查询结果（xml格式）</returns>
        public static string Run(string transactionId, string outTradeNo)
        {
            LogHelper.Info("OrderQuery", "OrderQuery is processing...");

            WxPayData data = new WxPayData();
            if (!string.IsNullOrEmpty(transactionId))//如果微信订单号存在，则以微信订单号为准
            {
                data.SetValue("transaction_id", transactionId);
            }
            else//微信订单号不存在，才根据商户订单号去查单
            {
                data.SetValue("out_trade_no", outTradeNo);
            }

            WxPayData result = WxPayApi.OrderQuery(data);//提交订单查询请求给API，接收返回数据

            LogHelper.Info("OrderQuery", "OrderQuery process complete, result : " + result.ToXml());
            return result.ToPrintStr();
        }
    }
}