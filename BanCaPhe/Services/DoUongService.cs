using BanCaPhe.Helpers;
using BanCaPhe.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Services
{
    internal class DoUongService
    {
        public List<DoUong> GetAll()
        {
            return StoreHelper.QueryList<DoUong>("sp_DoUong_GetAll");
        }

        public List<DoUong> GetAllAdmin()
        {
            return StoreHelper.QueryList<DoUong>("sp_DoUong_GetAllAdmin");
        }

        public bool CheckTen(string tenDoUong, int loaiId, int? id = null)
        {
            int count = StoreHelper.ExecuteScalar<int>(
                "sp_DoUong_CheckTen",
                new { TenDoUong = tenDoUong, LoaiID = loaiId, ID = id });

            return count > 0;
        }

        public bool Insert(DoUong d)
        {
            return StoreHelper.Execute("sp_DoUong_Insert", d) > 0;
        }

        public bool Update(DoUong d)
        {
            return StoreHelper.Execute("sp_DoUong_Update", d) > 0;
        }

        public bool Delete(int id)
        {
            return StoreHelper.Execute("sp_DoUong_Delete", new { ID = id }) > 0;
        }

        public List<DanhMucDouong> GetLoai()
        {
           
            return StoreHelper.QueryList<DanhMucDouong>("sp_LoaiDoUong_GetAll");
        }

        public ObservableCollection<DoUong> GetAllNV()
        {
            var list = StoreHelper.QueryList<DoUong>("sp_DoUong_GetAll");
            return new ObservableCollection<DoUong>(list);
            ///
        }
    }
}
