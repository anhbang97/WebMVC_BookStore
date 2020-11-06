using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMVC_BookStore.Models;
using System.Data;

namespace WebMVC_BookStore.Controllers
{
    public class NguoiDungController : Controller
    {
        dbQuanLyBanSachDataContext data = new dbQuanLyBanSachDataContext();
        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }


        [HttpPost]

        public ActionResult DangKy(FormCollection collection , KhachHang kh )
        {

            var hoten = collection["HoTenKhachHang"];

            var tendangnhap = collection ["TenDangNhap"];

            var matkhau = collection["MatKhau"];

            var matkhaunhaplai = collection["MatKhauNhapLai"];

            var diachi = collection["DiaChi"];

            var email = collection["Email"];

            var dienthoai = collection["DienThoai"];

            var ngaysinh = String.Format("{0:MM/dd/yyyy}",collection["NgaySinh"]);

            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống"; 
            }
            else if(String.IsNullOrEmpty(tendangnhap))
            {
                ViewData["Loi2"] = "Phải nhập tên đăng nhập ";
            }
            else if(String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Phải nhập mật khẩu";
            }
            else if(String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi4"] = "Phải nhập lại mật khẩu";
            }
            if(String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi5"] = "Phải nhập số điện thoại";
            }
            if(String.IsNullOrEmpty(email))
            {
                ViewData["Loi6"] = "Phải nhập địa chỉ email";
            }
            if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi7"] = "Phải nhập địa chỉ ";
            }
            else
            {
                kh.HoTen = hoten;

                kh.TaiKhoan = tendangnhap;

                kh.MatKhau = matkhau;

                kh.Email = email;

                kh.DiaChiKhachHang = diachi;

                kh.SoDienThoaiKhachHang = dienthoai;

                kh.NgaySinh = DateTime.Parse(ngaysinh);
                data.KhachHangs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("DangNhap");


            }
            return this.DangKy();
        }






        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection )
        {
            var tendangnhap = collection["TenDN"];
            var matkhau = collection["MatKhau"];
            if (String.IsNullOrEmpty(tendangnhap))
            {
                ViewData["Loi1"] = "Tên đăng nhập không được trống";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Mật khẩu không được trống";
            }
                else
                {
                    KhachHang kh = data.KhachHangs.SingleOrDefault(n => n.TaiKhoan == tendangnhap && n.MatKhau == matkhau);
                    if (kh != null)
                    {
                        ViewBag.ThongBao = "Chúc mừng bạn đã đăng nhập thành công!";
                        Session["TaiKhoan"] = kh;
                    }
                    else
                    {
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng!";

                }
                }

            return View();
        }

    }
}