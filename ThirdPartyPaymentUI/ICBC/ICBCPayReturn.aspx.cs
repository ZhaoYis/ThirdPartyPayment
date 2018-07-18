//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using ICBCPayment;

//namespace ThirdPartyPaymentUI.ICBC
//{
//    public partial class ICBCPayReturn : System.Web.UI.Page
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            if (Request.Form["notifyData"] != null)
//            {
//                try
//                {
//                    ICBCConfig icbcInfo = new ICBCConfig();

//                    icbcInfo.TranData = Request.Form["notifyData"];
//                    icbcInfo.MerSignMsg = Request.Form["signMsg"].ToString();

//                    icbcInfo = ICBCPayOnline.GetCheckReturnInfo(icbcInfo);
//                    //自定义返回
//                    string strOrderSN = Request.Form["merVAR"].ToString();

//                    if (icbcInfo.IsCheck)
//                    {
//                        DataSet myds = new DataSet();
//                        StringReader strReader = new StringReader(icbcInfo.TranData);
//                        myds.ReadXml(strReader);
//                        DataTable mytable = new DataTable();
//                        mytable = myds.Tables["bank"];

//                        if (null != mytable && mytable.Rows.Count > 0)
//                        {
//                            if (mytable.Rows[0]["tranStat"].ToString().Trim() == "1")
//                            {
//                                //这里做成功操作
//                                Response.Write("http://XXXXXX/PaySuccess.aspx");
//                            }
//                            else
//                            {
//                                Response.Write("http://XXXX/PayError.aspx");
//                                //Response.Write(mytable.Rows[0]["comment"].ToString());
//                            }
//                        }
//                    }
//                    else
//                    {
//                        Response.Write("http://XXXX/PayError.aspx");
//                        //Response.Write("签名失败！");
//                    }
//                }
//                catch (Exception)
//                {
//                    Response.Write("http://XXXX/PayError.aspx");
//                }
//            }
//            else
//            {
//                //Response.Write("银行数据返回失败，请通知管理员！");
//                Response.Write("http://XXXXX/PayError.aspx");
//            }
//        }
//    }
//}