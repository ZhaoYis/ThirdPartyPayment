﻿using System.Threading;
using WeCahtPayment.Util;

namespace WeCahtPayment.Bussiness
{
    public static class MicroPay
    {
        /// <summary>
        /// 刷卡支付完整业务流程逻辑
        /// </summary>
        /// <param name="body">商品描述</param>
        /// <param name="totalFee">总金额</param>
        /// <param name="authCode">支付授权码</param>
        /// <returns>刷卡支付结果</returns>
        public static string Run(string body, string totalFee, string authCode)
        {
            LogHelper.Info("MicroPay", "Micropay is processing...");

            WxPayData data = new WxPayData();
            data.SetValue("auth_code", authCode);//授权码
            data.SetValue("body", body);//商品描述
            data.SetValue("total_fee", int.Parse(totalFee));//总金额
            data.SetValue("out_trade_no", WxPayApi.GenerateOutTradeNo());//产生随机的商户订单号

            WxPayData result = WxPayApi.Micropay(data, 10); //提交被扫支付，接收返回结果

            //如果提交被扫支付接口调用失败，则抛异常
            if (!result.IsSet("return_code") || result.GetValue("return_code").ToString() == "FAIL")
            {
                string returnMsg = result.IsSet("return_msg") ? result.GetValue("return_msg").ToString() : "";
                LogHelper.Error("MicroPay", "Micropay API interface call failure, result : " + result.ToXml());
                throw new WxPayException("Micropay API interface call failure, return_msg : " + returnMsg);
            }

            //签名验证
            result.CheckSign();
            LogHelper.Debug("MicroPay", "Micropay response check sign success");

            //刷卡支付直接成功
            if (result.GetValue("return_code").ToString() == "SUCCESS" &&
                result.GetValue("result_code").ToString() == "SUCCESS")
            {
                LogHelper.Debug("MicroPay", "Micropay business success, result : " + result.ToXml());
                return result.ToPrintStr();
            }

            /******************************************************************
             * 剩下的都是接口调用成功，业务失败的情况
             * ****************************************************************/
            //1）业务结果明确失败
            if (result.GetValue("err_code").ToString() != "USERPAYING" &&
            result.GetValue("err_code").ToString() != "SYSTEMERROR")
            {
                LogHelper.Error("MicroPay", "micropay API interface call success, business failure, result : " + result.ToXml());
                return result.ToPrintStr();
            }

            //2）不能确定是否失败，需查单
            //用商户订单号去查单
            string out_trade_no = data.GetValue("out_trade_no").ToString();

            //确认支付是否成功,每隔一段时间查询一次订单，共查询10次
            int queryTimes = 10;//查询次数计数器
            while (queryTimes-- > 0)
            {
                int succResult = 0;//查询结果
                WxPayData queryResult = Query(out_trade_no, out succResult);
                //如果需要继续查询，则等待2s后继续
                if (succResult == 2)
                {
                    Thread.Sleep(2000);
                    continue;
                }
                //查询成功,返回订单查询接口返回的数据
                else if (succResult == 1)
                {
                    LogHelper.Debug("MicroPay", "Mircopay success, return order query result : " + queryResult.ToXml());
                    return queryResult.ToPrintStr();
                }
                //订单交易失败，直接返回刷卡支付接口返回的结果，失败原因会在err_code中描述
                else
                {
                    LogHelper.Error("MicroPay", "Micropay failure, return micropay result : " + result.ToXml());
                    return result.ToPrintStr();
                }
            }

            //确认失败，则撤销订单
            LogHelper.Error("MicroPay", "Micropay failure, Reverse order is processing...");
            if (!Cancel(out_trade_no))
            {
                LogHelper.Error("MicroPay", "Reverse order failure");
                throw new WxPayException("Reverse order failure！");
            }

            return result.ToPrintStr();
        }

        /// <summary>
        /// 查询订单情况
        /// </summary>
        /// <param name="outTradeNo">商户订单号</param>
        /// <param name="succCode">查询订单结果：0-表示订单不成功，1-表示订单成功，2-表示继续查询</param>
        /// <returns>订单查询接口返回的数据，参见协议接口</returns>
        private static WxPayData Query(string outTradeNo, out int succCode)
        {
            WxPayData queryOrderInput = new WxPayData();
            queryOrderInput.SetValue("out_trade_no", outTradeNo);
            WxPayData result = WxPayApi.OrderQuery(queryOrderInput);

            if (result.GetValue("return_code").ToString() == "SUCCESS"
                && result.GetValue("result_code").ToString() == "SUCCESS")
            {
                //支付成功
                if (result.GetValue("trade_state").ToString() == "SUCCESS")
                {
                    succCode = 1;
                    return result;
                }
                //用户支付中，需要继续查询
                else if (result.GetValue("trade_state").ToString() == "USERPAYING")
                {
                    succCode = 2;
                    return result;
                }
            }

            //如果返回错误码为“此交易订单号不存在”则直接认定失败
            if (result.GetValue("err_code").ToString() == "ORDERNOTEXIST")
            {
                succCode = 0;
            }
            else
            {
                //如果是系统错误，则后续继续
                succCode = 2;
            }
            return result;
        }

        /// <summary>
        /// 撤销订单，如果失败会重复调用10次
        /// </summary>
        /// <param name="outTradeNo">商户订单号</param>
        /// <param name="depth">调用次数，这里用递归深度表示</param>
        /// <returns>false表示撤销失败，true表示撤销成功</returns>
        private static bool Cancel(string outTradeNo, int depth = 0)
        {
            if (depth > 10)
            {
                return false;
            }

            WxPayData reverseInput = new WxPayData();
            reverseInput.SetValue("out_trade_no", outTradeNo);
            WxPayData result = WxPayApi.Reverse(reverseInput);

            //接口调用失败
            if (result.GetValue("return_code").ToString() != "SUCCESS")
            {
                return false;
            }

            //如果结果为success且不需要重新调用撤销，则表示撤销成功
            if (result.GetValue("result_code").ToString() != "SUCCESS" && result.GetValue("recall").ToString() == "N")
            {
                return true;
            }
            else if (result.GetValue("recall").ToString() == "Y")
            {
                return Cancel(outTradeNo, ++depth);
            }
            return false;
        }
    }
}