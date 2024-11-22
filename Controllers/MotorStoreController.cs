using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _123.Models;

namespace _123.Controllers
{
    public class MotorStoreController : Controller
    {
        // GET: MotorStore
        //QLBanxeganmayDataContext data =  new QLBanxeganmayDataContext();

        QLBanxeganmayDataContext data;

        public MotorStoreController()
        {
            // Get the connection string from the Web.config file
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["QLBanXeGanMayConnectionString1"].ConnectionString;

            // Initialize the DataContext with the connection string
            data = new QLBanxeganmayDataContext(connectionString);
        }
        private  List<XEGANMAY> Layxemoi(int count)
        {
            return data.XEGANMAYs.OrderByDescending(x => x.Ngaycapnhat).Take(count).ToList();
        }
        public ActionResult Index()
        {
            var xemoi = Layxemoi(5);
            return View(xemoi); 
        }
        public ActionResult Loaixe()
        {
            var loaixe = from cd in data.LOAIXEs select cd;
            return PartialView(loaixe);
        }
        public ActionResult Nhaphanphoi()
        {
            var nhaphanphoi = from cd in data.NHAPHANPHOIs select cd;
            return PartialView(nhaphanphoi);
        }
        public ActionResult SPTheoloaixe(int id)
        {
            var xe = from s in data.XEGANMAYs where s.MaXe == id select s;
            return View(xe);
        }
        public ActionResult SPTheoNhaphanphoi(int id)
        {
            var xe = from s in data.XEGANMAYs where s.MaNPP == id select s;
            return View(xe);
        }
        public ActionResult Details(int id)
        {
            var xe = from s in data.XEGANMAYs where s.MaXe == id
                     select s;
            return View(xe.Single());
        }
    }
}