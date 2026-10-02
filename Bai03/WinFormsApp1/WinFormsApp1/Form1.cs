using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class Product
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ImagePath { get; set; }
    }

    public class Form1 : Form
    {
        private MenuStrip menuStrip;
        private ToolStripMenuItem mnuFile;
        private ToolStripMenuItem mnuExport;
        private ToolStripMenuItem mnuExit;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel tslTotal;

        private TableLayoutPanel tblMain;
        private TextBox txtProductId;
        private TextBox txtProductName;
        private TextBox txtUnitPrice;
        private TextBox txtQuantity;
        private ComboBox cboCategory;
        private PictureBox picAvatar;
        private Button btnChooseImage;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnClear;
        private ErrorProvider errorProvider;

        private TextBox txtSearch;
        private Button btnExport;
        private DataGridView dgvProducts;

        private List<Product> _all = new List<Product>();
        private BindingList<Product> _view = new BindingList<Product>();
        private BindingSource _bs = new BindingSource();
        private string _imagePath = "";
        private bool _isBinding = false;

        public Form1()
        {
            Text = "TechMart Product Manager";
            Font = new Font("Segoe UI", 10F);
            Size = new Size(1400, 800);
            MinimumSize = new Size(1000, 600);
            StartPosition = FormStartPosition.CenterScreen;

            errorProvider = new ErrorProvider();
            errorProvider.BlinkStyle = ErrorBlinkStyle.AlwaysBlink;

            BuildLayout();
            BuildStatusStrip();
            BuildMenu();
            BindData();

            Shown += Form1_Shown;
        }

        private void BuildLayout()
        {
            tblMain = new TableLayoutPanel();
            tblMain.Dock = DockStyle.Fill;
            tblMain.ColumnCount = 2;
            tblMain.RowCount = 1;
            tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblMain.Padding = new Padding(8);

            GroupBox grpInput = new GroupBox();
            grpInput.Text = "Thông tin sản phẩm";
            grpInput.Dock = DockStyle.Fill;
            grpInput.Controls.Add(BuildInputTable());

            GroupBox grpList = new GroupBox();
            grpList.Text = "Danh sách sản phẩm";
            grpList.Dock = DockStyle.Fill;
            grpList.Controls.Add(BuildRightTable());

            tblMain.Controls.Add(grpInput, 0, 0);
            tblMain.Controls.Add(grpList, 1, 0);

            Controls.Add(tblMain);
        }

        private TableLayoutPanel BuildInputTable()
        {
            TableLayoutPanel t = new TableLayoutPanel();
            t.Dock = DockStyle.Fill;
            t.Padding = new Padding(6);
            t.ColumnCount = 2;
            t.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            t.RowCount = 7;
            for (int i = 0; i < 5; i++)
            {
                t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }
            t.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            t.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            txtProductId = new TextBox();
            txtProductName = new TextBox();
            txtUnitPrice = new TextBox();
            txtQuantity = new TextBox();
            cboCategory = new ComboBox();
            cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;

            AddRow(t, 0, "Mã SP:", txtProductId);
            AddRow(t, 1, "Tên SP:", txtProductName);
            AddRow(t, 2, "Đơn giá:", txtUnitPrice);
            AddRow(t, 3, "Số lượng:", txtQuantity);
            AddRow(t, 4, "Danh mục:", cboCategory);

            btnChooseImage = new Button();
            btnChooseImage.Text = "Chọn ảnh";
            btnChooseImage.AutoSize = true;
            btnChooseImage.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnChooseImage.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnChooseImage.Padding = new Padding(6, 3, 6, 3);
            btnChooseImage.Click += btnChooseImage_Click;
            t.Controls.Add(btnChooseImage, 0, 5);

            picAvatar = new PictureBox();
            picAvatar.Dock = DockStyle.Fill;
            picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            picAvatar.BorderStyle = BorderStyle.FixedSingle;
            t.Controls.Add(picAvatar, 1, 5);

            FlowLayoutPanel tblButtons = new FlowLayoutPanel();
            tblButtons.Dock = DockStyle.Fill;
            tblButtons.AutoSize = true;
            tblButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tblButtons.WrapContents = true;

            btnAdd = CreateButton("Thêm mới");
            btnUpdate = CreateButton("Cập nhật");
            btnDelete = CreateButton("Xóa");
            btnClear = CreateButton("Làm mới");
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;

            tblButtons.Controls.Add(btnAdd);
            tblButtons.Controls.Add(btnUpdate);
            tblButtons.Controls.Add(btnDelete);
            tblButtons.Controls.Add(btnClear);

            t.Controls.Add(tblButtons, 0, 6);
            t.SetColumnSpan(tblButtons, 2);

            return t;
        }

        private TableLayoutPanel BuildRightTable()
        {
            TableLayoutPanel t = new TableLayoutPanel();
            t.Dock = DockStyle.Fill;
            t.Padding = new Padding(6);
            t.ColumnCount = 3;
            t.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            t.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            t.RowCount = 2;
            t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            t.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Label lblSearch = new Label();
            lblSearch.Text = "Tìm kiếm:";
            lblSearch.AutoSize = true;
            lblSearch.Anchor = AnchorStyles.Left;
            t.Controls.Add(lblSearch, 0, 0);

            txtSearch = new TextBox();
            txtSearch.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtSearch.TextChanged += txtSearch_TextChanged;
            t.Controls.Add(txtSearch, 1, 0);

            btnExport = CreateButton("Xuất CSV");
            btnExport.Anchor = AnchorStyles.Left;
            btnExport.Click += btnExport_Click;
            t.Controls.Add(btnExport, 2, 0);

            dgvProducts = new DataGridView();
            dgvProducts.Dock = DockStyle.Fill;
            dgvProducts.AutoGenerateColumns = false;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;
            dgvProducts.ReadOnly = true;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.BackgroundColor = Color.White;
            dgvProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvProducts.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            AddColumn("Mã SP", "ProductId", 15F, null);
            AddColumn("Tên SP", "ProductName", 35F, null);
            AddColumn("Danh Mục", "CategoryName", 18F, null);
            AddColumn("Đơn Giá (VNĐ)", "UnitPrice", 20F, "N0");
            AddColumn("Số Lượng", "Quantity", 12F, null);

            dgvProducts.SelectionChanged += dgvProducts_SelectionChanged;

            t.Controls.Add(dgvProducts, 0, 1);
            t.SetColumnSpan(dgvProducts, 3);

            return t;
        }

        private void BuildStatusStrip()
        {
            statusStrip = new StatusStrip();
            tslTotal = new ToolStripStatusLabel();
            tslTotal.Text = "Tổng số sản phẩm: 0";
            statusStrip.Items.Add(tslTotal);
            Controls.Add(statusStrip);
        }

        private void BuildMenu()
        {
            menuStrip = new MenuStrip();

            mnuFile = new ToolStripMenuItem("File");
            mnuExport = new ToolStripMenuItem("Export CSV");
            mnuExport.ShortcutKeys = Keys.Control | Keys.E;
            mnuExport.Click += btnExport_Click;

            mnuExit = new ToolStripMenuItem("Exit");
            mnuExit.ShortcutKeys = Keys.Control | Keys.X;
            mnuExit.Click += mnuExit_Click;

            mnuFile.DropDownItems.Add(mnuExport);
            mnuFile.DropDownItems.Add(mnuExit);
            menuStrip.Items.Add(mnuFile);

            MainMenuStrip = menuStrip;
            Controls.Add(menuStrip);
        }

        private void BindData()
        {
            List<Category> categories = new List<Category>();
            categories.Add(new Category(1, "Điện thoại"));
            categories.Add(new Category(2, "Laptop"));
            categories.Add(new Category(3, "Phụ kiện"));
            cboCategory.DataSource = categories;
            cboCategory.DisplayMember = "Name";
            cboCategory.ValueMember = "Id";

            _bs.DataSource = _view;
            dgvProducts.DataSource = _bs;

            _all.Add(CreateProduct("SP001", "iPhone 15", 1, "Điện thoại", 20000000m, 10));
            _all.Add(CreateProduct("SP002", "MacBook Air M2", 2, "Laptop", 28000000m, 5));
            _all.Add(CreateProduct("SP003", "Tai nghe AirPods", 3, "Phụ kiện", 4500000m, 30));
            ApplyFilter();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            ClearInputs();
            dgvProducts.ClearSelection();
        }

        private void AddRow(TableLayoutPanel t, int row, string text, Control input)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.AutoSize = true;
            lbl.Anchor = AnchorStyles.Left;
            t.Controls.Add(lbl, 0, row);

            input.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            t.Controls.Add(input, 1, row);
        }

        private Button CreateButton(string text)
        {
            Button b = new Button();
            b.Text = text;
            b.AutoSize = true;
            b.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            b.Padding = new Padding(6, 3, 6, 3);
            b.Margin = new Padding(3);
            return b;
        }

        private void AddColumn(string header, string property, float weight, string format)
        {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.HeaderText = header;
            col.DataPropertyName = property;
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            col.FillWeight = weight;
            col.MinimumWidth = 100;
            if (format != null)
            {
                col.DefaultCellStyle.Format = format;
                col.DefaultCellStyle.FormatProvider = new CultureInfo("en-US");
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            dgvProducts.Columns.Add(col);
        }

        private Product CreateProduct(string id, string name, int catId, string catName, decimal price, int qty)
        {
            Product p = new Product();
            p.ProductId = id;
            p.ProductName = name;
            p.CategoryId = catId;
            p.CategoryName = catName;
            p.UnitPrice = price;
            p.Quantity = qty;
            p.ImagePath = "";
            return p;
        }

        private void ApplyFilter()
        {
            _isBinding = true;
            string keyword = txtSearch == null ? "" : txtSearch.Text.Trim().ToLower();
            _view.Clear();
            foreach (Product p in _all)
            {
                if (keyword == "" || p.ProductName.ToLower().Contains(keyword))
                {
                    _view.Add(p);
                }
            }
            _isBinding = false;
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            tslTotal.Text = "Tổng số sản phẩm: " + _all.Count;
        }

        private Product GetSelectedProduct()
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                return null;
            }
            return dgvProducts.SelectedRows[0].DataBoundItem as Product;
        }

        private void LoadToInputs(Product p)
        {
            txtProductId.Text = p.ProductId;
            txtProductName.Text = p.ProductName;
            txtUnitPrice.Text = p.UnitPrice.ToString("0");
            txtQuantity.Text = p.Quantity.ToString();
            cboCategory.SelectedValue = p.CategoryId;
            _imagePath = p.ImagePath;
            ShowImage(_imagePath);
            errorProvider.Clear();
        }

        private void ClearInputs()
        {
            txtProductId.Clear();
            txtProductName.Clear();
            txtUnitPrice.Clear();
            txtQuantity.Text = "0";
            cboCategory.SelectedIndex = 0;
            _imagePath = "";
            ShowImage("");
            errorProvider.Clear();
            txtProductName.Focus();
        }

        private void ShowImage(string path)
        {
            if (picAvatar.Image != null)
            {
                picAvatar.Image.Dispose();
                picAvatar.Image = null;
            }

            if (path != "" && File.Exists(path))
            {
                using (Image img = Image.FromFile(path))
                {
                    picAvatar.Image = new Bitmap(img);
                }
            }
        }

        private bool IsDuplicateId(string id, Product exclude)
        {
            foreach (Product p in _all)
            {
                if (p != exclude && p.ProductId.ToLower() == id.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        private string GenerateId()
        {
            int n = _all.Count + 1;
            string id = "SP" + n.ToString("000");
            while (IsDuplicateId(id, null))
            {
                n++;
                id = "SP" + n.ToString("000");
            }
            return id;
        }

        private bool ValidateInput(Product editing)
        {
            errorProvider.Clear();
            bool ok = true;

            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                errorProvider.SetError(txtProductName, "Tên sản phẩm không được để trống!");
                ok = false;
            }

            decimal price;
            if (!decimal.TryParse(txtUnitPrice.Text.Trim(), out price) || price <= 0)
            {
                errorProvider.SetError(txtUnitPrice, "Đơn giá phải là số lớn hơn 0!");
                ok = false;
            }

            int qty;
            if (!int.TryParse(txtQuantity.Text.Trim(), out qty) || qty < 0)
            {
                errorProvider.SetError(txtQuantity, "Số lượng phải là số nguyên từ 0 trở lên!");
                ok = false;
            }

            string id = txtProductId.Text.Trim();
            if (id != "" && IsDuplicateId(id, editing))
            {
                errorProvider.SetError(txtProductId, "Mã sản phẩm đã tồn tại!");
                ok = false;
            }

            return ok;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput(null))
            {
                return;
            }

            string id = txtProductId.Text.Trim();
            if (id == "")
            {
                id = GenerateId();
            }

            Category cat = (Category)cboCategory.SelectedItem;
            Product p = CreateProduct(id, txtProductName.Text.Trim(), cat.Id, cat.Name,
                                      decimal.Parse(txtUnitPrice.Text.Trim()),
                                      int.Parse(txtQuantity.Text.Trim()));
            p.ImagePath = _imagePath;

            _all.Add(p);
            ApplyFilter();
            ClearInputs();
            dgvProducts.ClearSelection();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Product p = GetSelectedProduct();
            if (p == null)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm trên bảng để cập nhật!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!ValidateInput(p))
            {
                return;
            }

            Category cat = (Category)cboCategory.SelectedItem;
            string id = txtProductId.Text.Trim();
            if (id != "")
            {
                p.ProductId = id;
            }
            p.ProductName = txtProductName.Text.Trim();
            p.CategoryId = cat.Id;
            p.CategoryName = cat.Name;
            p.UnitPrice = decimal.Parse(txtUnitPrice.Text.Trim());
            p.Quantity = int.Parse(txtQuantity.Text.Trim());
            p.ImagePath = _imagePath;

            _bs.ResetBindings(false);
            MessageBox.Show("Cập nhật thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Product p = GetSelectedProduct();
            if (p == null)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm trên bảng để xóa!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn xóa sản phẩm \"" + p.ProductName + "\" không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _all.Remove(p);
                _view.Remove(p);
                UpdateStatus();
                if (_view.Count == 0)
                {
                    ClearInputs();
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
            dgvProducts.ClearSelection();
        }

        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Chọn ảnh sản phẩm";
            dlg.Filter = "Tệp ảnh (*.png;*.jpg;*.jpeg;*.bmp;*.gif)|*.png;*.jpg;*.jpeg;*.bmp;*.gif|Tất cả tệp (*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ShowImage(dlg.FileName);
                    _imagePath = dlg.FileName;
                }
                catch (Exception)
                {
                    MessageBox.Show("Không thể mở tệp ảnh này!", "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (_isBinding)
            {
                return;
            }

            Product p = GetSelectedProduct();
            if (p != null)
            {
                LoadToInputs(p);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private string Csv(string s)
        {
            if (s == null)
            {
                s = "";
            }
            if (s.Contains(",") || s.Contains("\"") || s.Contains("\n"))
            {
                return "\"" + s.Replace("\"", "\"\"") + "\"";
            }
            return s;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Xuất danh sách ra CSV";
            dlg.Filter = "Tệp CSV (*.csv)|*.csv";
            dlg.FileName = "DanhSachSanPham.csv";

            if (dlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                using (StreamWriter sw = new StreamWriter(dlg.FileName, false, new UTF8Encoding(true)))
                {
                    sw.WriteLine("Mã SP,Tên SP,Danh Mục,Đơn Giá,Số Lượng");
                    foreach (Product p in _all)
                    {
                        sw.WriteLine(Csv(p.ProductId) + "," + Csv(p.ProductName) + "," +
                                     Csv(p.CategoryName) + "," + p.UnitPrice.ToString("0") + "," +
                                     p.Quantity);
                    }
                }
                MessageBox.Show("Xuất file thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể ghi file: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}