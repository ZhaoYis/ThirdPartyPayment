using System;
using System.Collections.Generic;
using ChinaPayment.Util;

namespace ThirdPartyPaymentUI.ChinaPay.wtz
{
    public partial class Form_6_6_SmsConsume : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /**
             * ��Ҫ����������ʱ����ϸ�Ķ�ע�ͣ�
             * 
             * ��Ʒ������ת��Ʒ<br>
             * ���ף����Ѷ��ţ���̨���ף���֪ͨ<br>
             * ���ڣ� 2015-11<br>
             * �汾�� 1.0.0
             * ��Ȩ�� �й�����<br>
             * ˵�������´���ֻ��Ϊ�˷����̻����Զ��ṩ���������룬�̻����Ը����Լ���Ҫ�����ռ����ĵ���д���ô�������ο������ṩ�������ܹ淶�Եȷ���ı���<br>
             * ����˵��: ���  + ����  + �ֻ���phoneNo��customerInfo�򣩣�ͬ��Ӧ��ȷ�����׳ɹ���
             */

            Dictionary<string, string> param = new Dictionary<string, string>();

            //������Ϣ��Ҫ��д
            param["orderId"] = Request.Form["orderId"].ToString();//�̻������ţ�8-32λ������ĸ���ɰ��Լ�����������˴�Ĭ��ȡdemo��ʾҳ�洫�ݵĲ���
            param["merId"] = Request.Form["merId"].ToString();//�̻����룬��ĳ��Լ��Ĳ����̻��ţ��˴�Ĭ��ȡdemo��ʾҳ�洫�ݵĲ���
            param["txnTime"] = Request.Form["txnTime"].ToString();//��������ʱ�䣬ȡϵͳʱ�䣬�˴�Ĭ��ȡdemo��ʾҳ�洫�ݵĲ���
            param["txnAmt"] = Request.Form["txnAmt"].ToString();//���׽���λ�֣��˴�Ĭ��ȡdemo��ʾҳ�洫�ݵĲ���

            //֧������Ϣ��д
            string accNo = "6226090000000048"; //����
            Dictionary<string, string> customerInfo = new Dictionary<string, string>();
            customerInfo["phoneNo"] = "18100000000"; //�ֻ���

            //param["accNo"] = accNo; //���ţ��ɹ淶�밴�˷�ʽ��д
            //param["customerInfo"] = AcpService.GetCustomerInfo(customerInfo, System.Text.Encoding.UTF8); //�ֿ��������Ϣ���ɹ淶�밴�˷�ʽ��д
            param["accNo"] = AcpService.EncryptData(accNo, System.Text.Encoding.UTF8); //���ţ��¹淶�밴�˷�ʽ��д
            param["customerInfo"] = AcpService.GetCustomerInfoWithEncrypt(customerInfo, System.Text.Encoding.UTF8); //�ֿ��������Ϣ���¹淶�밴�˷�ʽ��д
            
            //������Ϣ�������������Ҫ�Ķ�
             param["version"] = SdkConfig.Version;//�汾��
            param["encoding"] = "UTF-8";//���뷽ʽ
            param["signMethod"] = SdkConfig.SignMethod;//ǩ������
            param["txnType"] = "77";//��������
            param["txnSubType"] = "02";//��������
            param["bizType"] = "000301";//ҵ������
            param["accessType"] = "0";//��������
            param["channelType"] = "07";//��������
            param["currencyCode"] = "156";//���ױ��֣������̻����
            param["encryptCertId"] = CertUtil.GetEncryptCertId();//����֤��ID


            AcpService.Sign(param, System.Text.Encoding.UTF8);  // ǩ��
            string url = SdkConfig.BackTransUrl;
            Dictionary<String, String> rspData = AcpService.Post(param, url, System.Text.Encoding.UTF8);

            //Response.Write(DemoUtil.GetPrintResult(url, param, rspData));

            if (rspData.Count != 0)
            {
                if (AcpService.Validate(rspData, System.Text.Encoding.UTF8))
                {
                    Response.Write("�̻�����֤���ر���ǩ���ɹ���<br>\n");
                    string respcode = rspData["respCode"]; //����Ӧ�����Ҳ���ô˷�����ȡ
                    if ("00" == respcode)
                    {
                        //TODO
                        Response.Write("���Ѷ��Ž��׳ɹ���<br>\n");
                    }
                    else
                    {
                        //����Ӧ��������ʧ�ܴ���
                        //TODO
                        Response.Write("ʧ�ܣ�" + rspData["respMsg"] + "��<br>\n");
                    }
                }
                else
                {
                    Response.Write("�̻�����֤���ر���ǩ��ʧ�ܡ�<br>\n");
                }
            }
            else
            {
                Response.Write("����ʧ��<br>\n");
            }
        }
    }
}