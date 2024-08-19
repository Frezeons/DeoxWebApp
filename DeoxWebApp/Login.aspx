<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DeoxWebApp.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <style>
        body {
            height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
            background: linear-gradient(135deg, #e0e7ef 0%, #f0f4f8 100%);
            margin: 0;
        }
        .login {
            width:400px;
            padding: 40px;
            border-radius: 5px;
            background: rgba(255, 255, 255, 0.8);
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }
        .login h2 {
            color: #333;
            text-align: center;
            margin-bottom: 1rem;
        }
        .form-control {
            background: rgba(255, 255, 255, 0.9);
            color: #333;
            border: 1px solid rgba(0, 0, 0, 0.1);
        }
        .form-control:focus {
            box-shadow: inset 0 -5px 45px rgba(100, 100, 100, 0.2), 0 1px 1px rgba(255, 255, 255, 0.2);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login">
            <h2>Login</h2>
            <div class="mb-3">
                <asp:Label ID="username_label" runat="server" Text="Username" CssClass="form-label" />
                <asp:TextBox ID="username_text" runat="server" CssClass="form-control" />
            </div>
            <div class="mb-3">
                <asp:Label ID="pass_label" runat="server" Text="Password" CssClass="form-label" />
                <asp:TextBox ID="pass_text" runat="server" TextMode="Password" CssClass="form-control" />
            </div>
            <asp:Label ID="message_login" runat="server" CssClass="text-danger"></asp:Label>
            <br />
            <div class="d-grid gap-2">
                <asp:Button ID="button_login" CssClass="btn btn-primary btn-block" runat="server" Text="Login" OnClick="button_login_Click" />
                <asp:LinkButton ID="button_to_register" runat="server" Text="Register" CssClass="btn btn-secondary btn-block" OnClick="button_to_register_Click" />
                <asp:LinkButton ID="btn_reset_password" runat="server" Text="Reset Password" CssClass="btn btn-link" OnClick="btn_reset_password_Click" />
            </div>
        </div>
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
