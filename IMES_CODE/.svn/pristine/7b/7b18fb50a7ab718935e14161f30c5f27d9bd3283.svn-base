--=======執行命令D:\SH\trunk\Code\Archivedata\ArchiveDB\bin\Debug\ArchiveDB.exe "PM"
 --= { "P", "M","PM","MAIL","FK","?"}; //P:Prepare data ; M:Move data ; PM: P+M ; Mail : Test mail
--1.首次跑數據 需要帶FK 參數執行（賽外建關係）
   select*from ArchiveAllFKConstraint
--2.主表
   select*from ArchiveMainTable
--3.次表
   select*from ArchiveTableList
--4.外鍵
   select*from ArchiveFKTableList
--5 執行完P 后ArchiveIDList 記錄需要轉的DN PLT Product PCB.  成功后修改狀態為OK
   select*from   ArchiveIDList 
--6.第一次轉數據可用如下SQL 查看轉的數據是否一樣ArchiveTableLog 記錄本次執行的
--所有表總數和本次結轉數量
select b.TableName,b.ArchiveCount,a.datacount from 
(select  b.name as tablename ,  
        a.rowcnt as datacount  
from    sysindexes a ,  
        sysobjects b  
where   a.id = b.id  
        and a.indid < 2  
        and objectproperty(b.id, 'IsMSShipped') = 0   )as  a,
  HPIMES_TEST..ArchiveTableLog  b
  where a.tablename collate Chinese_Taiwan_Stroke_BIN =b.TableName and b.TimeStamp='201409051535'-- 取 ArchiveTableLog 最後的201409051535
  
  