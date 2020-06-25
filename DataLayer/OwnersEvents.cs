using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class OwnersEvents
    {
        public List<Owners> getOwners(out int resultCount, out int pageCount, int pagesize = 0, int pageIndex = 0)
        {
            AsanShahrDBEntities db = new AsanShahrDBEntities();
            db.Configuration.ProxyCreationEnabled = false;
            resultCount = 0;
            pageCount = 0;
            IEnumerable<Owners> query = db.Owners.Where(q => true);

            List<Owners> lstOwners = query.ToList();
            if (lstOwners != null)
            {
                if (pageIndex == 0 && pagesize == 0)
                {
                    resultCount = 1;
                    pageCount = 1;
                }
                else
                {
                    resultCount = lstOwners.Count;
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
                    lstOwners = lstOwners.Skip(skip).Take(pagesize).ToList();
                }
            }
            return lstOwners;

        }
    }
}
