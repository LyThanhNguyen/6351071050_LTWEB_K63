using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _123.Models
{
    public class Giohang
    {
        QLBanxeganmayDataContext data;
        public int iMaXe { set; get; }
        public string sTenxe { set; get; }
        public string sAnhbia { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }
        }
        public Giohang(int Maxe)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["QLBanXeGanMayConnectionString1"].ConnectionString;

            // Initialize the DataContext with the connection string

            data = new QLBanxeganmayDataContext(connectionString);


            iMaXe = Maxe;
            XEGANMAY xeganmay = data.XEGANMAYs.Single(n=>n.MaXe == iMaXe);
            sTenxe = xeganmay.TenXe;
            sAnhbia = xeganmay.Anhbia;
            dDongia = Double.Parse(xeganmay.Giaban.ToString());
            iSoluong = 1;
        }
    }
}