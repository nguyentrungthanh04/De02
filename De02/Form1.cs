using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using De02.Models;
using static De02.frnSanPham;

namespace De02
{
    public partial class frnSanPham : Form
    {
        public frnSanPham()
        {
            InitializeComponent();
        }

        private void frnSanPham_Load(object sender, EventArgs e)
        {
            try
            {
                SanphamContextDB context = new SanphamContextDB();
                string maLoaiSP = txtMaSP.Text;
                string tenLoaiSP = cbLoaiSP.Text;

                List<LoaiSP> ListSP = context.LoaiSPs.ToList();
                FillcbLoaiSPCombobox(ListSP);

                List<Sanpham> listProduct = context.Sanphams.ToList();
                BindGrid(listProduct);
                btLuu.Enabled = false;
                btKLuu.Enabled = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void BindGrid(List<Sanpham> ListSanPham)
        {
            lvSanPham.Rows.Clear();
            foreach (var item in ListSanPham)
            {
                int index = lvSanPham.Rows.Add();
                lvSanPham.Rows[index].Cells[0].Value = item.MaSP;
                lvSanPham.Rows[index].Cells[1].Value = item.TenSP;
                lvSanPham.Rows[index].Cells[2].Value = item.Ngaynhap;
                if (item.MaLoai != null)
                {
                    lvSanPham.Rows[index].Cells[3].Value = item.LoaiSP.TenLoai;
                }
                else
                {
                    lvSanPham.Rows[index].Cells[3].Value = "Điện Thoại";
                }
            }
        }

        private void FillcbLoaiSPCombobox(List<LoaiSP> listLoaiSP)
        {
            this.cbLoaiSP.DataSource = listLoaiSP;
            this.cbLoaiSP.DisplayMember = "TenLoai";
            this.cbLoaiSP.ValueMember = "MaLoai";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaSP.Text) ||
                    string.IsNullOrWhiteSpace(txtTenSP.Text) ||
                    string.IsNullOrWhiteSpace(dtNgaynhap.Text))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (txtMaSP.Text.Length != 6)
                {
                    MessageBox.Show("Mã SP sinh viên phải đúng 6 ký tự. Vui lòng nhập lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SanphamContextDB db = new SanphamContextDB();
                List<Sanpham> studentList = db.Sanphams.ToList();
                if (studentList.Any(s => s.MaSP == txtMaSP.Text))
                {
                    MessageBox.Show("Mã SV đã tồn tại. Vui lòng nhập một mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var newSP = new Sanpham
                {
                    MaSP = txtMaSP.Text,
                    TenSP = txtTenSP.Text,
                    Ngaynhap = dtNgaynhap.Value,
                    MaLoai = cbLoaiSP.SelectedValue.ToString()
                };


                db.Sanphams.Add(newSP);
                db.SaveChanges();

                BindGrid(db.Sanphams.ToList());

                MessageBox.Show("Thêm San pham thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm dữ liệu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaSP.Text))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtTenSP.Text) || string.IsNullOrWhiteSpace(dtNgaynhap.Text))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SanphamContextDB db = new SanphamContextDB();

                string maSP = txtMaSP.Text;
                Sanpham sp = db.Sanphams.FirstOrDefault(p => p.MaSP == maSP);

                if (sp == null)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm với mã này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                sp.TenSP = txtTenSP.Text;
                sp.Ngaynhap = dtNgaynhap.Value;
                sp.MaLoai = cbLoaiSP.SelectedValue.ToString();

                db.SaveChanges();

                BindGrid(db.Sanphams.ToList());

                MessageBox.Show("Sửa thông tin sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sửa thông tin: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            btLuu.Enabled = true;
            btKLuu.Enabled = true;
            txtMaSP.Enabled = false;
        }

        private void lvSanPham_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = lvSanPham.Rows[e.RowIndex];

                txtMaSP.Text = row.Cells[0].Value.ToString();
                txtTenSP.Text = row.Cells[1].Value.ToString();
                dtNgaynhap.Value = Convert.ToDateTime(row.Cells[2].Value);
                cbLoaiSP.Text = row.Cells[3].Value.ToString();
            }
        }

        private void ClearText()
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            dtNgaynhap.Value = DateTime.Now;
            cbLoaiSP.SelectedIndex = -1;
        }
        private void btXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaSP.Text))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    return;
                }

                using (SanphamContextDB db = new SanphamContextDB())
                {
                    var sp = db.Sanphams.FirstOrDefault(p => p.MaSP == txtMaSP.Text);

                    if (sp != null)
                    {
                        db.Sanphams.Remove(sp);
                        db.SaveChanges();

                        BindGrid(db.Sanphams.ToList());

                        MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        ClearText();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa dữ liệu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            btLuu.Enabled = true;
            btKLuu.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string searchValue = txtTimKiem.Text.Trim();

                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    MessageBox.Show("Vui lòng nhập tên sản phẩm cần tìm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SanphamContextDB db = new SanphamContextDB())
                {
                    var searchResult = db.Sanphams
                        .Where(sp => sp.TenSP.Contains(searchValue))
                        .ToList();

                    if (searchResult.Count > 0)
                    {
                        BindGrid(searchResult);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        BindGrid(db.Sanphams.ToList());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool isAdding = false;
        private void btKLuu_Click(object sender, EventArgs e)
        {
            if (isAdding)
            {
                ClearText();
                isAdding = false;

                btLuu.Enabled = false;
                btKLuu.Enabled = false;

                MessageBox.Show("Hủy thao tác thêm sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btLuu_Click(object sender, EventArgs e)
        {
            if (isAdding)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(txtMaSP.Text) ||
                        string.IsNullOrWhiteSpace(txtTenSP.Text) ||
                        cbLoaiSP.SelectedIndex == -1)
                    {
                        MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var newRow = new DataGridViewRow();
                    newRow.CreateCells(lvSanPham);
                    newRow.Cells[0].Value = txtMaSP.Text;
                    newRow.Cells[1].Value = txtTenSP.Text;
                    newRow.Cells[2].Value = dtNgaynhap.Value.ToString("dd/MM/yyyy");
                    newRow.Cells[3].Value = cbLoaiSP.Text;

                    lvSanPham.Rows.Add(newRow);

                    SanphamContextDB db = new SanphamContextDB();
                    var newSP = new Sanpham
                    {
                        MaSP = txtMaSP.Text,
                        TenSP = txtTenSP.Text,
                        Ngaynhap = dtNgaynhap.Value,
                        MaLoai = cbLoaiSP.SelectedValue.ToString()
                    };
                    db.Sanphams.Add(newSP);
                    db.SaveChanges();

                    MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    isAdding = false;
                    btLuu.Enabled = false;
                    btKLuu.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lưu dữ liệu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
