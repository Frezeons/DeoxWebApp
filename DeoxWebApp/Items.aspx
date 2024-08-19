<%@ Page Title="Ürün Listesi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Items.aspx.cs" Inherits="DeoxWebApp.Items" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <style>
            body {
                font-family: Arial, sans-serif;
                background-color: #f8f9fa;
                margin: 0;
                padding: 0;
            }

            .container {
                max-width: 1200px;
                margin: 0 auto;
                padding: 20px;
            }

            .header {
                background-color: #28a745;
                color: #fff;
                padding: 20px;
                border-radius: 8px 8px 0 0;
                text-align: center;
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            }

            .header h1 {
                margin: 0;
                font-size: 32px;
                font-weight: 600;
            }

            .gridview-container {
                margin-top: 20px;
                background-color: #fff;
                border-radius: 8px;
                overflow: hidden;
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            }

            .gridview {
                width: 100%;
                border-collapse: collapse;
                margin: 0;
            }

            .gridview th, .gridview td {
                border: 1px solid #ddd;
                padding: 15px;
                text-align: left;
            }

            .gridview th {
                background-color: #28a745;
                color: #fff;
                font-weight: 600;
            }

            .gridview tr:nth-child(even) {
                background-color: #f9f9f9;
            }

            .gridview tr:hover {
                background-color: #f1f1f1;
            }

            .button-container {
                text-align: center;
                margin-top: 30px;
            }

            .button-primary {
                background-color: #28a745;
                color: #fff;
                border: none;
                padding: 14px 28px;
                font-size: 18px;
                border-radius: 8px;
                cursor: pointer;
                transition: background-color 0.3s ease, transform 0.3s ease;
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
                display: inline-block;
                text-decoration: none;
                margin: 10px;
            }

            .button-primary:hover {
                background-color: #218838;
                transform: scale(1.05);
            }

            .button-primary:active {
                background-color: #1e7e34;
                transform: scale(0.98);
            }

            .modal-content {
                border-radius: 8px;
                padding: 20px;
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
            }

            .modal-header {
                border-bottom: 1px solid #ddd;
                padding-bottom: 10px;
            }

            .modal-title {
                font-size: 24px;
                font-weight: 600;
            }

            .modal-footer {
                border-top: 1px solid #ddd;
                padding-top: 10px;
            }

            .form-group {
                margin-bottom: 15px;
            }

            .form-group-cbx {
                margin-bottom: 15px;
                padding-bottom: 2px;
            }

            .form-group-hidden {
                margin-bottom: 15px;
                display: none;
            }

            .form-group label {
                font-weight: 600;
                margin-bottom: 5px;
                display: block;
            }

            .form-group input, .form-group select {
                width: 100%;
                padding: 10px;
                border: 1px solid #ddd;
                border-radius: 4px;
                box-sizing: border-box;
            }

            .form-group .error-message, .form-group .success-message {
                color: #dc3545;
                font-weight: 600;
            }

            .form-group .success-message {
                color: #28a745;
            }

            /* styles.css */

            .loader {
                position: fixed;
                left: 50%;
                top: 50%;
                transform: translate(-50%, -50%);
                border: 16px solid #f3f3f3; /* Light grey */
                border-top: 16px solid #3498db; /* Blue */
                border-radius: 50%;
                width: 120px;
                height: 120px;
                animation: spin 2s linear infinite;
            }

            @keyframes spin {
                0% {
                    transform: rotate(0deg);
                }

                100% {
                    transform: rotate(360deg);
                }
            }
        </style>
        <div class="container">
            <div class="header">
                <h1>Ürün Listesi</h1>
            </div>
            <div id="loader" class="loader"></div>
            <div id="content" style="display: none;">

                <div class="gridview-container">
                    <table id="fd_table" class="table table-striped table-bordered">
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Item Name</th>
                                <th>Item Color</th>
                                <th>Item Count</th>
                                <th id="actionHeader">İşlem</th>
                            </tr>
                        </thead>
                        <tbody id="tbodyFD">
                        </tbody>
                    </table>
                </div>
                <div>
                    <asp:Label ID="ErrorMessage" runat="server" CssClass="error-message" />
                    <asp:Label ID="SuccessMessage" runat="server" CssClass="success-message" />
                </div>
                <div class="button-container">
                    <button type="button" id="modal_open" onclick="ClearModal(); VisibleButton('add')" class="button-primary" data-bs-toggle="modal" data-bs-target="#myModal">
                        Ürün Ekle
                    </button>
                </div>
            </div>

        </div>

        <!-- Ürün Ekle Modal  -->
        <div class="modal fade" id="myModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabel"></h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group-hidden">
                            <label for="txtProductId" class="form-label">Product Id</label>
                            <input type="text" id="txtProductId" class="form-textbox" style="visibility: hidden;">
                        </div>
                        <div class="form-group">
                            <label for="txtProductName">Ürün Adı:</label>
                            <input type="text" id="txtProductName" class="form-textbox" required>
                        </div>
                        <div class="form-group">
                            <label for="txtProductCount">Ürün Adeti:</label>
                            <input type="text" id="txtProductCount" class="form-textbox" required>
                        </div>
                        <div class="form-group-cbx">
                            <label for="cbxColors">Renk Seçiniz:</label>
                            <select id="cbxColors" class="form-control">
                            </select>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-success" data-bs-dismiss="modal" id="btnAdd" onclick="AddProducts();ClearModal();">Ekle</button>
                        <button type="button" class="btn btn-warning" data-bs-dismiss="modal" id="btnEdit" onclick=" UpdateProduct();ClearModal();">Düzenle</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- XHR KODU -->
        <script>


            function GetUserRole() {
                //debugger;
                var currentPath = window.location.pathname;
                var xhr = new XMLHttpRequest();
                xhr.open("POST", currentPath, true);
                xhr.onreadystatechange = function () {
                    if (xhr.readyState === XMLHttpRequest.DONE) {
                        if (xhr.status === 200) {
                            debugger;
                            var response = xhr.response.split("|")[0].replace("\"", "");
                            var editButtons = document.getElementsByName('TableEdit');
                            var deleteButtons = document.getElementsByName('TableDel');
                            if (response == 1) {
                                document.getElementById("modal_open").style.display = 'none';
                                document.getElementById("actionHeader").style.display = 'none';


                                // Her bir buton üzerinde döngü
                                for (var i = 0; i < editButtons.length; i++) {
                                    // Butonu gizle
                                    editButtons[i].style.display = 'none';
                                }
                                for (var i = 0; i < deleteButtons.length; i++) {
                                    // Butonu gizle
                                    deleteButtons[i].style.display = 'none';
                                }
                            }
                            else if (response == 2) {
                                // Her bir buton üzerinde döngü
                                for (var i = 0; i < editButtons.length; i++) {
                                    // Butonu gizle
                                    editButtons[i].style.display = '';
                                }
                                for (var i = 0; i < deleteButtons.length; i++) {
                                    // Butonu gizle
                                    deleteButtons[i].style.display = 'none';
                                }

                                document.getElementById("modal_open").style.display = '';
                                document.getElementById("actionHeader").style.display = '';
                            }
                            else if (response == 3) {//1 user, 2 moderator , 3 admin
                                document.getElementById("modal_open").style.display = '';
                                document.getElementById("actionHeader").style.display = '';
                                for (var i = 0; i < editButtons.length; i++) {
                                    // Butonu gizle
                                    editButtons[i].style.display = '';
                                }
                                for (var i = 0; i < deleteButtons.length; i++) {
                                    // Butonu gizle
                                    deleteButtons[i].style.display = '';
                                }

                            }
                            else {
                            }
                            console.log("Kullanıcı Rolü " + response + "");
                        } else if (xhr.status === 300) {
                        }
                    };
                }
                xhr.send(JSON.stringify({ role: "" }));
            }

            function FilltoModal(id, name, count, color) {
                debugger;
                document.getElementById("txtProductId").value = id;
                document.getElementById("txtProductName").value = name;
                document.getElementById("txtProductCount").value = count;

                var select = document.getElementById("cbxColors");
                for (var i = 0; i < select.options.length; i++) {
                    if (select.options[i].innerText === color) { 
                        select.selectedIndex = i; 
                        break; 
                    }
                }
            }


            function VisibleButton(action) {
                if (action == "add") {
                    document.getElementById("btnEdit").style.display = "none";
                    document.getElementById("btnAdd").style.display = "";
                }
                if (action == "edit") {
                    debugger;
                    document.getElementById("btnAdd").style.display = "none";
                    document.getElementById("btnEdit").style.display = "";
                }
            }

            function ClearModal() {
                debugger;
                document.getElementById("txtProductId").value = "";
                document.getElementById("txtProductName").value = "";
                document.getElementById("txtProductCount").value = "";
                document.getElementById("cbxColors").value = "";
            }

            function UpdateProduct() {
                debugger;
                var pId = document.getElementById("txtProductId").value;
                var pName = document.getElementById("txtProductName").value;
                var pCount = document.getElementById("txtProductCount").value;
                var pColor = document.getElementById("cbxColors").value;
                var SelectedOptions = pId + "_" + pName + "_" + pCount + "_" + pColor;
                var currentPath = window.location.pathname;
                var xhr = new XMLHttpRequest();
                xhr.open("POST", currentPath, true);
                xhr.onreadystatechange = function () {
                    if (xhr.readyState === XMLHttpRequest.DONE) {
                        if (xhr.status === 200) {
                            GetProducts();
                            ClearModal();
                        } else if (xhr.status === 300) {
                        }
                    };
                }
                xhr.send(JSON.stringify({ edit: SelectedOptions }));
            }

            function SendRequest(id, action) {
                if (action = "delete") {
                    if (confirm('Emin misiniz?')) {
                        event.preventDefault();
                        var currentPath = window.location.pathname;
                        var values = action + "_" + id;
                        var xhr = new XMLHttpRequest();
                        xhr.open("POST", currentPath, true);
                        xhr.onreadystatechange = function () {
                            if (xhr.readyState === XMLHttpRequest.DONE) {
                                if (xhr.status === 200) {
                                    //var response = xhr.response.split("|")[0].replace("\"", "");
                                    console.log("Http Requsest 200 Success");
                                    window.location.reload();
                                } else if (xhr.status === 300) {
                                }
                                else if (xhr.status === 500) {
                                }
                            }
                        }
                    }
                }
                xhr.send(JSON.stringify({ action: values }));
            }

            function GetProducts() {
                // Loader'ı göster
                document.getElementById("loader").style.display = "block";
                document.getElementById("content").style.display = "none";
                var currentPath = window.location.pathname;
                var related = "";
                var xhr = new XMLHttpRequest();
                xhr.open("POST", currentPath, true);
                xhr.onreadystatechange = function () {
                    if (xhr.readyState === XMLHttpRequest.DONE) {
                        if (xhr.status === 200) {
                            //debugger;
                            var response = xhr.response.split('|')[0];
                            document.getElementById("tbodyFD").innerHTML = response;
                            GetColors();
                            document.getElementById("loader").style.display = "none";
                            document.getElementById("content").style.display = "block";

                        } else if (xhr.status === 300) {

                        }
                    };
                }
                xhr.send(JSON.stringify({ products: related }));
            }

            function GetColors() {
                var currentPath = window.location.pathname;
                var related = "";
                var xhr = new XMLHttpRequest();
                xhr.open("POST", currentPath, true);
                xhr.onreadystatechange = function () {
                    if (xhr.readyState === XMLHttpRequest.DONE) {
                        if (xhr.status === 200) {
                            var response = xhr.response.split('|')[0];
                            document.getElementById("cbxColors").innerHTML = response;

                        } else if (xhr.status === 300) {

                        }
                    };
                }
                xhr.send(JSON.stringify({ colors: related }));

            }

            function AddProducts() {
                //debugger;
                //hata var
                var pName = document.getElementById("txtProductName").value;
                var pCount = document.getElementById("txtProductCount").value;
                var pColor = document.getElementById("cbxColors").value;
                var SelectedOptions = pName + "_" + pCount + "_" + pColor;
                var currentPath = window.location.pathname;
                var xhr = new XMLHttpRequest();
                xhr.open("POST", currentPath, true);
                xhr.onreadystatechange = function () {
                    if (xhr.readyState === XMLHttpRequest.DONE) {
                        if (xhr.status === 200) {
                            GetProducts();
                            alert("Ekleme Başarılı");
                            ClearModal();
                        } else if (xhr.status === 300) {
                        }
                    };
                }
                xhr.send(JSON.stringify({ add: SelectedOptions }));
            }

            window.onload = GetUserRole();

            window.onload = GetProducts();

            window.onload = ClearModal();

        </script>
    </main>
</asp:Content>
