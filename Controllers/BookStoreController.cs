using System.Collections.Generic;
using System.Linq;

using System.Web.Mvc;
using WebMVC_BookStore.Models;
using PagedList;
using PagedList.Mvc;





namespace WebMVC_BookStore.Controllers
{
    public class BookStoreController : Controller
    {
        // Tạo đối tượng chứa toàn bộ CSDL từ dbQuanLyBanSach .
        dbQuanLyBanSachDataContext data = new dbQuanLyBanSachDataContext();



        private List<Sach> LaySachMoi(int count)
        {
            return data.Saches.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }


        public ActionResult Index(int? page)
        {
            int pageSize = 8;
            int pageNum = (page ?? 1);

            var sachmoi = LaySachMoi(40);
            return View(sachmoi.ToPagedList(pageNum, pageSize));
        }


        // GET: BookStore

        //public ActionResult Index()
        //{
        //    var sachmoi = LaySachMoi(5);
        //    return View(sachmoi);

        //}


        public ActionResult Chu_De()
        {
            var chude = from ChuDe in data.ChuDes select ChuDe;
            return PartialView(chude);

        }


        public ActionResult Nha_Xuat_Ban()
        {
            var nhaxuatban = from NhaXuatBan in data.NhaXuatBans select NhaXuatBan;
            return PartialView(nhaxuatban);
        }






        public ActionResult SanPhamTheo_ChuDe(int MaChuDe)
        {
            ChuDe cd = data.ChuDes.SingleOrDefault(n => n.MaChuDe == MaChuDe);
            if (cd == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            List<Sach> listsach = data.Saches.Where(n => n.MaChuDe == MaChuDe).OrderBy(n => n.GiaBan).ToList();

            return View(listsach);



        }



        public ActionResult SanPhamTheo_NXB(int MaNXB )
        {
            NhaXuatBan nxb = data.NhaXuatBans.SingleOrDefault(n => n.MaNXB == MaNXB);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }


            List<Sach> listsach = data.Saches.Where(n => n.MaNXB == MaNXB).OrderBy(n => n.GiaBan).ToList();
            
            return View(listsach);

        }




        public ActionResult Details(int MaSach ) 
        {



            Sach sach = data.Saches.SingleOrDefault(n => n.MaSach == MaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(sach);
            

        }




    }
}
