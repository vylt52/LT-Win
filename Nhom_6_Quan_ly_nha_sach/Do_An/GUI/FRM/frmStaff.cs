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
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.FRM
{
    public partial class frmStaff : Form
    {
        SqlConnection connection = new SqlConnection("Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234");
        string staffId;
        frmMain main;
        private ImageList images = new ImageList();

        public frmStaff (string staffId, frmMain main)
        {
            InitializeComponent();
            this.staffId = staffId;
            this.main = main;
            connection.Open();

            GetDataGV_Staff(gcStaff);
            GetDataLk_Role(lkRole);
            GetDataGV_Role(gcRole);
            loadImage();
            gvStaff.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
            //gvRole.IndicatorWidth = 50;
            //gvStaff.IndicatorWidth = 50;
        }

        private void loadImage()
        {
            for (int i = 0; i < gvStaff.RowCount; i++)
            {
                object imageObj = gvStaff.GetRowCellValue(i, "image");
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
            imageStaff.Images = images;
        }

        //Staff

        public void GetDataGV_Staff(GridControl gv)
        {
            DataTable table = new DataTable();
            {
                string query = "SELECT * FROM Staff";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(table);

                gv.DataSource = table;
            }
            loadImage();
        }
        // Thêm nhân viên
        public static int Insert_Staff(string id, string name, string address, DateTime dateOfBirth, string phone, string image, string roleId, string username, string password)
        {
            using (SqlConnection connection = new SqlConnection("Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234"))
            {
                connection.Open();

                // Kiểm tra username có tồn tại chưa
                string checkUsernameQuery = "SELECT COUNT(*) FROM Staff WHERE username = @username";
                SqlCommand checkUsernameCommand = new SqlCommand(checkUsernameQuery, connection);
                checkUsernameCommand.Parameters.AddWithValue("@username", username.ToLower());
                int existingUserCount = (int)checkUsernameCommand.ExecuteScalar();
                if (existingUserCount > 0)
                    return 0;

                // Mã hóa mật khẩu mặc định là "12345"
                string encryptedPassword = EndCodeMD5("12345");

                // Thêm nhân viên mới
                string insertQuery = "INSERT INTO Staff (id, name, address, dateOfBirth, phone, image, roleId, username, password) " +
                                    "VALUES (@id, @name, @address, @dateOfBirth, @phone, @image, @roleId, @username, @password)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@id", id);
                insertCommand.Parameters.AddWithValue("@name", name);
                insertCommand.Parameters.AddWithValue("@address", address);
                insertCommand.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);
                insertCommand.Parameters.AddWithValue("@phone", phone);
                insertCommand.Parameters.AddWithValue("@image", image);
                insertCommand.Parameters.AddWithValue("@roleId", roleId);
                insertCommand.Parameters.AddWithValue("@username", username);
                insertCommand.Parameters.AddWithValue("@password", encryptedPassword);

                try
                {
                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        return 1;
                    else
                        return -1;
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }
        public static int Update_Staff(string id, string name, string address, DateTime dateOfBirth, string phone, string image, string roleId)
        {
            using (SqlConnection connection = new SqlConnection("Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234"))
            {
                connection.Open();

                // Kiểm tra xem nhân viên có tồn tại hay không
                string checkExistenceQuery = "SELECT COUNT(*) FROM Staff WHERE id = @id";
                SqlCommand checkExistenceCommand = new SqlCommand(checkExistenceQuery, connection);
                checkExistenceCommand.Parameters.AddWithValue("@id", id);
                int existingUserCount = (int)checkExistenceCommand.ExecuteScalar();
                if (existingUserCount == 0)
                    return -1;

                // Cập nhật thông tin nhân viên
                string updateQuery = "UPDATE Staff " +
                                     "SET name = @name, " +
                                         "image = @image, " +
                                         "phone = @phone, " +
                                         "roleId = @roleId, " +
                                         "address = @address, " +
                                         "dateOfBirth = @dateOfBirth " +
                                     "WHERE id = @id";
                SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@id", id);
                updateCommand.Parameters.AddWithValue("@name", name);
                updateCommand.Parameters.AddWithValue("@image", image);
                updateCommand.Parameters.AddWithValue("@phone", phone);
                updateCommand.Parameters.AddWithValue("@roleId", roleId);
                updateCommand.Parameters.AddWithValue("@address", address);
                updateCommand.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);

                try
                {
                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        return 1;
                    else
                        return -1;
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }
        public static int Delete_Staff(string id)
        {
            using (SqlConnection connection = new SqlConnection("Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234"))
            {
                connection.Open();

                // Kiểm tra xe nhân viên có tồn tại không
                string checkQuery = "SELECT COUNT(*) FROM Staff WHERE id = @Id";
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@Id", id);
                int count = (int)checkCommand.ExecuteScalar();
                if (count == 0)
                    return -1; // Trả về -1 nếu nhân viên không tồn tại

                // Xóa nhân viên
                string deleteQuery = "DELETE FROM Staff WHERE id = @Id";
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

        // Click Xoa nhân viên
        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataRow dr = gvStaff.GetFocusedDataRow();
            if (dr != null)
            {
                string name = dr["name"]?.ToString();
                string id = dr["id"]?.ToString();

                if (XtraMessageBox.Show($"Bạn có muốn xóa nhân viên '{name}' ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int result = Delete_Staff(id);
                    if (result == 1)
                    {
                        XtraMessageBox.Show("Xóa thành công", "Thông báo");
                        GetDataGV_Staff(gcStaff);
                    }
                    else
                        XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Thêm- sửa nhân viên

        private void gvStaff_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            string sErr = "";
            bool bVali = true;
            string pattern = @"^[\p{L}\s]+$";
            // Kiểm tra tên nhân viên
            string name = gvStaff.GetRowCellValue(e.RowHandle, "name")?.ToString().Trim();
            if (string.IsNullOrEmpty(name))
            {
                bVali = false;
                sErr = "Vui lòng điền tên nhân viên.\n";
            }
            else if (!Regex.IsMatch(name, pattern))
            {
                bVali = false;
                sErr = "Vui lòng điền tên nhân viên không có ký tự đặc biệt.\n";
            }

            // Kiểm tra ngày sinh
            object dateOfBirthObj = gvStaff.GetRowCellValue(e.RowHandle, "dateOfBirth");
            if (string.IsNullOrEmpty(name))
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
            string phone = gvStaff.GetRowCellValue(e.RowHandle, "phone")?.ToString().Trim();
            if (string.IsNullOrEmpty(phone))
            {
                bVali = false;
                sErr += "Vui lòng điền số điện thoại.\n";
            }

            // Kiểm tra địa chỉ
            string address = gvStaff.GetRowCellValue(e.RowHandle, "address")?.ToString().Trim();
            if (string.IsNullOrEmpty(address))
            {
                bVali = false;
                sErr += "Vui lòng điền địa chỉ.\n";
            }

            // Kiểm tra vai trò
            object roleIDObj = gvStaff.GetRowCellValue(e.RowHandle, "roleId");
            if (roleIDObj == null || roleIDObj == DBNull.Value)
            {
                bVali = false;
                sErr += "Vui lòng chọn quyền.";
            }
            // Kiểm tra username
            string username = gvStaff.GetRowCellValue(e.RowHandle, "username")?.ToString().Trim();
            if (string.IsNullOrEmpty(username))
            {
                bVali = false;
                sErr += "Vui lòng điền tài khoản\n";
            }

            if (bVali)
            {
                // Thêm mới
                if (e.RowHandle < 0)
                {
                    try
                    {
                        string id = gvStaff.GetRowCellValue(e.RowHandle, "id")?.ToString();
                        string image = gvStaff.GetRowCellValue(e.RowHandle, "image")?.ToString();
                        int i = Insert_Staff(id, name, address, DateTime.Parse(dateOfBirthObj.ToString()), phone, image, roleIDObj.ToString(), username, "12345");
                        if (i == 1)
                            XtraMessageBox.Show("Thêm thành công. Mật khẩu mặc định là 12345", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        else if (i == 0)
                            XtraMessageBox.Show("Trùng tên tài khoản", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        // Xử lý ngoại lệ
                    }
                    GetDataGV_Staff(gcStaff);
                }

                // Sửa
                else
                {
                    try
                    {
                        string id = gvStaff.GetRowCellValue(e.RowHandle, "id")?.ToString();
                        string image = gvStaff.GetRowCellValue(e.RowHandle, "image")?.ToString();
                        int i = Update_Staff(id, name, address, DateTime.Parse(dateOfBirthObj.ToString()), phone, image, roleIDObj.ToString());
                        if (i == 1)
                            XtraMessageBox.Show("Cập nhật thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        else if (i == -1)
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        // Xử lý ngoại lệ
                    }
                    GetDataGV_Staff(gcStaff);
                }
            }
            else
            {
                e.Valid = false;
                XtraMessageBox.Show(sErr, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Role
        public void GetDataLk_Role(RepositoryItemLookUpEdit lk)
        {
            DataTable table = new DataTable();
            {
                string query = "SELECT id, name FROM Role";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);

                lk.DataSource = table;
                lk.DisplayMember = "name";
                lk.ValueMember = "id";
            }
        }
        public void GetDataGV_Role(GridControl gv)
        {
            DataTable table = new DataTable();
            {
                string query = "SELECT * FROM Role";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(table);

                gv.DataSource = table;
            }
        }
        // Mã hóa password
        public static string EndCodeMD5(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        //Load hình ảnh
        private void gvStaff_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "image")
            {
                // Kiểm tra xem có dữ liệu trong dòng hiện tại không
                if (gvStaff.GetDataRow(e.RowHandle) != null)
                {
                    object imageObj = gvStaff.GetDataRow(e.RowHandle)["image"];
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

                imageStaff.Images = images;
            }
        }
        //Sửa hình ảnh
        private void imageStaff_Click(object sender, EventArgs e)
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

                string staffId = gvStaff.GetFocusedDataRow()["id"].ToString().Trim();
                string query = "UPDATE Staff SET image = @image WHERE id = @id";
                using (SqlConnection connection = new SqlConnection("Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234"))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@image", open.SafeFileName);
                    command.Parameters.AddWithValue("@id", staffId);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 1)
                    {
                        // Refresh the GridView
                        GetDataGV_Staff(gcStaff);
                        XtraMessageBox.Show("Cập nhật thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                    }
                    else if (rowsAffected == 0)
                    {
                        gvStaff.SetFocusedRowCellValue("image", open.SafeFileName);
                    }
                    else
                    {
                        XtraMessageBox.Show("Có lỗi xảy ra." + rowsAffected, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                open = null;
            }
        }

        private void btnDong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        //Reset Pass
        public int ResetPassword(string id)
        {
            using (SqlConnection connection = new SqlConnection("Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234"))
            {
                connection.Open();

                // Kiểm tra xem nhân viên có tồn tại không
                string checkQuery = "SELECT COUNT(*) FROM Staff WHERE id = @Id";
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@Id", id);
                int count = (int)checkCommand.ExecuteScalar();
                if (count == 0)
                    return -1; // Trả về -1 nếu nhân viên không tồn tại

                // Reset mật khẩu
                string resetQuery = "UPDATE Staff SET password = @Password WHERE id = @Id";
                SqlCommand resetCommand = new SqlCommand(resetQuery, connection);
                resetCommand.Parameters.AddWithValue("@Id", id);
                resetCommand.Parameters.AddWithValue("@Password", EndCodeMD5("12345"));

                try
                {
                    int rowsAffected = resetCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        return 1; // Trả về 1 nếu reset mật khẩu thành công
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ
                    return -1; // Trả về -1 nếu có lỗi
                }
            }
            return -1; // Trả về -1 nếu không thể reset mật khẩu
        }

        private void btnReset_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                DataRow dr = gvStaff.GetFocusedDataRow();
                if (dr != null)
                {
                    if (XtraMessageBox.Show("Bạn có muốn reset mật khẩu nhân viên " + dr["name"].ToString() + " ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        string id = dr["id"].ToString();
                        int i = ResetPassword(id);
                        if (i == 1)
                        {
                            if (id == staffId)
                            {
                                XtraMessageBox.Show("Reset mật khẩu thành công. Mật khẩu mới là 12345", "Thông báo");
                                XtraMessageBox.Show("Vui lòng đăng nhập lại.", "Thông báo");
                                main.checkClose = false;
                                main.Close();
                                Form system = new frmSystem();
                                system.Show();
                            }
                            else
                            {
                                XtraMessageBox.Show("Reset mật khẩu thành công. Mật khẩu mới là 12345", "Thông báo");
                                GetDataGV_Staff(gcStaff);                                                         
                            }
                        }
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
