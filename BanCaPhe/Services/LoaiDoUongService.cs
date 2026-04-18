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
    internal class LoaiDoUongService
    {
        public List<DanhMucDouong> GetAll()
        {
            return StoreHelper.QueryList<DanhMucDouong>("sp_LoaiDoUong_GetAll");
        }

        public bool Insert(DanhMucDouong loai)
        {
            return StoreHelper.Execute("sp_LoaiDoUong_Insert", loai) > 0;
        }

        public bool Update(DanhMucDouong loai)
        {
            return StoreHelper.Execute("sp_LoaiDoUong_Update", loai) > 0;
        }

        public bool Delete(int id)
        {
            return StoreHelper.Execute("sp_LoaiDoUong_Delete", new { ID = id }) > 0;
        }

        public bool IsTenLoaiExists(string tenLoai, int ViTri)
        {
            int count = StoreHelper.ExecuteScalar<int>(
                "sp_LoaiDoUong_CheckTenLoai",
                new { TenLoai = tenLoai, ViTri = ViTri });
            return count > 0;
        }

        public ObservableCollection<DanhMucDouong> GetAllND()
        {
            using (IDbConnection conn = DoUongDbConnection.GetConnection())
            {
                string sql = "SELECT ID, TenLoai, ViTri FROM LoaiDoUong ORDER BY ViTri";
                return new ObservableCollection<DanhMucDouong>(conn.Query<DanhMucDouong>(sql));
            }
        }
    }
}
