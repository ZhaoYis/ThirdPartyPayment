using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace ICBCPayment
{
    public class ICBCPayOnline
    {
        /// <summary>
        /// 订单金额，单位：分
        /// </summary>
        private static string _amount;

        /// <summary>
        /// 银行证书文件地址
        /// </summary>
        private static readonly string strCertFn = AppDomain.CurrentDomain.BaseDirectory + "Cert_ICBC\\银行证书文件地址.crt";

        /// <summary>
        /// 商户证书文件地址
        /// </summary>
        private static readonly string strCertFNM = AppDomain.CurrentDomain.BaseDirectory + "Cert_ICBC\\商户证书文件地址.crt";

        /// <summary>
        /// 私钥文件地址
        /// </summary>
        private static readonly string strKeyFN = AppDomain.CurrentDomain.BaseDirectory + "Cert_ICBC\\私钥文件地址.key";

        /// <summary>
        /// 商户证书
        /// </summary>
        //private static string cert_path = AppDomain.CurrentDomain.BaseDirectory + "Cert_ICBC\\商户证书.crt";

        /// <summary>
        /// 私钥口令
        /// </summary>
        private static string strKey = "私钥口令";

        /// <summary>
        /// 接口地址
        /// </summary>
        private static string api_url = "https://corporbank.icbc.com.cn/servlet/ICBCINBSEBusinessServlet";
        /// <summary>
        /// 请求参数
        /// </summary>
        private static string post_params = "APIName=EAPI&APIVersion=001.001.002.001&MerReqData=";

        public static void Load()
        {
            System.Threading.Thread t = new System.Threading.Thread(CheckOrder);
            t.Start();
        }

        static ICBCPayOnline()
        {

        }

        /// <summary>
        /// 检查未提交订单
        /// </summary>
        public static void CheckOrder()
        {
            //var query = SpringFactory.BusinessFactory.GetBusinessAnonymousUser();
            //while (true)
            //{
            //    //var list = query.GetOrderPayList();

            //    System.Threading.Thread.Sleep(1000 * 60 * 30);
            //}

            ICBCConfig icbcInfo = new ICBCConfig();
            //TODO var list = query.GetOrderPayList();
            List<OrderInfoEntity> orderInfoList = new List<OrderInfoEntity>();

            foreach (var order in orderInfoList)
            {
                try
                {
                    var user = SpringFactory.BusinessFactory.GetStudent(order.UserName);

                    string mess = CheckOrder(order.OrderId, order.PayDate, icbcInfo.MerID, icbcInfo.MerAcct, out string outMess);
                    if (mess.Length > 5)//缴费成功，未返回错误编码,返回xml数据
                    {
                        DataSet myds = new DataSet();
                        StringReader strReader = new StringReader(mess);
                        myds.ReadXml(strReader);
                        string stat = myds.Tables["out"].Rows[0]["tranStat"].ToString();
                        if (stat == "1" || stat == "0")
                        {
                            _amount = myds.Tables["out"].Rows[0]["amount"].ToString();
                            user.PaymentSucceed(_amount, order.OrderId);
                        }
                        else//支付失败
                        {
                            //SpringFactory.BusinessFactory.GetStudent(order.UserName).OrderFaild(order.OrderId);

                            //TODO 删除订单
                        }
                    }
                    else
                    {
                        string pays = "";
                        switch (mess)
                        {
                            case "40972": pays = "API查询的订单不存在"; break;
                            case "40973": pays = "API查询过程中系统异常"; break;
                            case "40976": pays = "API查询系统异常"; break;
                            case "40977": pays = "商户证书信息错"; break;
                            case "40978": pays = "解包商户请求数据报错"; break;
                            case "40979": pays = "查询的订单不存在"; break;
                            case "40980": pays = "API查询过程中系统异常"; break;
                            case "40981": pays = "给商户打包返回数据错"; break;

                            case "40982": pays = "系统错误"; break;
                            case "40983": pays = "查询的订单不唯一"; break;
                            case "40987": pays = "商户代码或者商城账号有误"; break;
                            case "40947": pays = "给商户打包返回数据错"; break;
                            case "40948": pays = "商城状态非法"; break;
                            case "40949": pays = "商城类别非法"; break;

                            case "40950": pays = "商城应用类别非法"; break;
                            case "40951": pays = "商户证书id状态非法"; break;
                            case "40952": pays = "商户证书id未绑定"; break;
                            case "40953": pays = "商户id权限非法"; break;
                            case "40954": pays = "检查商户状态时数据库异常"; break;
                        }
                        //清除不存在的订单
                        if (mess == "40972")
                        {
                            //var result = SpringFactory.BusinessFactory.GetStudent(order.UserName);
                            //result.OrderFaild(order.OrderId);

                            //TODO 删除订单

                        }
                        else//添加失败失败日志
                        {
                            //var result = SpringFactory.BusinessFactory.GetStudent(order.UserName);
                            //result.AddOrderLogs(order.Amount.ToString(), false, order.OrderId, "错误编码：" + mess + pays);

                            LogHelper.Error("CBCPayOnline", $"用户：{order.UserName}，订单编号：{order.OrderId},校验失败，错误编码：" + mess + pays);
                        }
                    }
                }
                catch (Exception e)
                {

                    //var result = SpringFactory.BusinessFactory.GetStudent(order.UserName);
                    //result.AddOrderLogs("0", false, order.OrderId, e.ToString());

                    LogHelper.Error("CBCPayOnline",$"用户：{order.UserName}，订单编号：{order.OrderId},校验失败，异常信息：{e.Message}");

                }
            }
        }

        /// <summary>
        /// 根据订单号查询订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="payDate"></param>
        /// <param name="userIdCardNumber"></param>
        /// <returns></returns>
        public static string ChenkOrder(string orderId, DateTime? payDate, string userIdCardNumber)
        {
            if (orderId.Length != 24)
            {
                return "订单号不正确，请输入24位订单号";
            }
            ICBCConfig icbcInfo = new ICBCConfig();
            //查询订单列表
            var payModel = SpringFactory.BusinessFactory.GetBusinessAnonymousUser().GetOrderModel(orderId, userIdCardNumber);
            //查询充值日志
            var logModel = SpringFactory.BusinessFactory.GetBusinessAnonymousUser().GetPayLogsByOrderId(orderId, userIdCardNumber);
            if (logModel != null)
            {
                return "已缴费成功，请退出系统重新登录。";
            }

            string zfrq = orderId.Substring(0, 8);
            if (payDate != null)
            {
                zfrq = payDate.Value.ToString("yyyyMMdd");
            }
            else
            {
                if (payModel != null)
                {
                    zfrq = payModel.PayDate.ToString("yyyyMMdd");
                }
            }
            string mess = CheckOrder(orderId, zfrq, icbcInfo.MerID, icbcInfo.MerAcct, out string outMess);
            if (mess.Length > 5)//缴费成功，未返回错误编码,返回xml数据
            {
                DataSet myds = new DataSet();
                StringReader strReader = new StringReader(mess);
                try
                {
                    myds.ReadXml(strReader);
                }
                catch
                {
                    throw new Exception("错误数据：" + mess);
                }

                var user = SpringFactory.BusinessFactory.GetStudent(payModel.userName);
                string stat = myds.Tables["out"].Rows[0]["tranStat"].ToString();
                if (stat == "1" || stat == "0")
                {
                    if (payModel != null)
                    {
                        _amount = myds.Tables["out"].Rows[0]["amount"].ToString();
                        user.PaymentSucceed(_amount, orderId);
                        return "支付成功！\r\n订单号" + orderId + "\r\n支付金额：" + _amount;
                    }
                    else
                    {
                        return "支付成功，但未查询到用户";
                    }
                }
                else
                {
                    string pays = "";
                    if (stat == "2")
                        pays = "支付失败";
                    else
                        pays = "可疑交易";

                    if (payModel != null)
                    {
                        var result = SpringFactory.BusinessFactory.GetStudent(payModel.userName);
                        result.OrderFaild(orderId);
                    }
                    return pays;
                }
            }
            else
            {
                string pays = "";
                switch (mess)
                {
                    case "40972": pays = "API查询的订单不存在"; break;
                    case "40973": pays = "API查询过程中系统异常"; break;
                    case "40976": pays = "API查询系统异常"; break;
                    case "40977": pays = "商户证书信息错"; break;
                    case "40978": pays = "解包商户请求数据报错"; break;
                    case "40979": pays = "查询的订单不存在"; break;
                    case "40980": pays = "API查询过程中系统异常"; break;
                    case "40981": pays = "给商户打包返回数据错"; break;

                    case "40982": pays = "系统错误"; break;
                    case "40983": pays = "查询的订单不唯一"; break;
                    case "40987": pays = "商户代码或者商城账号有误"; break;
                    case "40947": pays = "给商户打包返回数据错"; break;
                    case "40948": pays = "商城状态非法"; break;
                    case "40949": pays = "商城类别非法"; break;

                    case "40950": pays = "商城应用类别非法"; break;
                    case "40951": pays = "商户证书id状态非法"; break;
                    case "40952": pays = "商户证书id未绑定"; break;
                    case "40953": pays = "商户id权限非法"; break;
                    case "40954": pays = "检查商户状态时数据库异常"; break;

                }
                //清除不存在的订单
                if (mess == "40972")
                {
                    if (payModel != null)
                    {
                        var result = SpringFactory.BusinessFactory.GetStudent(payModel.userName);
                        result.OrderFaild(orderId);
                    }

                }
                else//添加失败失败日志
                {
                    if (payModel != null)
                    {
                        var result = SpringFactory.BusinessFactory.GetStudent(payModel.userName);
                        result.AddOrderLogs(payModel.Amount.ToString(), false, orderId, "错误编码：" + mess + pays);
                    }
                }
                return pays;
            }
        }

        /// <summary>
        /// 获取工商银行验证信息
        /// </summary>
        /// <param name="argIcbc"></param>
        /// <returns></returns>
        public static ICBCConfig GetCheckInfo(ICBCConfig argIcbc)
        {
            string strMerSignMsg = string.Empty;
            B2CUtil icbcObj = new B2CUtil();
            int jg = icbcObj.init(strCertFn, strCertFNM, strKeyFN, strKey);
            if (jg == 0)
            {
                argIcbc.MerSignMsg = icbcObj.signC(argIcbc.TranData, argIcbc.TranData.Length);
                if (argIcbc.MerSignMsg == "")
                {
                    int returnCode = icbcObj.getRC();
                    SpringFactory.BusinessFactory.GetBusinessAnonymousUser().AddLogs("错误编码:" + returnCode + "，签名错误");
                }
                argIcbc.MerCert = icbcObj.getCert(1);
                byte[] bytes = Encoding.Default.GetBytes(argIcbc.TranData);
                argIcbc.TranData = Convert.ToBase64String(bytes);
            }
            else
            {
                SpringFactory.BusinessFactory.GetBusinessAnonymousUser().AddLogs(jg.ToString() + ",证书错误或私钥错误编码");
            }
            return argIcbc;
        }

        /// <summary>
        /// 获取工商银行验证信息
        /// </summary>
        /// <param name="argIcbc"></param>
        /// <returns></returns>
        public static ICBCConfig GetCheckReturnInfo(ICBCConfig argIcbc)
        {
            string strMerSignMsg = string.Empty;
            B2CUtil icbcObj = new B2CUtil();

            if (icbcObj.init(strCertFn, strCertFNM, strKeyFN, strKey) == 0)
            {
                argIcbc.TranData = Decode(argIcbc.TranData);
                //判断验证银行签名是否成功
                if (icbcObj.verifySignC(argIcbc.TranData, argIcbc.TranData.Length, argIcbc.MerSignMsg, argIcbc.MerSignMsg.Length) == 0)
                {
                    argIcbc.IsCheck = true;
                }
                else
                    argIcbc.IsCheck = true;
            }
            else
            {
                argIcbc.IsCheck = false;
            }
            return argIcbc;
        }
        
        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="strOrderNum">订单号</param>
        /// <param name="strTranDate">交易日期</param>
        /// <param name="strShopCode">商家代码</param>
        /// <param name="strShopAccount">商城账号</param>
        /// <param name="errInfo"></param>
        /// <returns></returns>
        public static string CheckOrder(string strOrderNum, string strTranDate, string strShopCode, string strShopAccount, out string errInfo)
        {
            try
            {
                errInfo = string.Empty;
                StringBuilder sb = new StringBuilder();
                sb.Append("<?xml  version=\"1.0\" encoding=\"GBK\" standalone=\"no\" ?><ICBCAPI><in><orderNum>");
                sb.Append(strOrderNum);
                sb.Append("</orderNum><tranDate>");
                sb.Append(strTranDate);
                sb.Append("</tranDate><ShopCode>");
                sb.Append(strShopCode);
                sb.Append("</ShopCode><ShopAccount>");
                sb.Append(strShopAccount);
                sb.Append("</ShopAccount></in></ICBCAPI>");

                string post_data = post_params + sb.ToString();
                string retruenstring = PostDataBySSL(post_data, api_url, strCertFNM, strKey, out errInfo);

                //var result = SpringFactory.BusinessFactory.GetBusinessAnonymousUser();
                //result.AddLogs("返回3：" + (retruenstring.Length > 400 ? retruenstring.Substring(0, 400) : retruenstring));
                if (retruenstring.Length <= 5)
                {
                    return retruenstring;
                }
                return HttpUtility.UrlDecode(retruenstring);
            }
            catch (Exception ex)
            {
                var result = SpringFactory.BusinessFactory.GetBusinessAnonymousUser();
                result.AddLogs("返回1：" + "查询缴费接口失败" + ex.Message);
                errInfo = "查询缴费接口失败";

                return "99";
            }
        }

        /// <summary>
        /// 发送SSL加密请求
        /// </summary>
        /// <param name="post_data"></param>
        /// <param name="url"></param>
        /// <param name="cert_path"></param>
        /// <param name="cert_password"></param>
        /// <param name="errInfo"></param>
        /// <returns></returns>
        public static string PostDataBySSL(string post_data, string url, string cert_path, string cert_password, out string errInfo)
        {
            errInfo = string.Empty;
            try
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] data = encoding.GetBytes(post_data);
                if (cert_path != string.Empty)
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);

                WebRequest webRequest = WebRequest.Create(url);
                HttpWebRequest httpRequest = webRequest as HttpWebRequest;

                if (cert_path.ToLower().EndsWith(".cer"))
                {
                    httpRequest.ClientCertificates.Add(X509Certificate.CreateFromCertFile(cert_path));
                }

                else
                {
                    //SpringFactory.BusinessFactory.GetBusinessAnonymousUser().AddLogs(cert_path);
                    httpRequest.ClientCertificates.Add(new X509Certificate2(cert_path, cert_password, X509KeyStorageFlags.MachineKeySet));


                }
                httpRequest.KeepAlive = true;
                httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0)";
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                httpRequest.Method = "POST";

                httpRequest.ContentLength = data.Length;
                Stream requestStream = httpRequest.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
                Stream responseStream = null;
                responseStream = httpRequest.GetResponse().GetResponseStream();
                string stringResponse = string.Empty;
                if (responseStream != null)
                {
                    using (StreamReader responseReader =
                        new StreamReader(responseStream, Encoding.GetEncoding("GBK")))
                    {
                        stringResponse = responseReader.ReadToEnd();
                    }
                    responseStream.Close();
                }
                return stringResponse;
            }
            catch (Exception e)
            {
                errInfo = e.Message;

                SpringFactory.BusinessFactory.GetBusinessAnonymousUser().AddLogs(e.Message);
                return string.Empty;
            }
        }
        
        /// <summary>
        /// 加密信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Encode(string data)
        {
            try
            {
                return Inhua.Common.Encryption.Encrypt(data);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 解密信息
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Decode(string str)
        {
            byte[] outputb = Convert.FromBase64String(str);
            string orgStr = Encoding.Default.GetString(outputb);
            return orgStr;
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}