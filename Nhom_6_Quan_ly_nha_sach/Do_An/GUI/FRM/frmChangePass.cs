using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ClassSupport;
using System.Data.SqlClient;

namespace GUI.FRM
{
    public partial class frmChangePass : DevExpress.XtraEditors.XtraForm
    {
        SqlConnection connection = new SqlConnection("Data Source =.; Initial Catalog = QLTHUVIEN; User ID = sa; Password = 1234");

        string staffId;
        frmMain frm;
        public frmChangePass(frmMain frm, string staffId)
        {
            InitializeComponent();
            this.frm = frm;
            this.staffId = staffId;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        bool validateTextBox(TextEdit txt)
        {
            if (txt.Text.Trim().Length == 0)
            {
                XtraMessageBox.Show(txt.Tag + " không được rỗng.", "Thông báo");
                txt.Focus();
                return true;
            }
            return false;
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (validateTextBox(txtOldPass) || validateTextBox(txtNewPass))
                return;
            if (txtNewPass.Text.Length < 5)
            {
                XtraMessageBox.Show("Mật khẩu từ 5 kí tự trở lên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNewPass.Focus();
                return;
            }
            if(!txtNewPass.Text.Equals(txtConfirmPass.Text))
            {
                XtraMessageBox.Show("Xác nhận mật khẩu không giống nhau.", "Thông báo",MessageBoxButtons.OK ,MessageBoxIcon.Error);
                txtConfirmPass.Focus();
                return;
            }
            connection.Open();
            string oldPass = Support.EndCodeMD5(txtOldPass.Text);
            string newPass = Support.EndCodeMD5(txtNewPass.Text);
            string query =@"UPDATE Staff
                            SET password = @NewPasswordHashed
                            WHERE id = @Id AND password = @OldPasswordHashed";
            SqlCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@Id", staffId);
            command.Parameters.AddWithValue("@NewPasswordHashed", newPass);
            command.Parameters.AddWithValue("@OldPasswordHashed", oldPass);

            int i =command.ExecuteNonQuery();
            connection.Close();
            if (i <= 0)
            { 
                XtraMessageBox.Show("Mật khẩu cũ không chính xác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtOldPass.Focus();
            }
            else
            {
                XtraMessageBox.Show("Đổi mật khẩu thành công.Vui lòng đăng nhập lại", "Thông báo");
                frm.logout(1);
            }
        }
    }
}