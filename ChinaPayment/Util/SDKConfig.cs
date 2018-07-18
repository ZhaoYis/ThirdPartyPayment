using System;
using System.Collections;
using System.Configuration;
using System.Web.Configuration;

namespace ChinaPayment.Util
{
    /// <summary>
    /// 配置
    /// </summary>
    public class SdkConfig
    {
        private static string _signCertPath;//获取签名证书路径
        private static string _signCertPwd;//获取签名证书密码
        private static string _validateCertDir;//获取验签目录
        private static string _encryptCert;//加密公钥证书路径

        private static string _cardRequestUrl;//有卡交易路径;
        private static string _appRequestUrl;//appj交易路径;
        private static string _singleQueryUrl;//获取交易查询地址
        private static string _fileTransUrl;//获取文件传输类交易地址
        private static string _frontTransUrl;//获取前台交易地址
        private static string _backTransUrl;//获取后台交易地址
        private static string _batTransUrl;//读取配批量交易地址
        
        private static string _frontUrl;//获取前台通知地址
        private static string _backUrl;//获取前台通知地址

        private static string _jfCardRequestUrl;//缴费产品有卡交易路径;
        private static string _jfAppRequestUrl;//缴费产品app交易路径;
        private static string _jfSingleQueryUrl;//获取缴费产品交易查询地址
        private static string _jfFrontTransUrl;//获取缴费产品前台交易地址
        private static string _jfBackTransUrl;//获取缴费产品后台交易地址

        private static string _ifValidateRemoteCert = "false";//是否验证后台https证书
        private static string _ifValidateCnName = "true";//是否验证证书cn
        private static string _middleCertPath;//中级证书路径
        private static string _rootCertPath;//根证书路径
        private static string _secureKey;//散列方式签名密钥
        private static string _signMethod = "01";//指定signMethod
        private static string _version = "5.0.0";//指定version

        static SdkConfig()
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            IList keys = config.AppSettings.Settings.AllKeys;
            //项目根地址
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            if (keys.Contains("acpsdk.signCert.path"))
                _signCertPath = basePath + config.AppSettings.Settings["acpsdk.signCert.path"].Value;  //获取签名证书路径
            if (keys.Contains("acpsdk.signCert.pwd"))
                _signCertPwd = config.AppSettings.Settings["acpsdk.signCert.pwd"].Value;//获取签名证书密码
            if (keys.Contains("acpsdk.validateCert.dir"))
                _validateCertDir = config.AppSettings.Settings["acpsdk.validateCert.dir"].Value;//获取验签目录
            if (keys.Contains("acpsdk.encryptCert.path"))
                _encryptCert = basePath + config.AppSettings.Settings["acpsdk.encryptCert.path"].Value;  //加密公钥证书路径

            if (keys.Contains("acpsdk.cardRequestUrl"))
                _cardRequestUrl = config.AppSettings.Settings["acpsdk.cardRequestUrl"].Value;  //有卡交易路径;
            if (keys.Contains("acpsdk.appRequestUrl"))
                _appRequestUrl = config.AppSettings.Settings["acpsdk.appRequestUrl"].Value;  //appj交易路径;
            if (keys.Contains("acpsdk.singleQueryUrl"))
                _singleQueryUrl = config.AppSettings.Settings["acpsdk.singleQueryUrl"].Value; //获取交易查询地址
            if (keys.Contains("acpsdk.fileTransUrl"))
                _fileTransUrl = config.AppSettings.Settings["acpsdk.fileTransUrl"].Value;  //获取文件传输类交易地址
            if (keys.Contains("acpsdk.frontTransUrl"))
                _frontTransUrl = config.AppSettings.Settings["acpsdk.frontTransUrl"].Value; //获取前台交易地址
            if (keys.Contains("acpsdk.backTransUrl"))
                _backTransUrl = config.AppSettings.Settings["acpsdk.backTransUrl"].Value;//获取后台交易地址
            if (keys.Contains("acpsdk.batTransUrl"))
                _batTransUrl = config.AppSettings.Settings["acpsdk.batTransUrl"].Value;//读取配批量交易地址

