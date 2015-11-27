if not exists(select * from sys.databases where name = 'AngularJSAuthentication')
begin
create database AngularJSAuthentication
end
go
use AngularJSAuthentication
go
if exists(select * from sys.tables where name = 'Account')
drop table Account
go
create table Account
(
    Id int not null identity(1, 1) primary key
    , Name varchar(100)
    , Password varchar(1000)
    , Provider varchar(50)
    , ProviderKey varchar(50)
)
go
insert into Account values('Dan Facebook', '', 'Facebook', '557816437704297')
insert into Account values('Dan Google', '', 'Google', '100479831828236018110')
insert into Account values('Dan', 'password', '', '')
