using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DataLayer
{
    public class RealestatesEvents
    {
        public List<Realestates> getRealestates(out int resultCount, out int pageCount, int pagesize = 0, int pageIndex = 0)
        {
            AsanShahrDBEntities db = new AsanShahrDBEntities();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            resultCount = 0;
            pageCount = 0;
            IEnumerable<Realestates> query = db.Realestates.Where(q => true);

            List<Realestates> lstR = query.ToList();
            if (lstR != null)
            {
                if (pageIndex == 0 && pagesize == 0)
                {
                    resultCount = 1;
                    pageCount = 1;
                }
                else
                {
                    resultCount = lstR.Count;
                    int skip = 0;
                    pageCount = resultCount > 0 ? ((resultCount % pagesize) > 0 ? (resultCount / pagesize) + 1 : (resultCount / pagesize)) : 0;

                    if (pageIndex == 1)
                    {
                        skip = 0;
                    }
                    else
                    {
                        skip = (pageIndex - 1) * pagesize;
                    }
                    lstR = lstR.Skip(skip).Take(pagesize).ToList();
                }
            }
            return lstR;

        }

        public Realestates getRealestateByID(long id, out List<vwRealestateOwners> lstRO)
        {
            try
            {
                AsanShahrDBEntities db = new AsanShahrDBEntities();
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                lstRO = db.vwRealestateOwners.Where(w => w.RealestateID == id).ToList();
                return db.Realestates.SingleOrDefault(q => q.ID == id);

            }
            catch (Exception ex)
            {
                lstRO = null;
                return null;
            }
        }

        public bool deleteRealestatebyId(long id, out string error)
        {
            error = "";
            AsanShahrDBEntities db = new AsanShahrDBEntities();
            db.Configuration.ProxyCreationEnabled = false;
            bool flag = true;
            #region 
            try
            {
                IEnumerable<RealestateOwners> query = db.RealestateOwners.Where(w => w.RealestateID == id);
                List<RealestateOwners> lstRo = query.ToList();
                if (lstRo != null)
                {
                    foreach (var item in lstRo)
                    {
                        RealestateOwners objdel = db.RealestateOwners.Where(w => w.ID == item.ID).SingleOrDefault();
                        if (objdel != null) {
                            db.RealestateOwners.Remove(objdel);
                            db.SaveChanges();
                        }

                    }
                }

            }
            catch (Exception)
            {
                error = "در حذف مالکان ملک خطایی رخ داده است !";
                flag = false;
            }
            #endregion
            if (flag)
            {
                try {
                    Realestates objr = db.Realestates.Where(q => q.ID == id).SingleOrDefault();
                    if (objr != null) {
                        db.Realestates.Remove(objr);
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        error = "ملک با شناسه جاری یافت نشد !";
                        return false;
                    }
                }
                catch (Exception ex) {
                    error = "در حذف ملک خطایی رخ داده است !";
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool saveRealestates(Realestates objR, List<vwRealestateOwners> lstOwners, List<long> lstdeleteownerIds, out string err)
        {
            err = "";
            using (var context = new AsanShahrDBEntities())
            {
                try
                {
                    if (objR.ID == 0)
                    {
                        context.Realestates.Add(objR);
                        if (context.SaveChanges() > 0)
                        {
                            #region 'Owner'
                            foreach (var item in lstOwners)
                            {
                                try
                                {
                                    RealestateOwners objro = new RealestateOwners()
                                    {
                                        ID = 0,
                                        OwnerID = item.OwnerID,
                                        RealestateID = objR.ID,
                                    };
                                    context.RealestateOwners.Add(objro);
                                    context.SaveChanges();
                                }
                                catch (Exception ee)
                                {
                                    err = "در ثبت مالکان ملک خطایی رخ داده است !" + ee.Message;
                                    return false;
                                }
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        List<RealestateOwners> lstLastOwners = context.RealestateOwners.Where(q => q.RealestateID == objR.ID).ToList();
                        var r = context.Realestates.Find(objR.ID);
                        if (r != null)
                        {
                            #region 'Delete'
                            foreach (var del in lstdeleteownerIds)
                            {
                                RealestateOwners objdel = context.RealestateOwners.Where(q => q.ID == del).FirstOrDefault();
                                if (objdel != null)
                                {
                                    try
                                    {
                                        context.RealestateOwners.Remove(objdel);
                                        context.SaveChanges();
                                    }
                                    catch (Exception e)
                                    {
                                        err = "در حذف نهایی مالکان خطایی رخ داده است !";
                                        return false;
                                    }
                                }
                            }
                            #endregion
                            r.Address = objR.Address;
                            r.Area = objR.Area;
                            r.IsNorth = objR.IsNorth;
                            r.RegisterPlak = objR.RegisterPlak;
                            r.Title = objR.Title;
                            if (context.SaveChanges() > 0 )
                            {
                                #region 'SaveOwner'
                                foreach (var item in lstOwners)
                                {
                                    //if (lstLastOwners.Where(q => q.OwnerID == item.ID).Count() == 0)
                                    if (item.ID == 0)
                                    {
                                        try
                                        {
                                            RealestateOwners objro = new RealestateOwners()
                                            {
                                                ID = 0,
                                                OwnerID = item.OwnerID,
                                                RealestateID = objR.ID,
                                            };
                                            context.RealestateOwners.Add(objro);
                                            context.SaveChanges();
                                        }
                                        catch (Exception ee)
                                        {
                                            err = "در ثبت مالکان ملک خطایی رخ داده است !" + ee.Message;
                                            return false;
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    err = "در ثبت اطلاعات خطایی رخ داده است !" + ex.Message;
                    return false;
                }
            }

        }
    }
}
