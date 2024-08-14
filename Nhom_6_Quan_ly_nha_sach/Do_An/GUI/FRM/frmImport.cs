using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;
using ClassSupport;
using GUI.Report;

namespace GUI.FRM
{
    public partial class frmImport : Form
    {
        SqlConnection connection = new SqlConnection("Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234");
        string staffId;
        string staffName;
        CultureInfo culture = new CultureInfo("en-US");

        public frmImport(string staffId,string staffName)
        {
            InitializeComponent();
            this.staffId = staffId;
            this.staffName = staffName;
        }
        //load data khi form khởi chạy
        private void frmImport_Load(object sender, EventArgs e)
        {
            LoadImport();
            gvImport.IndicatorWidth = 50;
            gvImportDetail.IndicatorWidth = 50;
        }

        private void LoadImport()
        {
            string query = @"SELECT E.id AS id, S.name AS staffName, SP.name AS supplierName, E.createDate as createDate, E.staffId as staffId, E.supplierId as supplierId, E.isPay as isPay
                        FROM EntrySlip E 
                        LEFT JOIN Staff S ON E.staffId = S.id 
                        LEFT JOIN Supplier SP ON E.supplierId = SP.id";

            DataTable table = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
                table.Columns.Add("total", typeof(string));
                gcImport.DataSource = table;

                for (int i = 0; i < gvImport.RowCount; i++)
                {
                    var (_, totalImport) = ConvertList(LoadImportDetail(gvImport.GetRowCellValue(i, "id").ToString()));
                    gvImport.SetRowCellValue(i, "total", String.Format(culture, "{0:N0}", totalImport));
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Thông báo LoadgcImport", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private DataTable LoadImportDetail(string entrySlipId)
        {
            DataTable table = new DataTable();
            try
            {
                string query = @"SELECT E.id as id,E.bookId as bookId,E.price as price,E.quantity as quantity, B.name as bookName, E.entrySlipId as entrySlipId, B.name as bookName
                                    FROM EntrySlipDetail E
                                    LEFT JOIN Book B ON E.bookId = B.id 
                                    WHERE entrySlipId = @entrySlipId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@entrySlipId", entrySlipId);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);

                return table;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Thông báo  LoadgcImportDetail", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }
        //đóng form
        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void gvImport_MasterRowEmpty(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowEmptyEventArgs e)
        {
            var id = gvImport.GetRowCellValue(e.RowHandle, "id");
            if (id != null)
                e.IsEmpty = LoadImportDetail(id.ToString()).Rows.Count == 0;
        }

        private void gvImport_MasterRowGetChildList(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetChildListEventArgs e)
        {
            var id = gvImport.GetRowCellValue(e.RowHandle, "id").ToString();
            if (id != null)
            {
                var (ListImportDetail, _) = ConvertList(LoadImportDetail(id));
                e.ChildList = ListImportDetail;
                gvImportDetail.ViewCaption = "Chi tiết phiếu nhập " + id;
            }
        }

        private (List<ImportDetail>, decimal) ConvertList(DataTable table)
        {
            // Khởi tạo danh sách để chứa dữ liệu
            List<ImportDetail> importDetails = new List<ImportDetail>();
            decimal tong = 0;
            foreach (DataRow row in table.Rows)
            {
                // Tạo đối tượng ImportDetail từ DataRow
                ImportDetail detail = new ImportDetail
                {
                    Id = row["id"].ToString(),
                    EntryStripId = row["entrySlipId"].ToString(),
                    //BookId = row["bookId"].ToString(),
                    BookName = row["bookName"].ToString(),
                    Price = String.Format(culture, "{0:N0}", row["price"]),
                    Quantity = String.Format(culture, "{0:N0}", row["quantity"]),
                };
                decimal thanhtien = (int.Parse(row["quantity"].ToString()) * decimal.Parse(row["price"].ToString()));
                tong += thanhtien;
                detail.TotalEntrySlipDetail = String.Format(culture, "{0:N0}", thanhtien);
                // Thêm đối tượng vào danh sách
                importDetails.Add(detail);
            }

            // Trả về danh sách ImportDetail
            return (importDetails, tong);
        }

        // Ví dụ hàm tính toán giá trị totalEntrySlipDetail
        private string CalculateTotalEntrySlipDetail(DataRow row)
        {
            // Tính toán tổng số lượng hoặc các phép tính khác dựa trên các cột của hàng
            // Ví dụ: Giá * Số lượng
            decimal price = Convert.ToDecimal(row["price"]);
            int quantity = Convert.ToInt32(row["quantity"]);
            decimal total = price * quantity;

            // Chuyển đổi giá trị sang chuỗi
            return String.Format(culture, "{0:N0}", total); // "C" định dạng là tiền tệ
        }


        private void gvImport_MasterRowGetRelationCount(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationCountEventArgs e)
        {
            e.RelationCount = 1;
        }

        private void gvImport_MasterRowGetRelationName(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationNameEventArgs e)
        {
            e.RelationName = "Chi tiết phiếu nhập";
        }


        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int row = gvImport.FocusedRowHandle;
            if (row >= 0)
            {
                string entrySlipId = gvImport.GetRowCellValue(row, "id").ToString();
                var rp = new rpImport();
                var (ListImportDetail, _) = ConvertList(LoadImportDetail(entrySlipId));

                rp.DataSource = ListImportDetail;
                rp.lbNguoiLap.Value = staffName;
                rp.lbCodeImport.Value = "BÁO CÁO PHIẾU NHẬP " + entrySlipId;
                rp.lbDate.Value = gvImport.GetRowCellValue(row, "createDate").ToString();
                rp.lbStaff.Value = gvImport.GetRowCellValue(row, "staffName").ToString();
                rp.lbSupplier.Value = gvImport.GetRowCellValue(row, "supplierName").ToString();

                rp.ShowPreviewDialog();
            }
        }

        private void gvImport_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (!e.Info.IsRowIndicator || e.RowHandle < 0)
                return;
            e.Info.DisplayText = (e.RowHandle + 1) + "";
        }

        private void gvImportDetail_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {

            if (!e.Info.IsRowIndicator || e.RowHandle < 0)
                return;
            e.Info.DisplayText = (e.RowHandle + 1) + "";
        }
    }
}