            if (keys.Contains("acpsdk.frontUrl"))
                _frontUrl = config.AppSettings.Settings["acpsdk.frontUrl"].Value;//获取前台通知地址
            if (keys.Contains("acpsdk.backUrl"))
                _backUrl = config.AppSettings.Settings["acpsdk.backUrl"].Value;//获取前台通知地址

            if (keys.Contains("acpsdk.jf.cardRequestUrl"))
                _jfCardRequestUrl = config.AppSettings.Settings["acpsdk.jf.cardRequestUrl"].Value;  //缴费产品有卡交易路径;
            if (keys.Contains("acpsdk.jf.appRequestUrl"))
                _jfAppRequestUrl = config.AppSettings.Settings["acpsdk.jf.appRequestUrl"].Value;  //缴费产品app交易路径;
            if (keys.Contains("acpsdk.jf.singleQueryUrl"))
                _jfSingleQueryUrl = config.AppSettings.Settings["acpsdk.jf.singleQueryUrl"].Value; //获取缴费产品交易查询地址
            if (keys.Contains("acpsdk.jf.frontTransUrl"))
                _jfFrontTransUrl = config.AppSettings.Settings["acpsdk.jf.frontTransUrl"].Value; //获取缴费产品前台交易地址
            if (keys.Contains("acpsdk.jf.backTransUrl"))
                _jfBackTransUrl = config.AppSettings.Settings["acpsdk.jf.backTransUrl"].Value;//获取缴费产品后台交易地址

            if (keys.Contains("acpsdk.ifValidateRemoteCert"))
                _ifValidateRemoteCert = config.AppSettings.Settings["acpsdk.ifValidateRemoteCert"].Value;//是否验证后台https证书
            if (keys.Contains("acpsdk.ifValidateCNName"))
                _ifValidateCnName = config.AppSettings.Settings["acpsdk.ifValidateCNName"].Value;//是否验证证书cn
            if (keys.Contains("acpsdk.middleCert.path"))
                _middleCertPath = basePath + config.AppSettings.Settings["acpsdk.middleCert.path"].Value;//中级证书路径
            if (keys.Contains("acpsdk.rootCert.path"))
                _rootCertPath = basePath + config.AppSettings.Settings["acpsdk.rootCert.path"].Value;//根证书路径
            if (keys.Contains("acpsdk.secureKey"))
                _secureKey = config.AppSettings.Settings["acpsdk.secureKey"].Value;//散列方式签名密钥
            if (keys.Contains("acpsdk.signMethod"))
                _signMethod = config.AppSettings.Settings["acpsdk.signMethod"].Value;//设置signMethod
            if (keys.Contains("acpsdk.version"))
                _version = config.AppSettings.Settings["acpsdk.version"].Value;//设置signMethod
        }

