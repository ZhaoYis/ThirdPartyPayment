using System;
using System.Collections.Generic;
using System.Text;
using ChinaPayment.Util;

namespace ThirdPartyPaymentUI.ChinaPay.wtz
{
    public partial class Form_6_2_FrontOpen : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /**
              * ��Ҫ����������ʱ����ϸ�Ķ�ע�ͣ�
              * 
              * ��Ʒ������ת��Ʒ<br>
              * ���ף������࿪ͨ��ǰ̨���ף���ǰ̨֪ͨ��̨֪ͨ<br>
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
              * ����˵��:��̨֪ͨ���߷���ͨ��ѯ����ȷ�Ͽ��Ƿ��Ѿ���ͨ��
              */

            Dictionary<string, string> param = new Dictionary<string, string>();

            //  ������Ϣ��Ҫ��д
            param["orderId"] = Request.Form["orderId"].ToString();//�̻������ţ�8-32λ������ĸ���ɰ��Լ�����������˴�Ĭ��ȡdemo��ʾҳ�洫�ݵĲ���
            param["merId"] = Request.Form["merId"].ToString();//�̻����룬��ĳ��Լ��Ĳ����̻��ţ��˴�Ĭ��ȡdemo��ʾҳ�洫�ݵĲ���
            param["txnTime"] = Request.Form["txnTime"].ToString();//��������ʱ�䣬ȡϵͳʱ�䣬�˴�Ĭ��ȡdemo��ʾҳ�洫�ݵĲ���
            //param["reserved"] = "{customPage=true}";//�����ͨҳ����Ҫʹ��Ƕ��ҳ��Ļ��������ʹ��÷�

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
            //��ѡ�Ϳ��š��ֻ��š�֤������+֤���š�������Ҳ���Զ����ͣ��ڽ���������ЩҪ��
            string accNo = "6226090000000048";//����
            Dictionary<string, string> customerInfo = new Dictionary<string, string>();
            customerInfo["phoneNo"] = "18100000000"; //�ֻ���
            customerInfo["certifTp"] = "01"; //֤�����ͣ�01-���֤
            customerInfo["certifId"] = "510265790128303"; //֤���ţ�15λ���֤��У��β�ţ�18λ��У��β�ţ��������ǰ��д��У�����
            customerInfo["customerNm"] = "����"; //����

            //param["accNo"] = accNo; //���ţ��ɹ淶�밴�˷�ʽ��д
            //param["customerInfo"] = AcpService.GetCustomerInfo(customerInfo, System.Text.Encoding.UTF8); //�ֿ��������Ϣ���ɹ淶�밴�˷�ʽ��д
            param["accNo"] = AcpService.EncryptData(accNo, System.Text.Encoding.UTF8); //���ţ��¹淶�밴�˷�ʽ��д
            param["customerInfo"] = AcpService.GetCustomerInfoWithEncrypt(customerInfo, System.Text.Encoding.UTF8); //�ֿ��������Ϣ���¹淶�밴�˷�ʽ��д

            // ������ʱʱ�䡣
            // ������ʱ��󣬳����������⣬������������ϵͳ��ܾ�������ʾ��ʱ�� ��ת�����������������ʱ���׳ɹ������Զ��˿��Լ5�������ս������ֿ����˻���
            // ��ʱ�佨��ȡ֧��ʱ�ı���ʱ���15���ӡ�
            // ������ʱʱ�����ѯ�ӿ�Ӧ��origRespCode����A6����00�ľͿ����ж�Ϊʧ�ܡ�
            param["payTimeout"] = DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss");


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
            param["frontUrl"] = SdkConfig.FrontUrl;  //ǰ̨֪ͨ��ַ
            param["backUrl"] = SdkConfig.BackUrl;  //��̨֪ͨ��ַ

            AcpService.Sign(param, System.Text.Encoding.UTF8);  // ǩ��
            string url = SdkConfig.FrontTransUrl;
            string html = AcpService.CreateAutoFormHtml(url, param, System.Text.Encoding.UTF8);
            Response.ContentEncoding = Encoding.UTF8; // ָ���������
            Response.Write(html);
        }
    }
}