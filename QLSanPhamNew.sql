CREATE DATABASE QLSanPham  
USE QLSanPham 

CREATE TABLE LoaiSP(
    MaLoai  CHAR (2) PRIMARY KEY,
    TenLoai NVARCHAR (30) NOT NULL
);

CREATE TABLE Sanpham (
    MaSP CHAR (6) PRIMARY KEY,
    TenSP NVARCHAR(30) NOT NULL,
    Ngaynhap DateTime,
    MaLoai CHAR (2)
);

ALTER TABLE Sanpham
	ADD CONSTRAINT FK_Sanpham_LoaiSP FOREIGN KEY (MaLoai)
	REFERENCES LoaiSP (MaLoai);

INSERT INTO LoaiSP (MaLoai, TenLoai)
VALUES 
    ('L1', N'Điện thoại'),
    ('L2', N'Laptop');

SELECT * FROM LoaiSP

INSERT INTO Sanpham (MaSP, TenSP, Ngaynhap, MaLoai)
VALUES 
    ('SP0001', N'iPhone 15', '2024-12-01', 'L1'),
    ('SP0002', N'MacBook Pro', '2024-12-05', 'L2'),
    ('SP0003', N'Dell XPS 13', '2024-12-10', 'L2');

SELECT * FROM Sanpham


