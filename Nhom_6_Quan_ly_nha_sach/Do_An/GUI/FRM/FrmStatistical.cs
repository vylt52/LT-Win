using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraReports.UI;
using GUI.Report;
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

namespace GUI.FRM
{
    public partial class FrmStatistical : Form
    {
        SqlConnection connection = new SqlConnection("Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234");
        string staffName;
        DataTable table;
        public FrmStatistical(string staffName)
        {
            InitializeComponent();
            this.staffName = staffName;
            gvStatistical.IndicatorWidth = 50;
            dateFrom.DateTime = DateTime.Now;
            dateTo.DateTime = DateTime.Now;
        }
        // Tổng tiền nhập hàng
        public double TotalEntrySlip(DateTime fromDate, DateTime toDate)
        {
            double totalAmount = 0;

            string queryString = "SELECT SUM(ESD.price * ESD.quantity) AS TotalAmount " +
                                "FROM EntrySlip AS ES " +
                                "JOIN EntrySlipDetail AS ESD ON ES.id = ESD.entrySlipId " +
                                "WHERE ES.isPay = 1 " +
                                "AND ES.createDate >= @FromDate " +
                                "AND ES.createDate <= @ToDate";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.Add("@FromDate", SqlDbType.Date).Value = fromDate;
                command.Parameters.Add("@ToDate", SqlDbType.Date).Value = toDate;

                connection.Open();
                var result = command.ExecuteScalar();
                totalAmount = result != DBNull.Value ? Convert.ToDouble(result) : 0;
                connection.Close();
            }

