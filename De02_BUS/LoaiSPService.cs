using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using De02_DAL.Models;

namespace De02_BUS
{
    public class LoaiSPService
    {
        public List<LoaiSP> GetAll()
        {
            SanphamContextDB context = new SanphamContextDB();
            return context.LoaiSPs.ToList();
        }
    }
}
