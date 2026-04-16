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
    internal class NhanVienService
    {
        public void DangKy(NhanVien nv)
        {
            try
            {
                StoreHelper.Execute("sp_NhanVien_DangKy", nv);
            }
            catch (Exception)
            {
                throw; 
            }
        }

        public NhanVien? DangNhap(DangNhap nv)
        {
            return StoreHelper.QueryFirstOrDefault<NhanVien>("sp_NhanVien_DangNhap", nv);
        }

        public List<NhanVien> GetAll()
        {
            return StoreHelper.QueryList<NhanVien>("sp_NhanVien_GetAll");
        }

        public NhanVien? GetById(int id)
        {
            return StoreHelper.QueryFirstOrDefault<NhanVien>("sp_NhanVien_GetById", new { ID = id });
        }

        public bool Insert(NhanVien nv)
        {
            return StoreHelper.Execute("sp_NhanVien_Insert", nv) > 0;
        }

        public bool Update(NhanVien nv)
        {
            return StoreHelper.Execute("sp_NhanVien_Update", nv) > 0;
        }

        public bool UpdatePassword(int id, string newHashedPassword)
        {
            return StoreHelper.Execute("sp_NhanVien_UpdatePassword", new { ID = id, MatKhau = newHashedPassword }) > 0;
        }

        public bool Delete(int id)
        {
            return StoreHelper.Execute("sp_NhanVien_Delete", new { ID = id }) > 0;
        }
    }
}
