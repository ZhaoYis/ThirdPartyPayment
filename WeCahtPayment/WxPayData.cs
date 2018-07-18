using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using LitJson;
using WeCahtPayment.Util;

namespace WeCahtPayment
{
    public class WxPayData
    {
        /// <summary>
        /// 采用排序的Dictionary的好处是方便对数据包进行签名，不用再签名之前再做一次排序
        /// </summary>
        private readonly SortedDictionary<string, object> _mValues = new SortedDictionary<string, object>();

        /// <summary>
        /// 默认构造函数
        /// 如果initDefault为true，则自动填入字段（appid,mch_id,time_stamp,nonce_str,out_trade_no,）
        /// </summary>
        public WxPayData(bool initDefault = false)
        {
            if (initDefault)
            {
                Init();
            }
        }

        /// <summary>
        /// 对象初始化后，自动填入字段（appid,mch_id,time_stamp,nonce_str,out_trade_no,）
        /// </summary>
        private void Init()
        {
            //初始化几个参数
            this.SetValue("appid", WxPayConfig.AppId);//公众帐号id
            this.SetValue("mch_id", WxPayConfig.MchId);//商户号
            this.SetValue("nonce_str", this.GenerateNonceStr());//随机字符串
            this.SetValue("out_trade_no", this.GenerateOutTradeNo());//订单号
        }
        
        /// <summary>
        /// 获取Dictionary
        /// </summary>
        /// <returns></returns>
        public SortedDictionary<string, object> GetValues()
        {
            return _mValues;
        }

        /// <summary>
        /// 设置某个字段的值
        /// </summary>
        /// <param name="key">字段名</param>
        /// <param name="value">字段值</param>
        public void SetValue(string key, object value)
        {
            _mValues[key] = value;
        }

        /// <summary>
        /// 根据字段名获取某个字段的值
        /// </summary>
        /// <param name="key">字段名</param>
        /// <returns>key对应的字段值</returns>
        public object GetValue(string key)
        {
            _mValues.TryGetValue(key, out object o);
            return o;
        }

        /// <summary>
        /// 判断某个字段是否已设置
        /// </summary>
        /// <param name="key">字段名</param>
        /// <returns>若字段key已被设置，则返回true，否则返回false</returns>
        public bool IsSet(string key)
        {
            _mValues.TryGetValue(key, out object o);
            if (null != o)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 将Dictionary转成xml
        /// </summary>
        /// <returns>经转换得到的xml串</returns>
        public string ToXml()
        {
            //数据为空时不能转化为xml格式
            if (0 == _mValues.Count)
            {
                LogHelper.Error(this.GetType().ToString(), "WxPayData数据为空!");
                throw new WxPayException("WxPayData数据为空!");
            }

            string xml = "<xml>";
            foreach (KeyValuePair<string, object> pair in _mValues)
            {
                //字段值不能为null，会影响后续流程
                if (pair.Value == null)
                {
                    LogHelper.Error(this.GetType().ToString(), "WxPayData内部含有值为null的字段!");
                    throw new WxPayException("WxPayData内部含有值为null的字段!");
                }

                if (pair.Value is int)
                {
                    xml += "<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">";
                }
                else if (pair.Value is string)
                {
                    xml += "<" + pair.Key + ">" + "<![CDATA[" + pair.Value + "]]></" + pair.Key + ">";
                }
                else//除了string和int类型不能含有其他数据类型
                {
                    LogHelper.Error(this.GetType().ToString(), "WxPayData字段数据类型错误!");
                    throw new WxPayException("WxPayData字段数据类型错误!");
                }
            }
            xml += "</xml>";
            return xml;
        }

        /// <summary>
        /// 将xml转为WxPayData对象并返回对象内部的数据
        /// </summary>
        /// <param name="xml">待转换的xml串</param>
        /// <returns>经转换得到的Dictionary</returns>
        public SortedDictionary<string, object> FromXml(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                LogHelper.Error(this.GetType().ToString(), "将空的xml串转换为WxPayData不合法!");
                throw new WxPayException("将空的xml串转换为WxPayData不合法!");
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNode xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                _mValues[xe.Name] = xe.InnerText;//获取xml的键值对到WxPayData内部的数据中
            }

            try
            {
                //2015-06-29 错误是没有签名
                if (_mValues != null && (string) _mValues["return_code"] != "SUCCESS")
                {
                    return _mValues;
                }
                CheckSign();//验证签名,不通过会抛异常
            }
            catch (WxPayException ex)
            {
                throw new WxPayException(ex.Message);
            }

            return _mValues;
        }

