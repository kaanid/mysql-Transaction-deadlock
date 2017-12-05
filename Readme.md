# mysql: Deadlock found when trying to get lock; try restarting transaction

IsolationLevel.Chaos 不支持

IsolationLevel.Snapshot 不支持

IsolationLevel.Serializable 死锁

IsolationLevel.Unspecified 死锁

other ok.

https://msdn.microsoft.com/zh-cn/library/system.transactions.isolationlevel(v=vs.110).aspx


## TransactionScope
default:IsolationLevel.Serializable



## BeginTransaction
Dapper:
default:IsolationLevel.ReadCommitted

Testing.......

### MySQl 查看
SHOW INNODB STATUS;

命令后，在LASTEST DETECTED DEADLOCK节会看到最近发生的一个（最近的一个）死锁信息，在里面可以找到发生死锁的线程ID和SQL语句。
