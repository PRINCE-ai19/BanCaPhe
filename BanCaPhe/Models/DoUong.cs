using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Models
{
    public class DoUong
    {
        public int ID { get; set; }

        [StringLength(100)]
        public string TenDoUong { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Gia { get; set; }
        public bool ConBan { get; set; }

        public int? LoaiID { get; set; }
        public string? TenLoai { get; set; }

        public string? Mota { get; set; }    
        public string HinhAnh { get; set; }


       
    }
}
