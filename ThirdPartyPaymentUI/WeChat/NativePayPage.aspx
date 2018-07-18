<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NativePayPage.aspx.cs" Inherits="ThirdPartyPaymentUI.WeChat.NativePayPage" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="content-type" content="text/html;image/gif;charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>微信支付样例-扫码支付</title>
</head>
<body>
    <div style="margin-left: 10px; color: #00CD00; font-size: 30px; font-weight: bolder;">扫码支付模式一</div>
    <br />
    <asp:Image ID="Image1" runat="server" Style="width: 200px; height: 200px;" />
    <br />
    <br />
    <br />
    <div style="margin-left: 10px; color: #00CD00; font-size: 30px; font-weight: bolder;">扫码支付模式二</div>
    <br />
    <asp:Image ID="Image2" runat="server" Style="width: 200px; height: 200px;" />

</body>
</html>
