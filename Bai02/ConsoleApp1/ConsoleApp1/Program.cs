using System;
using System.Collections.Generic;
using System.Text;

namespace AutoSpeed
{
    public abstract class PhuongTien
    {
        private string _maPT = "PT000";
        private string _tenHang;
        private int _namSanXuat;
        private decimal _giaGoc;

        public string MaPT
        {
            get { return _maPT; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Mã phương tiện không được để trống!");
                _maPT = value;
            }
        }

        public string TenHang
        {
            get { return _tenHang; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tên hãng không được để trống!");
                _tenHang = value;
            }
        }

        public int NamSanXuat
        {
            get { return _namSanXuat; }
            set
            {
                if (value < 1900 || value > DateTime.Now.Year)
                    throw new ArgumentException("Năm sản xuất không hợp lệ!");
                _namSanXuat = value;
            }
        }

        public decimal GiaGoc
        {
            get { return _giaGoc; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Giá gốc phải lớn hơn 0!");
                _giaGoc = value;
            }
        }

        public PhuongTien(string maPT, string tenHang, int namSanXuat, decimal giaGoc)
        {
            MaPT = maPT;
            TenHang = tenHang;
            NamSanXuat = namSanXuat;
            GiaGoc = giaGoc;
        }

        public abstract decimal TinhGiaLanBanh();

        public virtual string GetInfo()
        {
            return "Mã: " + MaPT + " | Hãng: " + TenHang +
                   " | Năm SX: " + NamSanXuat +
                   " | Giá gốc: " + GiaGoc.ToString("N0") + " VND";
        }
    }

    public class OTo : PhuongTien
    {
        private int _soChoNgoi;
        private double _dungTichDongCo;

        public int SoChoNgoi
        {
            get { return _soChoNgoi; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Số chỗ ngồi phải lớn hơn 0!");
                _soChoNgoi = value;
            }
        }

        public double DungTichDongCo
        {
            get { return _dungTichDongCo; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Dung tích động cơ phải lớn hơn 0!");
                _dungTichDongCo = value;
            }
        }

        public OTo(string maPT, string tenHang, int namSanXuat, decimal giaGoc,
                   int soChoNgoi, double dungTichDongCo)
            : base(maPT, tenHang, namSanXuat, giaGoc)
        {
            SoChoNgoi = soChoNgoi;
            DungTichDongCo = dungTichDongCo;
        }

        public override decimal TinhGiaLanBanh()
        {
            if (SoChoNgoi <= 9)
            {
                return GiaGoc + GiaGoc * 0.12m + GiaGoc * 0.30m;
            }
            else
            {
                return GiaGoc + GiaGoc * 0.10m;
            }
        }

        public override string GetInfo()
        {
            return "[Ô tô] " + base.GetInfo() +
                   " | Số chỗ: " + SoChoNgoi +
                   " | Dung tích động cơ: " + DungTichDongCo + " L";
        }
    }

    public class XeMay : PhuongTien
    {
        private int _dungTichXylanh;

        public int DungTichXylanh
        {
            get { return _dungTichXylanh; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Dung tích xylanh phải lớn hơn 0!");
                _dungTichXylanh = value;
            }
        }

        public XeMay(string maPT, string tenHang, int namSanXuat, decimal giaGoc,
                     int dungTichXylanh)
            : base(maPT, tenHang, namSanXuat, giaGoc)
        {
            DungTichXylanh = dungTichXylanh;
        }

        public override decimal TinhGiaLanBanh()
        {
            if (DungTichXylanh < 175)
            {
                return GiaGoc + GiaGoc * 0.02m;
            }
            else
            {
                return GiaGoc + GiaGoc * 0.05m;
            }
        }

        public override string GetInfo()
        {
            return "[Xe máy] " + base.GetInfo() +
                   " | Dung tích xylanh: " + DungTichXylanh + " cc";
        }
    }

    public class QuanLyPhuongTien
    {
        private List<PhuongTien> _danhSach = new List<PhuongTien>();

        public void AddPhuongTien(PhuongTien pt)
        {
            _danhSach.Add(pt);
        }

