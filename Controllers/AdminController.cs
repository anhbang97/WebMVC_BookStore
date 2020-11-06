using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMVC_BookStore.Models;


using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Web.UI.WebControls;

namespace WebMVC_BookStore.Controllers
{
    public class AdminController : Controller
    {
        dbQuanLyBanSachDataContext data = new dbQuanLyBanSachDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }


       

        public ActionResult Sach(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
           
            return View(data.Saches.ToList().OrderBy(n => n.MaSach).ToPagedList(pageNumber, pageSize));
        }












        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendangnhap = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendangnhap))
            {
                ViewData["Loi1"] = "Cần phải nhập tên đăng nhập! ";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Cần phải nhập mật khẩu!";
            }
            else
            {
                Admin admin = data.Admin.SingleOrDefault(n => n.UserAdmin == tendangnhap && n.PassAdmin == matkhau);
                
               
                if (admin != null)
                {

                    Session["TaiKhoanAdmin"] = admin;
                    return RedirectToAction("Index","Admin");

                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng!";
                }
            }
            return View();
        }





        [HttpGet]
        public ActionResult ThemMoiSach()
        {

            ViewBag.MaChuDe = new SelectList(data.ChuDes.ToList().OrderBy(n =>n.TenChuDe),"MaChuDe","TenChuDe");
            ViewBag.MaNXB = new SelectList(data.NhaXuatBans.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }





        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoiSach( Sach sach ,HttpPostedFileBase fileupload )
        {
            ViewBag.MaChuDe = new SelectList(data.ChuDes.ToList().OrderBy(n => n.TenChuDe), "MaChuDe", "TenChuDe");
            ViewBag.MaNXB = new SelectList(data.NhaXuatBans.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            if (fileupload == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn ảnh bìa";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    
                    var path = Path.Combine(Server.MapPath("~/img/HinhAnhSanPham"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    sach.AnhBia = fileName;
                    data.Saches.InsertOnSubmit(sach);
                    data.SubmitChanges();
                }
            }

            return RedirectToAction("Sach");
            
            //var fileName = Path.GetFileName(fileupload.FileName);
            //var path = Path.Combine(Server.MapPath("~/img/HinhAnhSanPham"), fileName);
            //if (System.IO.File.Exists(path))
            //{
            //    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
            //}
            //else
            //{
            //    fileupload.SaveAs(path);
            //}

          

            //return View();
        }

        public ActionResult ChiTietSach( int id)
        {
            Sach sach = data.Saches.SingleOrDefault(n => n.MaSach == id);
            ViewBag.MaSach = sach.MaSach;

            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }








        [HttpGet]
        public ActionResult XoaSach(int id)
        {
            Sach sach = data.Saches.SingleOrDefault(n => n.MaSach == id);
            ViewBag.MaSach = sach.MaSach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }


        [HttpPost ,ActionName("XoaSach")]
      
        public ActionResult XacNhanXoa(int id)
        {

            Sach sach = data.Saches.SingleOrDefault(n => n.MaSach ==  id);
            ViewBag.MaSach = sach.MaSach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.Saches.DeleteOnSubmit(sach);
            data.SubmitChanges();
            return  View(sach);
        }




        [HttpGet]
        public ActionResult SuaSach(int id)
        {
            Sach sach = data.Saches.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaChuDe = new SelectList(data.ChuDes.ToList().OrderBy(n => n.TenChuDe), "MaChuDe", "TenChuDe",sach.MaChuDe);
            ViewBag.MaNXB = new SelectList(data.NhaXuatBans.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB",sach.MaNXB);


            return View(sach);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaSach(Sach sach,HttpPostedFileBase fileupload)
        {
            ViewBag.MaChuDe = new SelectList(data.ChuDes.ToList().OrderBy(n => n.TenChuDe), "MaChuDe", "TenChuDe");
            ViewBag.MaNXB = new SelectList(data.NhaXuatBans.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            if (fileupload == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn ảnh bìa!";
                return View();
            }

            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/img/HinhAnhSanPham"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    sach.AnhBia = fileName;
                    UpdateModel(sach);
                    data.SubmitChanges();
                }

            }
           

            return RedirectToAction("Sach");
        }



        public ActionResult NhaXuatBan()
        {
            return View(data.NhaXuatBans.ToList());
        }

        public ActionResult ChuDe()
        {
            return View(data.ChuDes.ToList());
        }
    }
}