using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using System.Globalization;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace GUI.FRM
{
    public partial class frmInvoice : Form
    {
        string staffId;
        double discount = 0;
        CultureInfo culture = new CultureInfo("en-US");
        SqlConnection connection = new SqlConnection("Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234");

        public frmInvoice(string staffId)
        {
            InitializeComponent();
            this.staffId = staffId;
            connection.Open();
        }
        //load form khi chạy lần đầu
        private void frmInvoice_Load(object sender, EventArgs e)
        {
            //lấy danh sách khách hàng
            LoadCustommer();
            //load danh sách hoá đơn chưa thanh toán
            LoadInvoice();
            LoadBook();
            clearDataGVOrderDetail();
            gvOrderDetail.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
            gvOrder.IndicatorWidth = 50;
            gvOrderDetail.IndicatorWidth = 50;
          
        }

        private void LoadBook(string supplierId = "0")
        {
            try
            {
                string query;
                if (supplierId == "0")
                    query = @"SELECT
                                b.id, 
                                b.name, 
                                b.authorId, 
                                b.categoryId, 
                                b.publishCompanyId, 
                                b.supplierId, 
                                b.price, 
                                b.yearPublish, 
                                b.image,
                                ISNULL(entry_quantity, 0) - ISNULL(sold_quantity, 0) AS[quantity]
                            FROM Book b
                            LEFT JOIN(
                                SELECT
                                    bookId,
                                    SUM(quantity) AS entry_quantity
                                FROM EntrySlipDetail
                                GROUP BY bookId
                            ) AS entry_details ON b.id = entry_details.bookId
                            LEFT JOIN(
                                SELECT
                                    bookId,
                                    SUM(quantity) AS sold_quantity
                                FROM InvoiceDetail
                                GROUP BY bookId
                            ) AS sold_details ON b.id = sold_details.bookId
                            where ISNULL(entry_quantity, 0) - ISNULL(sold_quantity, 0) > 0";
                else
                    query = "select * from Book where supplierId = '" + supplierId + "'";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                lkBook.DataSource = table;
                lkBook.DisplayMember = "name";
                lkBook.ValueMember = "id";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Thông báo LoadCustommer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LoadInvoice()
        {
            string query = @"SELECT E.id AS id, S.name AS StaffName, SP.name AS CustomerName, E.createDate as createDate, E.discount as discount, E.id as id, E.isPay as isPay
                        FROM Invoice E 
                        LEFT JOIN Staff S ON E.staffId = S.id 
                        LEFT JOIN Customer SP ON E.customerId = SP.id";

            DataTable table = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
                table.Columns.Add("TotalInvoice", typeof(string));

                gcOrder.DataSource = table;

                for (int i = 0; i < gvOrder.RowCount; i++)
                {
                    gvOrder.SetRowCellValue(i, "TotalInvoice", String.Format(culture, "{0:N2}", LoadInvoiceDetail(gvOrder.GetRowCellValue(i, "id").ToString())));
                    this.discount = double.Parse(gvOrder.GetRowCellValue(i, "discount").ToString());
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Thông báo LoadInvoice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LoadCustommer()
        {
            try
            {
                string query = "Select * from Customer";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                cbbCustomer.Properties.DataSource = table;
                cbbCustomer.Properties.DisplayMember = "name";
                cbbCustomer.Properties.ValueMember = "id";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Thông báo LoadCustommer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //xoá data gridview chi tiết hoá đơn
        void clearDataGVOrderDetail()
        {
            gcOrderDetail.DataSource = null;
            layoutGroupOrderDetail.Enabled = txtTienKhachDua.Enabled = false;
            txtTienKhachDua.Text = txtTienThua.Text = txtTienPhaiTra.Text = "";

        }
        //đóng user control bán hàng
        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private double LoadInvoiceDetail(string mahd)
        {
            DataTable table = new DataTable();
            try
            {
                string query = @"SELECT * FROM InvoiceDetail WHERE invoiceId = @invoiceId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@invoiceId", mahd);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);

                table.Columns.Add("totalInvoiceDetail", typeof(string));

                gcOrderDetail.DataSource = table;

                return TongTien();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Thông báo  LoadInvoiceDetail", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return -1;
            }
        }

        private double TongTien()
        {
            double totalAmount = 0;

            double d = (100 - this.discount) / 100;
            for (int i = 0; i < gvOrderDetail.RowCount; i++)
            {
                double price = Convert.ToDouble(gvOrderDetail.GetRowCellValue(i, "price"));
                int quantity = Convert.ToInt32(gvOrderDetail.GetRowCellValue(i, "quantity"));
                
                totalAmount += price * quantity*d;
                gvOrderDetail.SetRowCellValue(i, "totalInvoiceDetail", String.Format(culture, "{0:N2}", price * quantity * d));
            }
            txtTienPhaiTra.Text = String.Format(culture, "{0:N2}", totalAmount);
            return totalAmount;
        }
        //gọi các chi tiết của 1 hoá đơn có mã hoá đơn truyền vào
        private void callDataGVOrderDetail(string mahd)
        {
            LoadInvoiceDetail(mahd);
            LoadBook();
            layoutGroupOrderDetail.Enabled = txtTienKhachDua.Enabled = true;
            layoutGroupOrderDetail.Text = "Chi tiết hoá đơn " + mahd;
        }
        //click 1 dòng trong gridview hoá đơn
        private void gvOrder_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.RowHandle > -1 && gvOrder.GetRowCellValue(e.RowHandle, "isPay").ToString() != "True")
            {
                discount = double.Parse(gvOrder.GetRowCellValue(e.RowHandle, "discount").ToString());
                callDataGVOrderDetail(gvOrder.GetRowCellValue(e.RowHandle, "id").ToString());
            }
            else
            {
                layoutGroupOrderDetail.Enabled = txtTienKhachDua.Enabled = false;
                clearDataGVOrderDetail();
            }
                
        }
        //tạo 1 hoá đơn mới cho khách hàng       
        private void btnCreateOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaHoaDon.Text.Trim() == "")
                {
                    XtraMessageBox.Show("Vui lòng nhập Hóa đơn", "Thông báo");
                    return;
                }
                if (cbbCustomer.EditValue == null)
                {
                    XtraMessageBox.Show("Vui lòng nhập chọn Khách hàng", "Thông báo");
                    return;
                }

                string idInvoice = txtMaHoaDon.Text.Trim();
                //int dis = getDiscount(cbbCustomer.EditValue.ToString());
                
                string sqlQuery = "INSERT INTO Invoice (id,staffId,createDate,customerId,discount,isPay) VALUES (@ID,@STAFFID,@CREATEDATE,@CUSTOMERID,@DISCOUNT,@ISPAY)";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@ID", idInvoice);
                    command.Parameters.AddWithValue("@STAFFID", staffId);
                    command.Parameters.Add("@CREATEDATE", SqlDbType.DateTime).Value = DateTime.Now;
                    command.Parameters.AddWithValue("@CUSTOMERID", cbbCustomer.EditValue.ToString());
                    command.Parameters.AddWithValue("@DISCOUNT", getDiscount(cbbCustomer.EditValue.ToString())); // lấy discount từ bảng typeofcustomer
                    command.Parameters.AddWithValue("@ISPAY", false);

                    command.ExecuteNonQuery();
                    XtraMessageBox.Show("Tạo hóa đơn thành công.", "Thông báo");
                    LoadInvoice();
                    gvOrder.FocusedRowHandle = gvOrder.RowCount - 2;
                    callDataGVOrderDetail(txtMaHoaDon.Text.ToString());
                }
                    
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Vui lòng nhập Mã hóa đơn không trùng nhau"+ex.Message, "Thông báo");
            }
        }

        private int getDiscount(string idCus)
        {
            string query = "SELECT tc.discount FROM Customer c INNER JOIN TypeOfCustomer tc ON c.typeOfCustomerId = tc.id WHERE c.id = @customerId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@customerId", idCus);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            string d = table.Rows[0][0].ToString();
            return int.Parse(d);
        }

        //huỷ 1 hoá đơn trong gridview hoá đơn
        private void destroyOrder()
        {
            var invoiceId = gvOrder.GetRowCellValue(gvOrder.FocusedRowHandle, "id");
            if (invoiceId != null)
            {
                if (XtraMessageBox.Show("Bạn chắc chắn huỷ hoá đơn " + invoiceId.ToString() + "?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    string query = "DELETE FROM Invoice WHERE id = @id;";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", invoiceId);
                    var i = command.ExecuteNonQuery();

                    if (i != -1)
                    {
                        XtraMessageBox.Show("Huỷ hoá đơn thành công " + invoiceId.ToString() + ".", "Thông báo");
                        LoadInvoice();
                        clearDataGVOrderDetail();
                    }
                    else
                        XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        //sự kiện gọi hàm huỷ hoá đơn
        private void btnDestroy_Click(object sender, EventArgs e)
        {
            destroyOrder();
        }
        //click nút delete xoá 1 dòng trong chi tiết hoá đơn
        private void gcOrderDetail_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && gvOrderDetail.State != GridState.Editing)
            {
                var id = gvOrderDetail.GetRowCellValue(gvOrderDetail.FocusedRowHandle, "id");
                int quantity = int.Parse(gvOrderDetail.GetRowCellValue(gvOrderDetail.FocusedRowHandle, "quantity").ToString());
                double price = double.Parse(gvOrderDetail.GetRowCellValue(gvOrderDetail.FocusedRowHandle, "price").ToString());
                if (id != null)
                {
                    if (XtraMessageBox.Show("Bạn chắc chắn xoá sách " + id + "?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        int i = DeleteInvoiceDetail(gvOrderDetail.GetRowCellValue(gvOrderDetail.FocusedRowHandle, "id").ToString());
                        if (i != -1)
                        {
                            XtraMessageBox.Show("Xoá thành công.", "Thông báo");
                            LoadInvoiceDetail(id.ToString());
                            //LoadInvoice();
                            //LoadBook();
                            int row = gvOrder.FocusedRowHandle;
                            double totalold = double.Parse(gvOrder.GetRowCellValue(row, "TotalInvoice").ToString());

                            gvOrder.SetRowCellValue(row, "TotalInvoice", String.Format(culture, "{0:N2}",totalold - price * quantity*(100-discount)/100));
                            callDataGVOrderDetail(id.ToString());
                            gvOrder.FocusedRowHandle = row;
                        }
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            else if (e.KeyCode == Keys.Enter && gvOrderDetail.State != GridState.Editing)
            {
                string id = gvOrderDetail.GetRowCellValue(gvOrderDetail.FocusedRowHandle, "id").ToString();
                string invoiceIdInsert = gvOrder.GetRowCellValue(gvOrder.FocusedRowHandle, "id").ToString();
                string bookId = gvOrderDetail.GetRowCellValue(gvOrderDetail.FocusedRowHandle, "bookId").ToString();
                int quantity = int.Parse(gvOrderDetail.GetRowCellValue(gvOrderDetail.FocusedRowHandle, "quantity").ToString());
                double price = double.Parse(gvOrderDetail.GetRowCellValue(gvOrderDetail.FocusedRowHandle, "price").ToString());


                //XtraMessageBox.Show("Insert " + id + "  " + bookId + "  " + quantity + "  " + price);
                if(checkTonKho(bookId, quantity))
                {
                    int i = InsertInvoiceDetail(id, invoiceIdInsert, bookId, quantity, price);

                    //XtraMessageBox.Show(i.ToString(), "i");

                    if (i > 0)
                        XtraMessageBox.Show("Thêm thành công", "Thông báo", DevExpress.Utils.DefaultBoolean.True);
                    int row = gvOrder.FocusedRowHandle;
                    string invoiceId = gvOrder.GetRowCellValue(row, "id").ToString();
                    double totalold = double.Parse(gvOrder.GetRowCellValue(row, "TotalInvoice").ToString());
                    double totalNew = 0;
                    if (i == 1)
                        totalNew = totalold + price * quantity * (100 - discount) / 100;
                    else if (i == 3)
                        totalNew = totalold - price * quantity;
                    gvOrder.SetRowCellValue(row, "TotalInvoice", String.Format(culture, "{0:N2}", LoadInvoiceDetail(invoiceId)));
                    //LoadInvoice();

                    callDataGVOrderDetail(invoiceId);
                    gvOrder.FocusedRowHandle = row;
                }
                else
                {
                    XtraMessageBox.Show("Số lượng không đủ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
        }

        //thanh toán 1 hoá đơn
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (txtTienPhaiTra.Text.Trim().Length == 0)
            {
                XtraMessageBox.Show("Mời bạn chọn hoá đơn muốn thanh toán.", "Thông báo");
                return;
            }
            double tienPhaiTra = double.Parse(gvOrder.GetRowCellValue(gvOrder.FocusedRowHandle, "TotalInvoice").ToString());

            if (tienPhaiTra == 0)
            {
                XtraMessageBox.Show("Hoá đơn chưa có sản phẩm không cần thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txtTienKhachDua.Text.Trim().Length == 0)
            {
                XtraMessageBox.Show("Khách chưa đưa tiền.", "Thông báo");
                return;
            }
            double tienKhachDua = double.Parse(txtTienKhachDua.Text.Trim());

            if (tienPhaiTra > tienKhachDua)
            {
                XtraMessageBox.Show("Khách đưa không đủ tiền.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string query = "UPDATE Invoice SET isPay = @isPay WHERE id = @id;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@isPay", true);
            string id = gvOrder.GetRowCellValue(gvOrder.FocusedRowHandle, "id").ToString();
            command.Parameters.AddWithValue("@id", id);
            var i = command.ExecuteNonQuery();

            if (i != -1)
            {
                XtraMessageBox.Show("Thanh toán thành công.", "Thông báo");
                txtTienThua.Text = (tienKhachDua - tienPhaiTra).ToString();
                LoadInvoice();
                clearDataGVOrderDetail();
            }
        }

        //ngăn không cho thao tác khi thêm sửa 1 dòng trong bảng cthd khi dữ liệu sai
        private void gvOrderDetail_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }
        //thêm sửa 1 dòng trong bảng chi tiết hoá đơn
        private void gvOrderDetail_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            string sErr = "";
            bool bVali = true;
            // Lấy giá trị của các cột bookId, quantity, price
            var id = gvOrderDetail.GetRowCellValue(e.RowHandle, "id").ToString().Trim();
            var bookId = gvOrderDetail.GetRowCellValue(e.RowHandle, "bookId").ToString().Trim();
            var quantityStr = gvOrderDetail.GetRowCellValue(e.RowHandle, "quantity").ToString().Trim();
            var priceStr = gvOrderDetail.GetRowCellValue(e.RowHandle, "price").ToString().Trim();

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
            else if (int.Parse(quantityStr) < 0)
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
                gvOrderDetail.SetRowCellValue(e.RowHandle, "totalInvoiceDetail", (int.Parse(quantityStr) * double.Parse(priceStr)*(100-discount)/100).ToString());
            }
        }

        //chuyển về kiểu tiền tệ khi nhập tiền vào textbox
        private void txtTienKhachDua_KeyUp(object sender, KeyEventArgs e)
        {

            decimal value;
            try
            {
                value = decimal.Parse(txtTienKhachDua.Text, NumberStyles.AllowThousands);

            }
            catch (Exception ex)
            {
                value = 0;
            }
            txtTienKhachDua.Text = String.Format(culture, "{0:N0}", value);
            txtTienKhachDua.Select(txtTienKhachDua.Text.Length, 0);
            decimal tienPhaiTra = decimal.Parse(gvOrder.GetRowCellValue(gvOrder.FocusedRowHandle, "TotalInvoice").ToString());
            txtTienThua.Text = String.Format(culture, "{0:N2}", (value - tienPhaiTra));
        }
        //không cho nhập chữ vào ô textbox
        private void txtTienKhachDua_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }
        //xoá 1 hoá đơn bằng nút delete
        private void gcOrder_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                destroyOrder();
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

        private int InsertInvoiceDetail(string id, string entrySlipId, string bookId, int quantity, double price)
        {
            try
            {
                // Kiểm tra xem có bản ghi nào đã tồn tại với cùng id hoặc bookId không
                string queryCheckExisting = "SELECT * FROM InvoiceDetail WHERE invoiceId = @invoiceId AND (id = @id OR bookId = @bookId)";

                // Sử dụng khối `using` để quản lý tài nguyên
                using (SqlCommand command = new SqlCommand(queryCheckExisting, connection))
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@bookId", bookId);
                    command.Parameters.AddWithValue("@invoiceId", entrySlipId);

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
                                    return UpdateInvoiceDetail(id, entrySlipId, bookId, quantity, price);
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
                            
                            string queryInsert = "INSERT INTO InvoiceDetail (id, invoiceId, bookId, quantity, price) VALUES (@id, @invoiceId, @bookId, @quantity, @price)";
                            command.CommandText = queryInsert;
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.AddWithValue("@invoiceId", entrySlipId);
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

        private bool checkTonKho(string idBook, int quantity)
        {
            string queryCheck = @"SELECT
                                b.id,
                                ISNULL(entry_quantity, 0) - ISNULL(sold_quantity, 0) AS[quantityCheck]
                            FROM Book b
                            LEFT JOIN(
                                SELECT
                                    bookId,
                                    SUM(quantity) AS entry_quantity
                                FROM EntrySlipDetail
                                GROUP BY bookId
                            ) AS entry_details ON b.id = entry_details.bookId
                            LEFT JOIN(
                                SELECT
                                    bookId,
                                    SUM(quantity) AS sold_quantity
                                FROM InvoiceDetail
                                GROUP BY bookId
                            ) AS sold_details ON b.id = sold_details.bookId
                            where ISNULL(entry_quantity, 0) - ISNULL(sold_quantity, 0) > 0 and b.id = @idBook";

            SqlCommand command = new SqlCommand(queryCheck, connection);
            command.Parameters.AddWithValue("@idBook", idBook);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            int tonkho = int.Parse(table.Rows[0]["quantityCheck"].ToString());
            return quantity <= tonkho;
        }

        private int UpdateInvoiceDetail(string id, string entrySlipId, string bookId, int quantity, double price)
        {
            if (quantity == 0)
            {
                return DeleteInvoiceDetail(id);
            }
            else
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE InvoiceDetail SET bookId = @bookId, quantity = @quantity, price = @price WHERE id = @id AND invoiceId = @invoiceId";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@invoiceId", entrySlipId);
                command.Parameters.AddWithValue("@bookId", bookId);
                command.Parameters.AddWithValue("@quantity", quantity);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();

                return 2;
            }

        }
        private int DeleteInvoiceDetail(string id)
        {
            try
            {
                SqlCommand command = connection.CreateCommand();

                // Xoá bản ghi dựa trên ID
                string queryDelete = "DELETE FROM InvoiceDetail WHERE id = @id";
                command.CommandText = queryDelete;
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();

                return 3; // Đánh dấu là đã xoá thành công

            }
            catch (Exception ex)
            {
                // Xử lý exception ở đây
                XtraMessageBox.Show("Error: " + ex.Message);
                return -1; // Đánh dấu là có lỗi
            }
        }

        private void lkBook_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void gvOrderDetail_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if ((e.Value == null) || e.Value.ToString() == "") return;

            if (e.Column.FieldName.ToString() == "bookId")
            {
                string bookId = gvOrderDetail.GetRowCellValue(e.RowHandle, "bookId").ToString();
                string query = "select * from Book where id = @id";

                DataTable table = new DataTable();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", bookId);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);

                gvOrderDetail.SetRowCellValue(e.RowHandle, "price", String.Format(culture, "{0:N2}", table.Rows[0]["price"]));
            }
        }

        private void frmInvoice_FormClosed(object sender, FormClosedEventArgs e)
        {
            connection.Close();
        }
    }
}
