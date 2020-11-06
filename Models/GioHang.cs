using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMVC_BookStore.Models
{
    public class GioHang
    {
        dbQuanLyBanSachDataContext data = new dbQuanLyBanSachDataContext();
        public int iMaSach { set; get; }
        public string sTenSach { set; get; }
        public string sAnhBia { set; get; }
        public double dDonGia { set; get; }
        public int iSoLuong { set; get; }

        public double dThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }


        public GioHang(int MaSach)
        {
            iMaSach = MaSach;
            Sach sach = data.Saches.Single(n => n.MaSach == iMaSach);
            sTenSach = sach.TenSach;
            sAnhBia = sach.AnhBia;
            dDonGia = double.Parse(sach.GiaBan.ToString());
            iSoLuong = 1;
        }

    }
}