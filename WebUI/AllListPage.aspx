<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllListPage.aspx.cs" Inherits="WeiXinApi.WebUI.AllListPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta charset="utf-8" name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
 <title>获取微信用户信息和地理位置</title>
 <script type="text/javascript" src="http://libs.useso.com/js/jquery/1.7.2/jquery.min.js"></script>
 <script>
     function getLocation() {
         if (navigator.geolocation) {
             navigator.geolocation.getCurrentPosition(showPosition, showError);
         } else {
             alert("浏览器不支持地理定位。");
         }
     }
     function showPosition(position) {
         $("#latlon").html("纬度:" + position.coords.latitude + '，经度:' + position.coords.longitude);
         var latlon = position.coords.latitude + ',' + position.coords.longitude;
         //google
         var url = 'http://maps.google.cn/maps/api/geocode/json?latlng=' + latlon + '&language=CN';
         $.ajax({
             type: "GET",
             url: url,
             beforeSend: function () {
                 $("#google_geo").html('正在定位...');
             },
             success: function (json) {
                 if (json.status == 'OK') {
                     var results = json.results;
                     $.each(results, function (index, array) {
                         if (index == 0) {
                             $("#google_geo").html(array['formatted_address']);
                         }
                     });
                 }
             },
             error: function (XMLHttpRequest, textStatus, errorThrown) {
                 $("#google_geo").html(latlon + "地址位置获取失败");
             }
         });
     }

     function showError(error) {
         switch (error.code) {
             case error.PERMISSION_DENIED:
                 alert("定位失败,用户拒绝请求地理定位");
                 break;
             case error.POSITION_UNAVAILABLE:
                 alert("定位失败,位置信息是不可用");
                 break;
             case error.TIMEOUT:
                 alert("定位失败,请求获取用户位置超时");
                 break;
             case error.UNKNOWN_ERROR:
                 alert("定位失败,定位系统失效");
                 break;
         }
     }

     getLocation();
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="contatin">
    <div class="header">
    </div>
    <div class="geo">
    <p>您当前所在位置为：</p>
    <p id="google_geo"></p>
    
    </div>
    </div>
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    </form>
</body>
</html>
