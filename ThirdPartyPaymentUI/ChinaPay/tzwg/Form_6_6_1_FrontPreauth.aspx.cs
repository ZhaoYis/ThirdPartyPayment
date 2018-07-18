﻿using System;
using System.Collections.Generic;
using System.Text;
using ChinaPayment.Util;

namespace ThirdPartyPaymentUI.ChinaPay
{
    public partial class Form_6_6_1_FrontPreauth : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /**
             * 重要：联调测试时请仔细阅读注释！
             * 
             * 产品：跳转网关支付产品<br>
             * 交易：预授权：前台跳转，有前台通知应答和后台通知应答<br>
             * 日期： 2015-09<br>
             * 版本： 1.0.0
             * 版权： 中国银联<br>
             * 说明：以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己需要，按照技术文档编写。该代码仅供参考，不提供编码性能规范性等方面的保障<br>
             * 提示：该接口参考文档位置：open.unionpay.com帮助中心 下载  产品接口规范  《网关支付产品接口规范》，<br>
             *              《平台接入接口规范-第5部分-附录》（内包含应答码接口规范，全渠道平台银行名称-简码对照表)<br>
             *              《全渠道平台接入接口规范 第3部分 文件接口》（对账文件格式说明）<br>
             * 测试过程中的如果遇到疑问或问题您可以：1）优先在open平台中查找答案：
             * 							        调试过程中的问题或其他问题请在 https://open.unionpay.com/ajweb/help/faq/list 帮助中心 FAQ 搜索解决方案
             *                             测试过程中产生的7位应答码问题疑问请在https://open.unionpay.com/ajweb/help/respCode/respCodeList 输入应答码搜索解决方案
             *                          2） 咨询在线人工支持： open.unionpay.com注册一个用户并登陆在右上角点击“在线客服”，咨询人工QQ测试支持。
             * 交易说明:1）以后台通知或交易状态查询交易确定交易成功,前台通知不能作为判断成功的标准.
             *       2）交易状态查询交易（Form_6_5_Query）建议调用机制：前台类交易间隔（5分、10分、30分、60分、120分）发起交易查询，如果查询到结果成功，则不用再查询。（失败，处理中，查询不到订单均可能为中间状态）。也可以建议商户使用payTimeout（支付超时时间），过了这个时间点查询，得到的结果为最终结果。
             */

            Dictionary<string, string> param = new Dictionary<string, string>();

            //以下信息非特殊情况不需要改动
            param["version"] = SdkConfig.Version;//版本号
            param["encoding"] = "UTF-8";//编码方式
            param["txnType"] = "02";//交易类型
            param["txnSubType"] = "01";//交易子类
            param["bizType"] = "000201";//业务类型
            param["signMethod"] =SdkConfig.SignMethod;//签名方法
            param["channelType"] = "08";//渠道类型
            param["accessType"] = "0";//接入类型
            param["frontUrl"] = SdkConfig.FrontUrl;  //前台通知地址      
            param["backUrl"] = SdkConfig.BackUrl;  //后台通知地址
            param["currencyCode"] = "156";//交易币种

            // 订单超时时间。
            // 超过此时间后，除网银交易外，其他交易银联系统会拒绝受理，提示超时。 跳转银行网银交易如果超时后交易成功，会自动退款，大约5个工作日金额返还到持卡人账户。
            // 此时间建议取支付时的北京时间加15分钟。
            // 超过超时时间调查询接口应答origRespCode不是A6或者00的就可以判断为失败。
            param["payTimeout"] = DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss");


            //TODO 以下信息需要填写
            param["merId"] = Request.Form["merId"].ToString();//商户号，请改自己的测试商户号，此处默认取demo演示页面传递的参数
            param["orderId"] = Request.Form["orderId"].ToString();//商户订单号，8-32位数字字母，不能含“-”或“_”，此处默认取demo演示页面传递的参数，可以自行定制规则
            param["txnTime"] = Request.Form["txnTime"].ToString();//订单发送时间，格式为YYYYMMDDhhmmss，取北京时间，此处默认取demo演示页面传递的参数，参考取法： DateTime.Now.ToString("yyyyMMddHHmmss")
            param["txnAmt"] = Request.Form["txnAmt"].ToString();//交易金额，单位分，此处默认取demo演示页面传递的参数

            // 请求方保留域，
            // 透传字段，查询、通知、对账文件中均会原样出现，如有需要请启用并修改自己希望透传的数据。
            // 出现部分特殊字符时可能影响解析，请按下面建议的方式填写：
            // 1. 如果能确定内容不会出现&={}[]"'等符号时，可以直接填写数据，建议的方法如下。
            //param["reqReserved"] = "透传信息1|透传信息2|透传信息3";
            // 2. 内容可能出现&={}[]"'符号时：
            // 1) 如果需要对账文件里能显示，可将字符替换成全角＆＝｛｝【】“‘字符（自己写代码，此处不演示）；
            // 2) 如果对账文件没有显示要求，可做一下base64（如下）。
            //    注意控制数据长度，实际传输的数据长度不能超过1024位。
            //    查询、通知等接口解析时使用System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reqReserved))解base64后再对数据做后续解析。
            //param["reqReserved"] = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("任意格式的信息都可以"));

            //TODO 其他特殊用法请查看 ~/ChinaPay/Pages/Template/special_use_preauth.htm

            AcpService.Sign(param, System.Text.Encoding.UTF8);
            string html = AcpService.CreateAutoFormHtml(SdkConfig.FrontTransUrl, param, System.Text.Encoding.UTF8);// 将SDKUtil产生的Html文档写入页面，从而引导用户浏览器重定向   
            Response.ContentEncoding = Encoding.UTF8; // 指定输出编码
            Response.Write(html);
 
        }
    }
}