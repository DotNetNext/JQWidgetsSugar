using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetJQWidgets.Models;
using SqlSugar;
using DAL;
using JQWidgetsSugar;
using Models;
namespace NetJQWidgets.Controllers
{
    public class GridController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [OutputCache(Duration = 0)]
        public JsonResult Data(JQXGridParams pars)
        {
            using (SqlSugarClient db = SugarDao.GetInstance())
            {
                var sable = db.Sqlable().Form("GridTable", "g");
                var model = JQXGrid.GetWidgetsSource<GridTable>(sable, pars);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
