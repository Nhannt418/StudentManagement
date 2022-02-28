using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiThongTinSinhVien
{
    public partial class fQuanLiSinhVien : Form
    {
        public fQuanLiSinhVien()
        {
            InitializeComponent();


            //lstDanhSach.FullRowSelect = true;
        }

        List<SinhVienModel> sinhVienModels = new List<SinhVienModel>();

        int indexlv = -1;
        int stt = 1;
        Dictionary<string, string> errors = new Dictionary<string, string>();


        private void btnThem_Click(object sender, EventArgs e)
        {
            SinhVienModel sinhVien = new SinhVienModel();
            sinhVien.stt = stt;
            sinhVien.mssv = txtMSSV.Text;
            sinhVien.hoten = txtHoTen.Text;
            sinhVien.ngaysinh = dtpkNgaySinh.Text;
            sinhVien.lop = cbxLop.Text;
            sinhVien.diachi = txtDiaChi.Text;
            sinhVien.gioitinh = cbxGioiTinh.Text;
            sinhVien.sdt = txtSDT.Text;
            sinhVien.noisinh = cbxNoiSinh.Text;
            sinhVien.email = txtEmail.Text;

            ListViewItem listViewItem = new ListViewItem(new string[] { sinhVien.stt.ToString(), sinhVien.mssv, sinhVien.hoten, sinhVien.ngaysinh, sinhVien.lop, sinhVien.diachi, sinhVien.gioitinh, sinhVien.sdt, sinhVien.noisinh, sinhVien.email });

            errors.Remove("HOTEN");
            errors.Remove("DIACHI");
            KiemTraRong();

            if (errors.Count == 0)
            {
                if (!KiemTraTrung(sinhVien.mssv, sinhVien.email, sinhVien.sdt))
                {
                    lstDanhSach.Items.Add(listViewItem);
                    stt += 1;
                    MessageBox.Show("Them Thanh Cong", "Thong Bao");
                    SetDefault();
                }
                else
                {
                    MessageBox.Show("Thông tin bị trùng!", "Thong Bao");
                }
            }
            else
            {
                string errorsString = "";
                foreach (var error in errors)
                {
                    errorsString += error.Value + "\n";
                }
                MessageBox.Show(errorsString, "Thông báo");
            }
        }

        private void txtMSSV_TextChanged(object sender, EventArgs e)
        {
            errors.Remove("MSSV");
            if (txtMSSV.Text.Length != 10)
            {
                errors.Add("MSSV", "Mã số sinh viên không hợp lệ!");
                lbKiemTraMSSV.Text = errors["MSSV"];
            }
            else
            {
                lbKiemTraMSSV.Text = "";
            }

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            errors.Remove("EMAIL");

            Regex regex = new Regex(@"^\D[\w-\.]*@uef\.edu\.vn$");
            if (!regex.IsMatch(txtEmail.Text))
            {
                errors.Add("EMAIL", "Email không hợp lệ!");
                lbCheckEmail.Text = errors["EMAIL"];
            }
            else
            {
                lbCheckEmail.Text = "";
            }
        }
        private void txtSDT_TextChanged(object sender, EventArgs e)
        {
            errors.Remove("SDT");
            Regex regex = new Regex(@"^0\d{9}$");
            if (!regex.IsMatch(txtSDT.Text))
            {
                errors.Add("SDT", "SDT không hợp lệ!");
                lbCheckSDT.Text = errors["SDT"];
            }
            else
            {
                lbCheckSDT.Text = "";
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (lstDanhSach.SelectedItems != null)
            {
                var result = MessageBox.Show("Bạn có thực sự muốn xóa chương trình không ?", "Thong Bao", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    //for (int i = 0; i < lstDanhSach.Items.Count; i++)
                    //{
                    //    if (lstDanhSach.Items[i].Selected)
                    //    {
                    //        lstDanhSach.Items[i].Remove();
                    //        i--;
                    //    }
                    //}
                    lstDanhSach.Items.RemoveAt(indexlv);

                }
                else
                {
                    MessageBox.Show("không muốn xóa thì thôi", "Loi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                indexlv = -1;
                stt -= 1;
            }
        }

        private void lstDanhSach_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            indexlv = e.ItemIndex;
            Sua(indexlv);
        }

        public void SetDefault()
        {
            txtMSSV.Text = "";
            txtHoTen.Text = "";
            cbxNoiSinh.Text = "";
            cbxGioiTinh.Text = "";
            cbxLop.Text = "";
            txtEmail.Text = "";
            txtSDT.Text = "";
            txtDiaChi.Text = "";

        }

        public void Sua(int index)
        {
            int tong = lstDanhSach.Items.Count;
            int i = index;

            for (; i < tong; i++)
            {
                lstDanhSach.Items[i].SubItems[0].Text = (index++).ToString();
            }

            string[] ngay = lstDanhSach.Items[indexlv].SubItems[3].Text.Split('/');
            DateTime dateTime = new DateTime(Convert.ToInt32(ngay[2]), Convert.ToInt32(ngay[1]), Convert.ToInt32(ngay[0]));

            txtMSSV.Text = lstDanhSach.Items[indexlv].SubItems[1].Text;
            txtHoTen.Text = lstDanhSach.Items[indexlv].SubItems[2].Text;
            dtpkNgaySinh.Value = dateTime; 
            cbxLop.Text = lstDanhSach.Items[indexlv].SubItems[4].Text;
            txtDiaChi.Text = lstDanhSach.Items[indexlv].SubItems[5].Text;
            cbxGioiTinh.Text = lstDanhSach.Items[indexlv].SubItems[6].Text;
            txtSDT.Text = lstDanhSach.Items[indexlv].SubItems[7].Text;
            cbxNoiSinh.Text = lstDanhSach.Items[indexlv].SubItems[8].Text;
            txtEmail.Text = lstDanhSach.Items[indexlv].SubItems[9].Text;

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (indexlv < 0)
            {
                MessageBox.Show("Chưa chọn thong tin");
                return;
            }
            lstDanhSach.Items[indexlv].SubItems[1].Text = txtMSSV.Text;
            lstDanhSach.Items[indexlv].SubItems[2].Text = txtHoTen.Text;
            lstDanhSach.Items[indexlv].SubItems[3].Text = dtpkNgaySinh.Text;
            lstDanhSach.Items[indexlv].SubItems[4].Text = cbxLop.Text;
            lstDanhSach.Items[indexlv].SubItems[5].Text = txtDiaChi.Text;
            lstDanhSach.Items[indexlv].SubItems[6].Text = cbxGioiTinh.Text;
            lstDanhSach.Items[indexlv].SubItems[7].Text = txtSDT.Text;
            lstDanhSach.Items[indexlv].SubItems[8].Text = cbxNoiSinh.Text;
            lstDanhSach.Items[indexlv].SubItems[9].Text = txtEmail.Text;


            indexlv = -1;
            SetDefault();
            MessageBox.Show("Cap Nhat Thanh Cong", "Thong Bao");

        }

        private void cbxGioiTinh_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void fQuanLiSinhVien_Load(object sender, EventArgs e)
        {
            cbxGioiTinh.Text = cbxGioiTinh.Items[0].ToString();
            cbxNoiSinh.Text = cbxNoiSinh.Items[0].ToString();
            cbxLop.Text = cbxLop.Items[0].ToString();
            errors.Add("MSSV", "Mã số sinh viên không hợp lệ!");
            errors.Add("EMAIL", "Email không hợp lệ!");
            errors.Add("SDT", "Sdt không hợp lệ!");


            for(int i=0;i<10; i++)
            {
                SinhVienModel sinhVienModel = new SinhVienModel();
                sinhVienModel.stt = (i);
                sinhVienModel.hoten = "TanTien" + (i).ToString();
                sinhVienModel.ngaysinh = "10/" + (i).ToString() +"/2020";
                sinhVienModel.lop = (i).ToString() ;
                sinhVienModel.gioitinh = "Nam";
                sinhVienModel.mssv = "185050740" + (i).ToString();
                sinhVienModel.email = "Tien0" + (i).ToString() + "@uef.edu.vn";
                sinhVienModel.sdt = "012345678" + (i).ToString();
                sinhVienModel.diachi = "Hue" + (i).ToString();
                sinhVienModel.noisinh = "Hue" + (i).ToString();

                sinhVienModels.Add(sinhVienModel);
            }
        }


        private void fQuanLiSinhVien_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có thật sự muốn thoát chương trình?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private bool KiemTraTrung(string mssv, string email, string sdt)
        {
            return lstDanhSach.FindItemWithText(mssv) != null
                || lstDanhSach.FindItemWithText(email) != null
                || lstDanhSach.FindItemWithText(sdt) != null;
        }

        private void KiemTraRong()
        {
            if (txtHoTen.Text == "") errors.Add("HOTEN", "Điền họ tên vào bạn ei!");
            if (txtDiaChi.Text == "") errors.Add("DIACHI", "Điền địa chỉ vào bạn ei!");
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string tukhoa = txtSearch.Text;
            lstDanhSach.Items.Clear();

            List<SinhVienModel> kqtimkiem = new List<SinhVienModel>();
            kqtimkiem = sinhVienModels.Where(s => s.mssv.Contains(tukhoa) || s.sdt.Contains(tukhoa) | s.diachi.Contains(tukhoa) || s.hoten.Contains(tukhoa) || s.email.Contains(tukhoa)
            ).ToList();

            if (kqtimkiem.Count != 0)
            {
                for (int i = 0; i < kqtimkiem.Count; i++)
                {
                    SinhVienModel sinhVienModel = kqtimkiem[i];
                    ListViewItem listViewItem = new ListViewItem(new string[] { sinhVienModel.stt.ToString(), sinhVienModel.mssv.ToString(), sinhVienModel.hoten.ToString(), sinhVienModel.ngaysinh.ToString(), sinhVienModel.lop.ToString(), sinhVienModel.diachi.ToString(),
                            sinhVienModel.gioitinh.ToString(), sinhVienModel.sdt.ToString(), sinhVienModel.noisinh.ToString(), sinhVienModel.email.ToString() });

                    lstDanhSach.Items.Add(listViewItem);
                }
            }
        }

        public void TimKiem(string tuKhoa)
        {
            if(!string.IsNullOrEmpty(tuKhoa))
            {
                lstDanhSach.Items.Clear();

                List<SinhVienModel> kqtimkiem = new List<SinhVienModel>();
                kqtimkiem = sinhVienModels.Where(s => s.mssv.Contains(tuKhoa) || s.sdt.Contains(tuKhoa) | s.diachi.Contains(tuKhoa) || s.hoten.Contains(tuKhoa) || s.email.Contains(tuKhoa)).ToList();
                if (kqtimkiem.Count != 0)
                {
                    for (int i = 0; i < kqtimkiem.Count; i++)
                    {
                        SinhVienModel sinhVienModel = kqtimkiem[i];
                        ListViewItem listViewItem = new ListViewItem(new string[] { sinhVienModel.stt.ToString(), sinhVienModel.mssv.ToString(), sinhVienModel.hoten.ToString(), sinhVienModel.ngaysinh.ToString(), sinhVienModel.lop.ToString(), sinhVienModel.diachi.ToString(),
                            sinhVienModel.gioitinh.ToString(), sinhVienModel.sdt.ToString(), sinhVienModel.noisinh.ToString(), sinhVienModel.email.ToString() });

                        lstDanhSach.Items.Add(listViewItem);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui Long Nhap Thong Tin !", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string tukhoa = txtSearch.Text;
            TimKiem(tukhoa);
        }

        
    }
}
