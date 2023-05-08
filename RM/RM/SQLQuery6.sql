create table tblMain
(MainID int primary key identity,
aDate date,
aTime varchar (15),
status varchar (15),
OrderType varchar (15),
total float,
received float,
change float
)

create table tblDetails
(
DetailID int primary key identity,
MainID int,
proID int,
qty int,
price float,
amount float
)