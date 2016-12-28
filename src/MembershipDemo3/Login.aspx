<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MembershipDemo3.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Login
            </h1>
            <asp:Login ID="Login1" runat="server" CreateUserText="注册" CreateUserUrl="~/Register.aspx" PasswordRecoveryText="忘记密码" PasswordRecoveryUrl="~/PasswordRecovery.aspx" OnLoggedIn="Login1_LoggedIn">
            </asp:Login>
        </div>
    </form>
</body>
</html>
