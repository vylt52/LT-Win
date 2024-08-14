using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.Utils;
using System.Drawing;
using System.IO;
using DevExpress.XtraGrid.Views.Base;
using System.Text.RegularExpressions;

namespace GUI.FRM
{
    public partial class frmBook : Form
    {
        SqlConnection cnn = new SqlConnection("Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234");
        //private ImageCollection images = new ImageCollection(); //{ ImageSize=new Size(20, 20) };
        private OpenFileDialog open;
        private string staffId;
        private ImageList images = new ImageList();

        /*public DataTable taobang(string sql)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(sql, cnn);
            ds.Fill(dt);
            return dt;

        }*/
        public frmBook(string staffId)
        {
            InitializeComponent();
            cnn.Open();
            this.staffId = staffId;
        }
        private void LoadDuLieu()
        {
            LoadgcBook();
            LoadAuthor();
            LoadCategory();
            LoadPublishCompany();
            LoadSupplier();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadgcBook();
            loadImage();
            LoadgcAuthor();
            LoadgcCategory();
            LoadgcPublishCompany();
            LoadgcSupplier();
            LoadAuthor();
            LoadCategory();
            LoadPublishCompany();
            LoadSupplier();
            gvBook.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
            gvAuthor.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
            gvCategory.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
            gvPublishCompnay.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
            gvSupplier.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;

            gvBook.IndicatorWidth = 50;
            gvAuthor.IndicatorWidth = 50;
            gvCategory.IndicatorWidth = 50;
            gvPublishCompnay.IndicatorWidth = 50;
            gvSupplier.IndicatorWidth = 50;
        }

        private void btnDong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void loadImage()
        {
            for (int i = 0; i < gvBook.RowCount; i++)
            {
                object imageObj = gvBook.GetRowCellValue(i, "image");
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
            imageBook.Images = images;
        }


        #region Loadgc

        private void LoadgcBook()
        {

            string query = @"SELECT
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
                            ) AS sold_details ON b.id = sold_details.bookId";
            SqlCommand command = new SqlCommand(query, cnn);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            gcBook.DataSource = table;
            loadImage();
        }
        private void LoadgcAuthor()
        {
            DataTable table = new DataTable();

            string query = "Select id, name, website, note from Author";
            SqlCommand command = new SqlCommand(query, cnn);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(table);

            gcAuthor.DataSource = table;
        }
        private void LoadgcCategory()
        {
            DataTable table = new DataTable();
            string query = "Select id, name from Category";
            SqlCommand command = new SqlCommand(query, cnn);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(table);
            gcCategory.DataSource = table;
        }
        private void LoadgcPublishCompany()
        {
            DataTable table = new DataTable();
            string query = "Select id, name, address, email from PublishCompany";
            SqlCommand command = new SqlCommand(query, cnn);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(table);
            gcPublishCompany.DataSource = table;

        }
        private void LoadgcSupplier()
        {
            DataTable table = new DataTable();
            string query = "Select id, name, address, phone from Supplier";
            SqlCommand command = new SqlCommand(query, cnn);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(table);
            gcSupplier.DataSource = table;
        }

        #endregion

        #region Loadlk
        private void LoadSupplier()
        {
            DataTable table = new DataTable();
            string query = "Select * from Supplier";
            SqlCommand command = new SqlCommand(query, cnn);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(table);
            lkSupplier.DataSource = table;
            lkSupplier.DisplayMember = "name";
            lkSupplier.ValueMember = "id";

        }
        private void LoadAuthor()
        {
            DataTable table = new DataTable();
            string query = "Select * from Author";
            SqlCommand command = new SqlCommand(query, cnn);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(table);
            lkAuthor.DataSource = table;
            lkAuthor.DisplayMember = "name";
            lkAuthor.ValueMember = "id";
        }
        private void LoadCategory()
        {
            DataTable table = new DataTable();
            string query = "Select * from Category";
            SqlCommand command = new SqlCommand(query, cnn);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(table);
            lkCategory.DataSource = table;
            lkCategory.DisplayMember = "name";
            lkCategory.ValueMember = "id";


        }
        private void LoadPublishCompany()
        {
            DataTable table = new DataTable();

            string query = "Select * from PublishCompany";
            SqlCommand command = new SqlCommand(query, cnn);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(table);
            lkPublishCompany.DataSource = table;
            lkPublishCompany.DisplayMember = "name";
            lkPublishCompany.ValueMember = "id";


        }
        #endregion

