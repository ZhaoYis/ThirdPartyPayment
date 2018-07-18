using WeCahtPayment.Util;

namespace WeCahtPayment.Bussiness
{
    /// <summary>
    /// 下载账单
    /// </summary>
    public static class DownloadBill
    {
        /// <summary>
        /// 下载对账单完整业务流程逻辑
        /// </summary>
        /// <param name="billDate">下载对账单的日期（格式：20140603，一次只能下载一天的对账单）</param>
        /// <param name="billType">账单类型 ALL-返回当日所有订单信息，默认值  SUCCESS-返回当日成功支付的订单  REFUND-返回当日退款订单  REVOKED-已撤销的订单</param>
        /// <returns></returns>
        public static string Run(string billDate, string billType)
        {
            LogHelper.Info("DownloadBill", "DownloadBill is processing...");

            WxPayData data = new WxPayData();
            data.SetValue("bill_date", billDate);//账单日期
            data.SetValue("bill_type", billType);//账单类型
            WxPayData result = WxPayApi.DownloadBill(data);//提交下载对账单请求给API，接收返回结果

            LogHelper.Info("DownloadBill", "DownloadBill process complete, result : " + result.ToXml());
            return result.ToPrintStr();
        }
    }
}