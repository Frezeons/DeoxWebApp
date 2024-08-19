<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reset_Password.aspx.cs" Inherits="DeoxWebApp.Reset_Password" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Şifremi Unuttum</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <style>
        body {
            background-color: #f8f9fa;
        }

        .card {
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

        .card-header {
            background-color: #007bff;
            color: white;
        }

        .btn-primary {
            background-color: #007bff;
            border: none;
        }

            .btn-primary:hover {
                background-color: #0056b3;
            }

        .container {
            margin-top: 100px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <div class="row justify-content-center">
                <div class="col-md-6">
                    <div class="card">
                        <div class="card-header text-center">
                            <h3>Şifremi Unuttum</h3>
                        </div>
                        <div class="card-body">
                            <div class="mb-3">
                                <label for="emailOrUsername" class="form-label">E-posta veya Kullanıcı Adı</label>
                                <input type="text" class="form-control" id="emailOrUsername" placeholder="E-posta veya Kullanıcı Adınızı Giriniz" />
                            </div>
                            <div class="d-grid">
                                <button type="button" class="btn btn-primary" onclick="SendRequest()">Gönder</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
<script>
    function SendRequest() {
        event.preventDefault();
        debugger;
        var currentPath = window.location.pathname;
        var mailOrUser = document.getElementById("emailOrUsername").value;
        var mailOrUserValue = mailOrUser;
        var xhr = new XMLHttpRequest();
        xhr.open("POST", currentPath, true);
        xhr.onreadystatechange = function () {
            if (xhr.readyState === XMLHttpRequest.DONE) {
                if (xhr.status === 200) {
                    //var response = xhr.response.split("|")[0].replace("\"", "");
                    console.log("Http Requsest 200 Success");
                    window.location.reload();
                    alert("E-mailinizi kontrol ediniz.");
                } else if (xhr.status === 404) {
                    alert("Böyle bir E-mail adresi bulunmamaktadır.");
                } else if (xhr.status === 405) {
                    alert("Böyle bir Kullanıcı Adı bulunmamaktadır.");
                }
                else if (xhr.status === 500) {
                }
            }
        }
        xhr.send(JSON.stringify({ mailOrUser: mailOrUserValue }));
    }
</script>

</html>
