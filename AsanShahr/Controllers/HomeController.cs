using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AsanShahr.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetRealestatesList(int pageSize, int pageIndex)
        {
            try
            {
                DataLayer.RealestatesEvents RealestateCtrl = new DataLayer.RealestatesEvents();
                int pcount = 0;
                int rCount = 0;
                List<DataLayer.Realestates> lstR = RealestateCtrl.getRealestates(out rCount, out pcount, pageSize, pageIndex);
                var result = new
                {
                    realestatesList = lstR.ToArray(),
                    count = rCount,
                    pageCount = pcount,
                    ErrorMessage = "",
                    successful = true
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new
                {
                    realestatesList = new List<DataLayer.Realestates>(),
                    count = 0,
                    pageCount = 0,
                    ErrorMessage = "در بدست آوردن اطلاعات املاک خطایی رخ داده است!",
                    successful = true
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetRealestateByID(long id)
        {
            try
            {
                DataLayer.RealestatesEvents rCtrl = new DataLayer.RealestatesEvents();
                List<DataLayer.vwRealestateOwners> lstOw = new List<DataLayer.vwRealestateOwners>();
                DataLayer.Realestates objr = rCtrl.getRealestateByID(id, out lstOw);
                var result = new
                {
                    realestate = objr,
                    lstOwners = lstOw,
                    ErrorMessage = "",
                    successful = true
                };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var result = new
                {
                    realestate = new DataLayer.Realestates(),
                    lstOwners = new List<DataLayer.Owners>(),
                    ErrorMessage = "در بدست آوردن اطلاعات ملک جاری خطایی رخ داده است !",
                    successful = false
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteRealestateByID(long rid)
        {
            DataLayer.RealestatesEvents db = new DataLayer.RealestatesEvents();
            try
            {
                DataLayer.RealestatesEvents RealestateCtrl = new DataLayer.RealestatesEvents();
                string error = "";
                if (RealestateCtrl.deleteRealestatebyId(rid, out error))
                {
                    var result = new
                    {
                        success = true,
                        error = "",
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new
                    {
                        success = false,
                        error = error
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception ex)
            {
                var result = new
                {
                    success = false,
                    error = "در حذف دارایی خطایی رخ داده است !"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult SaveRealestateChanges(DataLayer.Realestates objRealestate, List<DataLayer.vwRealestateOwners> lstOwners, List<long> lstdeletedowners)
        {
            DataLayer.RealestatesEvents ctrl = new DataLayer.RealestatesEvents();
            string error = "";
            if (ctrl.saveRealestates(objRealestate, lstOwners, lstdeletedowners, out error))
            {
                var result = new
                {
                    success = true,
                    err = ""
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new
                {
                    success = false,
                    err = error
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
    }
}