        /// <summary>
        /// 有卡交易路径
        /// </summary>
        public static string CardRequestUrl
        {
            get { return _cardRequestUrl; }
            set { _cardRequestUrl = value; }
        }
        /// <summary>
        /// app交易路径
        /// </summary>
        public static string AppRequestUrl
        {
            get { return _appRequestUrl; }
            set { _appRequestUrl = value; }
        }
        /// <summary>
        /// 获取前台交易地址
        /// </summary>
        public static string FrontTransUrl
        {
            get { return _frontTransUrl; }
            set { _frontTransUrl = value; }
        }
        /// <summary>
        /// 加密公钥证书路径
        /// </summary>
        public static string EncryptCert
        {
            get { return _encryptCert; }
            set { _encryptCert = value; }
        }
        /// <summary>
        /// 读取配批量交易地址
        /// </summary>
        public static string BackTransUrl
        {
            get { return _backTransUrl; }
            set { _backTransUrl = value; }
        }
        /// <summary>
        /// 获取交易查询地址
        /// </summary>
        public static string SingleQueryUrl
        {
            get { return _singleQueryUrl; }
            set { _singleQueryUrl = value; }
        }
        /// <summary>
        /// 获取文件传输类交易地址
        /// </summary>
        public static string FileTransUrl
        {
            get { return _fileTransUrl; }
            set { _fileTransUrl = value; }
        }
        /// <summary>
        /// 获取签名证书路径
        /// </summary>
        public static string SignCertPath
        {
            get { return _signCertPath; }
            set { _signCertPath = value; }
        }
        /// <summary>
        /// 获取签名证书密码
        /// </summary>
        public static string SignCertPwd
        {
            get { return _signCertPwd; }
            set { _signCertPwd = value; }
        }
        /// <summary>
        /// 获取验签目录
        /// </summary>
        public static string ValidateCertDir
        {
            get { return _validateCertDir; }
            set { _validateCertDir = value; }
        }
        /// <summary>
        /// 读取配批量交易地址
        /// </summary>
        public static string BatTransUrl
        {
            get { return _batTransUrl; }
            set { _batTransUrl = value; }
        }
        /// <summary>
        /// 获取后台通知地址
        /// </summary>
        public static string BackUrl
        {
            get { return _backUrl; }
            set { _backUrl = value; }
        }
        /// <summary>
        /// 获取前台通知地址
        /// </summary>
        public static string FrontUrl
        {
            get { return _frontUrl; }
            set { _frontUrl = value; }
        }
        /// <summary>
        /// 缴费产品有卡交易路径
        /// </summary>
        public static string JfCardRequestUrl
        {
            get { return _jfCardRequestUrl; }
            set { _jfCardRequestUrl = value; }
        }
        /// <summary>
        /// 缴费产品app交易路径
        /// </summary>
        public static string JfAppRequestUrl
        {
            get { return _jfAppRequestUrl; }
            set { _jfAppRequestUrl = value; }
        }
        /// <summary>
        /// 获取缴费产品前台交易地址
        /// </summary>
        public static string JfFrontTransUrl
        {
            get { return _jfFrontTransUrl; }
            set { _jfFrontTransUrl = value; }
        }
        /// <summary>
        /// 获取缴费产品后台交易地址
        /// </summary>
        public static string JfBackTransUrl
        {
            get { return _jfBackTransUrl; }
            set { _jfBackTransUrl = value; }
        }
        /// <summary>
        /// 获取缴费产品交易查询地址
        /// </summary>
        public static string JfSingleQueryUrl
        {
            get { return _jfSingleQueryUrl; }
            set { _jfSingleQueryUrl = value; }
        }
        /// <summary>
        /// 是否验证后台https证书
        /// </summary>
        public static string IfValidateRemoteCert
        {
            get { return _ifValidateRemoteCert; }
            set { _ifValidateRemoteCert = value; }
        }
        /// <summary>
        /// 是否验证证书cn
        /// </summary>
        public static string IfValidateCNName
        {
            get { return _ifValidateCnName; }
            set { _ifValidateCnName = value; }
        }
        /// <summary>
        /// 中级证书路径
        /// </summary>
        public static string MiddleCertPath
        {
            get { return _middleCertPath; }
            set { _middleCertPath = value; }
        }
        /// <summary>
        /// 根证书路径
        /// </summary>
        public static string RootCertPath
        {
            get { return _rootCertPath; }
            set { _rootCertPath = value; }
        }
        /// <summary>
        /// 散列方式签名密钥
        /// </summary>
        public static string SecureKey
        {
            get { return _secureKey; }
            set { _secureKey = value; }
        }
        /// <summary>
        /// 指定signMethod
        /// </summary>
        public static string SignMethod
        {
            get { return _signMethod; }
            set { _signMethod = value; }
        }
        /// <summary>
        /// 指定version
        /// </summary>
        public static string Version
        {
            get { return _version; }
            set { _version = value; }
        }
    }
}