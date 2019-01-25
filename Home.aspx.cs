using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using MySql.Data.MySqlClient;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace rfidwithnavbar
{
    public partial class Home : System.Web.UI.Page
    {
        //string connectionString = "Server=127.0.0.1; Port=5432; User Id=postgres; Password=root123; Database=my_rfid;";
        string connectionString = @"Server=localhost;Database=test;Uid=root;Pwd=;";
        public static DataTable dt;
        public static String timeStamp;
        public static String id;
        public static String tid;
        public static String epc;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                readTextBox.Focus();
                lblErrorMessage.Text = "";
                lblSuccessMessage.Text = "";
            }
        }

        protected void readTextBox_TextChanged(object sender, EventArgs e)
        {
            // -------------- ADD COLUMNS ---------------------
            if (readTextBox.Text != string.Empty)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Tag ID");
                dt.Columns.Add("Detected Time");
                dt.Columns.Add("EPC");

                // -------------- ADD ROWS -----------------
                for (int row = 0; row < GridView1.Rows.Count; row++)
                {
                    DataRow dr = dt.NewRow();
                    dr["Tag ID"] = GridView1.Rows[row].Cells[1].Text;
                    dr["Detected Time"] = GridView1.Rows[row].Cells[2].Text;
                    dr["EPC"] = GridView1.Rows[row].Cells[3].Text;
                    dt.Rows.Add(dr);
                }

                timeStamp = DateTime.Now.ToString("yyyy-MM-ddTHH\\:mm\\:ss.ffffffzzz"); //Retrieving timestamp (ISO8601)
                string[] lines = readTextBox.Text.Split('*');
                for (int i = 0; i < (lines.Length - 1); i++)
                {
                    string[] data = lines[i].Split(','); //eg:     33. 18-12-04 15:45 37.39 3000,30395dfa83296b00000a17d6,0000,e28068902000000050b1db01*
                    epc = data[1];
                    if (data[3].Length < 25)
                    {
                        tid = data[3];
                    }
                    else
                    {
                        tid = data[3].Substring(0, 24);
                    }
                    id = timeStamp + tid.ToUpper();
                    dt.Rows.Add(tid.ToUpper(), timeStamp, data[1].ToUpper());
                    
                }

                //Removing duplicate TagID
                Hashtable hTable = new Hashtable();
                ArrayList duplicateList = new ArrayList();

                //Add list of all the unique item value to hashtable, which stores combination of key, value pair.
                //And add duplicate item value in arraylist

                foreach (DataRow drow in dt.Rows)
                {
                    if (hTable.Contains(drow["Tag ID"]))
                        duplicateList.Add(drow);
                    else
                        hTable.Add(drow["Tag ID"], string.Empty);
                }

                //Removing a list of duplicate items from datatable 
                foreach (DataRow dRow in duplicateList)
                {
                    MessageBox.Show("Duplicate Tag ID of : " + dRow["Tag ID"]);
                    dt.Rows.Remove(dRow);
                }

                //Sort by the newest detected time
                if (newestRadioButton.Checked)
                {
                    dt.DefaultView.Sort = "Detected Time DESC";
                    dt = dt.DefaultView.ToTable();
                }
                //Sort by the oldest detected time
                if (oldestRadioButton.Checked)
                {
                    dt.DefaultView.Sort = "Detected Time ASC";
                    dt = dt.DefaultView.ToTable();
                }

                //------------INSERT DATA TO DB---------------//
                //-----------MySQL or POSTGRESQL--------------//

                //postgresqlInsert();
                mysqlinsert();
                
                //-------------------------------------------//

                GridView1.DataSource = dt;
                GridView1.DataBind();
                TotalLabel.Text = GridView1.Rows.Count.ToString();
                readTextBox.Text = string.Empty;
            }
            
        }

        //------------COPY------------

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (index != 0)
            {
                GridViewRow row = GridView1.Rows[index];
                tid = row.Cells[1].Text.ToString();
                Thread newThread = new Thread(new ThreadStart(ThreadMethod)); //Single Thread Apartment(STA) mode
                newThread.SetApartmentState(ApartmentState.STA);
                newThread.Start();
            }
            else
            {
                GridViewRow row = GridView1.Rows[index];
                tid = row.Cells[1].Text.ToString();
                Thread newThread = new Thread(new ThreadStart(ThreadMethod)); //Single Thread Apartment(STA) mode
                newThread.SetApartmentState(ApartmentState.STA);
                newThread.Start();
            }
        }
        protected void ThreadMethod()
        {
            Clipboard.SetText(tid); //Copied TID of the tag to clipboard eg: E28068902000000050B1DB01#@EVT0001

        }

        protected void delLinkButton_Click(object sender, EventArgs e)
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
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
            GridView1.GridLines = GridLines.Both;
            GridView1.HeaderStyle.Font.Bold = true;
            GridView1.RowStyle.Height = 30;
            GridView1.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();
        }

        protected void pdfButton_Click(object sender, EventArgs e)
        {
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    GridView1.RenderControl(hw);
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

        void mysqlinsert()
        {
            try
            {

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand sql = connection.CreateCommand();
                    sql.CommandText = "INSERT INTO test.test_db(detected_id, tag_id, tag_epc, tag_time) VALUES(@id, @tagid, @epc, @time)";
                    sql.Parameters.AddWithValue("@id", id.ToUpper());
                    sql.Parameters.AddWithValue("@tagid", tid.ToUpper());
                    sql.Parameters.AddWithValue("@epc", epc.ToUpper());
                    sql.Parameters.AddWithValue("@time", timeStamp);
                    sql.ExecuteNonQuery();
                    connection.Close();

                    lblSuccessMessage.Text = "Submitted Successfully";
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
            }
        }

        void postgresqlInsert()
        {
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            NpgsqlCommand sql = new NpgsqlCommand();
            sql.CommandText = "INSERT INTO test.test_db( detected_id, tag_id, tag_epc, tag_time)VALUES(@id, @tagid, @epc, @time);";
            sql.Parameters.AddWithValue("@id", id.ToUpper());
            sql.Parameters.AddWithValue("@tagid", tid.ToUpper());
            sql.Parameters.AddWithValue("@epc", epc.ToUpper());
            sql.Parameters.AddWithValue("@time", timeStamp);
            sql.Connection = connection;
            connection.Open();
            try
            {
                int aff = sql.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Error encountered during INSERT operation");
            }
            finally
            {
                connection.Close();
            }
        }
    }
}