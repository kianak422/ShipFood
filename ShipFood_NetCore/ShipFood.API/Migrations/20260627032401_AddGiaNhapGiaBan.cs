using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipFood.API.Migrations
{
    /// <inheritdoc />
    public partial class AddGiaNhapGiaBan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbDanhMuc",
                columns: table => new
                {
                    madanhmuc = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    tendanhmuc = table.Column<string>(type: "TEXT", nullable: true),
                    mota = table.Column<string>(type: "TEXT", nullable: true),
                    hinhanh = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbDanhMuc", x => x.madanhmuc);
                });

            migrationBuilder.CreateTable(
                name: "tbDonHang",
                columns: table => new
                {
                    madh = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ngaydathang = table.Column<DateTime>(type: "TEXT", nullable: false),
                    trangthai = table.Column<string>(type: "TEXT", nullable: false),
                    tongtien = table.Column<decimal>(type: "TEXT", nullable: false),
                    ghichu = table.Column<string>(type: "TEXT", nullable: true),
                    phiship = table.Column<decimal>(type: "TEXT", nullable: false),
                    mashipper = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbDonHang", x => x.madh);
                });

            migrationBuilder.CreateTable(
                name: "tbMonAn",
                columns: table => new
                {
                    mamon = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    tenmon = table.Column<string>(type: "TEXT", nullable: false),
                    mota = table.Column<string>(type: "TEXT", nullable: true),
                    giatien = table.Column<decimal>(type: "TEXT", nullable: false),
                    hinhanh = table.Column<string>(type: "TEXT", nullable: true),
                    maquanan = table.Column<int>(type: "INTEGER", nullable: true),
                    madanhmuc = table.Column<int>(type: "INTEGER", nullable: true),
                    soluongban = table.Column<int>(type: "INTEGER", nullable: false),
                    phantramgiam = table.Column<int>(type: "INTEGER", nullable: false),
                    ngaytao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    noibat = table.Column<bool>(type: "INTEGER", nullable: false),
                    GiaNhap = table.Column<decimal>(type: "TEXT", nullable: false),
                    GiaBan = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbMonAn", x => x.mamon);
                });

            migrationBuilder.CreateTable(
                name: "tbUser",
                columns: table => new
                {
                    userid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    username = table.Column<string>(type: "TEXT", nullable: true),
                    pwd = table.Column<string>(type: "TEXT", nullable: true),
                    loaitaikhoan = table.Column<string>(type: "TEXT", nullable: true),
                    sdt = table.Column<string>(type: "TEXT", nullable: true),
                    vitien = table.Column<decimal>(type: "TEXT", nullable: true),
                    email = table.Column<string>(type: "TEXT", nullable: true),
                    trangthai = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbUser", x => x.userid);
                });

            migrationBuilder.CreateTable(
                name: "TbVoucher",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    DiscountValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsPercent = table.Column<bool>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbVoucher", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbChiTietDonHang",
                columns: table => new
                {
                    mactdh = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    madh = table.Column<int>(type: "INTEGER", nullable: false),
                    mamon = table.Column<int>(type: "INTEGER", nullable: false),
                    soluong = table.Column<int>(type: "INTEGER", nullable: false),
                    dongia = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbChiTietDonHang", x => x.mactdh);
                    table.ForeignKey(
                        name: "FK_tbChiTietDonHang_tbDonHang_madh",
                        column: x => x.madh,
                        principalTable: "tbDonHang",
                        principalColumn: "madh",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbChiTietDonHang_tbMonAn_mamon",
                        column: x => x.mamon,
                        principalTable: "tbMonAn",
                        principalColumn: "mamon",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbTonKho",
                columns: table => new
                {
                    makho = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    mamon = table.Column<int>(type: "INTEGER", nullable: false),
                    soluongton = table.Column<int>(type: "INTEGER", nullable: false),
                    soluongnhap = table.Column<int>(type: "INTEGER", nullable: false),
                    ngaycapnhat = table.Column<DateTime>(type: "TEXT", nullable: true),
                    GiaNhap = table.Column<decimal>(type: "TEXT", nullable: false),
                    GiaBan = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbTonKho", x => x.makho);
                    table.ForeignKey(
                        name: "FK_tbTonKho_tbMonAn_mamon",
                        column: x => x.mamon,
                        principalTable: "tbMonAn",
                        principalColumn: "mamon",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbKhachHang",
                columns: table => new
                {
                    userid = table.Column<int>(type: "INTEGER", nullable: false),
                    tenkh = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbKhachHang", x => x.userid);
                    table.ForeignKey(
                        name: "FK_tbKhachHang_tbUser_userid",
                        column: x => x.userid,
                        principalTable: "tbUser",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbQuanAn",
                columns: table => new
                {
                    userid = table.Column<int>(type: "INTEGER", nullable: false),
                    tenquanan = table.Column<string>(type: "TEXT", nullable: true),
                    diachi = table.Column<string>(type: "TEXT", nullable: true),
                    soluotdanhgia = table.Column<int>(type: "INTEGER", nullable: true),
                    diemdanhgia = table.Column<decimal>(type: "TEXT", nullable: true),
                    trangthai = table.Column<string>(type: "TEXT", nullable: true),
                    hinhanh = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbQuanAn", x => x.userid);
                    table.ForeignKey(
                        name: "FK_tbQuanAn_tbUser_userid",
                        column: x => x.userid,
                        principalTable: "tbUser",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbShipper",
                columns: table => new
                {
                    userid = table.Column<int>(type: "INTEGER", nullable: false),
                    tenshipper = table.Column<string>(type: "TEXT", nullable: true),
                    diachi = table.Column<string>(type: "TEXT", nullable: true),
                    diemdanhgia = table.Column<decimal>(type: "TEXT", nullable: true),
                    soluotdanhgia = table.Column<int>(type: "INTEGER", nullable: true),
                    trangthai = table.Column<string>(type: "TEXT", nullable: true),
                    hinhanh = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbShipper", x => x.userid);
                    table.ForeignKey(
                        name: "FK_tbShipper_tbUser_userid",
                        column: x => x.userid,
                        principalTable: "tbUser",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbChiTietDonHang_madh",
                table: "tbChiTietDonHang",
                column: "madh");

            migrationBuilder.CreateIndex(
                name: "IX_tbChiTietDonHang_mamon",
                table: "tbChiTietDonHang",
                column: "mamon");

            migrationBuilder.CreateIndex(
                name: "IX_tbTonKho_mamon",
                table: "tbTonKho",
                column: "mamon");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbChiTietDonHang");

            migrationBuilder.DropTable(
                name: "tbDanhMuc");

            migrationBuilder.DropTable(
                name: "tbKhachHang");

            migrationBuilder.DropTable(
                name: "tbQuanAn");

            migrationBuilder.DropTable(
                name: "tbShipper");

            migrationBuilder.DropTable(
                name: "tbTonKho");

            migrationBuilder.DropTable(
                name: "TbVoucher");

            migrationBuilder.DropTable(
                name: "tbDonHang");

            migrationBuilder.DropTable(
                name: "tbUser");

            migrationBuilder.DropTable(
                name: "tbMonAn");
        }
    }
}
