<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="rfidwithnavbar.Home" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>RFID TAG DETECTION</title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" type="text/css"/>
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap-glyphicons.css" rel="stylesheet"/>
    <style type="text/css">
        .auto-style2 {
            font-size: xx-large;
        }
        #form1 {
            text-align: center;
        }
        .auto-style3 {
            font-size: large;
        }

        .navbar {
            margin-bottom:0;
            z-index: 9999;
            border: 0;
            letter-spacing: 4px;
            border-radius: 0;
        }

        .logo {
            color: #0094ff;
            font-size: 20px;
        }

        .big {
            font-size:15px;
        }

        .btn-xl, .btn-group-xl > .btn {
            padding: 0.75rem 0.75rem;
            font-size: 1.3rem;
            line-height: 1.5;
            border-radius: 0.4rem;
            width: auto;
}

        .error {
            color:#8d0909;
        }

        .success {
            color:#0094ff;
        }

    </style>
</head>
<body>
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark sticky-top">
        <a href="#" class="navbar-brand">RFID TAG DETECTION</a>
        <button class="navbar-toggler" data-toggle="collapse" data-target="navbarTogglerDemo01" aria-controls="navbarTogglerDemo01" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarTogglerDemo01">
            <ul class="navbar-nav ml-auto">
                <li class="nav-item">
                    <a href="Home.aspx" class="nav-link">Home</a>
                </li>
                &nbsp;&nbsp;&nbsp;
                <li class="nav-item">
                    <a href="DatabasePage.aspx" class="nav-link">Database</a>
                </li>
            </ul> 
        </div>
    </nav>

    <form id="form1" runat="server">
        
        <br />
        <br />
        
        <asp:TextBox ID="readTextBox" runat="server" style="margin-left: 0px; text-align: center; border-radius:2px; border-width:medium" Height="37px" BorderStyle="Groove" OnTextChanged="readTextBox_TextChanged" CssClass="table-responsive"></asp:TextBox>

        <br />
        
        <asp:Label class="success" ID="lblSuccessMessage" runat="server"></asp:Label>
        <asp:Label class="error" ID="lblErrorMessage" runat="server"></asp:Label>
        
        <br />
        
        <p style="text-align: center">
            <span class="glyphicon glyphicon-sort logo"></span>
            <strong><span class="auto-style3">Sort:&nbsp; </span> </strong><span class="auto-style3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:RadioButton ID="newestRadioButton" runat="server" GroupName="sortgroup" Text="Newest"  AutoPostBack="true" Checked="True"/>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:RadioButton ID="oldestRadioButton" runat="server" GroupName="sortGroup" Text="Oldest" AutoPostBack="true" />  
            </span>
        </p>
        <p style="text-align: center">
            <span class="auto-style3">&nbsp;&nbsp;&nbsp;  
        </span>  
        </p>
        <p>
            <button type="button" class="btn btn-dark btn-xl">Number of tags: <span class="badge badge-danger big"><asp:Label ID="TotalLabel" runat="server"></asp:Label></span></button>
            <asp:LinkButton ID="excelLinkButton" CssClass="btn btn-info btn-xl" runat="server" Width="76px" OnClick="excelLinkButton_Click"><span class="glyphicon glyphicon-export"></span> Excel</asp:LinkButton>
            <asp:LinkButton ID="pdfButton" CssClass="btn btn-info btn-xl" runat="server" Width="76px" OnClick="pdfButton_Click"><span class="glyphicon glyphicon-export"></span> Pdf</asp:LinkButton>
             <asp:LinkButton ID="delLinkButton" CssClass="btn btn-danger btn-xl" runat="server" Width="76px" OnClick="delLinkButton_Click"><span class="glyphicon glyphicon-trash"></span> Delete</asp:LinkButton>
        </p>
        <div runat="server">
                <div class="col-lg-12">
                    <div class="table-responsive">
                        <p style="text-align: center">
                            <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" CssClass="table table-bordered table-hover table-striped" OnRowCommand="GridView1_RowCommand" Width="1068px" HorizontalAlign ="Center" Font-Size="Large" Height="179px" EmptyDataText="There are no data recods to display">
                                <AlternatingRowStyle BackColor="White" />        
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" ControlStyle-CssClass="button" ControlStyle-Height="20px" ControlStyle-Width="20px" ImageUrl="~/copy.png" Text="Copy">
                                        <ControlStyle CssClass="button" Height="20px" Width="20px"></ControlStyle>
                                        <ItemStyle Height="10px" Width="10px" HorizontalAlign="Center" VerticalAlign="Middle" ></ItemStyle>
                                   </asp:ButtonField>
                                </Columns>
                                <EditRowStyle BackColor="#2461BF" Wrap="true" HorizontalAlign="Center" VerticalAlign="Middle"/> 
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />        
                                <HeaderStyle BackColor="#343434" Font-Bold="True" ForeColor="White" Height="20px" HorizontalAlign="Center"/>        
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />        
                                <RowStyle BackColor="#EFF3FB" Wrap="true" HorizontalAlign="Center" VerticalAlign="Middle" />        
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />        
                                <SortedAscendingCellStyle BackColor="#F5F7FB" />        
                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />        
                                <SortedDescendingCellStyle BackColor="#E9EBEF" />        
                                <SortedDescendingHeaderStyle BackColor="#4870BE" />    
                            </asp:GridView>
                        </p>
                    </div>
                </div>
            </div>
    </form>
    <script src="Scripts/jquery-3.0.0.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/popper.min.js"></script>

</body>
</html>
