using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SqlSugar;
using DAL;
using JQWidgetsSugar;
using Models;
using SyntacticSugar;
namespace NetJQWidgets.Controllers
{
    public class GridController : Controller
    {
        public ActionResult Index()
        {
            var adp = new GridDataAdapterSource();
            adp.url = "/Grid/Data";
            var gc = new GridConfig();
            gc.gridbuttons = new List<GridButton>()
            {
               new GridButton(){ click="add", name="addbutton", icon="jqx-icon-plus", title="添加"},
               new GridButton(){ click="edit", name="editbutton", icon="jqx-icon-edit", title="编辑"},
               new GridButton(){ click="del", name="delbutton", icon="jqx-icon-delete", title="删除"}
            };
            gc.pageSize = 20;
            gc.width = "80%";
            gc.columns = new List<GridColumn>(){
               new GridColumn(){ text="编号", datafield="id", width="40px", cellsalign=AlignType.left,datatype=Datatype.dataint  },
               new GridColumn(){ text="名称", datafield="name", cellsalign=AlignType.left,datatype=Datatype.datastring },
               new GridColumn(){ text="产品名", datafield="productname", cellsalign=AlignType.left,datatype=Datatype.datastring },
               new GridColumn(){ text="数量", datafield="quantity", cellsalign=AlignType.right , datatype=Datatype.dataint },
               new GridColumn(){ text="创建时间", datafield="date", cellsformat="yyyy-MM-dd",cellsalign=AlignType.right, datatype=Datatype.datadate 
              }
            };
       
            var grid = JQXGrid.BindGrid("#netgrid", adp, gc);
            ViewBag.validationBind = ValidationSugar.GetBindScript("validate_key_grid_index");
            return View(grid);
        }

        [HttpDelete]
        public JsonResult Del(int id)
        {
            using (SqlSugarClient db = SugarDao.GetInstance())
            {
                ActionResultModel<string> model = new ActionResultModel<string>();
                model.isSuccess = db.Delete<GridTable>(id);
                model.respnseInfo = model.isSuccess ? "删除成功" : "删除失败";
                return Json(model);
            }
        }

        [HttpPost]
        public JsonResult Add(GridTable gt)
        {
            using (SqlSugarClient db = SugarDao.GetInstance())
            {
                string message = string.Empty;
                var isValid = ValidationSugar.PostValidation("validate_key_grid_index", out message);
                ActionResultModel<string> model = new ActionResultModel<string>();
                if (isValid)//后台验证数据完整性
                {
                    model.isSuccess = db.Insert(gt) != DBNull.Value;
                    model.respnseInfo = model.isSuccess ? "添加成功" : "添加失败";
                }
                else {
                    model.isSuccess = false;
                    model.respnseInfo = message;
                }
                return Json(model);
            }
        }
        [HttpPut]
        public JsonResult Edit(GridTable gt)
        {
            using (SqlSugarClient db = SugarDao.GetInstance())
            {
                ActionResultModel<string> model = new ActionResultModel<string>();
                string message = string.Empty;
                var isValid = ValidationSugar.PostValidation("validate_key_grid_index", out message);
                if (isValid)//后台验证数据完整性
                {
                    model.isSuccess = db.Update<GridTable>(gt, it => it.id == gt.id);
                    model.respnseInfo = model.isSuccess ? "编辑成功" : "编辑失败";
                }
                else {
                    model.isSuccess = false;
                    model.respnseInfo = message;
                }
                return Json(model);
            }
        }

        [OutputCache(Duration = 0)]
        public JsonResult Data(GridSearchParams pars)
        {
            using (SqlSugarClient db = SugarDao.GetInstance())
            {
                if (pars.sortdatafield == null) { //默认按id降序
                    pars.sortdatafield = "id";
                    pars.sortorder = "desc";
                }
                Sqlable sable = db.Sqlable().Form<GridTable>("g");//查询表的sqlable对象
                var model = JQXGrid.GetWidgetsSource<Models.GridTable>(sable, pars);//根据grid的参数自动查询
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