        #region sách, tác giả, thể loại, nhà sản xuất, nhà cung cấp
        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                var id = gvBook.GetRowCellValue(gvBook.FocusedRowHandle, "id");
                DataRow dr = gvBook.GetFocusedDataRow();
                if (dr != null)
                {
                    if (XtraMessageBox.Show("Bạn có muốn xoá sách " + dr["name"].ToString() + " ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string sql = "DELETE FROM Book WHERE id = @id";
                        SqlCommand command = new SqlCommand(sql, cnn);
                        command.Parameters.AddWithValue("@id", id);

                        // Thực thi command xóa dữ liệu
                        int rowsAffected = command.ExecuteNonQuery();


                        if (rowsAffected != -1)
                        {
                            XtraMessageBox.Show("Xoá thành công", "Thông báo");
                            LoadgcBook();
                        }
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }

            else if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                var id = gvAuthor.GetRowCellValue(gvAuthor.FocusedRowHandle, "id");
                DataRow dr = gvAuthor.GetFocusedDataRow();
                if (dr != null)
                {
                    if (XtraMessageBox.Show("Bạn có muốn xoá tác giả " + dr["name"].ToString() + " ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string sql = "DELETE FROM Author WHERE id = @id";
                        SqlCommand command = new SqlCommand(sql, cnn);
                        command.Parameters.AddWithValue("@id", id);

                        // Thực thi command xóa dữ liệu
                        int rowsAffected = command.ExecuteNonQuery();


                        if (rowsAffected != -1)
                        {
                            XtraMessageBox.Show("Xoá thành công", "Thông báo");
                            LoadgcAuthor();
                        }
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (xtraTabControl1.SelectedTabPageIndex == 2)
            {
                var id = gvCategory.GetRowCellValue(gvCategory.FocusedRowHandle, "id");
                DataRow dr = gvCategory.GetFocusedDataRow();
                if (dr != null)
                {
                    if (XtraMessageBox.Show("Bạn có muốn xoá thể loại " + dr["name"].ToString() + " ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string sql = "DELETE FROM Category WHERE id = @id";
                        SqlCommand command = new SqlCommand(sql, cnn);
                        command.Parameters.AddWithValue("@id", id);

                        // Thực thi command xóa dữ liệu
                        int rowsAffected = command.ExecuteNonQuery();


                        if (rowsAffected != -1)
                        {
                            XtraMessageBox.Show("Xoá thành công", "Thông báo");
                            LoadgcCategory();
                        }
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (xtraTabControl1.SelectedTabPageIndex == 3)
            {
                var id = gvPublishCompnay.GetRowCellValue(gvPublishCompnay.FocusedRowHandle, "id");
                DataRow dr = gvPublishCompnay.GetFocusedDataRow();
                if (dr != null)
                {
                    if (XtraMessageBox.Show("Bạn có muốn xoá nhà sản xuất " + dr["name"].ToString() + " ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string sql = "DELETE FROM PublishCompany WHERE id = @id";
                        SqlCommand command = new SqlCommand(sql, cnn);
                        command.Parameters.AddWithValue("@id", id);

                        // Thực thi command xóa dữ liệu
                        int rowsAffected = command.ExecuteNonQuery();


                        if (rowsAffected != -1)
                        {
                            XtraMessageBox.Show("Xoá thành công", "Thông báo");
                            LoadgcPublishCompany();
                        }
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                var id = gvSupplier.GetRowCellValue(gvSupplier.FocusedRowHandle, "id");
                DataRow dr = gvSupplier.GetFocusedDataRow();
                if (dr != null)
                {
                    if (XtraMessageBox.Show("Bạn có muốn xoá nhà cung cấp " + dr["name"].ToString() + " ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string sql = "DELETE FROM Supplier WHERE id = @id";
                        SqlCommand command = new SqlCommand(sql, cnn);
                        command.Parameters.AddWithValue("@id", id);

                        // Thực thi command xóa dữ liệu
                        int rowsAffected = command.ExecuteNonQuery();


                        if (rowsAffected != -1)
                        {
                            XtraMessageBox.Show("Xoá thành công", "Thông báo");
                            LoadgcSupplier();
                        }
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        #endregion

        #region sách
        //phím delete xoá sách
        private void gcBook_ProcessGridKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && gvBook.State != GridState.Editing)
            {
                var id = gvBook.GetRowCellValue(gvBook.FocusedRowHandle, "id");
                DataRow dr = gvBook.GetFocusedDataRow();
                if (dr != null)
                {
                    if (XtraMessageBox.Show("Bạn có muốn xoá sách " + dr["name"].ToString() + " ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string sql = "DELETE FROM Book WHERE id = @id";
                        SqlCommand command = new SqlCommand(sql, cnn);
                        command.Parameters.AddWithValue("@id", id);

                        // Thực thi command xóa dữ liệu
                        int rowsAffected = command.ExecuteNonQuery();


                        if (rowsAffected != -1)
                        {
                            XtraMessageBox.Show("Xoá thành công", "Thông báo");
                            LoadgcBook();
                            LoadAuthor();
                            LoadCategory();
                            LoadPublishCompany();
                            LoadSupplier();
                        }
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        //thêm sửa
        private void gvBook_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            string sErr = "";
            bool bVali = true;
            if (gvBook.GetRowCellValue(e.RowHandle, "name").ToString().Trim() == "")
            {
                bVali = false;
                sErr = "Vui lòng điền tên sách.\n";
            }
            if (gvBook.GetRowCellValue(e.RowHandle, "authorId").ToString() == "")
            {
                bVali = false;
                sErr += "Vui lòng chọn tác giả.\n";
            }
            if (gvBook.GetRowCellValue(e.RowHandle, "categoryId").ToString() == "")
            {
                bVali = false;
                sErr += "Vui lòng chọn thể loại.\n";
            }
            if (gvBook.GetRowCellValue(e.RowHandle, "publishCompanyId").ToString() == "")
            {
                bVali = false;
                sErr += "Vui lòng chọn nhà sản xuất.\n";
            }
            if (gvBook.GetRowCellValue(e.RowHandle, "supplierId").ToString() == "")
            {
                bVali = false;
                sErr += "Vui lòng chọn nhà cung cấp.\n";
            }

            if (gvBook.GetRowCellValue(e.RowHandle, "price").ToString() == "")
            {
                bVali = false;
                sErr += "Vui lòng điền giá.\n";
            }
            if (gvBook.GetRowCellValue(e.RowHandle, "yearPublish").ToString() == "")
            {
                bVali = false;
                sErr += "Vui lòng điền năm sản xuất.\n";
            }
            if (bVali)
            {

                //thêm mới
                if (e.RowHandle < 0)
                {
                    try
                    {
                        var id = gvBook.GetRowCellValue(e.RowHandle, "id").ToString().Trim();
                        var name = gvBook.GetRowCellValue(e.RowHandle, "name").ToString().Trim();
                        var image = open == null || open.SafeFileName == null ? gvBook.GetRowCellValue(e.RowHandle, "image").ToString() : open.SafeFileName;
                        var price = double.Parse(gvBook.GetRowCellValue(e.RowHandle, "price").ToString().Trim());
                        var yearPublish = int.Parse(gvBook.GetRowCellValue(e.RowHandle, "yearPublish").ToString().Trim());
                        var categoryId = gvBook.GetRowCellValue(e.RowHandle, "categoryId").ToString().Trim();
                        var publishCompanyId = gvBook.GetRowCellValue(e.RowHandle, "publishCompanyId").ToString().Trim();
                        var authorId = gvBook.GetRowCellValue(e.RowHandle, "authorId").ToString().Trim();
                        var supplierId = gvBook.GetRowCellValue(e.RowHandle, "supplierId").ToString().Trim();

                        string query = "Insert into Book (id,name,authorId,categoryId,publishCompanyId,supplierId,price,yearPublish,image) values (@id,@name, @authorId, @categoryId, @publishComanyId,@supplierId,@price,@yearPublish, @image)";
                        using(SqlCommand command  = new SqlCommand(query, cnn))
                        {
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.AddWithValue("@name", name);
                            command.Parameters.AddWithValue("@authorId", authorId);
                            command.Parameters.AddWithValue("@categoryId", categoryId);
                            command.Parameters.AddWithValue("@publishComanyId", publishCompanyId);
                            command.Parameters.AddWithValue("@supplierId", supplierId);
                            command.Parameters.AddWithValue("@price", price);
                            command.Parameters.AddWithValue("@yearPublish", yearPublish);
                            command.Parameters.AddWithValue("@image", image);

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                XtraMessageBox.Show("Thêm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                            else
                                XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }

                        //int i = InsertBook(name, yearPublish, image, price, supplierId, authorId, publishCompanyId, categoryId);
                        open = null;
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "Thông báo catch", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    LoadgcBook();
                }
                //sửa 
                else
                {

                    try
                    {
                        var id = gvBook.GetRowCellValue(e.RowHandle, "id").ToString().Trim();
                        var name = gvBook.GetRowCellValue(e.RowHandle, "name").ToString().Trim();
                        var image = open == null || open.SafeFileName == null ? gvBook.GetRowCellValue(e.RowHandle, "image").ToString() : open.SafeFileName;
                        var price = double.Parse(gvBook.GetRowCellValue(e.RowHandle, "price").ToString().Trim());
                        var yearPublish = int.Parse(gvBook.GetRowCellValue(e.RowHandle, "yearPublish").ToString().Trim());
                        var categoryId = gvBook.GetRowCellValue(e.RowHandle, "categoryId").ToString().Trim();
                        var publishCompanyId = gvBook.GetRowCellValue(e.RowHandle, "publishCompanyId").ToString().Trim();
                        var authorId = gvBook.GetRowCellValue(e.RowHandle, "authorId").ToString().Trim();
                        var supplierId = gvBook.GetRowCellValue(e.RowHandle, "supplierId").ToString().Trim();

                        string query = @"UPDATE Book SET name = @name, yearPublish = @yearPublish, image = @image, price = @price, supplierId = @supplierId, authorId = @authorId, publishCompanyId = @publishCompanyId, categoryId = @categoryId WHERE id = @id";
                        using (SqlCommand command = new SqlCommand(query, cnn))
                        {
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.AddWithValue("@name", name);
                            command.Parameters.AddWithValue("@yearPublish", yearPublish);
                            command.Parameters.AddWithValue("@image", image);
                            command.Parameters.AddWithValue("@price", price);
                            command.Parameters.AddWithValue("@supplierId", supplierId);
                            command.Parameters.AddWithValue("@authorId", authorId);
                            command.Parameters.AddWithValue("@publishCompanyId", publishCompanyId);
                            command.Parameters.AddWithValue("@categoryId", categoryId);
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected == 0)
                                XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        //i = UpdateBook(id, name, yearPublish, image, price, supplierId, authorId, publishCompanyId, categoryId);
                        open = null;
                    }
                    catch (Exception)
                    {

                    }

                    LoadgcBook();
                }
            }
            else
            {

                e.Valid = false;

                XtraMessageBox.Show(sErr, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //đánh số thứ tự bảng sách
        private void gvBook_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (!e.Info.IsRowIndicator || e.RowHandle < 0)
                return;
            e.Info.DisplayText = (e.RowHandle + 1) + "";
        }

        //ngăn không cho chuyển dòng khi sai dữ liệu sách
        private void gvBook_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }
        private int UpdateBook(string id, string name, int yearPublish, string image, double price, int supplierId, int authorId, int publishCompanyId, int categoryId)
        {
            try
            {
                string query = @"UPDATE Book SET name = @name, yearPublish = @yearPublish, image = @image, price = @price, supplierId = @supplierId, authorId = @authorId, publishCompanyId = @publishCompanyId, categoryId = @categoryId WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, cnn))
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@yearPublish", yearPublish);
                    command.Parameters.AddWithValue("@image", image);
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@supplierId", supplierId);
                    command.Parameters.AddWithValue("@authorId", authorId);
                    command.Parameters.AddWithValue("@publishCompanyId", publishCompanyId);
                    command.Parameters.AddWithValue("@categoryId", categoryId);
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0 ? 1 : -1;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        private void gvBook_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "image")
            {
                // Kiểm tra xem có dữ liệu trong dòng hiện tại không
                if (gvBook.GetDataRow(e.RowHandle) != null)
                {
                    object imageObj = gvBook.GetDataRow(e.RowHandle)["image"];
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

                imageBook.Images = images;
            }
        }
        //Sửa hình ảnh
        private void imageBook_Click(object sender, EventArgs e)
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

                string BookId = gvBook.GetFocusedDataRow()["id"].ToString().Trim();
                string query = "UPDATE Book SET image = @image WHERE id = @id";
                using (SqlConnection connection = new SqlConnection("Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234"))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@image", open.SafeFileName);
                    command.Parameters.AddWithValue("@id", BookId);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 1)
                    {
                        // Refresh the GridView
                        LoadgcBook();
                        XtraMessageBox.Show("Sửa thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else if (rowsAffected == 0)
                    {
                        gvBook.SetFocusedRowCellValue("image", open.SafeFileName);
                    }
                    else
                    {
                        XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                open = null;
            }
        }


        #endregion

        #region tác giả
        //phím delete xoá tác giả
        private void gcAuthor_ProcessGridKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && gvAuthor.State != GridState.Editing)
            {
                var id = gvAuthor.GetRowCellValue(gvAuthor.FocusedRowHandle, "id");
                DataRow dr = gvAuthor.GetFocusedDataRow();
                if (dr != null)
                {
                    if (XtraMessageBox.Show("Bạn có muốn xoá tác giả " + dr["name"].ToString() + " ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string sql = "DELETE FROM Author WHERE id = @id";
                        SqlCommand command = new SqlCommand(sql, cnn);
                        command.Parameters.AddWithValue("@id", id);

                        // Thực thi command xóa dữ liệu
                        int rowsAffected = command.ExecuteNonQuery();


                        if (rowsAffected != -1)
                        {
                            XtraMessageBox.Show("Xoá thành công", "Thông báo");
                            LoadgcAuthor();
                            LoadDuLieu();
                        }
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        //đánh số thứ tự bảng tác giả
        private void gvAuthor_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (!e.Info.IsRowIndicator || e.RowHandle < 0)
                return;
            e.Info.DisplayText = (e.RowHandle + 1) + "";
        }
        //ngăn không cho chuyển dòng khi sai dữ liệu tác giả
        private void gvAuthor_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }
        //thểm sửa tác giả
        private void gvAuthor_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            string sErr = "";
            bool bVali = true;
            string pattern = @"^[\p-{L}\s\]+$";
            if (gvAuthor.GetRowCellValue(e.RowHandle, "id").ToString().Trim() == "")
            {
                bVali = false;
                sErr = "Vui lòng điền id tác giả.\n";
            }

            if (gvAuthor.GetRowCellValue(e.RowHandle, "name").ToString().Trim() == "")
            {
                bVali = false;
                sErr = "Vui lòng điền tên tác giả.\n";
            }
            //else if (!Regex.IsMatch(gvAuthor.GetRowCellValue(e.RowHandle, "name").ToString().Trim(), pattern))
            //{
            //    bVali = false;
            //    sErr = "Vui lòng điền tên tác giả không có ký tự đặc biệt.\n";
            //}
            if (gvAuthor.GetRowCellValue(e.RowHandle, "website").ToString() == "")
            {
                bVali = false;
                sErr += "Vui lòng chọn website.\n";
            }

            if (bVali)
            {

                //thêm mới
                if (e.RowHandle < 0)
                {
                    try
                    {
                        var id = gvAuthor.GetRowCellValue(e.RowHandle, "id").ToString().Trim();
                        var name = gvAuthor.GetRowCellValue(e.RowHandle, "name").ToString().Trim();
                        var website = gvAuthor.GetRowCellValue(e.RowHandle, "website").ToString().Trim();
                        var note = gvAuthor.GetRowCellValue(e.RowHandle, "note").ToString().Trim();


                        string checkExistingQuery = "SELECT COUNT(*) FROM Author WHERE LOWER(name) = LOWER(@name)";

                        // Kiểm tra xem tác giả đã tồn tại hay chưa
                        SqlCommand checkExistingCommand = new SqlCommand(checkExistingQuery, cnn);
                        checkExistingCommand.Parameters.AddWithValue("@name", name);
                        int existingCount = (int)checkExistingCommand.ExecuteScalar();

                        // Nếu đã tồn tại tác giả có cùng tên, trả về 0 để báo rằng không thể thêm mới tác giả
                        if (existingCount > 0)
                        {
                            XtraMessageBox.Show("Trùng tên tác giả.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }

                        string insertQuery = "INSERT INTO Author (id, name, website, note) VALUES (@id, @name, @website, @note)";
                        SqlCommand insertCommand = new SqlCommand(insertQuery, cnn);
                        insertCommand.Parameters.Clear();
                        insertCommand.Parameters.AddWithValue("@id", id);
                        insertCommand.Parameters.AddWithValue("@name", name);
                        insertCommand.Parameters.AddWithValue("@website", website);
                        insertCommand.Parameters.AddWithValue("@note", (object)note ?? DBNull.Value); // Đảm bảo note có thể là null
                        int rowsAffected = insertCommand.ExecuteNonQuery();

                        // Nếu có ít nhất một hàng được thêm mới, trả về 1 để báo rằng thêm mới tác giả thành công
                        if (rowsAffected > 0)
                        {
                            XtraMessageBox.Show("Thêm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        open = null;

                    }

                    catch (Exception)
                    {

                    }
                    LoadgcAuthor();
                    LoadDuLieu();
                }
                //sửa 
                else
                {
                    try
                    {

                        var id = gvAuthor.GetRowCellValue(e.RowHandle, "id").ToString().Trim();
                        var name = gvAuthor.GetRowCellValue(e.RowHandle, "name").ToString().Trim();
                        var website = gvAuthor.GetRowCellValue(e.RowHandle, "website").ToString().Trim();
                        var note = gvAuthor.GetRowCellValue(e.RowHandle, "note").ToString().Trim();

                        string checkQuery = "SELECT COUNT(*) FROM Author WHERE id != @id AND LOWER(name) = LOWER(@name)";
                        SqlCommand checkCommand = new SqlCommand(checkQuery, cnn);
                        checkCommand.Parameters.AddWithValue("@id", id);
                        checkCommand.Parameters.AddWithValue("@name", name);
                        int count = (int)checkCommand.ExecuteScalar();
                        if (count > 0)
                            XtraMessageBox.Show("Trùng tên tác giả.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); // Tên tác giả đã tồn tại

                        // Cập nhật thông tin tác giả
                        string updateQuery = "UPDATE Author SET name = @name, website = @website, note = @note WHERE id = @id";
                        SqlCommand updateCommand = new SqlCommand(updateQuery, cnn);
                        updateCommand.Parameters.AddWithValue("@name", name);
                        updateCommand.Parameters.AddWithValue("@website", website ?? (object)DBNull.Value);
                        updateCommand.Parameters.AddWithValue("@note", note ?? (object)DBNull.Value);
                        updateCommand.Parameters.AddWithValue("@id", id);
                        int rowsAffected = updateCommand.ExecuteNonQuery();
                        if (rowsAffected > 0)
                            XtraMessageBox.Show("Sửa thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error); // Không tìm thấy tác giả cần cập nhật

                        open = null;
                    }
                    catch (Exception)
                    {

                    }
                    LoadgcAuthor();
                    LoadDuLieu();
                }
            }
            else
            {

                e.Valid = false;

                XtraMessageBox.Show(sErr, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        #endregion

        #region thể loại
        //phím delete xoá thể loại
        private void gcCategory_ProcessGridKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && gvCategory.State != GridState.Editing)
            {
                var id = gvCategory.GetRowCellValue(gvCategory.FocusedRowHandle, "id");
                DataRow dr = gvCategory.GetFocusedDataRow();
                if (dr != null)
                {
                    if (XtraMessageBox.Show("Bạn có muốn xoá thể loại " + dr["name"].ToString() + " ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string sql = "DELETE FROM Category WHERE id = @id";
                        SqlCommand command = new SqlCommand(sql, cnn);
                        command.Parameters.AddWithValue("@id", id);

                        // Thực thi command xóa dữ liệu
                        int rowsAffected = command.ExecuteNonQuery();


                        if (rowsAffected != -1)
                        {
                            XtraMessageBox.Show("Xoá thành công", "Thông báo");
                            LoadgcCategory();
                            LoadDuLieu();
                        }
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        //đánh số thứ tự
        private void gvCategory_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (!e.Info.IsRowIndicator || e.RowHandle < 0)
                return;
            e.Info.DisplayText = (e.RowHandle + 1) + "";
        }
        //ngăn không cho chuyển dòng khi sai dữ liệu
        private void gvCategory_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }
        //thêm sửa
        private void gvCategory_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            string sErr = "";
            bool bVali = true; 
            string pattern = @"^[\p{L}\s]+$";
            if (gvCategory.GetRowCellValue(e.RowHandle, "id").ToString().Trim() == "")
            {
                bVali = false;
                sErr = "Vui lòng điền id thể loại.\n";
            }
            if (gvCategory.GetRowCellValue(e.RowHandle, "name").ToString().Trim() == "")
            {
                bVali = false;
                sErr = "Vui lòng điền tên thể loại.\n";
            }
            //else if (!Regex.IsMatch(gvCategory.GetRowCellValue(e.RowHandle, "name").ToString().Trim(), pattern))
            //{
            //    bVali = false;
            //    sErr = "Vui lòng điền tên thể loại không có ký tự đặc biệt.\n";
            //}
            if (bVali)
            {
                //thêm mới
                if (e.RowHandle < 0)
                {
                    try
                    {
                        var id = gvCategory.GetRowCellValue(e.RowHandle, "id").ToString().Trim();
                        var name = gvCategory.GetRowCellValue(e.RowHandle, "name").ToString().Trim();

                        string checkExistingQuery = "SELECT COUNT(*) FROM Category WHERE LOWER(name) = LOWER(@name)";


                        using (SqlCommand checkExistingCommand = new SqlCommand(checkExistingQuery, cnn))
                        {
                            checkExistingCommand.Parameters.AddWithValue("@name", name);
                            int existingCount = (int)checkExistingCommand.ExecuteScalar();

                            // Nếu danh mục đã tồn tại, trả về 0 để báo rằng không thể chèn danh mục mới
                            if (existingCount > 0)
                            {
                                XtraMessageBox.Show("Trùng tên thể loại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                        }
                        string insertQuery = "INSERT INTO Category (id, name) VALUES (@id, @name)";
                        // Thực thi câu truy vấn chèn danh mục mới vào cơ sở dữ liệu
                        using (SqlCommand insertCommand = new SqlCommand(insertQuery, cnn))
                        {
                            insertCommand.Parameters.AddWithValue("@id", id);
                            insertCommand.Parameters.AddWithValue("@name", name);
                            int rowsAffected = insertCommand.ExecuteNonQuery();
                            if (rowsAffected > 0)
                                XtraMessageBox.Show("Thêm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            else
                                XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        open = null;

                    }
                    catch (Exception)
                    {

                    }
                    LoadgcCategory();
                    LoadDuLieu();
                }
                //sửa 
                else
                {

                    try
                    {

                        var id = gvCategory.GetRowCellValue(e.RowHandle, "id").ToString().Trim();
                        var name = gvCategory.GetRowCellValue(e.RowHandle, "name").ToString().Trim();

                        // Tạo câu truy vấn SQL kiểm tra xem danh mục đã tồn tại hay chưa
                        string checkExistingQuery = "SELECT COUNT(*) FROM Category WHERE id != @id AND LOWER(name) = LOWER(@name)";

                        using (SqlCommand checkExistingCommand = new SqlCommand(checkExistingQuery, cnn))
                        {
                            checkExistingCommand.Parameters.AddWithValue("@id", id);
                            checkExistingCommand.Parameters.AddWithValue("@name", name);
                            int existingCount = (int)checkExistingCommand.ExecuteScalar();

                            // Nếu đã tồn tại danh mục có cùng tên, trả về 0 để báo rằng không thể cập nhật danh mục
                            if (existingCount > 0)
                            {
                                XtraMessageBox.Show("Trùng tên thể loại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                        }
                        // Tạo câu truy vấn SQL để cập nhật danh mục
                        string updateQuery = "UPDATE Category SET name = @name WHERE id = @id";

                        // Thực thi câu truy vấn cập nhật danh mục vào cơ sở dữ liệu
                        using (SqlCommand updateCommand = new SqlCommand(updateQuery, cnn))
                        {
                            updateCommand.Parameters.AddWithValue("@id", id);
                            updateCommand.Parameters.AddWithValue("@name", name);

                            int rowsAffected = updateCommand.ExecuteNonQuery();
                            if (rowsAffected > 0)
                                XtraMessageBox.Show("Sửa thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            else
                                XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }


                        open = null;
                    }
                    catch (Exception)
                    {

                    }

                    LoadgcCategory();
                    LoadDuLieu();
                }
            }
            else
            {

                e.Valid = false;

                XtraMessageBox.Show(sErr, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        #endregion

        #region nhà sản xuất
        //phím delete xoá
        private void gcPublishCompany_ProcessGridKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && gvPublishCompnay.State != GridState.Editing)
            {
                var id = gvPublishCompnay.GetRowCellValue(gvPublishCompnay.FocusedRowHandle, "id");
                DataRow dr = gvPublishCompnay.GetFocusedDataRow();
                if (dr != null)
                {
                    if (XtraMessageBox.Show("Bạn có muốn xoá nhà sản xuất " + dr["name"].ToString() + " ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string sql = "DELETE FROM PublishCompany WHERE id = @id";
                        SqlCommand command = new SqlCommand(sql, cnn);
                        command.Parameters.AddWithValue("@id", id);

                        // Thực thi command xóa dữ liệu
                        int rowsAffected = command.ExecuteNonQuery();


                        if (rowsAffected != -1)
                        {
                            XtraMessageBox.Show("Xoá thành công", "Thông báo");
                            LoadgcPublishCompany();
                            LoadDuLieu();
                        }
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        //đánh số thứ tự
        private void gvPublishCompany_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (!e.Info.IsRowIndicator || e.RowHandle < 0)
                return;
            e.Info.DisplayText = (e.RowHandle + 1) + "";
        }
        //ngăn không cho chuyển dòng khi dữ liệu sai
        private void gvPublishCompany_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }
        //thêm sửa
        private void gvPublishCompany_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            string sErr = "";
            bool bVali = true;
            //string patternName = @"^[\p{L}\s\-]+$";
            //string patternMail = @"^\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b$";
            if (gvPublishCompnay.GetRowCellValue(e.RowHandle, "name").ToString().Trim() == "")
            {
                bVali = false;
                sErr = "Vui lòng điền tên nhà sản xuất.\n";
            }
            //else if (!Regex.IsMatch(gvPublishCompnay.GetRowCellValue(e.RowHandle, "name").ToString().Trim(), patternName))
            //{
            //    bVali = false;
            //    sErr = "Vui lòng điền tên nhà sản xuất không có ký tự đặc biệt.\n";
            //}
            if (gvPublishCompnay.GetRowCellValue(e.RowHandle, "address").ToString().Trim() == "")
            {
                bVali = false;
                sErr += "Vui lòng điền địa chỉ.\n";
            }
            if (gvPublishCompnay.GetRowCellValue(e.RowHandle, "email").ToString().Trim() == "")
            {
                bVali = false;
                sErr += "Vui lòng điền email.\n";
            }
            //else if (!Regex.IsMatch(gvPublishCompnay.GetRowCellValue(e.RowHandle, "name").ToString().Trim(), patternMail))
            //{
            //    bVali = false;
            //    sErr = "Vui lòng điền mail hợp lệ.\n";
            //}
            if (bVali)
            {
                //thêm mới
                if (e.RowHandle < 0)
                {
                    try
                    {
                        var id = gvPublishCompnay.GetRowCellValue(e.RowHandle, "id").ToString().Trim();
                        var name = gvPublishCompnay.GetRowCellValue(e.RowHandle, "name").ToString().Trim();
                        var address = gvPublishCompnay.GetRowCellValue(e.RowHandle, "address").ToString().Trim();
                        var email = gvPublishCompnay.GetRowCellValue(e.RowHandle, "email").ToString().Trim();

                        string checkQuery = "SELECT COUNT(*) FROM PublishCompany WHERE LOWER(name) = LOWER(@name)";
                        using (var checkCommand = new SqlCommand(checkQuery, cnn))
                        {
                            checkCommand.Parameters.AddWithValue("@name", name);
                            int existingCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                            // Nếu tên công ty xuất bản đã tồn tại, trả về 0 để biểu thị lỗi
                            if (existingCount > 0)
                            {
                                XtraMessageBox.Show("Trùng tên nhà sản xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                LoadgcPublishCompany();
                                return;
                            }

                        }

                        // Nếu tên công ty xuất bản chưa tồn tại, thêm mới vào cơ sở dữ liệu
                        string insertQuery = "INSERT INTO PublishCompany (id, name, address, email) VALUES (@id, @name, @address, @email)";
                        using (var insertCommand = new SqlCommand(insertQuery, cnn))
                        {
                            insertCommand.Parameters.Clear();
                            insertCommand.Parameters.AddWithValue("@id", id);
                            insertCommand.Parameters.AddWithValue("@name", name);
                            insertCommand.Parameters.AddWithValue("@address", address);
                            insertCommand.Parameters.AddWithValue("@email", email);
                            int rowsAffected = insertCommand.ExecuteNonQuery();

                            // Trả về 1 nếu thêm dữ liệu thành công
                            if (rowsAffected > 0)
                                XtraMessageBox.Show("Thêm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            else
                                XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        open = null;
                    }
                    catch (Exception)
                    {

                    }
                    LoadgcPublishCompany();
                    LoadDuLieu();
                }
                //sửa 
                else
                {
                    try
                    {

                        var id = gvPublishCompnay.GetRowCellValue(e.RowHandle, "id").ToString().Trim();
                        var name = gvPublishCompnay.GetRowCellValue(e.RowHandle, "name").ToString().Trim();
                        var address = gvPublishCompnay.GetRowCellValue(e.RowHandle, "address").ToString().Trim();
                        var email = gvPublishCompnay.GetRowCellValue(e.RowHandle, "email").ToString().Trim();

                        // Kiểm tra xem có công ty xuất bản khác có cùng tên nhưng khác ID không
                        string checkQuery = "SELECT COUNT(*) FROM PublishCompany WHERE id != @id AND LOWER(name) = LOWER(@name)";
                        using (var checkCommand = new SqlCommand(checkQuery, cnn))
                        {
                            checkCommand.Parameters.AddWithValue("@id", id);
                            checkCommand.Parameters.AddWithValue("@name", name);
                            int existingCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                            // Nếu tên công ty xuất bản đã tồn tại cho một bản ghi khác có ID khác, trả về 0 để biểu thị lỗi
                            if (existingCount > 0)
                            {
                                XtraMessageBox.Show("Trùng tên nhà sản xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                LoadgcPublishCompany();
                                return;
                            }
                        }

                        // Nếu không có công ty xuất bản khác có cùng tên nhưng khác ID, tiến hành cập nhật dữ liệu
                        string updateQuery = "UPDATE PublishCompany SET name = @name, address = @address, email = @email WHERE id = @id";
                        using (var updateCommand = new SqlCommand(updateQuery, cnn))
                        {
                            updateCommand.Parameters.AddWithValue("@id", id);
                            updateCommand.Parameters.AddWithValue("@name", name);
                            updateCommand.Parameters.AddWithValue("@address", address);
                            updateCommand.Parameters.AddWithValue("@email", email);
                            int rowsAffected = updateCommand.ExecuteNonQuery();

                            // Trả về 1 nếu cập nhật dữ liệu thành công
                            if (rowsAffected > 0)
                                XtraMessageBox.Show("Sửa thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            else
                                XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        //i = UpdatePublishCompany(id, name, address, email);
                        open = null;
                    }
                    catch (Exception)
                    {

                    }
                    LoadgcPublishCompany();
                    LoadDuLieu();
                }
            }
            else
            {

                e.Valid = false;

                XtraMessageBox.Show(sErr, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        #endregion

        #region nhà cung cấp
        //phím delete xoá
        private void gcSupplier_ProcessGridKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && gvSupplier.State != GridState.Editing)
            {
                var id = gvSupplier.GetRowCellValue(gvSupplier.FocusedRowHandle, "id");
                DataRow dr = gvSupplier.GetFocusedDataRow();
                if (dr != null)
                {
                    if (XtraMessageBox.Show("Bạn có muốn xoá nhà cung cấp " + dr["name"].ToString() + " ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string sql = "DELETE FROM Supplier WHERE id = @id";
                        SqlCommand command = new SqlCommand(sql, cnn);
                        command.Parameters.AddWithValue("@id", id);

                        // Thực thi command xóa dữ liệu
                        int rowsAffected = command.ExecuteNonQuery();


                        if (rowsAffected != -1)
                        {
                            XtraMessageBox.Show("Xoá thành công", "Thông báo");
                            LoadgcSupplier();
                            LoadDuLieu();
                        }
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        //đánh số thứ tự
        private void gvSupplier_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (!e.Info.IsRowIndicator || e.RowHandle < 0)
                return;
            e.Info.DisplayText = (e.RowHandle + 1) + "";
        }
        //ngăn không cho chuyển dòng khi dữ liệu sai
        private void gvSupplier_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }
        //thêm sửa
        private void gvSupplier_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            string sErr = "";
            bool bVali = true;
            //string patternName = @"^[\p{L}\s]+$";
            //string patternPhone = @"^\d+$";
            if (gvSupplier.GetRowCellValue(e.RowHandle, "id").ToString().Trim() == "")
            {
                bVali = false;
                sErr = "Vui lòng điền id nhà cung cấp.\n";
            }

            if (gvSupplier.GetRowCellValue(e.RowHandle, "name").ToString().Trim() == "")
            {
                bVali = false;
                sErr = "Vui lòng điền tên nhà cung cấp.\n";
            }
            if (gvSupplier.GetRowCellValue(e.RowHandle, "name").ToString().Trim() == "")
            {
                bVali = false;
                sErr = "Vui lòng điền tên nhà cung cấp.\n";
            }
            if (gvSupplier.GetRowCellValue(e.RowHandle, "address").ToString().Trim() == "")
            {
                bVali = false;
                sErr = "Vui lòng điền địa chỉ.\n";
            }
            //else if (!Regex.IsMatch(gvSupplier.GetRowCellValue(e.RowHandle, "name").ToString().Trim(), patternName))
            //{
            //    bVali = false;
            //    sErr = "Vui lòng điền tên nhà cung cấp không có ký tự đặc biệt.\n";
            //}
            if (gvSupplier.GetRowCellValue(e.RowHandle, "phone").ToString().Trim() == "")
            {
                bVali = false;
                sErr = "Vui lòng điền số điện thoại.\n";
            }
            //else if (!Regex.IsMatch(gvSupplier.GetRowCellValue(e.RowHandle, "phone").ToString().Trim(), patternPhone))
            //{
            //    bVali = false;
            //    sErr = "Vui lòng điền số điện thoại hợp lệ.\n";
            //}

            if (bVali)
            {
                //thêm mới
                if (e.RowHandle < 0)
                {
                    try
                    {
                        var id = gvSupplier.GetRowCellValue(e.RowHandle, "id").ToString().Trim();
                        var name = gvSupplier.GetRowCellValue(e.RowHandle, "name").ToString().Trim();
                        var address = gvSupplier.GetRowCellValue(e.RowHandle, "address").ToString().Trim();
                        var phone = gvSupplier.GetRowCellValue(e.RowHandle, "phone").ToString().Trim();

                        string checkQuery = "SELECT COUNT(*) FROM Supplier WHERE LOWER(name) = LOWER(@name)";
                        using (var checkCommand = new SqlCommand(checkQuery, cnn))
                        {
                            checkCommand.Parameters.AddWithValue("@name", name);
                            int existingCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                            // Nếu đã tồn tại nhà cung cấp có cùng tên, trả về 0 để biểu thị lỗi
                            if (existingCount > 0)
                            {
                                XtraMessageBox.Show("Trùng tên nhà cung cấp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                LoadgcSupplier();
                                return;
                            }
                            else
                            {
                                // Nếu không có nhà cung cấp nào có cùng tên, tiến hành chèn dữ liệu vào cơ sở dữ liệu
                                string insertQuery = "INSERT INTO Supplier (id,name, address, phone) VALUES (@id, @name, @address, @phone)";
                                using (var insertCommand = new SqlCommand(insertQuery, cnn))
                                {
                                    insertCommand.Parameters.Clear();
                                    insertCommand.Parameters.AddWithValue("@id", id);
                                    insertCommand.Parameters.AddWithValue("@name", name);
                                    insertCommand.Parameters.AddWithValue("@address", address);
                                    insertCommand.Parameters.AddWithValue("@phone", phone);
                                    int rowsAffected = insertCommand.ExecuteNonQuery();

                                    // Trả về 1 nếu chèn dữ liệu thành công
                                    if (rowsAffected > 0)
                                        XtraMessageBox.Show("Thêm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                    else
                                        XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                            }
                        }

                        
                        open = null;


                    }
                    catch (Exception)
                    {

                    }
                    LoadgcSupplier();
                    LoadDuLieu();
                }
                //sửa 
                else
                {
                    try
                    {
                        var id = gvSupplier.GetRowCellValue(e.RowHandle, "id").ToString().Trim();
                        var name = gvSupplier.GetRowCellValue(e.RowHandle, "name").ToString().Trim();
                        var address = gvSupplier.GetRowCellValue(e.RowHandle, "address").ToString().Trim();
                        var phone = gvSupplier.GetRowCellValue(e.RowHandle, "phone").ToString().Trim();


                        string checkQuery = "SELECT COUNT(*) FROM Supplier WHERE id != @id AND LOWER(name) = LOWER(@name)";
                        SqlCommand checkCommand = new SqlCommand(checkQuery, cnn);
                        checkCommand.Parameters.AddWithValue("@id", id);
                        checkCommand.Parameters.AddWithValue("@name", name);
                        int existingCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                        // Nếu đã tồn tại nhà cung cấp khác có cùng tên, trả về 0 để biểu thị lỗi
                        if (existingCount > 0)
                        {
                            XtraMessageBox.Show("Trùng tên nhà cung cấp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            LoadgcSupplier();
                            return;
                        }

                        // Nếu không có nhà cung cấp khác nào có cùng tên, tiến hành cập nhật dữ liệu trong cơ sở dữ liệu
                        string updateQuery = "UPDATE Supplier SET name =@name, address = @address, phone = @phone WHERE id = @id";
                        SqlCommand updateCommand = new SqlCommand(updateQuery, cnn);
                        updateCommand.Parameters.AddWithValue("@id", id);
                        updateCommand.Parameters.AddWithValue("@name", name);
                        updateCommand.Parameters.AddWithValue("@address", address);
                        updateCommand.Parameters.AddWithValue("@phone", phone);
                        int rowsAffected = updateCommand.ExecuteNonQuery();

                        // Trả về 1 nếu cập nhật dữ liệu thành công
                        if (rowsAffected > 0)
                            XtraMessageBox.Show("Sửa thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        open = null;
                    }
                    catch (Exception)
                    {

                    }
                    LoadgcPublishCompany();
                    LoadDuLieu();
                }
            }
            else
            {

                e.Valid = false;

                XtraMessageBox.Show(sErr, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
        private void btnExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "Excel Files (*.xlsx)|*.xls";
            sf.Title = "Xuất ra file excel";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                string str = "sách";
                if (xtraTabControl1.SelectedTabPageIndex == 0)
                    gvBook.ExportToXls(sf.FileName);
                else if (xtraTabControl1.SelectedTabPageIndex == 1)
                {
                    gvAuthor.ExportToXls(sf.FileName);
                    str = "tác giả";
                }
                else if (xtraTabControl1.SelectedTabPageIndex == 2)
                {
                    gvCategory.ExportToXls(sf.FileName);
                    str = "thể loại";
                }
                else if (xtraTabControl1.SelectedTabPageIndex == 3)
                {
                    gvPublishCompnay.ExportToXls(sf.FileName);
                    str = "nhà sản xuất";
                }
                else
                {
                    gvSupplier.ExportToXls(sf.FileName);
                    str = "nhà cung cấp";
                }
                XtraMessageBox.Show("Xuất file excel " + str + " thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

        }
    }
}
