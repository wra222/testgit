--=======����R�OD:\SH\trunk\Code\Archivedata\ArchiveDB\bin\Debug\ArchiveDB.exe "PM"
 --= { "P", "M","PM","MAIL","FK","?"}; //P:Prepare data ; M:Move data ; PM: P+M ; Mail : Test mail
--1.�����]�ƾ� �ݭn�aFK �Ѽư���]�ɥ~�����Y�^
   select*from ArchiveAllFKConstraint
--2.�D��
   select*from ArchiveMainTable
--3.����
   select*from ArchiveTableList
--4.�~��
   select*from ArchiveFKTableList
--5 ���槹P �ZArchiveIDList �O���ݭn�઺DN PLT Product PCB.  ���\�Z�ק窱�A��OK
   select*from   ArchiveIDList 
--6.�Ĥ@����ƾڥi�Φp�USQL �d���઺�ƾڬO�_�@��ArchiveTableLog �O���������檺
--�Ҧ����`�ƩM��������ƶq
select b.TableName,b.ArchiveCount,a.datacount from 
(select  b.name as tablename ,  
        a.rowcnt as datacount  
from    sysindexes a ,  
        sysobjects b  
where   a.id = b.id  
        and a.indid < 2  
        and objectproperty(b.id, 'IsMSShipped') = 0   )as  a,
  HPIMES_TEST..ArchiveTableLog  b
  where a.tablename collate Chinese_Taiwan_Stroke_BIN =b.TableName and b.TimeStamp='201409051535'-- �� ArchiveTableLog �̫᪺201409051535
  
  