        /// <summary>
        /// Dictionary格式转化成url参数格式
        /// </summary>
        /// <returns>url格式串, 该串不包含sign字段值</returns>
        public string ToUrl()
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in _mValues)
            {
                if (pair.Value == null)
                {
                    LogHelper.Error(this.GetType().ToString(), "WxPayData内部含有值为null的字段!");
                    throw new WxPayException("WxPayData内部含有值为null的字段!");
                }

                if (pair.Key != "sign" && pair.Value.ToString() != "")
                {
                    buff += pair.Key + "=" + pair.Value + "&";
                }
            }
            buff = buff.Trim('&');
            return buff;
        }

        /// <summary>
        /// Dictionary格式化成Json
        /// </summary>
        /// <returns>json串数据</returns>
        public string ToJson()
        {
            string jsonStr = JsonMapper.ToJson(_mValues);
            return jsonStr;
        }

        /// <summary>
        /// values格式化成能在Web页面上显示的结果（因为web页面上不能直接输出xml格式的字符串）
        /// </summary>
        /// <returns></returns>
        public string ToPrintStr()
        {
            string str = "";
            foreach (KeyValuePair<string, object> pair in _mValues)
            {
                if (pair.Value == null)
                {
                    LogHelper.Error(this.GetType().ToString(), "WxPayData内部含有值为null的字段!");
                    throw new WxPayException("WxPayData内部含有值为null的字段!");
                }

                str += string.Format("{0}={1}<br>", pair.Key, pair.Value.ToString());
            }
            LogHelper.Debug(this.GetType().ToString(), "Print in Web Page : " + str);
            return str;
        }

        /// <summary>
        /// 生成签名，详见签名生成算法
        /// </summary>
        /// <returns>签名, sign字段不参加签名</returns>
        public string MakeSign()
        {
            //转url格式
            string str = ToUrl();
            //在string后加入API KEY
            str += "&key=" + WxPayConfig.Key;
            //MD5加密
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            return sb.ToString().ToUpper();
        }

        /// <summary>
        /// 检测签名是否正确
        /// </summary>
        /// <returns>正确返回true，错误抛异常</returns>
        public bool CheckSign()
        {
            //如果没有设置签名，则跳过检测
            if (!IsSet("sign"))
            {
                LogHelper.Error(this.GetType().ToString(), "WxPayData签名存在但不合法!");
                throw new WxPayException("WxPayData签名存在但不合法!");
            }
            //如果设置了签名但是签名为空，则抛异常
            else if (GetValue("sign") == null || GetValue("sign").ToString() == "")
            {
                LogHelper.Error(this.GetType().ToString(), "WxPayData签名存在但不合法!");
                throw new WxPayException("WxPayData签名存在但不合法!");
            }

            //获取接收到的签名
            string return_sign = GetValue("sign").ToString();

            //在本地计算新的签名
            string cal_sign = MakeSign();

            if (cal_sign == return_sign)
            {
                return true;
            }

            LogHelper.Error(this.GetType().ToString(), "WxPayData签名验证错误!");
            throw new WxPayException("WxPayData签名验证错误!");
        }
        
        /// <summary>
        /// 生成随机串，随机串包含字母或数字
        /// </summary>
        /// <returns>随机串</returns>
        private string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        /// <summary>
        /// 生成时间戳，标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数
        /// </summary>
        /// <returns>时间戳</returns>
        private static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 根据当前系统时间加随机序列来生成订单号
        /// </summary>
        /// <returns>订单号</returns>
        private string GenerateOutTradeNo()
        {
            var ran = new Random();
            return string.Format("{0}{1:yyyyMMddHHmmss}{2}", WxPayConfig.MchId, DateTime.Now, ran.Next(999));
        }
    }
}