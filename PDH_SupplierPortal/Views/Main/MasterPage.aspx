<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MasterPage.aspx.cs" Inherits="SupplierPortal.Views.Main.MasterPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Supplier Portal</title>
    
    <link href="../../Scripts/ext4/resources/css/ext-all-neptune.css" rel="stylesheet" />
    <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/IconCls.css" rel="stylesheet" type="text/css" />

    <script>
        var sessionName = '<%=Session["User"].ToString()%>';
    </script> 

    <script type="text/javascript" charset="utf-8" src="../../Scripts/ext4/bootstrap.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../Scripts/ext4/PagingMemoryProxy.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../Scripts/ext4/ux/SlidingPager.js"></script>  
    
    <script type="text/javascript" charset="utf-8" src="../../Scripts/Main/MasterPage.aspx.js" ></script>
   
    
</head>
<body>
    <form id="form1" runat="server">

    <div id="grid-example">
    
    </div>
    </form>
</body>
</html>
