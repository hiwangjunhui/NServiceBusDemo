create database PdpDb
go
use PdpDb
go

drop table T_Customer
--create table custormer
create table T_Customer(
Id uniqueidentifier primary key, -- 'the primary key'
CustomerName varchar(20) not null, -- 'customer name'
CustomerType varchar(20) not null, -- 'customer type'
[Status] int not null default 0
)