using System;
using System.Collections.Generic;
using ChinaPayment.Util;

namespace ThirdPartyPaymentUI.ChinaPay.wtz
{
    public partial class Form_6_2_BackOpen : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /**
             * ��Ҫ����������ʱ����ϸ�Ķ�ע�ͣ�
             * 
             * ��Ʒ������ת��Ʒ<br>
             * ���ף���̨��ͨ����̨���ף�ֻ��ͬ��Ӧ��<br>
             * ���ڣ� 2015-11<br>
             * �汾�� 1.0.0
             * ��Ȩ�� �й�����<br>
             * ˵�������´���ֻ��Ϊ�˷����̻����Զ��ṩ���������룬�̻����Ը����Լ���Ҫ�����ռ����ĵ���д���ô�������ο������ṩ�������ܹ淶�Եȷ���ı���<br>
             * �ýӿڲο��ĵ�λ�ã�open.unionpay.com�������� ����  ��Ʒ�ӿڹ淶  �����ղ�Ʒ�ӿڹ淶��<br>
             *              ��ƽ̨����ӿڹ淶-��5����-��¼�����ڰ���Ӧ����ӿڹ淶��<br>
             * ���Թ����е�����������ʻ����������ԣ�1��������openƽ̨�в��Ҵ𰸣�
             * 							        ���Թ����е������������������ https://open.unionpay.com/ajweb/help/faq/list �������� FAQ �����������
             *                             ���Թ����в�����7λӦ����������������https://open.unionpay.com/ajweb/help/respCode/respCodeList ����Ӧ���������������
             *                          2�� ��ѯ�����˹�֧�֣� open.unionpay.comע��һ���û�����½�����Ͻǵ�������߿ͷ�������ѯ�˹�QQ����֧�֡�
             * ����˵��:ͬ�����ף�����Ҫ���պ�̨֪ͨ������״̬��ѯ���ף�ͬ��Ӧ��ȷ�����׳ɹ���
             */

            Dictionary<string, string> param = new Dictionary<string, string>();

            //  ������Ϣ��Ҫ��д
            param["orderId"] = Request.Form["orderId"].ToString();//�̻������ţ�8-32λ������ĸ���ɰ��Լ���������������Ͷ�����֤�룬����д��ȡ��֤��ʱһ����orderId���˴�Ĭ��ȡdemo��ʾҳ�洫�ݵĲ���
            param["merId"] = Request.Form["merId"].ToString();//�̻����룬��ĳ��Լ��Ĳ����̻��ţ��˴�Ĭ��ȡdemo��ʾҳ�洫�ݵĲ���
            param["txnTime"] = Request.Form["txnTime"].ToString();//��������ʱ�䣬ȡϵͳʱ�䣬�����Ͷ�����֤�룬����д��ȡ��֤��ʱһ����txnTime���˴�Ĭ��ȡdemo��ʾҳ�洫�ݵĲ���

            // ���󷽱�����
            // ͸���ֶΣ���ѯ��֪ͨ�������ļ��о���ԭ�����֣�������Ҫ�����ò��޸��Լ�ϣ��͸�������ݡ�
            // ���ֲ��������ַ�ʱ����Ӱ��������밴���潨��ķ�ʽ��д��
            // 1. �����ȷ�����ݲ������&={}[]"'�ȷ���ʱ������ֱ����д���ݣ�����ķ������¡�
            //param["reqReserved"] = "͸����Ϣ1|͸����Ϣ2|͸����Ϣ3";
            // 2. ���ݿ��ܳ���&={}[]"'����ʱ��
            // 1) �����Ҫ�����ļ�������ʾ���ɽ��ַ��滻��ȫ�ǣ����������������ַ����Լ�д���룬�˴�����ʾ����
            // 2) ��������ļ�û����ʾҪ�󣬿���һ��base64�����£���
            //    ע��������ݳ��ȣ�ʵ�ʴ�������ݳ��Ȳ��ܳ���1024λ��
            //    ��ѯ��֪ͨ�Ƚӿڽ���ʱʹ��System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reqReserved))��base64���ٶ�����������������
            //param["reqReserved"] = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("�����ʽ����Ϣ������"));

            //֧������Ϣ��д
            //���ǿ� ���ͣ����š��ֻ��š�CVN2����Ч�ڣ���֤�뿴ҵ�����ã�Ĭ�ϲ�Ҫ������֤�룩��
            //��ǿ� ���ͣ����š��ֻ��ţ�ѡ�ͣ�֤������+֤���š���������֤�뿴ҵ�����ã�Ĭ�ϲ�Ҫ������֤�룩��
            string accNo = "6226388000000095"; //����
            Dictionary<string, string> customerInfo = new Dictionary<string, string>();
            customerInfo["phoneNo"] = "18100000000"; //�ֻ���
            //customerInfo["certifTp"] = "01"; //֤�����ͣ�01-���֤
            //customerInfo["certifId"] = "510265790128303"; //֤���ţ�15λ���֤��У��β�ţ�18λ��У��β�ţ��������ǰ��д��У�����
            //customerInfo["customerNm"] = "����"; //����
            customerInfo["cvn2"] = "248"; //cvn2
            customerInfo["expired"] = "1912"; //��Ч�ڣ�YYMM��ʽ���ֿ��˿���ӡ����MMYY�ģ���ע��������õ�һ��
            customerInfo["smsCode"] = "111111"; //������֤��
            
            //param["accNo"] = accNo; //���ţ��ɹ淶�밴�˷�ʽ��д
            //param["customerInfo"] = DemoAcpServiceUtil.GetCustomerInfo(customerInfo, System.Text.Encoding.UTF8); //�ֿ��������Ϣ���ɹ淶�밴�˷�ʽ��д
            param["accNo"] = AcpService.EncryptData(accNo, System.Text.Encoding.UTF8); //���ţ��¹淶�밴�˷�ʽ��д
            param["customerInfo"] = AcpService.GetCustomerInfoWithEncrypt(customerInfo, System.Text.Encoding.UTF8); //�ֿ��������Ϣ���¹淶�밴�˷�ʽ��д

            //������Ϣ�������������Ҫ�Ķ�
             param["version"] = SdkConfig.Version;//�汾��
            param["encoding"] = "UTF-8";//���뷽ʽ
            param["signMethod"] = SdkConfig.SignMethod;//ǩ������
            param["txnType"] = "79";//��������
            param["txnSubType"] = "00";//��������
            param["bizType"] = "000301";//ҵ������
            param["accessType"] = "0";//��������
            param["channelType"] = "07";//��������
            param["encryptCertId"] = AcpService.GetEncryptCertId();//����֤��ID
            param["backUrl"] = SdkConfig.BackUrl;  //��̨֪ͨ��ַ


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
                        Response.Write("��ͨ�ɹ���<br>\n");
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