            return totalAmount;
        }
        // Tổng tiền bán hàng (đã trừ discount)
        public double TotalInvoice(DateTime fromDate, DateTime toDate)
        {
            double totalAmount = 0;

            string queryString = "SELECT SUM((ID.price * ID.quantity) * (1 - I.discount / 100)) AS TotalPaidAmount " +
                                "FROM Invoice AS I " +
                                "JOIN InvoiceDetail AS ID ON I.id = ID.invoiceId " +
                                "WHERE I.isPay = 1 " +
                                "AND I.createDate >= @FromDate " +
                                "AND I.createDate <= @ToDate";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.Add("@FromDate", SqlDbType.Date).Value = fromDate;
                command.Parameters.Add("@ToDate", SqlDbType.Date).Value = toDate;

                connection.Open();
                var result = command.ExecuteScalar();
                totalAmount = result != DBNull.Value ? Convert.ToDouble(result) : 0;
                connection.Close();
            }
            return totalAmount;
        }
        // Lấy dữ liệu chi tiết từng ngày trong khoảng thời gian
        public DataTable LoadDetailStatistical(GridControl gc, DateTime fromDate, DateTime toDate)
        {
            DataTable tb = new DataTable();
            tb.Columns.Add("date");
            tb.Columns.Add("invoice");
            tb.Columns.Add("entrySlip");
            tb.Columns.Add("profit");

            string queryString = "SELECT date, " +
                                 "ISNULL(SUM(invoice), 0) AS invoice, " +
                                 "ISNULL(SUM(entrySlip), 0) AS entrySlip, " +
                                 "ISNULL(SUM(invoice), 0) - ISNULL(SUM(entrySlip), 0) AS profit " +
                                 "FROM (SELECT I.createDate AS date, ID.price * ID.quantity AS invoice, 0 AS entrySlip " +
                                 "FROM Invoice AS I INNER JOIN InvoiceDetail AS ID ON I.id = ID.invoiceId " +
                                 "WHERE I.isPay = 1 AND I.createDate >= @FromDate AND I.createDate <= @ToDate " +
                                 "UNION ALL " +
                                 "SELECT ES.createDate AS date, 0 AS invoiceTotal, ESD.price * ESD.quantity AS entrySlip " +
                                 "FROM EntrySlip AS ES INNER JOIN EntrySlipDetail AS ESD ON ES.id = ESD.entrySlipId " +
                                 "WHERE ES.isPay = 1 AND ES.createDate >= @FromDate AND ES.createDate <= @ToDate) AS CombinedResults " +
                                 "GROUP BY date " +
                                 "ORDER BY date";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.Add("@FromDate", SqlDbType.Date).Value = fromDate;
                command.Parameters.Add("@ToDate", SqlDbType.Date).Value = toDate;

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(tb);
                }
            }

            gc.DataSource = tb;
            table = tb;
            return tb;
        }
        // btn Thống Kê
        private void btnThongKe_Click(object sender, EventArgs e)
        {
            if (DateTime.Parse(dateFrom.DateTime.ToShortDateString()).CompareTo(DateTime.Parse(dateTo.DateTime.ToShortDateString())) > 0)
            {
                XtraMessageBox.Show("Ngày tìm không hợp lệ.", "Thông báo");
                return;
            }
            var sumStatistic = TotalInvoice(dateFrom.DateTime, dateTo.DateTime);
            var sumSpend = TotalEntrySlip(dateFrom.DateTime, dateTo.DateTime);
            txtSumStatistic.Text = convertVND(sumStatistic.ToString());
            txtSumSpend.Text = convertVND(sumSpend.ToString());
            txtProfit.Text = convertVND((sumStatistic - sumSpend).ToString());
            DataTable tb = LoadDetailStatistical(gcStatistical, dateFrom.DateTime, dateTo.DateTime);
        }
        private void gvStatistical_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (!e.Info.IsRowIndicator || e.RowHandle < 0)
                return;
            e.Info.DisplayText = (e.RowHandle + 1) + "";
        }
        public static string convertVND(string money)
        {
            var format = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            string value = String.Format(format, "{0:N0}", Convert.ToDouble(money));
            return value;
        }
        
        private void btnDong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        private void btnPrint1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Kết nối cơ sở dữ liệu
            //using (SqlConnection connection = new SqlConnection("Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234"))
            //{
            //connection.Open();

            //// Lấy dữ liệu từ cơ sở dữ liệu
            //string query = "SELECT date, " +
            //                 "ISNULL(SUM(invoice), 0) AS invoice, " +
            //                 "ISNULL(SUM(entrySlip), 0) AS entrySlip, " +
            //                 "ISNULL(SUM(invoice), 0) - ISNULL(SUM(entrySlip), 0) AS profit " +
            //                 "FROM (SELECT I.createDate AS date, ID.price * ID.quantity AS invoice, 0 AS entrySlip " +
            //                 "FROM Invoice AS I INNER JOIN InvoiceDetail AS ID ON I.id = ID.invoiceId " +
            //                 "WHERE I.isPay = 1 AND I.createDate >= @FromDate AND I.createDate <= @ToDate " +
            //                 "UNION ALL " +
            //                 "SELECT ES.createDate AS date, 0 AS invoiceTotal, ESD.price * ESD.quantity AS entrySlip " +
            //                 "FROM EntrySlip AS ES INNER JOIN EntrySlipDetail AS ESD ON ES.id = ESD.entrySlipId " +
            //                 "WHERE ES.isPay = 1 AND ES.createDate >= @FromDate AND ES.createDate <= @ToDate) AS CombinedResults " +
            //                 "GROUP BY date " +
            //                 "ORDER BY date";
            //SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            //DataTable dataTable = new DataTable();
            //adapter.Fill(dataTable);

            // Kiểm tra xem có dữ liệu không
            //    if (table.Rows.Count == 0)
            //        return;

            //    // Tạo báo cáo
            //    var rp = new rpStatistical();
            //    rp.DataSource = table;
            //    rp.lbNguoiLap.Value = staffName;
            //    rp.ShowPreviewDialog();
            //}
            var rp = new rpStatistical();
            rp.DataSource = table;
            rp.lbNguoiLap.Value = staffName;
            rp.ShowPreviewDialog();
        }
    }
}
