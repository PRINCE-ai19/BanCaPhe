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
    internal class ToppingService
    {
        public List<Topping> GetAll()
        {
            return StoreHelper.QueryList<Topping>("sp_Topping_GetAll");
        }

        public List<Topping> GetAllAdmin()
        {
            return StoreHelper.QueryList<Topping>("sp_Topping_GetAllAdmin");
        }

        public Topping? GetById(int id)
        {
            return StoreHelper.QueryFirstOrDefault<Topping>("sp_Topping_GetById", new { ID = id });
        }

        public bool Insert(Topping t)
        {
            return StoreHelper.Execute("sp_Topping_Insert", t) > 0;
        }

        public bool Update(Topping t)
        {
            return StoreHelper.Execute("sp_Topping_Update", t) > 0;
        }

        public bool Delete(int id)
        {
            return StoreHelper.Execute("sp_Topping_Delete", new { ID = id }) > 0;
        }

        public bool Disable(int id)
        {
            return StoreHelper.Execute("sp_Topping_Disable", new { ID = id }) > 0;
        }
    }
}
