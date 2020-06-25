using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace AsanShahr.Controllers
{
    public class OwnersController : Controller
    {
        public JsonResult GetAllOwners(int ps, int px)
        {
            try
            {
                DataLayer.OwnersEvents ownerCtrl = new DataLayer.OwnersEvents();
                int pcount = 0;
                int rCount = 0;
                List<DataLayer.Owners> lst = ownerCtrl.getOwners(out rCount, out pcount, ps, px);
                //var O =  Json(new
                //{
                //    PropertyINeed1 = data.PropertyINeed1,
                //});
                //var result =  JsonConvert.SerializeObject(ErrorResult.ErrorData, Formatting.Indented,
                //       new JsonSerializerSettings
                //       {
                //           ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                //       });

                

                var result = new
                {
                    ownerList = lst.ToArray(),
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
                    ownerList = new List<DataLayer.Realestates>(),
                    count = 0,
                    pageCount = 0,
                    ErrorMessage = "در بدست آوردن اطلاعات مالکان خطایی رخ داده است!",
                    successful = true
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
    }
}