        public void DisplayAll()
        {
            string mau = "{0,-8} | {1,-6} | {2,-16} | {3,6} | {4,16} | {5,-20} | {6,16}";
            string gach = new string('-', 106);

            Console.WriteLine("===== DANH SÁCH PHƯƠNG TIỆN =====");
            Console.WriteLine(gach);
            Console.WriteLine(mau, "Loại", "Mã", "Hãng", "Năm SX", "Giá gốc (VND)", "Thông số", "Giá lăn bánh (VND)");
            Console.WriteLine(gach);

            foreach (PhuongTien pt in _danhSach)
            {
                string loai = "";
                string thongSo = "";

                if (pt is OTo)
                {
                    OTo o = (OTo)pt;
                    loai = "Ô tô";
                    thongSo = o.SoChoNgoi + " chỗ, " + o.DungTichDongCo + "L";
                }
                else if (pt is XeMay)
                {
                    XeMay x = (XeMay)pt;
                    loai = "Xe máy";
                    thongSo = x.DungTichXylanh + " cc";
                }

                Console.WriteLine(mau, loai, pt.MaPT, pt.TenHang, pt.NamSanXuat,
                                  pt.GiaGoc.ToString("N0"), thongSo,
                                  pt.TinhGiaLanBanh().ToString("N0"));
            }

            Console.WriteLine(gach);
            Console.WriteLine("Tổng số phương tiện: " + _danhSach.Count);
        }

        public PhuongTien FindMaxGiaLanBanh()
        {
            if (_danhSach.Count == 0)
                return null;

            PhuongTien max = _danhSach[0];
            foreach (PhuongTien pt in _danhSach)
            {
                if (pt.TinhGiaLanBanh() > max.TinhGiaLanBanh())
                    max = pt;
            }
            return max;
        }

        public List<PhuongTien> SearchByName(string keyword)
        {
            List<PhuongTien> ketQua = new List<PhuongTien>();
            foreach (PhuongTien pt in _danhSach)
            {
                if (pt.TenHang.ToLower().Contains(keyword.ToLower()))
                    ketQua.Add(pt);
            }
            return ketQua;
        }
    }

    public class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("--- TC01: Kiểm tra năm sản xuất ---");
            try
            {
                OTo xeLoi = new OTo("OT00", "Toyota", 1850, 1000000000m, 5, 2.0);
                Console.WriteLine("Tạo xe thành công");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }

            Console.WriteLine("\n--- TC02: Giá lăn bánh ô tô 5 chỗ ---");
            OTo oto = new OTo("OT01", "Toyota Camry", 2023, 1000000000m, 5, 2.5);
            Console.WriteLine("Giá lăn bánh: " + oto.TinhGiaLanBanh().ToString("N0") + " VND");

            Console.WriteLine("\n--- TC03: Giá lăn bánh xe máy 150cc ---");
            XeMay xeMay = new XeMay("XM01", "Honda Vision", 2024, 50000000m, 150);
            Console.WriteLine("Giá lăn bánh: " + xeMay.TinhGiaLanBanh().ToString("N0") + " VND");

            Console.WriteLine("\n--- TC04: Đa hình với List<PhuongTien> ---");
            List<PhuongTien> ds = new List<PhuongTien>();
            ds.Add(oto);
            ds.Add(xeMay);
            foreach (PhuongTien pt in ds)
            {
                Console.WriteLine(pt.GetInfo());
                Console.WriteLine("   => " + pt.TinhGiaLanBanh().ToString("N0") + " VND");
            }

            Console.WriteLine("\n--- TC05: Tìm xe có giá lăn bánh cao nhất ---");
            QuanLyPhuongTien ql = new QuanLyPhuongTien();
            ql.AddPhuongTien(oto);
            ql.AddPhuongTien(xeMay);
            ql.DisplayAll();

            PhuongTien max = ql.FindMaxGiaLanBanh();
            Console.WriteLine("Xe đắt nhất: " + max.GetInfo());

            Console.WriteLine("\n--- Tìm kiếm theo tên 'honda' ---");
            List<PhuongTien> ketQua = ql.SearchByName("honda");
            foreach (PhuongTien pt in ketQua)
            {
                Console.WriteLine(pt.GetInfo());
            }

            Console.ReadKey();
        }
    }
}