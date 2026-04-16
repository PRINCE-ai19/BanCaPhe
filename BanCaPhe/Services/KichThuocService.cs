using BanCaPhe.Helpers;
using BanCaPhe.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Services
{
    public class KichThuocService
    {
        public List<KichThuoc> GetBySanPhamId(int sanPhamId)
        {
            return StoreHelper.QueryList<KichThuoc>("sp_GetKichThuocBySanPham", new { SanPhamID = sanPhamId });
        }
    }
}
