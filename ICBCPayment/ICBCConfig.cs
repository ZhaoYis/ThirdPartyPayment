namespace ICBCPayment
{
    /// <summary>
    /// 配置
    /// </summary>
    public class ICBCConfig
    {
        /// <summary>
        /// 工商支付接口路径
        /// </summary>
        private string _orderPostUrl = "https://B2C.icbc.com.cn/servlet/ICBCINBSEBusinessServlet";
        /// <summary>
        /// 接口名称
        /// </summary>
        private string _interfaceName = "ICBC_PERBANK_B2C";
        /// <summary>
        /// 接口版本号
        /// </summary>
        private string _interfaceVersion = "1.0.0.11";

        //private string _orderid;

        /// <summary>
        /// 订单金额
        /// </summary>
        //private string _amount;

        /// <summary>
        /// 支付币种
        /// </summary>
        private string _curType = "001";
        /// <summary>
        /// 商户代码
        /// </summary>
        private string _merId = "向银行申请";
        /// <summary>
        /// 商户账号
        /// </summary>
        private string _merAcct = "交易账号";
        /// <summary>
        /// 检验联名标志
        /// 取值“1”：客户支付时，网银判断该客户是否与商户联名，是则按上送金额扣帐，否则展现未联名错误；取值“0”：不检验客户是否与商户联名，按上送金额扣帐。
        /// </summary>
        private string _verifyJoinFlag = "0";
        /// <summary>
        /// 通知类型
        /// 在交易转账处理完成后把交易结果通知商户的处理模式。
        /// 取值“HS”：在交易完成后实时将通知信息以HTTP协议POST方式，主动发送给商户，发送地址为商户端随订单数据提交的接收工行支付结果的URL即表单中的merURL字段；
        /// 取值“AG”：在交易完成后不通知商户。商户需使用浏览器登录工行的B2C商户服务网站，或者使用工行提供的客户端程序API主动获取通知信息。
        /// </summary>
        private string _notifyType = "HS";
        /// <summary>
        /// 返回商户URL
        /// 必须合法的URL，交易结束，将客户引导到商户的此url，即通过客户浏览器post交易结果信息到商户的此URL
        /// 注意：该URL应使用http协议（不能使用https协议），端口号应为80或不指定。
        /// </summary>
        public string MerUrl = "http://localhost/ICPCPayReturn.aspx";
        /// <summary>
        /// 结果发送类型
        /// 选输
        /// 取值“0”：无论支付成功或者失败，银行都向商户发送交易通知信息；
        /// 取值“1”，银行只向商户发送交易成功的通知信息。
        /// 只有通知方式为HS时此值有效，如果使用AG方式，可不上送此项，但签名数据中必须包含此项，取值可为空。
        /// </summary>
        private string _resultType = "1";

        ///// <summary>
        ///// 支付日期
        ///// </summary>
        //private string _orderDate = System.DateTime.Now.ToString("yyyyMMddHHmmss");

        /// <summary>
        /// 订单签名数据
        /// 必输，
        ///商户使用工行提供的签名API和商户证书将tranData的xml明文串进行签名，得到二进制签名数据，然后进行BASE64编码后得到可视的merSignMsg；
        ///注意：签名时是针对tranData的xml明文，不是将tranData进行BASE64编码后的串；
        /// </summary>
        private string _merSignMsg;

        /// <summary>
        /// 商城证书公钥
        /// 商户用二进制方式读取证书公钥文件后，进行BASE64编码后产生的字符串；
        /// </summary>
        private string _merCert;

        ///// <summary>
        ///// 商品编号
        ///// </summary>
        //private string _goodsID = "001";
        ///// <summary>
        ///// 商品名称
        ///// </summary>
        //private string _goodsName = "";
        ///// <summary>
        ///// 商品数量
        ///// </summary>
        //private string _goodsNum = "1";

        ///// <summary>
        ///// 已含运费金额
        ///// </summary>
        //private string _carriageAmt;

        ///// <summary>
        ///// 备注字段1
        ///// </summary>
        //private string _remark1;
        
        ///// <summary>
        ///// 备注字段2
        ///// </summary>
        //private string _remark2;

        ///// <summary>
        ///// 商城提示
        ///// </summary>
        //private string _merHint;

        /// <summary>
        /// 整合所有交易数据形成的xml明文串，并做BASE64编码；
        /// 具体格式定义见下文；
        /// 注意：
        /// 需有xml头属性；整个字段使用BASE64编码；
        /// xml明文中没有回车换行和多余空格；
        /// </summary>
        private string _tranData;

        /// <summary>
        /// 上送商户网站域名（支持通配符，例如“*.某B2C商城.com”），如果上送，工行会在客户支付订单时，校验商户上送域名与客户跳转工行支付页面之前网站域名的一致性。
        /// </summary>
        private string _merReference = "";//System.Configuration.ConfigurationManager.AppSettings["WebUrl"];

        /// <summary>
        /// 商城IP地址 b 
        /// </summary>
        private string _merIp = "";//System.Configuration.ConfigurationManager.AppSettings["WebIP"];
        /// <summary>
        /// 是否检测成功
        /// </summary>
        private bool _isCheck = false;

        /// <summary>
        /// 是否检测成功
        /// </summary>
        public bool IsCheck
        {
            get { return _isCheck; }
            set { _isCheck = value; }
        }

        /// <summary>
        /// 服务域名
        /// </summary>
        public string MerReference
        {
            get { return _merReference; }
            set { _merReference = value; }
        }

        /// <summary>
        /// 服务IP
        /// </summary>
        public string MerIP
        {
            get { return _merIp; }
            set { _merIp = value; }
        }

        /// <summary>
        /// 报文数据
        /// </summary>
        public string TranData
        {
            get { return _tranData; }
            set { _tranData = value; }
        }

        /// <summary>
        /// 工商支付接口路径
        /// </summary>
        public string OrderPostUrl
        {
            get { return _orderPostUrl; }
            set { _orderPostUrl = value; }
        }

        /// <summary>
        /// 接口名称
        /// </summary>
        public string InterfaceName
        {
            get { return _interfaceName; }
            set { _interfaceName = value; }
        }

        /// <summary>
        /// 接口版本号
        /// </summary>
        public string InterfaceVersion
        {
            get { return _interfaceVersion; }
            set { _interfaceVersion = value; }
        }

        ///// <summary>
        ///// 订单号
        ///// </summary>
        //public string Orderid
        //{
        //    get { return _orderid; }
        //    set { _orderid = value; }
        //}

        ///// <summary>
        ///// 订单金额
        ///// </summary>
        //public string Amount
        //{
        //    get { return _amount; }
        //    set { _amount = value; }
        //}

        /// <summary>
        /// 支付币种 RMB:001
        /// </summary>
        public string CurType
        {
            get { return _curType; }
            set { _curType = value; }
        }

        /// <summary>
        /// 商户代码
        /// </summary>
        public string MerID
        {
            get { return _merId; }
            set { _merId = value; }
        }

        /// <summary>
        /// 商户账号
        /// </summary>
        public string MerAcct
        {
            get { return _merAcct; }
            set { _merAcct = value; }
        }

        /// <summary>
        /// 检验联名标志 
        /// 取值“1”：客户支付时，网银判断该客户是否与商户联名，是则按上送金额扣帐，否则展现未联名错误；
        /// 取值“0”：不检验客户是否与商户联名，按上送金额扣帐。
        /// </summary>
        public string VerifyJoinFlag
        {
            get { return _verifyJoinFlag; }
            set { _verifyJoinFlag = value; }
        }

        /// <summary>
        /// 通知类型
        /// 取值“HS”：在交易完成后实时将通知信息以HTTP协议POST方式，主动发送给商户，发送地址为商户端随订单数据提交的接收工行支付结果的URL即表单中的merURL字段；
        /// 取值“AG”：在交易完成后不通知商户。商户需使用浏览器登录工行的B2C商户服务网站，或者使用工行提供的客户端程序API主动获取通知信息。
        /// </summary>
        public string NotifyType
        {
            get { return _notifyType; }
            set { _notifyType = value; }
        }

        ///// <summary>
        ///// 返回商户URL
        ///// </summary>
        //public string MerURL
        //{
        //    get { return _merURL; }
        //    set { _merURL = "http://localhost/ICPCPayReturn.aspx"; }
        //}

        /// <summary>
        /// 结果发送类型
        /// </summary>
        public string ResultType
        {
            get { return _resultType; }
            set { _resultType = value; }
        }

        ///// <summary>
        ///// 交易日期时间
        ///// </summary>
        //public string OrderDate
        //{
        //    get { return _orderDate; }
        //    set { _orderDate = value; }
        //}

        /// <summary>
        /// 订单签名数据
        /// </summary>
        public string MerSignMsg
        {
            get { return _merSignMsg; }
            set { _merSignMsg = value; }
        }

        /// <summary>
        /// 商城证书公钥
        /// </summary>
        public string MerCert
        {
            get { return _merCert; }
            set { _merCert = value; }
        }

        ///// <summary>
        ///// 商品编号
        ///// </summary>
        //public string GoodsID
        //{
        //    get { return _goodsID; }
        //    set { _goodsID = value; }
        //}

        ///// <summary>
        ///// 商品名称
        ///// </summary>
        //public string GoodsName
        //{
        //    get { return _goodsName; }
        //    set { _goodsName = value; }
        //}

        ///// <summary>
        ///// 商品数量
        ///// </summary>
        //public string GoodsNum
        //{
        //    get { return _goodsNum; }
        //    set { _goodsNum = value; }
        //}

        ///// <summary>
        ///// 已含运费金额
        ///// </summary>
        //public string CarriageAmt
        //{
        //    get { return _carriageAmt; }
        //    set { _carriageAmt = value; }
        //}

        ///// <summary>
        ///// 备注字段1
        ///// </summary>
        //public string Remark1
        //{
        //    get { return _remark1; }
        //    set { _remark1 = value; }
        //}

        ///// <summary>
        ///// 备注字段2
        ///// </summary>
        //public string Remark2
        //{
        //    get { return _remark2; }
        //    set { _remark2 = value; }
        //}

        ///// <summary>
        ///// 商城提示
        ///// </summary>
        //public string MerHint
        //{
        //    get { return _merHint; }
        //    set { _merHint = value; }
        //}
    }
}