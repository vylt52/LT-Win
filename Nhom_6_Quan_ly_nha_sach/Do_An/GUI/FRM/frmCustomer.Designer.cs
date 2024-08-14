
namespace GUI.FRM
{
    partial class frmCustomer
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btnDelete_Cus = new DevExpress.XtraBars.BarButtonItem();
            this.btnDelete_TypeOfCus = new DevExpress.XtraBars.BarButtonItem();
            this.btnDong = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.btnExcel = new DevExpress.XtraBars.BarButtonItem();
            this.btnWord = new DevExpress.XtraBars.BarButtonItem();
            this.btnPdf = new DevExpress.XtraBars.BarButtonItem();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.gcCustomer = new DevExpress.XtraGrid.GridControl();
            this.gvCustomer = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.imageCustomer = new DevExpress.XtraEditors.Repository.RepositoryItemImageEdit();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkTypeOfCustomer = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.gcTypeOfCustomer = new DevExpress.XtraGrid.GridControl();
            this.gvTypeOfCustomer = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkTypeOfCustomer)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcTypeOfCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTypeOfCustomer)).BeginInit();
            this.SuspendLayout();
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnDelete_TypeOfCus,
            this.btnDong,
            this.btnExcel,
            this.btnWord,
            this.btnPdf,
            this.btnDelete_Cus});
            this.barManager1.MainMenu = this.bar1;
            this.barManager1.MaxItemId = 15;
            // 
            // bar1
            // 
            this.bar1.BarName = "Main menu";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnDelete_Cus, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnDelete_TypeOfCus),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnDong, true)});
            this.bar1.OptionsBar.MultiLine = true;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Main menu";
            // 
            // btnDelete_Cus
            // 
            this.btnDelete_Cus.Caption = "Xóa khách hàng";
            this.btnDelete_Cus.Id = 14;
            this.btnDelete_Cus.Name = "btnDelete_Cus";
            this.btnDelete_Cus.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDelete_Cus_ItemClick);
            // 
            // btnDelete_TypeOfCus
            // 
            this.btnDelete_TypeOfCus.Caption = "Xoá loại khách hàng";
            this.btnDelete_TypeOfCus.Id = 1;
            this.btnDelete_TypeOfCus.Name = "btnDelete_TypeOfCus";
            this.btnDelete_TypeOfCus.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btnDelete_TypeOfCus.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDelete_ItemClick);
            // 
            // btnDong
            // 
            this.btnDong.Caption = "Đóng";
            this.btnDong.Id = 3;
            this.btnDong.Name = "btnDong";
            this.btnDong.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btnDong.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDong_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlTop.Size = new System.Drawing.Size(823, 30);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 337);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlBottom.Size = new System.Drawing.Size(823, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 30);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 307);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(823, 30);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 307);
            // 
            // btnExcel
            // 
            this.btnExcel.Id = 11;
            this.btnExcel.Name = "btnExcel";
            // 
            // btnWord
            // 
            this.btnWord.Id = 12;
            this.btnWord.Name = "btnWord";
            // 
            // btnPdf
            // 
            this.btnPdf.Id = 13;
            this.btnPdf.Name = "btnPdf";
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 30);
            this.xtraTabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(823, 307);
            this.xtraTabControl1.TabIndex = 15;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.AutoScroll = true;
            this.xtraTabPage1.Controls.Add(this.pictureBox1);
            this.xtraTabPage1.Controls.Add(this.gcCustomer);
            this.xtraTabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(815, 274);
            this.xtraTabPage1.Text = "Khách hàng";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(425, 112);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(5, 5);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // gcCustomer
            // 
            this.gcCustomer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcCustomer.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcCustomer.Location = new System.Drawing.Point(0, 0);
            this.gcCustomer.MainView = this.gvCustomer;
            this.gcCustomer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcCustomer.MenuManager = this.barManager1;
            this.gcCustomer.Name = "gcCustomer";
            this.gcCustomer.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.lkTypeOfCustomer,
            this.imageCustomer});
            this.gcCustomer.Size = new System.Drawing.Size(815, 274);
            this.gcCustomer.TabIndex = 11;
            this.gcCustomer.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvCustomer});
            this.gcCustomer.Click += new System.EventHandler(this.gcCustomer_Click);
            // 
            // gvCustomer
            // 
            this.gvCustomer.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn2,
            this.gridColumn6,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn1,
            this.gridColumn9,
            this.gridColumn8});
            this.gvCustomer.DetailHeight = 294;
            this.gvCustomer.FixedLineWidth = 1;
            this.gvCustomer.GridControl = this.gcCustomer;
            this.gvCustomer.Name = "gvCustomer";
            this.gvCustomer.OptionsDetail.EnableMasterViewMode = false;
            this.gvCustomer.OptionsFind.AlwaysVisible = true;
            this.gvCustomer.OptionsFind.Condition = DevExpress.Data.Filtering.FilterCondition.Contains;
            this.gvCustomer.OptionsFind.FindDelay = 100;
            this.gvCustomer.OptionsFind.FindNullPrompt = "Tìm kiếm tại đây...";
            this.gvCustomer.OptionsFind.SearchInPreview = true;
            this.gvCustomer.OptionsFind.ShowFindButton = false;
            this.gvCustomer.OptionsView.ShowGroupPanel = false;
            this.gvCustomer.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gvCustomer_CustomDrawCell);
            this.gvCustomer.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gvCustomer_ValidateRow);
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Tên khách hàng";
            this.gridColumn2.FieldName = "name";
            this.gridColumn2.MinWidth = 27;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 67;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Ngày sinh";
            this.gridColumn6.FieldName = "dateOfBirth";
            this.gridColumn6.MinWidth = 27;
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 2;
            this.gridColumn6.Width = 67;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Số điện thoại";
            this.gridColumn4.FieldName = "phone";
            this.gridColumn4.MinWidth = 27;
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            this.gridColumn4.Width = 67;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Địa chỉ";
            this.gridColumn5.FieldName = "address";
            this.gridColumn5.MinWidth = 27;
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            this.gridColumn5.Width = 100;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Hình ảnh";
            this.gridColumn1.ColumnEdit = this.imageCustomer;
            this.gridColumn1.FieldName = "image";
            this.gridColumn1.MinWidth = 27;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 5;
            this.gridColumn1.Width = 100;
            // 
            // imageCustomer
            // 
            this.imageCustomer.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.True;
            this.imageCustomer.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.imageCustomer.AutoHeight = false;
            this.imageCustomer.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.OK)});
            this.imageCustomer.Name = "imageCustomer";
            this.imageCustomer.PictureStoreMode = DevExpress.XtraEditors.Controls.PictureStoreMode.Image;
            this.imageCustomer.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.Style3D;
            this.imageCustomer.ReadOnly = true;
            this.imageCustomer.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.Never;
            this.imageCustomer.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            this.imageCustomer.Click += new System.EventHandler(this.imageCustomer_Click);
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "Loại khách hàng";
            this.gridColumn9.ColumnEdit = this.lkTypeOfCustomer;
            this.gridColumn9.FieldName = "typeOfCustomerId";
            this.gridColumn9.MinWidth = 27;
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 6;
            this.gridColumn9.Width = 67;
            // 
            // lkTypeOfCustomer
            // 
            this.lkTypeOfCustomer.AutoHeight = false;
            this.lkTypeOfCustomer.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkTypeOfCustomer.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "Mã loại khách hàng"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("name", "Tên loại khách hàng")});
            this.lkTypeOfCustomer.Name = "lkTypeOfCustomer";
            this.lkTypeOfCustomer.NullText = "";
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "Mã khách hàng";
            this.gridColumn8.FieldName = "id";
            this.gridColumn8.MinWidth = 25;
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 0;
            this.gridColumn8.Width = 94;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.gcTypeOfCustomer);
            this.xtraTabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(815, 274);
            this.xtraTabPage2.Text = "Loại khách hàng";
            // 
            // gcTypeOfCustomer
            // 
            this.gcTypeOfCustomer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcTypeOfCustomer.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcTypeOfCustomer.Location = new System.Drawing.Point(0, 0);
            this.gcTypeOfCustomer.MainView = this.gvTypeOfCustomer;
            this.gcTypeOfCustomer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcTypeOfCustomer.MenuManager = this.barManager1;
            this.gcTypeOfCustomer.Name = "gcTypeOfCustomer";
            this.gcTypeOfCustomer.Size = new System.Drawing.Size(815, 274);
            this.gcTypeOfCustomer.TabIndex = 0;
            this.gcTypeOfCustomer.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvTypeOfCustomer});
            this.gcTypeOfCustomer.Click += new System.EventHandler(this.gcTypeOfCustomer_Click);
            // 
            // gvTypeOfCustomer
            // 
            this.gvTypeOfCustomer.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn11,
            this.gridColumn3,
            this.gridColumn7});
            this.gvTypeOfCustomer.DetailHeight = 294;
            this.gvTypeOfCustomer.FixedLineWidth = 1;
            this.gvTypeOfCustomer.GridControl = this.gcTypeOfCustomer;
            this.gvTypeOfCustomer.Name = "gvTypeOfCustomer";
            this.gvTypeOfCustomer.OptionsDetail.EnableMasterViewMode = false;
            this.gvTypeOfCustomer.OptionsFind.AlwaysVisible = true;
            this.gvTypeOfCustomer.OptionsFind.Condition = DevExpress.Data.Filtering.FilterCondition.Contains;
            this.gvTypeOfCustomer.OptionsFind.FindDelay = 100;
            this.gvTypeOfCustomer.OptionsFind.FindNullPrompt = "Tìm kiếm tại đây...";
            this.gvTypeOfCustomer.OptionsFind.ShowFindButton = false;
            this.gvTypeOfCustomer.OptionsView.ShowGroupPanel = false;
            this.gvTypeOfCustomer.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gvTypeOfCustomer_InvalidRowException);
            this.gvTypeOfCustomer.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gvTypeOfCustomer_ValidateRow);
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "Tên loại khách hàng";
            this.gridColumn11.FieldName = "name";
            this.gridColumn11.MinWidth = 27;
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 1;
            this.gridColumn11.Width = 100;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Giảm giá";
            this.gridColumn3.FieldName = "discount";
            this.gridColumn3.MinWidth = 27;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 100;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Mã loại khách hàng";
            this.gridColumn7.FieldName = "id";
            this.gridColumn7.MinWidth = 25;
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 0;
            this.gridColumn7.Width = 100;
            // 
            // frmCustomer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 337);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frmCustomer";
            this.Tag = "Danh sách khách hàng";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcCustomer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCustomer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCustomer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkTypeOfCustomer)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcTypeOfCustomer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTypeOfCustomer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem btnDelete_TypeOfCus;
        private DevExpress.XtraBars.BarButtonItem btnDong;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraGrid.GridControl gcCustomer;
        private DevExpress.XtraGrid.Views.Grid.GridView gvCustomer;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkTypeOfCustomer;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageEdit imageCustomer;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraGrid.GridControl gcTypeOfCustomer;
        private DevExpress.XtraGrid.Views.Grid.GridView gvTypeOfCustomer;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DevExpress.XtraBars.BarButtonItem btnExcel;
        private DevExpress.XtraBars.BarButtonItem btnWord;
        private DevExpress.XtraBars.BarButtonItem btnPdf;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraBars.BarButtonItem btnDelete_Cus;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;

    }
}