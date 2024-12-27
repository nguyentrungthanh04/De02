using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using De02_DAL.Models;

namespace De02_BUS
{
    public class SanphamService
    {
        public List<Sanpham> GetAll()
        {
            SanphamContextDB context = new SanphamContextDB();
            return context.Sanphams.ToList();
        }
    }
}
