using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GUI.Report;
using DevExpress.XtraReports.UI;

namespace GUI.FRM
{
    public partial class frmInventory : Form
    {
        private string staffName;
        SqlConnection connection = new SqlConnection("Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234");
        DataTable tb;
        public class ItemInventory
        {
            public ItemInventory() { }

            public string Date { get; set; }
            public int BookId { get; set; }
            public int QuantityEntrySlip { get; set; }
            public int QuantityInvoice { get; set; }
        }
        public frmInventory(string staffName)
        {
            InitializeComponent();
            gvInventory.IndicatorWidth = 50;
            dateFrom.DateTime = DateTime.Now;
            dateTo.DateTime = DateTime.Now;
            this.staffName = staffName;
        }
        public DataTable LoadDetailInventory(GridControl gc, DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            DataTable tb = new DataTable();
            string query = @"SELECT 
	                            createDate AS date,
                                name as book,
                                SUM(entrySlipQuantity) AS entrySlip,
                                SUM(invoiceQuantity) AS invoice
                            FROM
                                (SELECT 
                                    B.name AS name, 
                                    ESD.quantity AS entrySlipQuantity, 
                                    0 AS invoiceQuantity,
                                    ES.createDate AS createDate
                                FROM 
                                    EntrySlipDetail ESD
                                JOIN 
                                    EntrySlip ES ON ESD.entrySlipId = ES.id
                                JOIN 
                                    Book B ON ESD.bookId = B.id
    
                                UNION ALL
    
                                SELECT 
                                    B.name AS name, 
                                    0 AS entrySlipQuantity, 
                                    ID.quantity AS invoiceQuantity,
                                    I.createDate AS createDate
                                FROM 
                                    InvoiceDetail ID
                                JOIN 
                                    Invoice I ON ID.invoiceId = I.id
                                JOIN 
                                    Book B ON ID.bookId = B.id) AS combinedData
                                WHERE
                                createDate >= @DateTimeFrom AND
                                createDate <= @DateTimeTo
                            GROUP BY 
                                name, createDate";



            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Clear();
                command.Parameters.Add("@DateTimeFrom", SqlDbType.Date).Value = dateTimeFrom;
                command.Parameters.Add("@DateTimeTo", SqlDbType.Date).Value = dateTimeTo;

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(tb);
                }
            }
            gc.DataSource = tb;
            return tb;
        }

        private void gvInventory_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (!e.Info.IsRowIndicator || e.RowHandle < 0)
                return;
            e.Info.DisplayText = (e.RowHandle + 1) + "";
        }
        private int QuantityEntrySlip(DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            int sum = 0;

            string query = @"SELECT SUM(esd.quantity) FROM EntrySlip es 
                    INNER JOIN EntrySlipDetail esd ON es.id = esd.entrySlipId 
                    WHERE es.createDate >= @DateTimeFrom AND es.createDate <= @DateTimeTo";

            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@DateTimeFrom", SqlDbType.Date).Value = dateTimeFrom;
                    command.Parameters.Add("@DateTimeTo", SqlDbType.Date).Value = dateTimeTo;

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        sum = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có
                sum = -1;
            }
            finally
            {
                connection.Close();
            }

            return sum;
        }

        private int QuantityInvoice(DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            int sum = 0;

            string query = @"SELECT SUM(quantity) FROM InvoiceDetail 
                    WHERE invoiceId IN 
                    (SELECT id FROM Invoice WHERE createDate >= @DateTimeFrom AND createDate <= @DateTimeTo)";

            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@DateTimeFrom", SqlDbType.Date).Value = dateTimeFrom;
                    command.Parameters.Add("@DateTimeTo", SqlDbType.Date).Value = dateTimeTo;

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        sum = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có
                sum = -1;
            }
            finally
            {
                connection.Close();
            }

            return sum;
        }
        private void btnDong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        public static string convertVND(string money)
        {
            var format = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            string value = String.Format(format, "{0:N0}", Convert.ToDouble(money));
            return value;
        }

        private void btnThongKe_Click_1(object sender, EventArgs e)
        {
            if (DateTime.Parse(dateFrom.DateTime.ToShortDateString()).CompareTo(DateTime.Parse(dateTo.DateTime.ToShortDateString())) > 0)
            {
                XtraMessageBox.Show("Ngày tìm không hợp lệ.", "Thông báo");
                return;
            }
            var quantityEntrySlip = QuantityEntrySlip(dateFrom.DateTime, dateTo.DateTime);
            var quantityInvoice = QuantityInvoice(dateFrom.DateTime, dateTo.DateTime);
            txtLuongNhap.Text = convertVND(quantityEntrySlip.ToString());
            txtLuongBan.Text = convertVND(quantityInvoice.ToString());
            tb = LoadDetailInventory(gcInventory, dateFrom.DateTime, dateTo.DateTime);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (tb == null || tb.Rows.Count == 0)
                return;
            var rp = new rpInventory();
            rp.DataSource = tb;
            rp.lbNguoiLap.Value = this.staffName;
            rp.ShowPreviewDialog();
        }
    }
}
