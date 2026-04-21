using BanCaPhe.Helpers;
using BanCaPhe.Models;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace BanCaPhe.Services
{
    public class KhachHangService
    {
        public KhachHang Register(string hoTen, string soDienThoai)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@HoTen", hoTen);
            parameters.Add("@SoDienThoai", soDienThoai);

            return StoreHelper.QueryFirstOrDefault<KhachHang>("sp_RegisterKhachHang", parameters);
        }

        public KhachHang GetByPhone(string soDienThoai)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@SoDienThoai", soDienThoai);

            return StoreHelper.QueryFirstOrDefault<KhachHang>("sp_GetKhachHangByPhone", parameters);
        }

        public List<KhachHang> SearchByPhone(string soDienThoai)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@SoDienThoai", soDienThoai);

            return StoreHelper.QueryList<KhachHang>("sp_SearchKhachHangByPhone", parameters);
        }

        public KhachHang Update(KhachHang kh)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ID", kh.ID);
            parameters.Add("@HoTen", kh.HoTen);
            parameters.Add("@SoDienThoai", kh.SoDienThoai);

            return StoreHelper.QueryFirstOrDefault<KhachHang>("sp_UpdateKhachHang", parameters);
        }

        public void Delete(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ID", id);

            StoreHelper.Execute("sp_DeleteKhachHang", parameters);
        }
    }
}
