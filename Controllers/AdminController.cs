using _123.Models;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;

namespace _123.Controllers
{
    public class AdminController : Controller
    {
        QLBanxeganmayDataContext data;
        public AdminController()
        {
            // Get the connection string from the Web.config file
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["QLBanXeGanMayConnectionString1"].ConnectionString;

            // Initialize the DataContext with the connection string
            data = new QLBanxeganmayDataContext(connectionString);

        }
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
         public ActionResult Login(FormCollection collection)
        {
            // Assign user input values ​​to variables
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "phai nhap ten dang nhap";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "phai nhap mat khau";
            }
            else
            {
                //Assign value to newly created object (ad)
                Admin ad = data.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)

                {

                    // ViewBag.Thongbao = "Congratulations on successful login";

                    Session["Taikhoanadmin"] = ad;

                    return RedirectToAction("Index", "Admin");

                }

                else

                    ViewBag.Thongbao = "Ten dang nhap hoac mat khau khong dung";

            }

            return View();

        }

        public ActionResult Xeganmay(int ?page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(data.XEGANMAYs.ToList().OrderBy(n=> n.MaXe).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Themmoixeganmay()
        {

            ViewBag.MaLX  = new SelectList(data.LOAIXEs.ToList().OrderBy(n=>n.TenLoaiXe),"MaLX","TenLoaiXe");
            ViewBag.MaNPP = new SelectList(data.NHAPHANPHOIs.ToList().OrderBy(n => n.TenNPP), "MaNPP", "TenNPP");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoixeganmay(XEGANMAY xeganmay, HttpPostedFileBase fileupload)
        {
            ViewBag.MaLX = new SelectList(data.LOAIXEs.ToList().OrderBy(n => n.TenLoaiXe), "MaLX", "TenLoaiXe");
            ViewBag.MaNPP = new SelectList(data.NHAPHANPHOIs.ToList().OrderBy(n => n.TenNPP), "MaNPP", "TenNPP");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui long chon anh bia";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);

                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);

                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hinh anh da ton tai";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    xeganmay.Anhbia = fileName;
                    data.XEGANMAYs.InsertOnSubmit(xeganmay);
                    data.SubmitChanges();

                }
                return View();
            }
        }
        public ActionResult Chitietxeganmay(int id)
        {
            XEGANMAY xeganmay = data.XEGANMAYs.SingleOrDefault(n => n.MaXe == id);
            ViewBag.MaXe = xeganmay.MaXe;
            if (xeganmay == null) 
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(xeganmay);
        }
        public ActionResult Xoaxeganmay(int id)
        {
            XEGANMAY xeganmay = data.XEGANMAYs.SingleOrDefault(n => n.MaXe == id);
            ViewBag.MaXe = xeganmay.MaXe;
            if(xeganmay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(xeganmay);
        }

        [HttpPost, ActionName("Xoaxeganmay")]

        public ActionResult Xacnhanxoa(int id)
        {
            XEGANMAY xeganmay = data.XEGANMAYs.SingleOrDefault(n => n.MaXe == id);
            ViewBag.MaXe = xeganmay.MaXe;
            if (xeganmay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.XEGANMAYs.DeleteOnSubmit(xeganmay);
            data.SubmitChanges();
            return RedirectToAction("Xeganmay");
        }

        [HttpGet]
        public ActionResult Suaxeganmay(int id)
        {
            XEGANMAY xeganmay = data.XEGANMAYs.SingleOrDefault(n => n.MaXe == id);
            if (xeganmay == null)
            {
                return HttpNotFound();
            }

            ViewBag.MaLX = new SelectList(data.LOAIXEs.ToList().OrderBy(n => n.TenLoaiXe), "MaLX", "TenLoaiXe",xeganmay.MaLX);
            ViewBag.MaNPP = new SelectList(data.NHAPHANPHOIs.ToList().OrderBy(n => n.TenNPP), "MaNPP", "TenNPP",xeganmay.MaNPP);
            return View(xeganmay);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suaxeganmay(XEGANMAY xe, HttpPostedFileBase fileupload)
        {
            string a = fileupload.FileName;
           
            // Gán dữ liệu vào dropdown
            ViewBag.MaLX = new SelectList(data.LOAIXEs.ToList().OrderBy(n => n.TenLoaiXe), "MaLX", "TenLoaixe", xe.MaLX);
            ViewBag.MaNPP = new SelectList(data.NHAPHANPHOIs.ToList().OrderBy(n => n.TenNPP), "MaNPP", "TenNPP", xe.MaNPP);

            // Kiểm tra fileupload
            if (fileupload == null)
            {
                ViewBag.Thongbao = a;
                /*ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";*/
                return View(xe); // Trả về model để giữ dữ liệu
            }

            if (ModelState.IsValid)
            {
                // Lấy tên file
                var fileName = Path.GetFileName(fileupload.FileName);
                var directory = Server.MapPath("~/Content/images");

                // Kiểm tra thư mục tồn tại
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var path = Path.Combine(directory, fileName);

                // Kiểm tra hình ảnh đã tồn tại
                if (!System.IO.File.Exists(path))
                {
                    fileupload.SaveAs(path);
                    xe.Anhbia = fileName;

                    // Cập nhật dữ liệu
                    var xeToUpdate = data.XEGANMAYs.SingleOrDefault(x => x.MaXe == xe.MaXe);
                    if (xeToUpdate != null)
                    {
                        xeToUpdate.TenXe = xe.TenXe;
                        xeToUpdate.Giaban = xe.Giaban;
                        xeToUpdate.Mota = xe.Mota;
                        xeToUpdate.Anhbia = fileName;
                        xeToUpdate.Ngaycapnhat = xe.Ngaycapnhat;
                        xeToUpdate.Soluongton = xe.Soluongton;
                        xeToUpdate.MaLX = xe.MaLX;
                        xeToUpdate.MaNPP = xe.MaNPP;

                        data.SubmitChanges();
                    }
                }
                else
                {
                    ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    return View(xe);
                }

                return RedirectToAction("Xeganmay");
            }

            return View(xe); // Nếu ModelState không hợp lệ, trả về View cùng dữ liệu đã nhập
        }



        public ActionResult Bieudoxe()
        {
            var thongKeXe = data.XEGANMAYs
               .GroupBy(x => x.MaLX)
               .Select(g => new
               {
                   TenLoaiXe = g.FirstOrDefault().LOAIXE.TenLoaiXe,
                   SoLuongXe = g.Count()
               })
               .ToList() // Chuyển về danh sách trong bộ nhớ
               .Select(x => new ThongKeXeViewModel
               {
                   TenLoaiXe = x.TenLoaiXe ?? "Không xác định",
                   SoLuongXe = x.SoLuongXe
               })
               .ToList();

            return View(thongKeXe);
        }
    }
}