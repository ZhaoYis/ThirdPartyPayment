//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using ICBCPayment;

//namespace ThirdPartyPaymentUI.ICBC
//{
//    public partial class ICBCPay : System.Web.UI.Page
//    {
//        protected ICBCConfig icmcModel = new ICBCConfig();

//        protected ICBCOrderInfo icbcOrderInfo = new ICBCOrderInfo();

//        protected void Page_Load(object sender, EventArgs e)
//        {
//            StringBuilder strXml = new StringBuilder();
//            //TODO 获取订单信息
//            OrderInfoEntity order = icbcOrderInfo.GetOrderInfo();//OrderFactory.GetOrderInfo();

//            strXml.Append("<?xml version=\"1.0\" encoding=\"GBK\" standalone=\"no\"?>");
//            strXml.Append("<B2CReq>");
//                //接口名称
//                strXml.Append("<interfaceName>" + icmcModel.InterfaceName + "</interfaceName>");
//                //接口版本号
//                strXml.Append("<interfaceVersion>" + icmcModel.InterfaceVersion + "</interfaceVersion>");

//                //订单信息
//                strXml.Append("<orderInfo>");
//                    //交易日期时间
//                    strXml.Append("<orderDate>" + order.PayDate + "</orderDate>");
//                    //支付币种
//                    strXml.Append("<curType>" + icmcModel.CurType + "</curType>");
//                    //商户代码
//                    strXml.Append("<merID>" + icmcModel.MerID + "</merID>");
//                    //订单信息列表
//                    strXml.Append("<subOrderInfoList>");
//                        //订单信息
//                        strXml.Append("<subOrderInfo>");
//                            //订单编号
//                            strXml.Append("<orderid>" + order.OrderId + "</orderid>");
//                            //订单金额
//                            strXml.Append("<amount>" + order.Amount + "</amount>");
//                            //分期付款期数 1代表全额付款
//                            strXml.Append("<installmentTimes>1</installmentTimes>");
//                            //商户账号
//                            strXml.Append("<merAcct>" + icmcModel.MerAcct + "</merAcct>");
//                            //商品编号
//                            strXml.Append("<goodsID>" + order.OrderNo + "</goodsID>");
//                            //商品名称
//                            strXml.Append("<goodsName>" + order.OrderName + "</goodsName>");
//                            //商品数量
//                            strXml.Append("<goodsNum>" + order.OrderNum + "</goodsNum>");
//                            //已含运费金额
//                            strXml.Append("<carriageAmt></carriageAmt>");
//                        strXml.Append("</subOrderInfo>");
//                    strXml.Append("</subOrderInfoList>");
//                strXml.Append("</orderInfo>");

//            strXml.Append("<custom>");
//                //检验联名标志 取值“1”：客户支付时，网银判断该客户是否与商户联名
//                strXml.Append("<verifyJoinFlag>" + icmcModel.VerifyJoinFlag + "</verifyJoinFlag>");
//                //语言版本 取值：“EN_US”为英文版；取值：“ZH_CN”或其他为中文版
//                strXml.Append("<Language>ZH_CN</Language>");
//            strXml.Append("</custom>");

//            strXml.Append("<message>");
//                //支持订单支付的银行卡种类
//                strXml.Append("<creditType>2</creditType>");
//                //通知类型
//                strXml.Append("<notifyType>" + icmcModel.NotifyType + "</notifyType>");
//                //结果发送类型
//                strXml.Append("<resultType>" + icmcModel.ResultType + "</resultType>");
//                //商户reference
//                strXml.Append("<merReference>" + icmcModel.MerReference + "</merReference>");
//                //客户端IP 当商户reference项送空时，该项必输
//                strXml.Append("<merCustomIp>"+ icmcModel.MerIP + "</merCustomIp>");
//                //虚拟商品/实物商品标志位 取值“0”：虚拟商品 取值“1”，实物商品
//                strXml.Append("<goodsType>1</goodsType>");
//                //买家用户号
//                strXml.Append("<merCustomID>" + order.MerCustomId + "</merCustomID>");
//                //买家联系电话
//                strXml.Append("<merCustomPhone>" + order.MerCustomPhone + "</merCustomPhone>");
//                //收货地址
//                strXml.Append("<goodsAddress>" + order.GoodsAddress + "</goodsAddress>");
//                //订单备注
//                strXml.Append("<merOrderRemark>" + order.MerOrderRemark + "</merOrderRemark>");
//                //商城提示
//                strXml.Append("<merHint>" + order.MerHint + "</merHint>");
//                //备注字段1
//                strXml.Append("<remark1>" + order.Remark1 + "</remark1>");
//                //备注字段2
//                strXml.Append("<remark2>" + order.Remark2 + "</remark2>");
//                //返回商户URL
//                strXml.Append("<merURL>" + icmcModel.MerUrl + "</merURL>");
//                //返回商户变量
//                strXml.Append("<merVAR>" + order.OrderId + "</merVAR>");
//            strXml.Append("</message>");

//            strXml.Append("</B2CReq>");

//            icmcModel.TranData = strXml.ToString();

//            icmcModel = ICBCPayOnline.GetCheckInfo(icmcModel);
//        }
//    }
//}