<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DatabasePage.aspx.cs" Inherits="rfidwithnavbar.DatabasePage" EnableEventValidation="false"%>

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
            margin-bottom: 0;
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
    <br />

   

    <form id="form1" runat="server">
        <p>
            
            <button type="button" class="btn btn-dark btn-xl">Number of tags: <span class="badge badge-danger big"><asp:Label ID="TotalLabel" runat="server"></asp:Label></span></button>
            <asp:LinkButton ID="excelLinkButton" CssClass="btn btn-info btn-xl" runat="server" Width="76px" OnClick="excelLinkButton_Click"><span class="glyphicon glyphicon-export"></span> Excel</asp:LinkButton>
            <asp:LinkButton ID="pdfButton" CssClass="btn btn-info btn-xl" runat="server" Width="76px" OnClick="pdfButton_Click"><span class="glyphicon glyphicon-export"></span> Pdf</asp:LinkButton>
        </p>
        <div runat="server">
                <div class="col-lg-12">
                    <div class="table-responsive">
                        <p style="text-align: center">
                            <asp:GridView ID="dbGridView" runat="server" ForeColor="#333333" GridLines="None" CssClass="table table-bordered table-hover table-striped" HorizontalAlign ="Center">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />        
                                <EditRowStyle BackColor="#999999" Wrap="true" HorizontalAlign="Center" VerticalAlign="Middle"/> 
                                <FooterStyle BackColor="#FF66CC" Font-Bold="True" ForeColor="White" />        
                                <HeaderStyle BackColor="#292E32" Font-Bold="True" ForeColor="White" Height="20px" HorizontalAlign="Center"/>        
                                <PagerSettings PageButtonCount="20" />
                                <PagerStyle BackColor="#2E2932" ForeColor="White" HorizontalAlign="Center" />        
                                <RowStyle BackColor="#F7F6F3" Wrap="true" HorizontalAlign="Center" VerticalAlign="Middle" ForeColor="#333333" />        
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />        
                                <SortedAscendingCellStyle BackColor="#E9E7E2" />        
                                <SortedAscendingHeaderStyle BackColor="#506C8C" />        
                                <SortedDescendingCellStyle BackColor="#FFFDF8" />        
                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />    
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
