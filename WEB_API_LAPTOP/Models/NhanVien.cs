﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_LAPTOP.Models
{
    [Table("NHANVIEN")]
    public class NhanVien
    {
        [Key]
        public String MANV { get; set; }
        public String EMAIL { get; set; }
        public String TEN { get; set; }
        public DateTime NGAYSINH { get; set; }
        public String SDT { get; set; }
        public String? TENDANGNHAP { get; set; }
    }

    public class NhanVienView
    {
        public String MANV { get; set; }
        public String EMAIL { get; set; }
        public String TEN { get; set; }
        public DateTime NGAYSINH { get; set; }
        public String SDT { get; set; }
        public String? TENDANGNHAP { get; set; }
        public int? MAQUYEN { get; set; }
        public String? TENQUYEN { get; set; }
        public bool? KICHHOAT { get; set; }
    }
}