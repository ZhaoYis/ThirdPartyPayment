using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using ChinaPayment.Util;
using WeCahtPayment.Util;

namespace ThirdPartyPaymentUI.ChinaPay
{
    /// <summary>
    /// �̻��˽����������ر���
    /// </summary>
    public partial class FrontRcvResponse : System.Web.UI.Page
    {
        protected string Html;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST")
            {
                // ʹ��Dictionary�������
                Dictionary<string, string> resData = new Dictionary<string, string>();

                NameValueCollection coll = Request.Form;

                string[] requestItem = coll.AllKeys;

                for (int i = 0; i < requestItem.Length; i++)
                {
                    resData.Add(requestItem[i], Request.Form[requestItem[i]]);
                }

                //�̻��˸��ݷ��ر������ݴ����Լ���ҵ���߼� ,DEMO�˴�ֻ������Ľ��
                StringBuilder builder = new StringBuilder();
                LogHelper.Info("FrontRcvResponse", "receive front notify: " + SDKUtil.CreateLinkString(resData, false, true, System.Text.Encoding.UTF8));

                builder.Append("<tr><td align=\"center\" colspan=\"2\"><b>�̻��˽����������ر��Ĳ����ձ����ʽ������</b></td></tr>");

                for (int i = 0; i < requestItem.Length; i++)
                {
                    builder.Append("<tr><td width=\"30%\" align=\"right\">" + requestItem[i] + "</td><td style='word-break:break-all'>" + Request.Form[requestItem[i]] + "</td></tr>");
                }

                if (AcpService.Validate(resData, System.Text.Encoding.UTF8))
                {
                    builder.Append("<tr><td width=\"30%\" align=\"right\">�̻�����֤�������ر��Ľ��</td><td>��֤ǩ���ɹ�.</td></tr>");

                    string respcode = resData["respCode"]; //00��A6Ϊ�ɹ�������Ϊʧ�ܡ������ֶ�Ҳ�ɰ��˷�ʽ��ȡ��

                    //�����������ҵ�����˻᷵����������Ҫ���ܵĻ����밴�˷�������
                    //if(resData.ContainsKey("accNo"))
                    //{
                    //    string accNo = SecurityUtil.DecryptData(resData["accNo"], System.Text.Encoding.UTF8); 
                    //}

                    //customerInfo����Ļ�ȡ
                    if (resData.ContainsKey("customerInfo"))
                    {
                        Dictionary<string, string> customerInfo = AcpService.ParseCustomerInfo(resData["customerInfo"], System.Text.Encoding.UTF8);
                        if (customerInfo.ContainsKey("phoneNo"))
                        {
                            string phoneNo = customerInfo["phoneNo"]; //customerInfo����������ɲο��˷�ʽ��ȡ  
                        }
                        foreach (KeyValuePair<string, string> pair in customerInfo)
                        {
                            builder.Append(pair.Key + "=" + pair.Value + "<br>\n");
                        }
                    }
                }
                else
                {
                    builder.Append("<tr><td width=\"30%\" align=\"right\">�̻�����֤�������ر��Ľ��</td><td>��֤ǩ��ʧ��.</td></tr>");
                }
                Html = builder.ToString();
            }
        }
    }
}