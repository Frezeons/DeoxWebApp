<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reset_User_Password.aspx.cs" Inherits="DeoxWebApp.Reset_User_Password" %>

<!DOCTYPE html>
<html lang="tr">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Şifre Değiştirme</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f8f9fa;
            margin: 0;
            padding: 0;
        }
        .container {
            max-width: 40%;
            margin: 50px auto;
            background-color: #fff;
            padding: 3%;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }
        .container h2 {
            margin-bottom: 20px;
        }
        .form-group {
            margin-bottom: 15px;
        }
        .form-group label {
            display: block;
            margin-bottom: 5px;
            font-weight: bold;
        }
        .form-group input {
            width: 100%;
            padding: 10px;
            border: 1px solid #ced4da;
            border-radius: 4px;
        }
        .form-group input:focus {
            border-color: #80bdff;
            outline: none;
            box-shadow: 0 0 5px rgba(128, 189, 255, 0.5);
        }
        .btn {
            display: inline-block;
            padding: 10px 20px;
            font-size: 16px;
            font-weight: bold;
            color: #fff;
            background-color: #007bff;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            text-align: center;
            text-decoration: none;
            transition: background-color 0.3s;
        }
        .btn:hover {
            background-color: #0056b3;
        }
        .alert {
            padding: 15px;
            border-radius: 4px;
            margin-bottom: 20px;
            display: none;
        }
        .alert-success {
            color: #155724;
            background-color: #d4edda;
            border-color: #c3e6cb;
        }
        .alert-danger {
            color: #721c24;
            background-color: #f8d7da;
            border-color: #f5c6cb;
        }
    </style>

</head>
<body>
    <div class="container">
        <h2>Şifre Değiştirme</h2>
        <div id="message" class="alert" role="alert"></div>
        <div class="form-group">
            <label for="newPassword">Yeni Şifre:</label>
            <input type="password" class="form-control" id="newPassword">
        </div>
        <div class="form-group">
            <label for="confirmPassword">Yeni Şifre Tekrar:</label>
            <input type="password" class="form-control" id="confirmPassword">
        </div>

        <button class="btn"  onclick="changePass()" >Şifreyi Değiştir</button>
    </div>

    <script>
        function getInfo() {
            debugger;
            var currentPath = window.location.pathname;
            var xhr = new XMLHttpRequest();
            xhr.open("POST", currentPath, true);
            xhr.onreadystatechange = function () {
                if (xhr.readyState === XMLHttpRequest.DONE) {
                    if (xhr.status === 200) {
                        var response = xhr.response.split("|")[0].replace("\"", "");
                        /*alert(response);*/
                    }
                }
            }
            xhr.send(JSON.stringify({ info: "" }));
        }

        function changePass() {
            debugger;
            var newPasswordnew = document.getElementById("newPassword").value;
            var confirmPasswordnew = document.getElementById("confirmPassword").value;

            if (newPasswordnew !== confirmPasswordnew) {
                alert("Şifreler eşleşmiyor!");
                return; 
            }
            var changedpassword = newPasswordnew + "_" + newPasswordnew
            var currentPath = window.location.pathname;
            var xhr = new XMLHttpRequest();
            xhr.open("POST", currentPath, true);
            xhr.onreadystatechange = function () {
                if (xhr.readyState === XMLHttpRequest.DONE) {
                    if (xhr.status === 200) {
                    /*    var response = xhr.response.split("|")[0].replace("\"", "");*/
                        /*alert(response);*/
                        alert("Şifreniz Değiştirildi");
                        window.location.href = "Login";
                    }
                    if (xhr.status === 345) {
                        /*var response = xhr.response.split("|")[0].replace("\"", "");*/
                        alert("Şifre en az 5 karakter olmalı, bir büyük harf ve bir özel karakter içermelidir.");
                    }
                    if (xhr.status === 432) {
                        /*var response = xhr.response.split("|")[0].replace("\"", "");*/
                        alert("Şifreniz Değiştirildi");
                        window.location.href = "Login";
                    }

                }
            }
            xhr.send(JSON.stringify({ password: changedpassword }));
        }

        window.onload = getInfo();

    </script>
</body>
</html>
