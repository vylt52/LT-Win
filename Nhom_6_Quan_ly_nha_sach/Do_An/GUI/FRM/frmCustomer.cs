using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace GUI.FRM
{
    public partial class frmCustomer : Form
    {
        SqlConnection connection = new SqlConnection("Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234");
        string staffId;
        private ImageList images = new ImageList();
        public frmCustomer(string staffId)
        {
            InitializeComponent();
            connection.Open();

            GetDataGV_TypeOfCus(gcTypeOfCustomer);
            GetDataLk_TypeOfCus(lkTypeOfCustomer);
            GetDataGV_Cus(gcCustomer);
            gvCustomer.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
            gvTypeOfCustomer.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
            gvTypeOfCustomer.IndicatorWidth = 50;
            gvCustomer.IndicatorWidth = 50;
        }

        private void loadImage()
        {
            for (int i = 0; i < gvCustomer.RowCount; i++)
            {
                object imageObj = gvCustomer.GetRowCellValue(i, "image");
                if (imageObj != null && imageObj != DBNull.Value && !string.IsNullOrEmpty(imageObj.ToString()))
                {
                    string imagePath = "../../Images/" + imageObj.ToString();
                    if (File.Exists(imagePath))
                    {
                        Image img = Image.FromFile(imagePath);
                        images.Images.Clear();
                        images.ImageSize = new Size(100, 100);
                        images.Images.Add(img);
                    }
                    else
                    {
                        Image img = Image.FromFile("../../Images/loadImg.png");
                        images.Images.Clear();
                        images.ImageSize = new Size(100, 100);
                        images.Images.Add(img);
                    }
                }
                else
                {
                    Image img = Image.FromFile("../../Images/loadImg.png");
                    images.Images.Clear();
                    images.ImageSize = new Size(100, 100);
                    images.Images.Add(img);
                }
            }
            imageCustomer.Images = images;
        }

        //TypeOfCustomer 
        public void GetDataLk_TypeOfCus(RepositoryItemLookUpEdit lk)
        {
            DataTable table = new DataTable();
            {
                string query = "SELECT id, name FROM TypeOfCustomer";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);

                lk.DataSource = table;
                lk.DisplayMember = "name";
                lk.ValueMember = "id";
            }
        }
        public void GetDataLk_TypeOfCus(LookUpEdit lk)
        {
            DataTable table = new DataTable();
            {
                string query = "SELECT id, name FROM TypeOfCustomer";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);

                lk.Properties.DataSource = table;
                lk.Properties.DisplayMember = "name";
                lk.Properties.ValueMember = "id";
            }
        }
        public void GetDataGV_TypeOfCus(GridControl gv)
        {
            DataTable table = new DataTable();
            {
                string query = "SELECT * FROM TypeOfCustomer";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(table);

                gv.DataSource = table;
            }
        }
        // Thêm TypeOfCus
        public static int Insert_TypeOfCus(string id, string name, string discount)
        {
            using (SqlConnection connection = new SqlConnection("Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234"))
            {
                connection.Open();

                // Kiểm tra xem loại khách hàng đã tồn tại chưa
                string checkQuery = "SELECT COUNT(*) FROM TypeOfCustomer WHERE name = @Name";
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@Name", name);
                int count = (int)checkCommand.ExecuteScalar();
                if (count > 0)
                    return 0; // Trả về 0 nếu loại khách hàng đã tồn tại

                // Thêm loại khách hàng mới
                string insertQuery = "INSERT INTO TypeOfCustomer (id, name, discount) VALUES (@Id, @Name, @Discount)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@Id", id);
                insertCommand.Parameters.AddWithValue("@Name", name);
                insertCommand.Parameters.AddWithValue("@Discount", discount);
                try
                {
                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        return 1; // Trả về 1 nếu thêm thành công
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ
                    return -1; // Trả về -1 nếu có lỗi
                }
            }
            return -1; // Trả về -1 nếu không thể thêm dữ liệu
        }
        // Sửa TypeOfCus
        public static int Update_TypeOfCus(string id, string name, string discount)
        {
            using (SqlConnection connection = new SqlConnection("Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234"))
            {
                connection.Open();

                // Kiểm tra xem loại khách hàng với tên khác đã tồn tại chưa
                string checkQuery = "SELECT COUNT(*) FROM TypeOfCustomer WHERE id <> @Id AND name = @Name";
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@Id", id);
                checkCommand.Parameters.AddWithValue("@Name", name);
                int count = (int)checkCommand.ExecuteScalar();
                if (count > 0)
                    return 0; // Trả về 0 nếu loại khách hàng với tên khác đã tồn tại

                // Cập nhật thông tin loại khách hàng
                string updateQuery = "UPDATE TypeOfCustomer SET name = @Name, discount = @Discount WHERE id = @Id";
                SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@Id", id);
                updateCommand.Parameters.AddWithValue("@Name", name);
                updateCommand.Parameters.AddWithValue("@Discount", discount);
                try
                {
                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        return 1; // Trả về 1 nếu cập nhật thành công
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ
                    return -1; // Trả về -1 nếu có lỗi
                }
            }
            return -1; // Trả về -1 nếu không thể cập nhật dữ liệu
        }
        // Xóa TypeOfCus
        public static int Delete_TypeOfCus(string id)
        {
            using (SqlConnection connection = new SqlConnection("Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234"))
            {
                connection.Open();

                // Kiểm tra xem loại khách hàng có tồn tại không
                string checkQuery = "SELECT COUNT(*) FROM TypeOfCustomer WHERE id = @Id";
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@Id", id);
                int count = (int)checkCommand.ExecuteScalar();
                if (count == 0)
                    return -1; // Trả về -1 nếu loại khách hàng không tồn tại

                // Xóa loại khách hàng
                string deleteQuery = "DELETE FROM TypeOfCustomer WHERE id = @Id";
                SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@Id", id);
                try
                {
                    int rowsAffected = deleteCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        return 1; // Trả về 1 nếu xóa thành công
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ
                    return -1; // Trả về -1 nếu có lỗi
                }
            }
            return -1; // Trả về -1 nếu không thể xóa dữ liệu
        }

        // Thêm - Sửa Type Of Customer
        private void gvTypeOfCustomer_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            string sErr = "";
            bool bVali = true;

            // Kiểm tra tên loại khách hàng
            string idc = gvTypeOfCustomer.GetRowCellValue(e.RowHandle, "id")?.ToString().Trim();
            string name = gvTypeOfCustomer.GetRowCellValue(e.RowHandle, "name")?.ToString().Trim();
            if (string.IsNullOrEmpty(idc))
            {
                bVali = false;
                sErr = "Vui lòng điền id loại khách hàng.\n";
            }
            if (string.IsNullOrEmpty(name))
            {
                bVali = false;
                sErr = "Vui lòng điền tên loại khách hàng.\n";
            }

            // Kiểm tra giảm giá
            string discountStr = gvTypeOfCustomer.GetRowCellValue(e.RowHandle, "discount")?.ToString().Trim();
            if (string.IsNullOrEmpty(discountStr))
            {
                bVali = false;
                sErr += "Vui lòng điền giảm giá.\n";
            }
            else
            {
                int discount = int.Parse(discountStr);
                if (discount < 0 || discount > 100)
                {
                    bVali = false;
                    sErr += "Giảm giá phải từ 0-100.";
                }
            }

            if (bVali)
            {
                // Thêm mới
                if (e.RowHandle < 0)
                {
                    try
                    {
                        string id = gvTypeOfCustomer.GetRowCellValue(e.RowHandle, "id")?.ToString();
                        int i = Insert_TypeOfCus(id, name, discountStr);
                        if (i == 1)
                            XtraMessageBox.Show("Thêm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        else if (i == 0)
                            XtraMessageBox.Show("Trùng tên loại khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        // Xử lý ngoại lệ
                    }
                    GetDataGV_TypeOfCus(gcTypeOfCustomer);
                }
                // Sửa
                else
                {
                    try
                    {
                        string id = gvTypeOfCustomer.GetRowCellValue(e.RowHandle, "id")?.ToString().Trim();
                        int i = Update_TypeOfCus(id, name, discountStr);
                        if (i == -1)
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else if (i == 0)
                            XtraMessageBox.Show("Trùng tên loại khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        else if (i == 1)
                            XtraMessageBox.Show("Cập nhật thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    catch (Exception ex)
                    {
                        // Xử lý ngoại lệ
                    }
                    GetDataGV_TypeOfCus(gcTypeOfCustomer);
                }
            }
            else
            {
                e.Valid = false;
                XtraMessageBox.Show(sErr, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Click Xóa Loại khách hàng
        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataRow dr = gvTypeOfCustomer.GetFocusedDataRow();
            if (dr != null)
            {
                string name = dr["name"]?.ToString();
                string id = dr["id"]?.ToString();

                if (XtraMessageBox.Show($"Bạn có muốn xóa loại khách hàng '{name}' ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int result = Delete_TypeOfCus(id);
                    if (result == 1)
                    {
                        XtraMessageBox.Show("Xóa thành công", "Thông báo");
                        GetDataGV_TypeOfCus(gcTypeOfCustomer);
                    }
                    else
                        XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        //ngăn không cho chuyển dòng khi sai dữ liệu loại khách hàng
        private void gvTypeOfCustomer_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }
        // Customer
        public void GetDataLk_Cus(LookUpEdit lk)
        {
            DataTable table = new DataTable();
            {
                string query = "SELECT id, name FROM Customer";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);

                lk.Properties.DataSource = table;
                lk.Properties.DisplayMember = "name";
                lk.Properties.ValueMember = "id";
            }
        }
        public void GetDataLk_Cus(RepositoryItemLookUpEdit lk)
        {
            DataTable table = new DataTable();
            {
                string query = "SELECT id, name FROM Customer";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);

                lk.DataSource = table;
                lk.DisplayMember = "name";
                lk.ValueMember = "id";
            }
        }
        public void GetDataGV_Cus(GridControl gv)
        {
            DataTable table = new DataTable();
            {
                string query = "SELECT * FROM Customer";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(table);

                gv.DataSource = table;
            }
            loadImage();
        }
        // Thêm khách hàng 

        public static int Insert_Cus(string id, string name, string address, DateTime dateOfBirth, string phone, string image, string typeOfCustomerId)
        {
            using (SqlConnection connection = new SqlConnection("Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234"))
            {
                connection.Open();

                // Kiểm tra xem khách hàng đã tồn tại chưa
                string checkQuery = "SELECT COUNT(*) FROM Customer WHERE phone = @Phone";
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@Phone", phone);
                int count = (int)checkCommand.ExecuteScalar();
                if (count > 0)
                    return 0; // Trả về 0 nếu khách hàng đã tồn tại

                // Thêm khách hàng mới
                string insertQuery = "INSERT INTO Customer (id, name, address, dateOfBirth, phone, image, typeOfCustomerId) VALUES (@Id, @Name, @Address, @DateOfBirth, @Phone, @Image, @TypeOfCustomerId)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@Id", id);
                insertCommand.Parameters.AddWithValue("@Name", name);
                insertCommand.Parameters.AddWithValue("@Address", address);
                insertCommand.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                insertCommand.Parameters.AddWithValue("@Phone", phone);
                insertCommand.Parameters.AddWithValue("@Image", image);
                insertCommand.Parameters.AddWithValue("@TypeOfCustomerId", typeOfCustomerId);
                try
                {
                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        return 1; // Trả về 1 nếu thêm thành công
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ
                    return -1; // Trả về -1 nếu có lỗi
                }
            }
            return -1; // Trả về -1 nếu không thể thêm dữ liệu
        }
        // Sửa khách hàng
        public static int Update_Cus(string id, string name, string address, DateTime dateOfBirth, string phone, string image, string typeOfCustomerId)
        {
            using (SqlConnection connection = new SqlConnection("Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234"))
            {
                connection.Open();

                // Kiểm tra xem khách hàng có tồn tại không
                string checkQuery = "SELECT COUNT(*) FROM Customer WHERE id = @Id";
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@Id", id);
                int count = (int)checkCommand.ExecuteScalar();
                if (count == 0)
                    return -1; // Trả về -1 nếu khách hàng không tồn tại

                // Cập nhật thông tin khách hàng
                string updateQuery = "UPDATE Customer SET name = @Name, address = @Address, dateOfBirth = @DateOfBirth, phone = @Phone, image = @Image, typeOfCustomerId = @TypeOfCustomerId WHERE id = @Id";
                SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@Id", id);
                updateCommand.Parameters.AddWithValue("@Name", name);
                updateCommand.Parameters.AddWithValue("@Address", address);
                updateCommand.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                updateCommand.Parameters.AddWithValue("@Phone", phone);
                updateCommand.Parameters.AddWithValue("@Image", image);
                updateCommand.Parameters.AddWithValue("@TypeOfCustomerId", typeOfCustomerId);
                try
                {
                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        return 1; // Trả về 1 nếu cập nhật thành công
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ
                    return -1; // Trả về -1 nếu có lỗi
                }
            }
            return -1; // Trả về -1 nếu không thể cập nhật dữ liệu
        }

        // Xóa khách hàng
        public static int Delete_Cus(string id)
        {
            using (SqlConnection connection = new SqlConnection("Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234"))
            {
                connection.Open();

                // Kiểm tra xem khách hàng có tồn tại không
                string checkQuery = "SELECT COUNT(*) FROM Customer WHERE id = @Id";
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@Id", id);
                int count = (int)checkCommand.ExecuteScalar();
                if (count == 0)
                    return -1; // Trả về -1 nếu khách hàng không tồn tại

                // Xóa khách hàng
                string deleteQuery = "DELETE FROM Customer WHERE id = @Id";
                SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@Id", id);
                try
                {
                    int rowsAffected = deleteCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        return 1; // Trả về 1 nếu xóa thành công
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ
                    return -1; // Trả về -1 nếu có lỗi
                }
            }
            return -1; // Trả về -1 nếu không thể xóa dữ liệu
        }
        // Thêm - sửa khách hàng
        private void gvCustomer_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            string sErr = "";
            bool bVali = true;
            string pattern = @"^[\p{L}\s]+$";
            // Kiểm tra tên khách hàng
            string name = gvCustomer.GetRowCellValue(e.RowHandle, "name")?.ToString().Trim();
            if (string.IsNullOrEmpty(gvCustomer.GetRowCellValue(e.RowHandle, "id")?.ToString().Trim()))
            {
                bVali = false;
                sErr = "Vui lòng điền id khách hàng.\n";
            }
            if (string.IsNullOrEmpty(name))
            {
                bVali = false;
                sErr = "Vui lòng điền tên khách hàng.\n";
            }
            else if (!Regex.IsMatch(name, pattern))
            {
                bVali = false;
                sErr = "Vui lòng điền tên khách hàng không có ký tự đặc biệt.\n";
            }

            // Kiểm tra ngày sinh
            object dateOfBirthObj = gvCustomer.GetRowCellValue(e.RowHandle, "dateOfBirth");
            if (dateOfBirthObj == null || dateOfBirthObj == DBNull.Value)
            {
                bVali = false;
                sErr += "Vui lòng điền ngày sinh.\n";
            }
            else
            {
                DateTime dateOfBirth = DateTime.Parse(dateOfBirthObj.ToString());
                if (dateOfBirth >= DateTime.Now)
                {
                    bVali = false;
                    sErr += "Ngày sinh phải nhỏ hơn ngày hiện tại.\n";
                }
            }

            // Kiểm tra số điện thoại
            string phone = gvCustomer.GetRowCellValue(e.RowHandle, "phone")?.ToString().Trim();
            if (string.IsNullOrEmpty(phone))
            {
                bVali = false;
                sErr += "Vui lòng điền số điện thoại.\n";
            }

            // Kiểm tra địa chỉ
            string address = gvCustomer.GetRowCellValue(e.RowHandle, "address")?.ToString().Trim();
            if (string.IsNullOrEmpty(address))
            {
                bVali = false;
                sErr += "Vui lòng điền địa chỉ.\n";
            }

            // Kiểm tra loại khách hàng
            object typeOfCustomerIdObj = gvCustomer.GetRowCellValue(e.RowHandle, "typeOfCustomerId");
            if (typeOfCustomerIdObj == null || typeOfCustomerIdObj == DBNull.Value)
            {
                bVali = false;
                sErr += "Vui lòng chọn loại khách hàng.\n";
            }

            if (bVali)
            {
                // Thêm mới
                if (e.RowHandle < 0)
                {
                    try
                    {
                        string id = gvCustomer.GetRowCellValue(e.RowHandle, "id")?.ToString();
                        string image = gvCustomer.GetRowCellValue(e.RowHandle, "image")?.ToString();
                        int i = Insert_Cus(id, name, address, DateTime.Parse(dateOfBirthObj.ToString()), phone, image, typeOfCustomerIdObj.ToString());
                        if (i == 1)
                            XtraMessageBox.Show("Thêm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        else if (i == 0)
                            XtraMessageBox.Show("Số điện thoại đã tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        // Xử lý ngoại lệ
                    }
                    GetDataGV_Cus(gcCustomer);
                }
                // Sửa
                else
                {
                    try
                    {
                        string id = gvCustomer.GetRowCellValue(e.RowHandle, "id")?.ToString().Trim();
                        string image = gvCustomer.GetRowCellValue(e.RowHandle, "image")?.ToString();
                        int i = Update_Cus(id, name, address, DateTime.Parse(dateOfBirthObj.ToString()), phone, image, typeOfCustomerIdObj.ToString());
                        if (i == 1)
                            XtraMessageBox.Show("Cập nhật thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        else if (i == -1)
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        // Xử lý ngoại lệ
                    }
                    GetDataGV_Cus(gcCustomer);
                }
            }
            else
            {
                e.Valid = false;
                XtraMessageBox.Show(sErr, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnDelete_Cus_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataRow dr = gvCustomer.GetFocusedDataRow();
            if (dr != null)
            {
                string name = dr["name"]?.ToString();
                string id = dr["id"]?.ToString();

                if (XtraMessageBox.Show($"Bạn có muốn xóa khách hàng '{name}' ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int result = Delete_Cus(id);
                    if (result == 1)
                    {
                        XtraMessageBox.Show("Xóa thành công", "Thông báo");
                        GetDataGV_Cus(gcCustomer);
                    }
                    else
                        XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        //Load hình ảnh
        private void gvCustomer_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "image")
            {
                // Kiểm tra xem có dữ liệu trong dòng hiện tại không
                if (gvCustomer.GetDataRow(e.RowHandle) != null)
                {
                    object imageObj = gvCustomer.GetDataRow(e.RowHandle)["image"];
                    if (imageObj != null && imageObj != DBNull.Value && !string.IsNullOrEmpty(imageObj.ToString()))
                    {
                        string imagePath = "../../Images/" + imageObj.ToString();
                        if (File.Exists(imagePath))
                        {
                            Image img = Image.FromFile(imagePath);
                            images.Images.Clear();
                            images.ImageSize = new Size(100, 100);
                            images.Images.Add(img);
                        }
                        else
                        {
                            Image img = Image.FromFile("../../Images/loadImg.png");
                            images.Images.Clear();
                            images.ImageSize = new Size(100, 100);
                            images.Images.Add(img);
                        }
                    }
                    else
                    {
                        Image img = Image.FromFile("../../Images/loadImg.png");
                        images.Images.Clear();
                        images.ImageSize = new Size(100, 100);
                        images.Images.Add(img);
                    }
                }
                else
                {
                    Image img = Image.FromFile("../../Images/loadImg.png");
                    images.Images.Clear();
                    images.ImageSize = new Size(100, 100);
                    images.Images.Add(img);
                }

                imageCustomer.Images = images;
            }
        }
        //Sửa hình ảnh
        private void imageCustomer_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(open.FileName);
                string newImagePath = "../../Images/" + open.SafeFileName;
                if (!File.Exists(newImagePath))
                {
                    pictureBox1.Image.Save(newImagePath);
                }

                string CustomerID = gvCustomer.GetFocusedDataRow()["id"].ToString().Trim();
                string query = "UPDATE Customer SET image = @image WHERE id = @id";
                using (SqlConnection connection = new SqlConnection("Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234"))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@image", open.SafeFileName);
                    command.Parameters.AddWithValue("@id", CustomerID);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 1)
                    {
                        // Refresh the GridView
                        GetDataGV_Cus(gcCustomer);
                        XtraMessageBox.Show("Cập nhật thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                    }
                    else if (rowsAffected == 0)
                    {
                        gvCustomer.SetFocusedRowCellValue("image", open.SafeFileName);
                    }
                    else
                    {
                        XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                open = null;
            }
        }

        private void btnDong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        private void gcTypeOfCustomer_Click(object sender, EventArgs e)
        {
            btnDelete_Cus.Enabled = false;
            btnDelete_TypeOfCus.Enabled = true;
        }

        private void gcCustomer_Click(object sender, EventArgs e)
        {
            btnDelete_Cus.Enabled = true;
            btnDelete_TypeOfCus.Enabled = false;
        }
    }
}
