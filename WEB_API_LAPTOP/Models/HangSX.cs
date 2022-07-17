using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_LAPTOP.Models
{
    [Table("HangSX")]
    public class HangSX
    {
        [Key]
        public int MAHANG { get; set; }
        public String? TENHANG { get; set; }
        public String? EMAIL { get; set; }
        public String? SDT { get; set; }
        public String? LOGO { get; set; }
    }
}

/*[MAHANG][int] NOT NULL,

    [TENHANG] [nvarchar] (50) NULL,
	[EMAIL][varchar] (30) NULL,
	[SDT][varchar] (50) NULL,*/