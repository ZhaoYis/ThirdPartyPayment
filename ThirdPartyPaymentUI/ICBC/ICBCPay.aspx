<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ICBCPay.aspx.cs" Inherits="ThirdPartyPaymentUI.ICBC.ICBCPay" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form name="sendOrder" method="post" action="<%= icmcModel.OrderPostUrl %>" id="order">
        <input type="hidden" name="interfaceName" value="<%= icmcModel.InterfaceName %>" />
        <input type="hidden" name="interfaceVersion" value="<%= icmcModel.InterfaceVersion %>" />
        <input type="hidden" name="tranData" value="<%= icmcModel.TranData %>" />
        <input type="hidden" name="merSignMsg" value="<%= icmcModel.MerSignMsg %>" />
        <input type="hidden" name="merCert" value="<%= icmcModel.MerCert %>" />
    </form>
    <script type="text/javascript">
        document.order.submit();
    </script>
</body>
</html>--%>
