using System;

namespace ICBCPayment
{
    /// <summary>
    /// 订单实体
    /// </summary>
    public class OrderInfoEntity
    {
        /// <summary>
        /// 买家用户号(用户ID)
        /// </summary>
        public string MerCustomId { get; set; }
        /// <summary>
        /// 订单用户姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 订单总金额，单位：分
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public string PayDate { get; set; } = DateTime.Now.ToString("yyyyMMddHHmmss");
        /// <summary>
        /// 商品编号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string OrderName { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public string OrderNum { get; set; }
        /// <summary>
        /// 买家联系电话
        /// </summary>
        public string MerCustomPhone { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string GoodsAddress { get; set; }
        /// <summary>
        /// 订单备注
        /// </summary>
        public string MerOrderRemark { get; set; }
        /// <summary>
        /// 商城提示
        /// </summary>
        public string MerHint { get; set; }
        /// <summary>
        /// 备注字段1
        /// </summary>
        public string Remark1 { get; set; }
        /// <summary>
        /// 备注字段2
        /// </summary>
        public string Remark2 { get; set; }
    }
}