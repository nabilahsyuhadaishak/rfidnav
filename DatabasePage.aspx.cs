using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using MySql.Data.MySqlClient;
using Npgsql;
using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace rfidwithnavbar
{
    public partial class DatabasePage : System.Web.UI.Page
    {
        //PostgreSQL connection string
        //string connectionString = "Server=127.0.0.1; Port=5432; User Id=postgres; Password=root123; Database=my_rfid";
        //MySQL connection string
        string connectionString = @"Server=localhost;Database=test;Uid=root;Pwd=;";
        public static string id_;

        protected void Page_Load(object sender, EventArgs e)
        {
            msqlGridFill();
        }

        //GidFill() is the function used to dill datagridview with tag's data from MySQL database

        void msqlGridFill()
        {
            //using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM test.test_db";
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sql, connection);
                //NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sql,connection);
                DataTable dtbl = new DataTable();
                dataAdapter.Fill(dtbl);

                dbGridView.DataSource = dtbl;
                dbGridView.DataBind();
                TotalLabel.Text = dbGridView.Rows.Count.ToString();
            }
        }

        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        protected void excelLinkButton_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "Result" + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            dbGridView.GridLines = GridLines.Both;
            dbGridView.HeaderStyle.Font.Bold = true;
            dbGridView.RowStyle.Height = 30;
            dbGridView.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();
        }

        protected void pdfButton_Click(object sender, EventArgs e)
        {
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    dbGridView.RenderControl(hw);
                    StringReader sr = new StringReader(sw.ToString());
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    string FileName = "Result" + DateTime.Now + ".pdf";
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.End();
                }
            }
        }
    }
}