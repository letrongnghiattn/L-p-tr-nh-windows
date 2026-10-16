using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Quanlythongtinhocvien
{
    public partial class Form1 : Form
    {
        // Dữ liệu khóa học: tên khóa học -> học phí / tháng (VNĐ)
        private readonly Dictionary<string, decimal> dsKhoaHoc = new Dictionary<string, decimal>
        {
            { "C# WinForms cơ bản",      800000m },
            { "SQL Server cơ bản",       700000m },
            { "Web Frontend cơ bản",     750000m },
            { "Lập trình Python cơ bản", 650000m }
        };

        private readonly CultureInfo viVN = new CultureInfo("vi-VN");

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;   // gắn sự kiện Form Load
        }

        // ===================== 5.1. FORM LOAD =====================
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "ĐĂNG KÝ KHÓA HỌC";

            textBox3.ReadOnly = true;   // ô tổng tiền để hiển thị

            // Nạp danh sách khóa học vào ComboBox
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Items.Clear();
            foreach (string tenKhoa in dsKhoaHoc.Keys)
                comboBox1.Items.Add(tenKhoa);

            // Mặc định: khóa học đầu tiên, hình thức Online
            comboBox1.SelectedIndex = 0;
            radioButton1.Checked = true;

            // Số tháng: tối thiểu 1, tối đa 12
            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = 12;
            numericUpDown1.Value = 1;

            CapNhatTongTien();
        }

        // ===================== TÍNH HỌC PHÍ =====================
        private decimal TinhTongTien()
        {
            string tenKhoa = comboBox1.SelectedItem as string;
            if (tenKhoa == null || !dsKhoaHoc.ContainsKey(tenKhoa))
                return 0m;
            return dsKhoaHoc[tenKhoa] * numericUpDown1.Value;   // học phí 1 tháng x số tháng
        }

        private void CapNhatTongTien()
        {
            // textBox3 hiển thị tổng tiền (chỉ đọc, người dùng không nhập được)
            textBox3.Text = TinhTongTien().ToString("N0", viVN) + " VNĐ";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CapNhatTongTien();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            CapNhatTongTien();
        }

        // ===================== 5.2. NÚT ĐĂNG KÝ =====================
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên học viên!", "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return;
            }

            if (comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn khóa học!", "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox1.Focus();
                return;
            }

            string phieu =
                "Họ tên: " + textBox1.Text.Trim() + Environment.NewLine +
                "Số điện thoại: " + textBox2.Text.Trim() + Environment.NewLine +
                "Ngày sinh: " + dateTimePicker1.Value.ToString("dd/MM/yyyy") + Environment.NewLine +
                "Khóa học: " + comboBox1.SelectedItem + Environment.NewLine +
                "Hình thức học: " + (radioButton1.Checked ? "Online" : "Offline") + Environment.NewLine +
                "Số tháng đăng ký: " + (int)numericUpDown1.Value + Environment.NewLine +
                "Tổng tiền: " + TinhTongTien().ToString("N0", viVN) + " VNĐ" + Environment.NewLine +
                "Nhận email thông báo: " + (checkBox1.Checked ? "Có" : "Không");

            MessageBox.Show(phieu, "PHIẾU ĐĂNG KÝ KHÓA HỌC",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ===================== 5.3. NÚT LÀM MỚI =====================
        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            dateTimePicker1.Value = DateTime.Now;
            checkBox1.Checked = false;
            comboBox1.SelectedIndex = 0;
            radioButton1.Checked = true;
            numericUpDown1.Value = 1;
            CapNhatTongTien();
            textBox1.Focus();
        }

        // ===================== 5.4. NÚT THOÁT =====================
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult kq = MessageBox.Show("Bạn có chắc chắn muốn thoát chương trình?",
                "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (kq == DialogResult.Yes)
                this.Close();
        }

        // ===== Các hàm sự kiện có sẵn từ Designer (giữ nguyên, để trống) =====
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void checkBox1_CheckedChanged(object sender, EventArgs e) { }
        private void radioButton1_CheckedChanged(object sender, EventArgs e) { }
        private void radioButton2_CheckedChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }

        private void textBox3_TextChanged(object sender, EventArgs e) { }
    }
}