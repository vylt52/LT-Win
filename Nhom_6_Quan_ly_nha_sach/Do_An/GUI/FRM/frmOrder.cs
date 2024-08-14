using ClassSupport;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using GUI.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;

namespace GUI.FRM
{
    public partial class frmOrder : Form
    {
        string staffId;
        string staffName;
        CultureInfo culture = new CultureInfo("en-US");
        SqlConnection connection = new SqlConnection("Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234");

        public frmOrder(string staffId,string staffName)
        {
            InitializeComponent();
            this.staffId = staffId;
            this.staffName = staffName;
        }
        //load data khi form khởi chạy
        private void frmOrder_Load(object sender, EventArgs e)
        {
            LoadInvoice();
            gvOrder.IndicatorWidth = 50;
            gvOrderDetail.IndicatorWidth = 50;
        }

        private void LoadInvoice()
        {
            string query = @"SELECT E.id AS id, S.name AS staffName, SP.name AS customerName, E.createDate as createDate, E.discount as discount, E.isPay as isPay
                        FROM Invoice E 
                        LEFT JOIN Staff S ON E.staffId = S.id 
                        LEFT JOIN Customer SP ON E.customerId = SP.id";

            DataTable table = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
                table.Columns.Add("totalOrder", typeof(string));

                gcOrder.DataSource = table;

                for (int i = 0; i < gvOrder.RowCount; i++)
                {
                    var (_, totalOrder) = ConvertList(LoadInvoiceDetail(gvOrder.GetRowCellValue(i, "id").ToString()));
                    gvOrder.SetRowCellValue(i, "totalOrder", String.Format(culture, "{0:N0}", totalOrder));
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Thông báo LoadInvoice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private DataTable LoadInvoiceDetail(string mahd)
        {
            DataTable table = new DataTable();
            try
            {
                string query = @"SELECT I.id as id, I.price as price, I.quantity as quantity, B.name as bookName, I.invoiceId as invoiceId 
                                FROM InvoiceDetail I
                                LEFT JOIN Book B ON B.id = I.bookId
                                WHERE invoiceId = @invoiceId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@invoiceId", mahd);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);

                table.Columns.Add("totalInvoiceDetail", typeof(string));

                return table;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Thông báo  LoadInvoiceDetail", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }

        private (List<OrderDetail>, decimal) ConvertList(DataTable table)
        {
            // Khởi tạo danh sách để chứa dữ liệu
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            decimal tong = 0;
            foreach (DataRow row in table.Rows)
            {
                // Tạo đối tượng ImportDetail từ DataRow
                OrderDetail detail = new OrderDetail
                {
                    Id = row["id"].ToString(),
                    InvoiceId = row["invoiceId"].ToString(),
                    BookName = row["bookName"].ToString(),
                    Price = String.Format(culture, "{0:N0}", row["price"]),
                    Quantity = String.Format(culture, "{0:N0}", row["quantity"]),
                };
                decimal thanhtien = (int.Parse(row["quantity"].ToString()) * decimal.Parse(row["price"].ToString()));
                tong += thanhtien;
                detail.TotalEntrySlipDetail = String.Format(culture, "{0:N0}", thanhtien);
                // Thêm đối tượng vào danh sách
                orderDetails.Add(detail);
            }

            // Trả về danh sách ImportDetail
            return (orderDetails, tong);
        }
        //đóng form hoá đơn
        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void gvOrder_MasterRowEmpty(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowEmptyEventArgs e)
        {
            var invoidId = gvOrder.GetRowCellValue(e.RowHandle, "id").ToString();
            if (invoidId != null)
                e.IsEmpty = LoadInvoiceDetail(invoidId).Rows.Count == 0;
        }

        private void gvOrder_MasterRowGetChildList(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetChildListEventArgs e)
        {
            var id = gvOrder.GetRowCellValue(e.RowHandle, "id").ToString();
            if (id != null)
            {
                var (listOrderDetail, _) = ConvertList(LoadInvoiceDetail(id));
                e.ChildList = listOrderDetail;
                gvOrderDetail.ViewCaption = "Chi tiết phiếu nhập " + id;

                gvOrderDetail.ViewCaption = "Chi tiết hoá đơn " + id.ToString();

            }
        }

        private void gvOrder_MasterRowGetRelationCount(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationCountEventArgs e)
        {
            e.RelationCount = 1;
        }

        private void gvOrder_MasterRowGetRelationName(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationNameEventArgs e)
        {
            e.RelationName = "Chi tiết hoá đơn";
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int row = gvOrder.FocusedRowHandle;
            if (row >= 0)
            {
                string id = gvOrder.GetRowCellValue(row, "id").ToString();
                var (listOrderDetail, _) = ConvertList(LoadInvoiceDetail(id));
                var rp = new rpOrder();
                rp.DataSource = listOrderDetail;
                rp.lbNguoiLap.Value = staffName;
                rp.lbCodeOrder.Value = "BÁO CÁO HOÁ ĐƠN " + id;
                rp.lbCustomer.Value = gvOrder.GetRowCellValue(row, "customerName");
                rp.lbDate.Value = gvOrder.GetRowCellValue(row, "createDate");
                rp.lbStaff.Value = gvOrder.GetRowCellValue(row, "staffName");
                rp.lbTienPhaiTra.Value = gvOrder.GetRowCellValue(row, "totalOrder");
                rp.lbDiscount.Value = gvOrder.GetRowCellValue(row, "discount") + "%";
                rp.ShowPreviewDialog();

            }
        }

        private void gvOrder_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (!e.Info.IsRowIndicator || e.RowHandle < 0)
                return;
            e.Info.DisplayText = (e.RowHandle + 1) + "";
        }

        private void gvOrderDetail_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (!e.Info.IsRowIndicator || e.RowHandle < 0)
                return;
            e.Info.DisplayText = (e.RowHandle + 1) + "";
        }
    }
}
