using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;

namespace GUI.FRM
{
    public partial class frmMain : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        frmSystem frm;
        public bool checkClose;
        string role;
        string staffId;
        string staffName;
        internal object staff;

        public frmMain(frmSystem frm, string role, string staffId, string staffName)
        {
            InitializeComponent();

            this.frm = frm;
            this.role = role;
            this.staffId = staffId;
            this.staffName = staffName;
            if(role == "R01")
            {
                btnCustomerOfStaff.Visible = false;
                lbAccount.Caption = "Nhân viên: " + this.staffName;
            }
            else if (role == "R02")
            {
                btnManagerment.Visible = btnStatistical.Visible = btnRestore.Enabled = btnBackup.Enabled = false;
                lbAccount.Caption = "Nhân viên: "+ this.staffName;
            }
            checkClose = true;
        }

       
        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            openChildForm(new frmCustomer(staffId));
        }
        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            openChildForm(new frmStaff(staffId, this));
        }
        private void btnProduct_Click(object sender, EventArgs e)
        {
            openChildForm(new frmBook(staffId));
        }
        private void btnBanHang_Click(object sender, EventArgs e)
        {
            openChildForm(new frmInvoice(staffId));
        }
        private void btnNhapHang_Click(object sender, EventArgs e)
        {
            openChildForm(new frmEntrySlip(staffId, staffName));
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            openChildForm(new frmOrder(staffId, staffName));
        }

        private void btnReceipt_Click(object sender, EventArgs e)
        {
            openChildForm(new frmImport(staffId,staffName));
        }
        private void btnChangePass_ItemClick(object sender, ItemClickEventArgs e)
        {
            new frmChangePass(this, staffId).ShowDialog();
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            openChildForm(new frmInventory(staffName));
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            openChildForm(new frmHome());
        }

        private void btnTurnover_Click(object sender, EventArgs e)
        {
            openChildForm(new FrmStatistical(staffName));
        }

        private void btnCustomerOfStaff_Click(object sender, EventArgs e)
        {
            openChildForm(new frmCustomer(staffId));
        }
        private void btnBackup_ItemClick(object sender, ItemClickEventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "SQL Backup (*.bak)|*.bak";
            sf.Title = "Backup database";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                new frmBKRS(sf.FileName, 0).ShowDialog();
            }
        }

        private void btnRestore_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "SQL Backup (*.bak)|*.bak";
            op.Title = "Restore database";
            if (op.ShowDialog() == DialogResult.OK)
            {
                new frmBKRS(op.FileName, 1).ShowDialog();

            }
        }
        public void logout(int check = 0)
        {
            checkClose = false;
            if (check == 0)
            {
                if (XtraMessageBox.Show("Bạn chắc chắn muốn đăng xuất?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    Close();
                    frm._show();
                }
            }
            else
            {
                Close();
                frm._show();
            }
            checkClose = true;

        }
        private void btnLogout_ItemClick(object sender, ItemClickEventArgs e)
        {
            logout();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(checkClose)
            e.Cancel = true;
        }

        public void _close() { }

        private void fluentDesignFormControl1_Resize(object sender, EventArgs e)
        {
            accordionControl1.Dock = DockStyle.Top;
        }

        public void openChildForm(Form childForm)
        {
            splashScreenManager1.ShowWaitForm();
            //if (mainContainer != null)  mainContainer.Controls.Clear();
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            mainContainer.Controls.Add(childForm);
            mainContainer.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            splashScreenManager1.CloseWaitForm();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            openChildForm(new frmHome());
        }
    }
}
