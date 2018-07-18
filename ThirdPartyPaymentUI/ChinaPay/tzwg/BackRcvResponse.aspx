<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BackRcvResponse.aspx.cs" Inherits="ThirdPartyPaymentUI.ChinaPay.BackRcvResponse" %>

<%@ Import Namespace="ChinaPayment.Util" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>后台接收银联后台通知</title>
</head>
<body>
    <form id="form1" runat="server">
        <table border="1">
            <%
                Response.Write(this.Html);
            %>
        </table>
    </form>
</body>

</html>



