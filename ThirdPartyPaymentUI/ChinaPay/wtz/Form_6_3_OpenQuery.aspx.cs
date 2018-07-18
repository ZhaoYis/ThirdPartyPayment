using System;
using System.Collections.Generic;
using ChinaPayment.Util;

namespace ThirdPartyPaymentUI.ChinaPay.wtz
{
    public partial class Form_6_3_OpenQuery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /**
             * ��Ҫ����������ʱ����ϸ�Ķ�ע�ͣ�
             * 
             * ��Ʒ������ת��Ʒ<br>
             * ���ף���ѯ��ͨ״̬����̨���ף�ͬ������<br>
             * ���ڣ� 2015-11<br>
             * ��Ȩ�� �й�����<br>
             * ˵�������´���ֻ��Ϊ�˷����̻����Զ��ṩ���������룬�̻����Ը����Լ���Ҫ�����ռ����ĵ���д���ô�������ο������ṩ�������ܹ淶�Եȷ���ı���<br>
             * ��ʾ���ýӿڲο��ĵ�λ�ã�open.unionpay.com�������� ����  ��Ʒ�ӿڹ淶  ������ת��Ʒ�ӿڹ淶����<br>
             *                  ��ȫ����ƽ̨����ӿڹ淶 ��3���� �ļ��ӿڡ���4.�����ļ�����Լ����<br>
             * ���Թ����е�����������ʻ����������ԣ�1��������openƽ̨�в��Ҵ𰸣�
             * 							        ���Թ����е������������������ https://open.unionpay.com/ajweb/help/faq/list �������� FAQ �����������
             *                             ���Թ����в�����7λӦ����������������https://open.unionpay.com/ajweb/help/respCode/respCodeList ����Ӧ���������������
             *                          2�� ��ѯ�����˹�֧�֣� open.unionpay.comע��һ���û�����½�����Ͻǵ�������߿ͷ�������ѯ�˹�QQ����֧�֡�
             *                          3�� ���Ի�������֧����ʹ�ò��Կ��Ų��ԣ� FAQ���������Կ��š�
             *                          4�� �л���������Ҫ����FAQ�������л���
             * ����˵�������ݿ��Ų�ѯ���Ƿ��Ѿ���ͨ��ͬ��Ӧ��ȷ�����׳ɹ���
             */

            Dictionary<string, string> param = new Dictionary<string, string>();

            //  ������Ϣ��Ҫ��д
            param["orderId"] = Request.Form["orderId"].ToString();//�̻������ţ�8-32λ������ĸ���ɰ��Լ�����������˴�Ĭ��ȡdemo��ʾҳ�洫�ݵĲ���
            param["merId"] = Request.Form["merId"].ToString();//�̻����룬��ĳ��Լ��Ĳ����̻��ţ��˴�Ĭ��ȡdemo��ʾҳ�洫�ݵĲ���
            param["txnTime"] = Request.Form["txnTime"].ToString();//��������ʱ�䣬ȡϵͳʱ�䣬�˴�Ĭ��ȡdemo��ʾҳ�洫�ݵĲ���

            //֧������Ϣ��д
            string accNo = "6226090000000048"; //����
            //param["accNo"] = accNo; //���ţ��ɹ淶�밴�˷�ʽ��д
            param["accNo"] = AcpService.EncryptData(accNo, System.Text.Encoding.UTF8); //���ţ��¹淶�밴�˷�ʽ��д
            
            //������Ϣ�������������Ҫ�Ķ�
             param["version"] = SdkConfig.Version;//�汾��
            param["encoding"] = "UTF-8";//���뷽ʽ
            param["signMethod"] = SdkConfig.SignMethod;//ǩ������
            param["txnType"] = "78";//��������
            param["txnSubType"] = "00";//��������
            param["bizType"] = "000301";//ҵ������
            param["accessType"] = "0";//��������
            param["channelType"] = "07";//��������
            param["encryptCertId"] = AcpService.GetEncryptCertId();//����֤��ID

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
                        //�ɹ�
                        //TODO
                        Response.Write("�ѿ�ͨ��<br>\n");
                        if (rspData.ContainsKey("customerInfo")) 
                        {
                            Dictionary<string, string> customerInfo = AcpService.ParseCustomerInfo(rspData["customerInfo"], System.Text.Encoding.UTF8);
                            if (customerInfo.ContainsKey("phoneNo")) {
                                string phoneNo = customerInfo["phoneNo"]; //customerInfo����������ɲο��˷�ʽ��ȡ  
                            }
                            foreach (KeyValuePair<string, string> pair in customerInfo)
                            {
                                Response.Write(pair.Key + "=" + pair.Value + "<br>\n");
                            }
                        }
                    }
                    else if ("77" == respcode)
                    {
                        //δ��ͨ
                        //TODO
                        Response.Write("δ��ͨ��<br>\n");
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
                    Response.Write("�̻�����֤���ر���ǩ��ʧ��\n");
                }
            }
            else
            {
                Response.Write("����ʧ��<br>\n");
            }
        }
    }
}