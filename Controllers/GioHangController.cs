using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMVC_BookStore.Models;

namespace WebMVC_BookStore.Controllers
{
    public class GioHangController : Controller
    {
        dbQuanLyBanSachDataContext data = new dbQuanLyBanSachDataContext();
        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }



        public List<GioHang> LayGioHang()
        {
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            if (listGioHang == null)
            {
                listGioHang = new List<GioHang>();
                Session["GioHang"] = listGioHang;
            }
            return listGioHang;
        }

        public ActionResult CapNhatGioHang(int iMaSach , FormCollection f)
        {
            Sach sach = data.Saches.SingleOrDefault(n => n.MaSach == iMaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> listGioHang = LayGioHang();
            GioHang s = listGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);
            if (s != null)
            {
                s.iSoLuong = int.Parse(f["txtSoLuong"].ToString());

            }
            return View("GioHang");
        }






        public ActionResult XoaGioHang(int iMaSach)
        {
            Sach sach = data.Saches.SingleOrDefault(n => n.MaSach == iMaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> listGioHang = LayGioHang();

            GioHang s = listGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);
            if (s != null)
            {
                listGioHang.RemoveAll(n=>n.iMaSach == iMaSach);
                return RedirectToAction("Details","BookStore");

            }
            return RedirectToAction("GioHang");


        }




        public ActionResult ThemGioHang(int iMaSach, string strURL)
        {
            List<GioHang> listGioHang = LayGioHang();

            GioHang sanpham = listGioHang.Find(n => n.iMaSach == iMaSach);
            if (sanpham == null)
            {
                sanpham = new GioHang(iMaSach);
                listGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoLuong++;
                return Redirect(strURL);
            }
        }






        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            if (listGioHang != null)
            {
                iTongSoLuong = listGioHang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }





        private double TongTien()
        {
            double iTongTien = 0;
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            if (listGioHang != null)
            {
                iTongTien = listGioHang.Sum(n => n.dThanhTien);
            }
            return iTongTien;
        }


        public ActionResult GioHang()
        {
            if (Session["GioHang"] ==null)
            {
                return RedirectToAction("Details", "BookStore");
            }
            List<GioHang> listGioHang = LayGioHang();
            if (listGioHang.Count == 0)
            {
                return RedirectToAction("Details", "BookStore");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(listGioHang);
        }












        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }






        public ActionResult Xoa_GioHang(int iMaSach)
        {
            List<GioHang> listGioHang = LayGioHang();
            GioHang sanpham = listGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);
            if (sanpham != null)
            {
                listGioHang.RemoveAll(n => n.iMaSach == iMaSach);
                return RedirectToAction("GioHang");
            }
            if (listGioHang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            return RedirectToAction("GioHang");
         }




        public ActionResult CapNhat_GioHang(int iMaSach , FormCollection collection)
        {
            List<GioHang> listGioHang = LayGioHang();
            GioHang sanpham = listGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);
            if (sanpham != null)
            {
                sanpham.iSoLuong = int.Parse(collection["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }




        public ActionResult XoaTatCa_GioHang()
        {
            List<GioHang> listGioHang = LayGioHang();
            listGioHang.Clear();
            return RedirectToAction("Index","BookStore");
        }



        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString()== "")
            {
                return RedirectToAction("DangNhap","NguoiDung");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index","BookStore");
            }
            List<GioHang> listGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(listGioHang);
        }
      
       







        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
           
                DonDatHang ddh = new DonDatHang();
                KhachHang kh = (KhachHang)Session["TaiKhoan"];
                List<GioHang> listGioHang = LayGioHang();
                ddh.MaKhachHang = kh.MaKhachHang;
                ddh.NgayDatHang = DateTime.Now;
                var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
                ddh.NgayGiaoHang = DateTime.Parse(ngaygiao);
                ddh.TinhTrangGiaoHang = false;
                ddh.DatThanhToan = false;
                data.DonDatHangs.InsertOnSubmit(ddh);

                data.SubmitChanges();

                foreach (var item in listGioHang)
                {
                    ChiTietDatHang ctdh = new ChiTietDatHang();
                    ctdh.MaDonHang = ddh.MaDonHang;
                    ctdh.MaSach = item.iMaSach;
                    ctdh.SoLuong = item.iSoLuong;
                    ctdh.DonGia = (decimal)item.dDonGia;
                    data.ChiTietDatHangs.InsertOnSubmit(ctdh);
                }
                data.SubmitChanges();
                Session["GioHang"] = null;
  

            return RedirectToAction("XacNhanDonHang","GioHang");
        }

        public ActionResult XacNhanDonHang()
        {
            return View();
        }

     }
}