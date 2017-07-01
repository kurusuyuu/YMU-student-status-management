﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="demo.aspx.cs" Inherits="demo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        #add 
        {
            width: 182px;
        }
        #Submit1 
        {
            width: 152px;
        }
        #chaxun 
        {
            width:152px;
        }
        #restart {
            width: 147px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <label class="label">Excel上传</label>
     
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <input id="up" type="submit" runat="server" onserverclick="shangchuan_Click">
       
    </div>
        <div>
            <p>
           <label class="label">信息单条上传</label>
            <p>   
           <label class="label">学号</label> <input id="upid" type="text" runat="server"/>
            <p>
           <label  class="label">姓名</label> <input id="upname" type="text" runat="server"/>
             <p>
           <label  class="label">班级</label> <input id="upclass" type="text" runat="server"/>
             <p>
           <label  class="label">密码</label> <input id="uppwd" type="text" runat="server"/>
             <p>
           <input id="add" type="submit" value="添加" runat="server" onserverClick="add_ServerClick"/>
       </div>
       <div>
         <p>
             <asp:ScriptManager ID="ScriptManager1" runat="server">
             </asp:ScriptManager>
             <label  class="label">查询学生信息</label>
              <asp:UpdatePanel ID="UpdatePanel1" runat="server">
              <ContentTemplate>
        <p>
            <label  class="label">选择系</label>
            <asp:DropDownList ID="xilist" runat="server" AutoPostBack="True" Width="138px" OnSelectedIndexChanged="xilist_SelectedIndexChanged"></asp:DropDownList>
            &nbsp; &nbsp; &nbsp; 
            <label  class="label">选择班级</label>
            <asp:DropDownList ID="banjilist" runat="server" Width="138px" AutoPostBack="True" ></asp:DropDownList>
             </ContentTemplate>
                </asp:UpdatePanel>
            &nbsp; &nbsp; &nbsp; 
            <label  class="label">输入学号</label> 
             <input id="xuehao" type="text" runat="server"/>
            <input id="chaxun" type="submit" value="查询" runat="server" onserverclick="chaxun_ServerClick"/>
            &nbsp; &nbsp; &nbsp; 
            <input id="restart" type="submit" value="重置" runat="server" onserverclick="restart_ServerClick"/>
           
        <p>
            <asp:GridView ID="GridView1" runat="server" Height="133px" Width="539px" AllowPaging="true" PageSize="10"
          AutoGenerateColumns="False" DataKeyNames="id"  
          OnPageindexChanging="Gridview1_PageIndexChanging" 
          OnRowEditing="GridView1_RowEditing" BackColor="White" BorderColor="White"   
          BorderStyle="Ridge" BorderWidth="2px" CellPadding="3"  CellSpacing="1"   
          GridLines="None"
          OnRowUpdating="GridView1_RowUpdating"   
          OnRowCancelingEdit="GridView1_RowCancelingEdit"   
          OnRowDeleting="GridView1_RowDeleting"
         >   
      <Columns>    
             <asp:TemplateField>
                <HeaderTemplate>
                    选择
                </HeaderTemplate>
                <ItemTemplate>
                     <asp:CheckBox ID="CheckBox1" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ID" HeaderText="学号" ReadOnly="True"/>    
            <asp:BoundField DataField="Name" HeaderText="姓名" DataFormatString="{0:000#}" /> 
            <asp:BoundField DataField="password" HeaderText="密码" DataFormatString="{0:000#}" /> 
            <asp:BoundField DataField="password" HeaderText="班级" DataFormatString="{0:000#}" ReadOnly="True"  />  
            <asp:CommandField HeaderText="查看信息" ShowEditButton="True" />            
            <asp:CommandField HeaderText="编辑" ShowEditButton="True" />    
            <asp:CommandField HeaderText="删除" ShowDeleteButton="True" />    
       </Columns>    
       <RowStyle BackColor="#DEDFDE" ForeColor="Black" />    
       <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />    
       <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />    
       <SelectedRowStyle BackColor="#9471DE" Font-Bold="True" ForeColor="White" />    
       <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#E7E7FF" />  
             
       <PagerTemplate>
        <br />
         <asp:Label ID="lblPage" runat="server" Text='<%# "第" + (((GridView)Container.NamingContainer).PageIndex + 1)  + "页/共" + (((GridView)Container.NamingContainer).PageCount) + "页" %> '></asp:Label>
         <asp:LinkButton ID="lbnFirst" runat="Server" Text="首页"  Enabled='<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>' CommandName="Page" CommandArgument="First" ></asp:LinkButton>
        <asp:LinkButton ID="lbnPrev" runat="server" Text="上一页" Enabled='<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>' CommandName="Page" CommandArgument="Prev" ></asp:LinkButton>
        <asp:LinkButton ID="lbnNext" runat="Server" Text="下一页" Enabled='<%# ((GridView)Container.NamingContainer).PageIndex != (((GridView)Container.NamingContainer).PageCount - 1) %>' CommandName="Page" CommandArgument="Next" ></asp:LinkButton>
         <asp:LinkButton ID="lbnLast" runat="Server" Text="尾页"   Enabled='<%# ((GridView)Container.NamingContainer).PageIndex != (((GridView)Container.NamingContainer).PageCount - 1) %>' CommandName="Page" CommandArgument="Last" ></asp:LinkButton>
         <br />
     </PagerTemplate>  
   </asp:GridView>  
       <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="True" Font-Sizep="9pt" OnCheckedChanged="CheckBox2_CheckedChanged" Text="全选" />
         &nbsp; &nbsp; 
       <asp:Button ID="Button1" runat="server" Font-Size="12pt" Text="取消" OnClick="Button1_Click" />
         &nbsp; &nbsp;
       <asp:Button ID="Button2" runat="server" Font-Size="12pt" Text="删除" OnClick="Button2_Click" />
          &nbsp; &nbsp;
       <asp:Button ID="excel" runat="server" Text="导出excel" OnClick="Excel_Click"  />
        </div>

    
    </form>
</body>
</html>
