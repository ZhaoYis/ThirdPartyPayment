using System.Collections.Generic;

namespace ICBCPayment
{
    /// <summary>
    /// ICBC订单信息
    /// </summary>
    public abstract class BaseICBCOrderInfo
    {
        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <returns></returns>
        public abstract OrderInfoEntity GetOrderInfo();
        /// <summary>
        /// 获取支付订单列表
        /// </summary>
        /// <returns></returns>
        public abstract List<OrderInfoEntity> GetOrderPayList();
        /// <summary>
        /// 根据订单号查询订单
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="userIdCardNumber"></param>
        /// <returns></returns>
        public abstract OrderInfoEntity GetOrderModel(string orderId,string userIdCardNumber);
    }
}