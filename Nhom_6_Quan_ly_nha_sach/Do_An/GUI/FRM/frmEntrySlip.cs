using DevExpress.XtraEditors;
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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GUI.FRM
{
    public partial class frmEntrySlip : Form
    {
        SqlConnection connection = new SqlConnection("Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234");
        string staffId;
        string staffName;
        CultureInfo culture = new CultureInfo("en-US");
        public frmEntrySlip(string staffId,string staffName)
        {
            InitializeComponent();
            this.staffId = staffId;
            this.staffName = staffName;
            connection.Open();
        }

        //load data khi form khởi chạy
        private void Form1_Load(object sender, EventArgs e)
        {
            //load danh sách hoá đơn chưa thanh toán
            LoadSupplier();
            LoadgcImport();
            //LoadBook();
            clearDataGVImportDetail();
            gvImportDetail.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
            gvImport.IndicatorWidth = 50;
            gvImportDetail.IndicatorWidth = 50;

        }

        private void LoadSupplier()
        {
            DataTable table = new DataTable();
            try
            {
                string query = "Select * from Supplier";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);

                lkSupplier.Properties.DataSource = table;
                lkSupplier.Properties.DisplayMember = "name";
                lkSupplier.Properties.ValueMember = "id";

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Thông báo LoadSupplier", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LoadgcImport()
        {
            string query = @"SELECT E.id AS id, S.name AS StaffName, SP.name AS SupplierName, E.createDate as createDate, E.staffId as staffId, E.supplierId as supplierId, E.isPay as isPay
                        FROM EntrySlip E 
                        LEFT JOIN Staff S ON E.staffId = S.id 
                        LEFT JOIN Supplier SP ON E.supplierId = SP.id";

            DataTable table = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
                table.Columns.Add("totalEntrySlip", typeof(string));
                gcImport.DataSource = table;

                for (int i = 0; i < gvImport.RowCount; i++)
                {
                    gvImport.SetRowCellValue(i, "totalEntrySlip", String.Format(culture, "{0:N0}", LoadgcImportDetail(gvImport.GetRowCellValue(i, "id").ToString())));
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Thông báo LoadgcImport", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void clearDataGVImportDetail()
        {
            gcImportDetail.DataSource = null;
            layoutGroupImportDetail.Enabled = false;
            txtTienPhaiTra.Text = "";
        }

        private double LoadgcImportDetail(string entrySlipId)
        {
            DataTable table = new DataTable();
            try
            {
                string query = @"SELECT id,bookId,price,quantity FROM EntrySlipDetail WHERE entrySlipId = @entrySlipId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@entrySlipId", entrySlipId);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
                table.Columns.Add("totalEntrySlipDetail", typeof(string));

                gcImportDetail.DataSource = table;

                return TongTien();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Thông báo  LoadgcImportDetail", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return -1;
            }
        }

        private void LoadBook(string supplierId)
        {
            try
            {
                string query = "SELECT * FROM Book WHERE supplierId = @supplierId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@supplierId", supplierId);

                DataTable table = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);

                lkBook.DataSource = table;
                lkBook.DisplayMember = "name";
                lkBook.ValueMember = "id";
            }
            catch (Exception ex)
            {
                connection.Close();
                XtraMessageBox.Show(ex.Message, "Thông báo  LoadBook", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //đóng form nhập hàng
        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        //gọi các chi tiết của 1 phiếu nhập có mã phiếu nhập truyền vào
        private void callDataGVImportDetail(string entrySlipId, string supplierId)
        {
            LoadgcImportDetail(entrySlipId);
            LoadBook(supplierId);

            layoutGroupImportDetail.Enabled = true;
            layoutGroupImportDetail.Text = "Chi tiết phiếu nhập " + entrySlipId;

            txtTienPhaiTra.Text = TongTien().ToString();

        }

        private double TongTien()
        {
            double totalAmount = 0;
            for (int i = 0; i < gvImportDetail.RowCount; i++)
            {
                double price = Convert.ToDouble(gvImportDetail.GetRowCellValue(i, "price"));
                int quantity = Convert.ToInt32(gvImportDetail.GetRowCellValue(i, "quantity"));
                totalAmount += price * quantity;
                gvImportDetail.SetRowCellValue(i, "totalEntrySlipDetail", String.Format(culture, "{0:N0}", price * quantity));
            }
            return totalAmount;
        }

        //click 1 dòng trong gridview nhập kho
        private void gvImport_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            var entrySlipId = gvImport.GetRowCellValue(e.RowHandle, "id").ToString();
            var supplierId = gvImport.GetRowCellValue(e.RowHandle, "supplierId").ToString();
            if (e.RowHandle > -1 && gvImport.GetRowCellValue(e.RowHandle, "isPay").ToString() != "True")
                callDataGVImportDetail(entrySlipId, supplierId);
            else
            {
                layoutGroupImportDetail.Enabled = true;
                clearDataGVImportDetail();
            }
        }
        //tạo 1 hoá đơn mới cho khách hàng       
        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtIdEntrySlip.Text.Trim() == "")
                {
                    XtraMessageBox.Show("Vui lòng nhập Mã phiếu nhập", "Thông báo click");
                    return;
                }
                if (lkSupplier.EditValue == null)
                {
                    XtraMessageBox.Show("Vui lòng nhập chọn nhà cung cấp", "Thông báo");
                    return;
                }
                string idEntrySlip = txtIdEntrySlip.Text.Trim();
                string sqlQuery = "INSERT INTO EntrySlip (id,staffId,createDate,supplierId,isPay) VALUES (@ID,@STAFFID,@CREATEDATE,@SUPPLIERID,@ISPAY)";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@ID", idEntrySlip);
                    command.Parameters.AddWithValue("@STAFFID", staffId);
                    command.Parameters.Add("@CREATEDATE", SqlDbType.DateTime).Value = DateTime.Now;
                    command.Parameters.AddWithValue("@SUPPLIERID", lkSupplier.EditValue.ToString());
                    command.Parameters.AddWithValue("@ISPAY", false);

                    command.ExecuteNonQuery();
                    XtraMessageBox.Show("Tạo phiếu nhập thành công.", "Thông báo");
                    LoadgcImport();
                    gvImport.FocusedRowHandle = gvImport.RowCount - 1;
                    callDataGVImportDetail(idEntrySlip, lkSupplier.EditValue.ToString());
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Vui lòng nhập Mã phiếu nhập không trùng nhau", "Thông báo");
            }


        }
        //huỷ 1 phiếu trong gridview nhập kho
        private void destroyImport()
        {
            var entrySlipId = gvImport.GetRowCellValue(gvImport.FocusedRowHandle, "id");
            var supplierId = gvImport.GetRowCellValue(gvImport.FocusedRowHandle, "supplierId");
            if (entrySlipId != null)
            {
                if (XtraMessageBox.Show("Bạn chắc chắn huỷ phiếu nhập " + entrySlipId.ToString() + "?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    string query = "DELETE FROM EntrySlip WHERE id = @id;";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", entrySlipId);
                    var excuting = command.ExecuteNonQuery();

                    if (excuting != -1)
                    {
                        XtraMessageBox.Show("Huỷ phiếu nhập thành công " + entrySlipId + ".", "Thông báo");
                        LoadgcImport();
                        clearDataGVImportDetail();
                    }
                    else
                        XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        //sự kiện gọi hàm huỷ phiếu nhập
        private void btnDestroy_Click(object sender, EventArgs e)
        {
            destroyImport();
        }
        //click nút delete xoá 1 dòng trong chi tiết hoá đơn
        private void gcImportDetail_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && gvImportDetail.State != GridState.Editing)
            {
                var entrySlipId = gvImportDetail.GetRowCellValue(gvImportDetail.FocusedRowHandle, "entrySlipId");
                var supplierId = gvImport.GetRowCellValue(gvImport.FocusedRowHandle, "supplierId");
                var bookId = gvImportDetail.GetRowCellValue(gvImportDetail.FocusedRowHandle, "bookId");

                if (entrySlipId != null)
                {
                    if (XtraMessageBox.Show("Bạn chắc chắn xoá sách " + bookId + "?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        string id = gvImportDetail.GetRowCellValue(gvImportDetail.FocusedRowHandle, "id").ToString();
                        int i = DeleteEntrySlipDetail(id);
                        int row = gvImport.FocusedRowHandle;
                        if (i != -1)
                        {
                            XtraMessageBox.Show("Xoá thành công.", "Thông báo");
                            LoadgcImportDetail(entrySlipId.ToString());
                            LoadgcImport();
                            callDataGVImportDetail(entrySlipId.ToString(), supplierId.ToString());
                            //clearDataGVImportDetail();
                            gvImport.FocusedRowHandle = row;
                        }
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (e.KeyCode == Keys.Enter && gvImportDetail.State != GridState.Editing)
            {
                string id = gvImportDetail.GetRowCellValue(gvImportDetail.FocusedRowHandle, "id").ToString();
                string entrySlipIdInsert = gvImport.GetRowCellValue(gvImport.FocusedRowHandle, "id").ToString();
                string bookId = gvImportDetail.GetRowCellValue(gvImportDetail.FocusedRowHandle, "bookId").ToString();
                int quantity = int.Parse(gvImportDetail.GetRowCellValue(gvImportDetail.FocusedRowHandle, "quantity").ToString());
                double price = double.Parse(gvImportDetail.GetRowCellValue(gvImportDetail.FocusedRowHandle, "price").ToString());


                //XtraMessageBox.Show("Insert " + id + "  " + bookId + "  " + quantity + "  " + price);

                int i = InsertEntrySlipDetail(id, entrySlipIdInsert, bookId, quantity, price);

                //XtraMessageBox.Show(i.ToString(), "i");

                if (i > 0)
                    XtraMessageBox.Show("Thêm thành công", "Thông báo", DevExpress.Utils.DefaultBoolean.True);
                int row = gvImport.FocusedRowHandle;
                string entrySlipId = gvImport.GetRowCellValue(row, "id").ToString();
                string supplierId = gvImport.GetRowCellValue(row, "supplierId").ToString();
                LoadgcImport();
                callDataGVImportDetail(entrySlipId, supplierId);
                gvImport.FocusedRowHandle = row;
            }


        }

        //ngăn không cho thao tác khi thêm sửa 1 dòng trong bảng ctnk khi dữ liệu sai
        private void gvImportDetail_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }
        //thêm sửa 1 dòng trong bảng chi tiết nhập kho
        private void gvImportDetail_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            string sErr = "";
            bool bVali = true;
            // Lấy giá trị của các cột bookId, quantity, price
            var id = gvImportDetail.GetRowCellValue(e.RowHandle, "id").ToString().Trim();
            var bookId = gvImportDetail.GetRowCellValue(e.RowHandle, "bookId").ToString().Trim();
            var quantityStr = gvImportDetail.GetRowCellValue(e.RowHandle, "quantity").ToString().Trim();
            var priceStr = gvImportDetail.GetRowCellValue(e.RowHandle, "price").ToString().Trim();

            if (string.IsNullOrWhiteSpace(id))
            {
                bVali = false;
                sErr = "Vui lòng điền Id.\n";
            }
            string pattern = "^[A-Za-z0-9]+$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(id))
            {
                bVali = false;
                sErr = "Vui lòng điền Id không chứa ký tự đặc biệt.\n";
            }


            if (string.IsNullOrWhiteSpace(bookId))
            {
                bVali = false;
                sErr = "Vui lòng chọn sách.\n";
            }

            if (string.IsNullOrWhiteSpace(quantityStr))
            {
                bVali = false;
                sErr += "Vui lòng điền số lượng.\n";
            }

            if (string.IsNullOrWhiteSpace(priceStr))
            {
                bVali = false;
                sErr += "Vui lòng điền đơn giá.\n";
            }
            else if (double.Parse(priceStr) <= 0)
            {
                bVali = false;
                sErr += "Đơn giá phải lớn hơn 0.\n";
            }
            else if (int.Parse(quantityStr) <= 0)
            {
                bVali = false;
                sErr += "Số lượng phải lớn hơn 0.\n";
            }

            if (!bVali)
            {
                e.Valid = false;
                XtraMessageBox.Show(sErr, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                gvImportDetail.SetRowCellValue(e.RowHandle, "totalEntrySlipDetail", String.Format(culture, "{0:N0}", (int.Parse(quantityStr) * double.Parse(priceStr))));

            }
        }
        //thanh toán 1 phiếu nhập
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (txtTienPhaiTra.Text.Trim().Length == 0)
            {
                XtraMessageBox.Show("Mời bạn chọn phiếu nhập muốn thanh toán.", "Thông báo");
                return;
            }
            double tienPhaiTra = double.Parse(gvImport.GetRowCellValue(gvImport.FocusedRowHandle, "totalEntrySlip").ToString());

            if (tienPhaiTra == 0)
            {
                XtraMessageBox.Show("Phiếu nhập chưa có sản phẩm không cần thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query = "UPDATE EntrySlip SET isPay = @isPay WHERE id = @id;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@isPay", true);
            string id = gvImport.GetRowCellValue(gvImport.FocusedRowHandle, "id").ToString();
            command.Parameters.AddWithValue("@id", id);
            var i = command.ExecuteNonQuery();

            if (i != -1)
            {
                XtraMessageBox.Show("Thanh toán thành công.", "Thông báo");
                LoadgcImport();
                clearDataGVImportDetail();
            }
        }
        //xoá 1 phiếu nhập bằng nút delete
        private void gcImport_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                destroyImport();
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

        private int InsertEntrySlipDetail(string id, string entrySlipId, string bookId, int quantity, double price)
        {
            try
            {
                // Kiểm tra xem có bản ghi nào đã tồn tại với cùng id hoặc bookId không
                string queryCheckExisting = "SELECT * FROM EntrySlipDetail WHERE entrySlipId = @entrySlipId AND (id = @id OR bookId = @bookId)";

                // Sử dụng khối `using` để quản lý tài nguyên
                using (SqlCommand command = new SqlCommand(queryCheckExisting, connection))
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@bookId", bookId);
                    command.Parameters.AddWithValue("@entrySlipId", entrySlipId);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        // Nếu có bản ghi tồn tại với cùng id hoặc bookId
                        if (table.Rows.Count > 0)
                        {
                            // Kiểm tra xem bản ghi trùng với id hay bookId
                            foreach (DataRow row in table.Rows)
                            {
                                if (row["id"].ToString() == id && row["bookId"].ToString() == bookId)
                                {
                                    UpdateEntrySlipDetail(id, entrySlipId, bookId, quantity, price);
                                    return 2;
                                }
                                else if (row["id"].ToString() == id)
                                {
                                    XtraMessageBox.Show("Bản ghi với id " + id + " đã tồn tại.", "Thông báo");
                                    return -1;
                                }
                                else if (row["bookId"].ToString() == bookId)
                                {
                                    XtraMessageBox.Show("Bản ghi với bookId " + bookId + " đã tồn tại.", "Thông báo");
                                    return -2;
                                }
                            }
                            return 200;
                        }
                        else
                        {
                            // Nếu không có bản ghi tồn tại, thêm mới bản ghi
                            string queryInsert = "INSERT INTO EntrySlipDetail (id, entrySlipId, bookId, quantity, price) VALUES (@id, @entrySlipId, @bookId, @quantity, @price)";
                            command.CommandText = queryInsert;
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.AddWithValue("@entrySlipId", entrySlipId);
                            command.Parameters.AddWithValue("@bookId", bookId);
                            command.Parameters.AddWithValue("@quantity", quantity);
                            command.Parameters.AddWithValue("@price", price);

                            command.ExecuteNonQuery();
                            return 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ
                XtraMessageBox.Show("Error: " + ex.Message, "Insert Error");
                return -100;
            }
        }


        private int UpdateEntrySlipDetail(string id, string entrySlipId, string bookId, int quantity, double price)
        {
            if (quantity == 0)
            {
                DeleteEntrySlipDetail(id);
                return 2; // danh dau da xoa
            }
            else
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE EntrySlipDetail SET bookId = @bookId, quantity = @quantity, price = @price WHERE id = @id AND entrySlipId = @entrySlipId";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@entrySlipId", entrySlipId);
                command.Parameters.AddWithValue("@bookId", bookId);
                command.Parameters.AddWithValue("@quantity", quantity);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();

                return 1;
            }

        }

        private int DeleteEntrySlipDetail(string id)
        {
            try
            {
                SqlCommand command = connection.CreateCommand();

                // Xoá bản ghi dựa trên ID
                string queryDelete = "DELETE FROM EntrySlipDetail WHERE id = @id";
                command.CommandText = queryDelete;
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();

                return 1; // Đánh dấu là đã xoá thành công

            }
            catch (Exception ex)
            {
                // Xử lý exception ở đây
                Console.WriteLine("Error: " + ex.Message);
                return -1; // Đánh dấu là có lỗi
            }
        }

        private void frmEntrySlip_FormClosed(object sender, FormClosedEventArgs e)
        {
            connection.Close();
        }

        private void gcImport_Click(object sender, EventArgs e)
        {

        }
        private void LookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            // Truy cập LookUpEdit
            DevExpress.XtraEditors.LookUpEdit lookUpEdit = sender as DevExpress.XtraEditors.LookUpEdit;

            // Lấy giá trị đã chọn
            object selectedValue = lookUpEdit.EditValue;

            // Xử lý giá trị đã chọn
            if (selectedValue != null)
            {
                // Làm gì đó với giá trị đã chọn
                // Ví dụ: Hiển thị giá trị đã chọn trong một MessageBox
                MessageBox.Show("Giá trị đã chọn: " + selectedValue.ToString());
            }
            else
            {
                // Không có giá trị nào được chọn
                MessageBox.Show("Không có giá trị nào được chọn.");
            }
        }
    }
}
