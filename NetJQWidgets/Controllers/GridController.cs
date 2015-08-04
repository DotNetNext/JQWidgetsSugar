using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

        public ActionResult Index2()
        {
            return View();
        }

        [OutputCache(Duration = 0)]
        public JsonResult Data(GridSearchParams pars)
        {
            using (SqlSugarClient db = SugarDao.GetInstance())
            {
                var sable = db.Sqlable().Form<GridTable>("g").Where("id>0");//查询表的sqlable对象
                var model = JQXGrid.GetWidgetsSource<Models.GridTable>(sable, pars);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
