using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using ClassSupport;

namespace GUI.FRM
{
    public partial class frmLogin : DevExpress.XtraEditors.XtraForm
    {
        private frmSystem frm;
        string connectionString = "Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234";
        public frmLogin(frmSystem frm)
        {
            InitializeComponent();
            this.frm = frm;
        }
        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Remember))
            {
                string[] arrStr = Properties.Settings.Default.Remember.Split('-');
                txtUsername.Text = arrStr[0];
                txtPassword.Text = arrStr[1];
                ckbRemember.Checked = true;
            }
            else
                ckbRemember.Checked = false;
        }
        private bool validateTextBox(TextEdit txt)
        {
            if (txt.Text.Trim().Length == 0)
            {
                XtraMessageBox.Show(txt.Tag + " không được rỗng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt.Focus();
                return true;
            }
            return false;
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {

            if (validateTextBox(txtUsername) || validateTextBox(txtPassword))
                return;
            splashScreenManager1.ShowWaitForm();

            SqlConnection connection = new SqlConnection(connectionString);
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();
            try
            {
                connection.Open();
                // Tạo câu truy vấn SQL để kiểm tra thông tin đăng nhập
                string query = "SELECT * FROM Staff WHERE username = @username AND password = @password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                command.Parameters.AddWithValue("@password", Support.EndCodeMD5(txtPassword.Text.Trim()));

                adapter.SelectCommand = command;
                adapter.Fill(table);

                if (table.Rows.Count > 0)
                {
                    if (ckbRemember.Checked)
                        Properties.Settings.Default.Remember = txtUsername.Text.Trim() + "-" + txtPassword.Text.Trim();
                    else
                        Properties.Settings.Default.Remember = "";
                    Properties.Settings.Default.Save();
                    frm.Hide();
                    splashScreenManager1.CloseWaitForm();
                    string role = table.Rows[0]["roleId"].ToString();
                    string staffId = table.Rows[0]["id"].ToString();
                    string staffName = table.Rows[0]["name"].ToString();

                    new frmMain(frm, role, staffId,staffName).Show();
                }
                else
                {
                    splashScreenManager1.CloseWaitForm();
                    XtraMessageBox.Show("Sai tài khoản hoặc mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    frm.setStatus("Sai tài khoản hoặc mật khẩu", Color.Red);
                }
            }
            catch (Exception ex)
            {
                //splashScreenManager1.CloseWaitForm();

                XtraMessageBox.Show("Lỗi kết nối server."+ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                frm.setStatus("Lỗi kết nối server.", Color.Red);
            }
            finally
            {
                connection.Close();
            }

        }


    }
}