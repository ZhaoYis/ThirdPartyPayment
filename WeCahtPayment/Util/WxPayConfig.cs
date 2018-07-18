using System;
using System.Collections;
using System.Configuration;
using System.Web.Configuration;

namespace WeCahtPayment.Util
{
    /// <summary>
    /// 配置账号信息
    /// </summary>
    public class WxPayConfig
    {
        private static string _appId;
        private static string _mchId;
        private static string _key;
        private static string _appSecret;
        private static string _sslCertPath;
        private static string _sslCertPassword;
        private static string _notifyUrl;
        private static string _ip;
        private static string _proxyUrl;
        private static int _reportLevenl;

        /// <summary>
        /// 初始化
        /// </summary>
        static WxPayConfig()
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            IList keys = config.AppSettings.Settings.AllKeys;
            //项目根地址
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            if (keys.Contains("AppId"))
                _appId = config.AppSettings.Settings["AppId"].Value;

            if (keys.Contains("MchId"))
                _mchId = config.AppSettings.Settings["MchId"].Value;

            if (keys.Contains("Key"))
                _key = config.AppSettings.Settings["Key"].Value;

            if (keys.Contains("AppSecret"))
                _appSecret = config.AppSettings.Settings["AppSecret"].Value;

            if (keys.Contains("SSLCertPath"))
                _sslCertPath = basePath + config.AppSettings.Settings["SSLCertPath"].Value;

            if (keys.Contains("SSLCertPassword"))
                _sslCertPassword = config.AppSettings.Settings["SSLCertPassword"].Value;

            if (keys.Contains("NotifyUrl"))
                _notifyUrl = config.AppSettings.Settings["NotifyUrl"].Value;

            if (keys.Contains("IP"))
                _ip = config.AppSettings.Settings["IP"].Value;

            if (keys.Contains("PROXY_URL"))
                _proxyUrl = config.AppSettings.Settings["PROXY_URL"].Value;

            if (keys.Contains("REPORT_LEVENL"))
                _reportLevenl = int.Parse(config.AppSettings.Settings["REPORT_LEVENL"].Value);
        }

        //=======【微信公众号信息配置】=====================================
        /// <summary>
        /// 绑定支付的APPID（必须配置）
        /// </summary>
        public static string AppId
        {
            get { return _appId; }
            set { _appId = value; }
        }

        /// <summary>
        /// 商户号（必须配置）
        /// </summary>
        public static string MchId
        {
            get { return _mchId; }
            set { _mchId = value; }
        }

        /// <summary>
        /// 商户支付密钥，参考开户邮件设置（必须配置）
        /// </summary>
        public static string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// 公众帐号secert（仅JSAPI支付的时候需要配置）
        /// </summary>
        public static string AppSecret
        {
            get { return _appSecret; }
            set { _appSecret = value; }
        }

        //=======【证书路径设置】===================================== 
        /// <summary>
        /// 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        /// </summary>
        public static string SSLCertPath
        {
            get { return _sslCertPath; }
            set { _sslCertPath = value; }
        }

        /// <summary>
        /// 证书密码
        /// </summary>
        public static string SSLCertPassword
        {
            get { return _sslCertPassword; }
            set { _sslCertPassword = value; }
        }

        //=======【支付结果通知url】===================================== 
        /// <summary>
        /// 支付结果通知回调url，用于商户接收支付结果
        /// </summary>
        //public string NOTIFY_URL = "http://paysdk.weixin.qq.com/example/ResultNotifyPage.aspx";
        public static string NotifyUrl
        {
            get { return _notifyUrl; }
            set { _notifyUrl = value; }
        }

        //=======【商户系统后台机器IP】===================================== 
        /// <summary>
        /// 此参数可手动配置也可在程序中自动获取
        /// </summary>
        public static string IP
        {
            get { return _ip; }
            set { _ip = value; }
        }

        //=======【代理服务器设置】===================================
        /// <summary>
        /// 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        /// </summary>
        public static string PROXY_URL
        {
            get { return _proxyUrl; }
            set { _proxyUrl = value; }
        }

        //=======【上报信息配置】===================================
        /// <summary>
        /// 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        /// </summary>
        public static int REPORT_LEVENL
        {
            get { return _reportLevenl; }
            set { _reportLevenl = value; }
        }
    }
}