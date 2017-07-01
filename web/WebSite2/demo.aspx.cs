﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Data.OleDb;

public partial class demo : System.Web.UI.Page
{
    string connStr = "server=DESKTOP-RNBGQQS;uid=sa;pwd=123456;database=StudentsInfo";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            selectbind();
            this.xilist.Items.Insert(0, new ListItem("请选择系", "0"));
            this.banjilist.Items.Insert(0, new ListItem("请选择班级", "0"));
          
        }
    }

    /// <summary>
    /// excel批量上传
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void shangchuan_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            string fileName = FileUpload1.FileName;              //获得上传文件文件名
            int position = fileName.LastIndexOf("\\");           //截取.的位置
            string splitName = fileName.Substring(position + 1);       //截取后缀
            string newPath = filechick(splitName);                 //判断后缀名是否合法并赋予新的文件名
            if (!String.IsNullOrEmpty(newPath))                        //指示指定的字符串是 null 还是 Empty 字符串。
            {
                string savePath = Server.MapPath("Excel/");            //Server.MapPath方法返回与指定虚拟路径相对应的物理路径。
                FileOperatpr(fileName, savePath);                      // FileOperatpr类，用于文件操作
                FileUpload1.SaveAs(savePath + newPath);                //FileUpload1.SaveAs方法：将上载文件的内容保存到 Web 服务器上的指定路径。后面加上文件名
                DataOperator(fileName, savePath);                      // DataOperator类，用于数据操作

            }
        }
        else
        {
        }

    }

    /// <summary>
    /// 单条添加
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void add_ServerClick(object sender, EventArgs e)
    {
        string id = upid.Value;
        string nm = upname.Value;
        string pwd = uppwd.Value;
        string cls = upclass.Value;


        string sestr = "SELECT * FROM si WHERE ID='" + id + "' AND Name='" + nm + "' AND password='" + pwd + "' AND classid=(SELECT Class_ID FROM banji WHERE CLM='" + cls + "')";

        if (id == "" || nm == "")
        {
            Response.Write(@"<script>alert('增添的信息不能为空！');</script>");
        }
        else
        {
            SqlConnection conn = new SqlConnection(connStr);                         //连接的数据库名称
            SqlDataAdapter da = new SqlDataAdapter(sestr, conn);
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            conn.Open();                                                             //打开数据库
            da.Fill(dt);                                                            //将数据填充到DataTable（dt）         
            conn.Close();
            //string a = dt1.Rows[0]["classid"].ToString();
            //int cid = 0;
            //int.TryParse(a, out cid);                                             //强制类型转换，string转int
            if (dt.Rows.Count == 1)                                               //从登录表读取数据，如果已经有输入信息，页面提示
            {
                Response.Write(@"<script>alert('已有该条记录！');</script>");
            }
            else
            {

                string instr = "INSERT INTO si(ID,Name,classid,password) VALUES('" + id + "','" + nm + "','" + cls + "','" + pwd + "'); insert into Experience(ID) VALUES ('" + id + "'),('" + id + "'),('" + id + "'),('" + id + "'),('" + id + "'),('" + id + "');  insert into marriage(ID) VALUES ('" + id + "'),('" + id + "'),('" + id + "'),('" + id + "'),('" + id + "'),('" + id + "'); insert into Relation(ID) VALUES ('" + id + "'),('" + id + "'),('" + id + "'),('" + id + "'),('" + id + "'),('" + id + "');";
                SqlDataAdapter da2 = new SqlDataAdapter(instr, conn);
                DataSet ds = new DataSet();
                conn.Open();                                                             //打开数据库
                da2.Fill(ds);                                                            //将数据填充到DataTable（dt）
                conn.Close();
                Response.Write(@"<script>alert('上传成功！');</script>");
                //  bind();
            }
        }

    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chaxun_ServerClick(object sender, EventArgs e)
    {
        string xi = xilist.SelectedItem.Text;
        string cl = banjilist.SelectedItem.Text;
        string id = xuehao.Value;

        if (id != "")
        {
            // 查询id
            string sqlStr = "SELECT ID,Name,password banji.CLM FROM si,banji WHERE si.ID='" + id + "' AND banji.Class_ID=si.classid";
            searchBind(sqlStr);
        }
        else if (xi != "" && cl == "")
        {
            //查询系
            string sqlStr1 = "SELECT ID,Name,password banji.CLM FROM si,banji WHERE xi_ID=(SELECT Xi_id FROM Xi WHERE XM='" + xi + "')";
            searchBind(sqlStr1);
        }
        else if (xi != "" && cl != "")
        {
            //查询班级
            string sqlStr2 = "SELECT ID,Name,password banji.CLM FROM si,banji WHERE classid=(SELECT Class_ID FROM banji WHERE CLM='" + cl + "')";
            searchBind(sqlStr2);
        }
        else
        {
            //提示错误
            Response.Write(@"<script>alert('请正确选择或填写查询信息！');</script>");
        }
    }


    /// <summary>
    /// 重置查询
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void restart_ServerClick(object sender, EventArgs e)
    {
        this.xilist.Items.Insert(0, new ListItem("请选择系", "0"));
        this.banjilist.Items.Insert(0, new ListItem("请选择班级", "0"));
        xuehao.Value = "";
    }


    /// <summary>
    /// 全选gridveiw
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CheckBox2_CheckedChanged(object sender, EventArgs e)
    {

        for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
        {
            CheckBox cbox = (CheckBox)GridView1.Rows[i].FindControl("CheckBox1");
            if (CheckBox2.Checked == true)
            {
                cbox.Checked = true;
            }
            else
            {
                cbox.Checked = false;
            }
        }
    }

    /// <summary>
    /// 删除全选
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button2_Click(object sender, EventArgs e)
    {
        SqlConnection sqlcon = new SqlConnection(connStr);
        for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
        {
            CheckBox cbox = (CheckBox)GridView1.Rows[i].FindControl("CheckBox1");
            if (cbox.Checked == true)
            {

                string delstr = "DELETE FROM si WHERE ID='" + GridView1.DataKeys[i].Value.ToString() + "';DELECT FROM Experience WHERE ID='" + GridView1.DataKeys[i].Value.ToString() + "';DELECT FROM marriage WHERE ID='" + GridView1.DataKeys[i].Value.ToString() + "';DELECT FROM Relation WHERE ID='" + GridView1.DataKeys[i].Value.ToString() + "'"; 
                SqlDataAdapter sda = new SqlDataAdapter(delstr, sqlcon);
                DataSet ds = new DataSet();
                sqlcon.Open();
                sda.Fill(ds);
                sqlcon.Close();
            }

        }
        bind();

    }

    /// <summary>
    /// 取消选择
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        CheckBox2.Checked = false;
        for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
        {
            CheckBox cbox = (CheckBox)GridView1.Rows[i].FindControl("CheckBox1");
            cbox.Checked = false;
        }
    }


    /// <summary>
    /// excle下载
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Excel_Click(object sender, EventArgs e)
    {
        /*
        string id = "2016级计算机科学与技术（专升本）";
        if (System.IO.File.Exists(Server.MapPath("/Excel/" + id + ".xlsx")))
        {
            delExcel();
            copyExcel();
            string selstr = "SELECT * FROM si WHERE ID='" + GridView1.DataKeys[e.RowIndex].Value.ToString() + "''";
            SqlConnection conn = new SqlConnection(connStr);
            SqlDataAdapter sda = new SqlDataAdapter(selstr, conn);
            DataSet ds = new DataSet();
            conn.Open();
            sda.Fill(ds);
            conn.Close();
            ExcelDataSetOperator(ds);
            string fileName = "" + id + ".xlsx";//客户端保存的文件名
            string filePath = Server.MapPath("/Excel/" + id + ".xlsx");//路径
            FileInfo fileInfo = new FileInfo(filePath);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            Response.WriteFile(fileInfo.FullName);
            Response.Flush();
            delExcel();
            Response.End();
            

        }
        else
        {
            copyExcel();
            string selstr = "SELECT * FROM si WHERE ID='" + GridView1.DataKeys[e.RowIndex].Value.ToString() + "''";
            SqlConnection conn = new SqlConnection(connStr);
            SqlDataAdapter sda = new SqlDataAdapter(selstr, conn);
            DataSet ds = new DataSet();
            conn.Open();
            sda.Fill(ds);
            conn.Close();
            ExcelDataSetOperator(ds);
            string fileName = "" + id + ".xlsx";//客户端保存的文件名
            string filePath = Server.MapPath("/Excel/" + id + ".xlsx");//路径
            FileInfo fileInfo = new FileInfo(filePath);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            Response.WriteFile(fileInfo.FullName);
            Response.Flush();
            delExcel();
            Response.End();
        }
        */


    }

    /// <summary>  
    /// 数据操作  
    /// </summary>  
    /// <param name="fileName"></param>  
    /// <param name="savePath"></param>  
    private void DataOperator(string fileName, string savePath)
    {
        /*
        Microsoft.ACE.OLEDB.12.0 基本操作
        Provider = Microsoft.ACE.OLEDB.12.0 链接驱动 需在服务器端下载  Microsoft.ACE.OLEDB.12.0，向下支持Microsoft Jet Database Engine 4.0
        Data Source=" + savePath + "; 数据库地址
        Extended Properties='Excel 12.0; 连接的是Exce 12.0
        HDR=YES;    有两个值:YES/ NO, 这2个值，说了你是否能直接读列名，NO，只可以读下标
        IMEX=1;     解决数字与字符混合时,识别不正常的情况. 
        */
        string myString = " Provider = Microsoft.ACE.OLEDB.12.0 ;Data Source =  " + savePath + fileName + ";Extended Properties='Excel 12.0;HDR = YES;'";
        OleDbConnection oconn = new OleDbConnection(myString);              //DbConnection是所有数据库连接类的基类 使用DbConnection连接数据库
        DataSet ds = new DataSet();                                         //创建DataSet缓存
        OleDbDataAdapter oda = new OleDbDataAdapter("select * from [Sheet1$]", oconn);   //将excel内容填充到dataSet
        oconn.Open();                                                       //打开数据库
        oda.Fill(ds);                                                       //将oda填充到dataset（ds）内
        DataSetOperator(ds, savePath + fileName);                           // DataSetOperator类，进行数据集操作
        oconn.Close();

    }

    /// <summary>  
    /// 数据集操作  
    /// </summary>  
    /// <param name="ds"></param>  
    private void DataSetOperator(DataSet ds, string filePath)
    {
        SqlConnection conn = new SqlConnection(connStr);    //创建数据库连接
        conn.Open();
        SqlTransaction str = conn.BeginTransaction();                                 //利用事务处理 防止中断  
        int k = 0;
        if (ds.Tables[0].Rows.Count < 1)                                              //判断是表内是否为空
        {
            Response.Write("<script>alert('没有数据！')</script>");
            return;
        }
        try          //检测异常语句
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                string sqlStr = "insert into si(ID,Name,classid,password) values('" + ds.Tables[0].Rows[i][0].ToString() + "','" + ds.Tables[0].Rows[i][1].ToString() + "','" + ds.Tables[0].Rows[i][2].ToString() + "','" + ds.Tables[0].Rows[i][3].ToString() + "'); insert into Experience(ID) VALUES ('" + ds.Tables[0].Rows[i][0].ToString() + "'),('" + ds.Tables[0].Rows[i][0].ToString() + "'),('" + ds.Tables[0].Rows[i][0].ToString() + "'),('" + ds.Tables[0].Rows[i][0].ToString() + "'),('" + ds.Tables[0].Rows[i][0].ToString() + "'),('" + ds.Tables[0].Rows[i][0].ToString() + "');  insert into marriage(ID) VALUES ('" + ds.Tables[0].Rows[i][0].ToString() + "'),('" + ds.Tables[0].Rows[i][0].ToString() + "'),('" + ds.Tables[0].Rows[i][0].ToString() + "'),('" + ds.Tables[0].Rows[i][0].ToString() + "'),('" + ds.Tables[0].Rows[i][0].ToString() + "'),('" + ds.Tables[0].Rows[i][0].ToString() + "'); insert into Relation(ID) VALUES ('" + ds.Tables[0].Rows[i][0].ToString() + "'),('" + ds.Tables[0].Rows[i][0].ToString() + "'),('" + ds.Tables[0].Rows[i][0].ToString() + "'),('" + ds.Tables[0].Rows[i][0].ToString() + "'),('" + ds.Tables[0].Rows[i][0].ToString() + "'),('" + ds.Tables[0].Rows[i][0].ToString() + "');";
                SqlCommand cmd = new SqlCommand(sqlStr, conn, str);          //表示要对 SQL Server 数据库执行的一个 Transact-SQL 语句或存储过程
                cmd.Transaction = str;                                        //执行SqlCommand的SqlTransaction
                k += cmd.ExecuteNonQuery();                                  //返回受影响的行数
            }
            str.Commit();                                                   //提交数据库事务
        }
        catch (Exception ex)                                                //获取一场信息
        {
            Response.Write(@"<script>alert('发生异常,数据已回滚/n信息/n" + ex.Message + "');</ script > ");
            str.Rollback();
        }
        finally                                                             //finally 块释放资源，例如，关闭在 try 块中打开的任何流或文件。
        {
            Response.Write(@"<script>alert('上传成功" + k + " 条');</script>");
            //File.Delete(filePath);                                          //上传完成删除文件
            // bind();

        }
    }
    /// <summary>  
    /// 文件操作  
    /// </summary>  
    /// <param name="fileName"></param>  
    /// <param name="savePath"></param>  
    private void FileOperatpr(string fileName, string savePath)
    {
        if (!Directory.Exists(savePath))       //Directory.Exists方法，判断是否有该文件       
        {
            Directory.CreateDirectory(savePath);       // Directory.CreateDirectory方法，在路径创建所有目录
        }
        /*
             if (File.Exists(savePath + fileName))            //确定指定的文件是否存在。
             {
                 File.Delete(savePath + fileName);            //File.Delete 方法，删除指定文件
             }
             */

    }

    /// <summary>  
    /// 文件检查
    /// </summary>  
    /// <param name="fileName"></param> 
    protected string filechick(string fileName)
    {
        //返回指定路径文件的扩展名 并转换成小写
        string fileType = Path.GetExtension(fileName).ToLower();
        //判断文件的后缀名
        if (fileType == ".xlsx")
        {
            //返回一个不具有文件扩展名的文件名
            string Name = Path.GetFileNameWithoutExtension(fileName);
            string NewName = Name + fileType;
            return NewName;
        }
        else
        {
            return null;
        }

    }


    /// <summary>
    /// 系选择DataSource
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void xilist_SelectedIndexChanged(object sender, EventArgs e)
    {

        string xl = xilist.SelectedItem.Text;
        selectbind2(xl);
        this.banjilist.Items.Clear();//清除第三分类 
        this.banjilist.Items.Insert(0, new ListItem("请选择班级", "0"));

    }
 

    /// <summary>
    /// 系dropdownlist 的DataSource 
    /// </summary>
    public void selectbind()
    {
        string ID = Request.QueryString["id"];
        string sqlstr = "SELECT XM FROM Xi where College_ID=(SELECT College_ID FROM Teacher WHERE id='"+ID+"')";
        SqlConnection sqlcon = new SqlConnection(connStr);
        SqlDataAdapter da = new SqlDataAdapter(sqlstr, sqlcon);
        DataSet ds = new DataSet();
        sqlcon.Open();
        da.Fill(ds);
        sqlcon.Close();
        xilist.DataSource = ds;
        xilist.DataTextField = "XM";
        xilist.DataBind();
    }

    /// <summary>
    /// 班级dropdownlist 的DataSource
    /// </summary>
    /// <param name="x"></param>
    public void selectbind2(string x)
    {
        string sqlstr = "SELECT CLM FROM banji WHERE xi_id=(SELECT Xi_id FROM Xi WHERE XM='" + x + "' )";

        SqlConnection sqlcon = new SqlConnection(connStr);
        SqlDataAdapter da = new SqlDataAdapter(sqlstr, sqlcon);
        DataSet ds = new DataSet();
        sqlcon.Open();
        da.Fill(ds);
        sqlcon.Close();
        banjilist.DataSource = ds;
        banjilist.DataTextField = "CLM";
        banjilist.DataBind();
    }


    /// <summary>
    /// 查询数据，重新进行gridview绑定
    /// </summary>
    /// <param name="sqlstr"></param>
    public void searchBind(string sqlstr)
    {
        SqlConnection sqlcon = new SqlConnection(connStr);
        SqlDataAdapter da = new SqlDataAdapter(sqlstr, sqlcon);
        DataSet ds = new DataSet();
        sqlcon.Open();
        da.Fill(ds);
        GridView1.DataSource = ds;                                     //设定gridview的datasourse
        GridView1.DataKeyNames = new string[] { "ID" };//主键
        GridView1.DataBind();
        sqlcon.Close();
    }


    /// <summary>
    /// gridview数据绑定
    /// </summary>
    public void bind()
    {


        string sqlstr = "select ID,Name,password CLM from si,banji WHERE banji.Class_ID=si.classid";            //查询数据
        SqlConnection sqlcon = new SqlConnection(connStr);
        SqlDataAdapter da = new SqlDataAdapter(sqlstr, sqlcon);
        DataSet ds = new DataSet();
        sqlcon.Open();
        da.Fill(ds);
        GridView1.DataSource = ds;                                     //设定gridview的datasourse
        GridView1.DataKeyNames = new string[] { "ID" };//主键
        GridView1.DataBind();
        sqlcon.Close();


    }

    /// <summary>
    /// gridview修改
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        bind();
    }

    /// <summary>
    /// gridview更新
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        // 将GridView1.Rows[e.RowIndex].Cells[1].Controls[0]强制转换类型成textbox，
        string nm = ((TextBox)(GridView1.Rows[e.RowIndex].Cells[2].Controls[0])).Text;
        string pwd = ((TextBox)(GridView1.Rows[e.RowIndex].Cells[3].Controls[0])).Text;
        string upstr = "UPDATE si SET Name='" + nm + "', password='" + pwd + "' WHERE ID='" + GridView1.DataKeys[e.RowIndex].Value.ToString() + "'";

        SqlConnection sqlcon = new SqlConnection(connStr);
        SqlDataAdapter sda = new SqlDataAdapter(upstr, sqlcon);
        DataSet ds = new DataSet();
        sqlcon.Open();
        sda.Fill(ds);                   //填充dataset
        sqlcon.Close();
        GridView1.EditIndex = -1;      // EditIndex属性 要编辑的行从0开始 预设值为-1
        bind();


    }

    /// <summary>
    /// gridview 更新内取消功能
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;          //要编辑的行从0开始
        bind();
    }

    /// <summary>
    /// gridview删除行功能
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string delstr = "DELETE FROM si WHERE ID='" + GridView1.DataKeys[e.RowIndex].Value.ToString() + "'; DELECT FROM Experience WHERE ID='" + GridView1.DataKeys[e.RowIndex].Value.ToString() + "';DELECT FROM marriage WHERE ID='" + GridView1.DataKeys[e.RowIndex].Value.ToString() + "';DELECT FROM Relation WHERE ID='" + GridView1.DataKeys[e.RowIndex].Value.ToString() + "'" ;
        SqlConnection sqlcon = new SqlConnection(connStr);
        SqlDataAdapter sda = new SqlDataAdapter(delstr, sqlcon);
        DataSet ds = new DataSet();
        sqlcon.Open();
        sda.Fill(ds);               //填充dataset
        sqlcon.Close();
        bind();

    }

    /// <summary>
    /// gridview分页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Gridview1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        bind();
    }




    public void copyExcel()
    {
        string id = "2016级计算机科学与技术（专升本）";
        string ExcelName = "" + id + ".xlsx";
        string Model = "C:/web/WebSite2/Excel/Model/1.xlsx";
        string excel = "C:/web/WebSite2/Excel/" + ExcelName + "";

        System.IO.File.Copy(Model, excel);

    }

    public void delExcel()
    {
        string id = "2016级计算机科学与技术（专升本）";
        string ExcelName = "" + id + ".xlsx";
        System.IO.File.Delete(@"C:/web/WebSite2/Excel/" + ExcelName + "");
    }

    /// <summary>
    /// excel数据操作
    /// </summary>
    /// <param name="ds"></param>
    private void ExcelDataSetOperator(DataSet ds)
    {
        string id = "2016级计算机科学与技术（专升本）";

        int a = ds.Tables[0].Rows.Count;
        String myString = "Provider = Microsoft.ACE.OLEDB.12.0 ;Data Source = C:/web/WebSite2/Excel/" + id + ".xlsx; Extended Properties='Excel 12.0;HDR = YES;'";

        OleDbConnection conn = new OleDbConnection(myString);
        OleDbCommand cmd = conn.CreateCommand();
        conn.Open();
        for (int i = 0; i < a; i++)
        {
            cmd.CommandText = "INSERT INTO [Sheet1$] (学号,姓名,密码,曾用名,班级,年级,生日,性别,民族,健康状况,婚姻状况,手机号码,银行卡号,开户行,电子邮箱,身份证号码,生源地,家庭住址,家庭电话,邮政编码,什么时候受过什么奖励,什么时候入的党团,受过什么处分,家庭主要成员和主要关系中有无重大问题，与本人关系如何,其他需要向组织说明的问题,记事) VALUES('" + ds.Tables[0].Rows[i][0].ToString() + "', '" + ds.Tables[0].Rows[i][1].ToString() + "', '" + ds.Tables[0].Rows[i][2].ToString() + "', '" + ds.Tables[0].Rows[i][3].ToString() + "','" + ds.Tables[0].Rows[i][6].ToString() + "', '" + ds.Tables[0].Rows[i][7].ToString() + "', '" + ds.Tables[0].Rows[i][8].ToString() + "', '" + ds.Tables[0].Rows[i][9].ToString() + "', '" + ds.Tables[0].Rows[i][10].ToString() + "', '" + ds.Tables[0].Rows[i][11].ToString() + "', '" + ds.Tables[0].Rows[i][12].ToString() + "', '" + ds.Tables[0].Rows[i][13].ToString() + "', '" + ds.Tables[0].Rows[i][14].ToString() + "', '" + ds.Tables[0].Rows[i][15].ToString() + "', '" + ds.Tables[0].Rows[i][16].ToString() + "', '" + ds.Tables[0].Rows[i][17].ToString() + "', '" + ds.Tables[0].Rows[i][18].ToString() + "', '" + ds.Tables[0].Rows[i][19].ToString() + "', '" + ds.Tables[0].Rows[i][20].ToString() + "', '" + ds.Tables[0].Rows[i][21].ToString() + "', '" + ds.Tables[0].Rows[i][22].ToString() + "', '" + ds.Tables[0].Rows[i][23].ToString() + "', '" + ds.Tables[0].Rows[i][24].ToString() + "', '" + ds.Tables[0].Rows[i][25].ToString() + "', '" + ds.Tables[0].Rows[i][26].ToString() + "', '" + ds.Tables[0].Rows[i][27].ToString() + "')";

            cmd.ExecuteNonQuery();
            //cmd.CommandText = "INSERT INTO stuInfo (学号,姓名,学院,系,班级,曾用名,生日,民族,政治面貌,健康状况,婚姻状况,电话号码,手机号码,电子邮箱,身份证号,生源地,家庭住址,家庭电话,邮政编码,备注,什么时候受过什么奖励,什么时候入的党团,受过什么处分,家庭主要成员和主要关系中有无重大问题与本人关系如何,其他需要向组织说明的问题, 记事) VALUES('" + ds.Tables[0].Rows[i][0].ToString() + "', '" + ds.Tables[0].Rows[i][1].ToString() + "', '" + ds.Tables[0].Rows[i][2].ToString() + "', '" + ds.Tables[0].Rows[i][3].ToString() + "', '" + ds.Tables[0].Rows[i][4].ToString() + "', '" + ds.Tables[0].Rows[i][5].ToString() + "', '" + ds.Tables[0].Rows[i][6].ToString() + "', '" + ds.Tables[0].Rows[i][7].ToString() + "', '" + ds.Tables[0].Rows[i][8].ToString() + "', '" + ds.Tables[0].Rows[i][9].ToString() + "', '" + ds.Tables[0].Rows[i][10].ToString() + "', '" + ds.Tables[0].Rows[i][11].ToString() + "', '" + ds.Tables[0].Rows[i][12].ToString() + "', '" + ds.Tables[0].Rows[i][13].ToString() + "', '" + ds.Tables[0].Rows[i][14].ToString() + "', '" + ds.Tables[0].Rows[i][15].ToString() + "', '" + ds.Tables[0].Rows[i][16].ToString() + "', '" + ds.Tables[0].Rows[i][17].ToString() + "', '" + ds.Tables[0].Rows[i][18].ToString() + "', '" + ds.Tables[0].Rows[i][19].ToString() + "', '" + ds.Tables[0].Rows[i][20].ToString() + "', '" + ds.Tables[0].Rows[i][21].ToString() + "', '" + ds.Tables[0].Rows[i][22].ToString() + "', '" + ds.Tables[0].Rows[i][23].ToString() + "', '" + ds.Tables[0].Rows[i][24].ToString() + "')";
        }
        conn.Close();
    }

    /// <summary>
    /// excel数据集操作
    /// </summary>
   public void ExcelDataOperator(object sender, GridViewDeleteEventArgs e)
    {
        string cm = "计算机科学与技术（专升本）";
        
    
        string selstr = "SELECT * FROM si WHERE ID='" + GridView1.DataKeys[e.RowIndex].Value.ToString() + "''";
        SqlConnection conn = new SqlConnection(connStr);
        SqlDataAdapter sda = new SqlDataAdapter(selstr, conn);
        DataSet ds = new DataSet();
        conn.Open();
        sda.Fill(ds);
        conn.Close();
        ExcelDataSetOperator(ds);

    }






}