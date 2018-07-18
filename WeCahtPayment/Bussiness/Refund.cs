using WeCahtPayment.Util;

namespace WeCahtPayment.Bussiness
{
    public static class Refund
    {
        /// <summary>
        /// 申请退款完整业务流程逻辑
        /// </summary>
        /// <param name="transactionId">微信订单号（优先使用）</param>
        /// <param name="outTradeNo">商户订单号</param>
        /// <param name="totalFee">订单总金额</param>
        /// <param name="refundFee">退款金额</param>
        /// <returns>退款结果（xml格式）</returns>
        public static string Run(string transactionId, string outTradeNo, string totalFee, string refundFee)
        {
            LogHelper.Info("Refund", "Refund is processing...");

            WxPayData data = new WxPayData();
            if (!string.IsNullOrEmpty(transactionId))//微信订单号存在的条件下，则已微信订单号为准
            {
                data.SetValue("transaction_id", transactionId);
            }
            else//微信订单号不存在，才根据商户订单号去退款
            {
                data.SetValue("out_trade_no", outTradeNo);
            }

            data.SetValue("total_fee", int.Parse(totalFee));//订单总金额
            data.SetValue("refund_fee", int.Parse(refundFee));//退款金额
            data.SetValue("out_refund_no", WxPayApi.GenerateOutTradeNo());//随机生成商户退款单号
            data.SetValue("op_user_id", WxPayConfig.MchId);//操作员，默认为商户号

            WxPayData result = WxPayApi.Refund(data);//提交退款申请给API，接收返回数据

            LogHelper.Info("Refund", "Refund process complete, result : " + result.ToXml());
            return result.ToPrintStr();
        }
    }
}