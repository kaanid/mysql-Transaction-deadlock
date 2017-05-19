# mysql: Deadlock found when trying to get lock; try restarting transaction

IsolationLevel.Chaos 不支持
IsolationLevel.Snapshot 不支持
IsolationLevel.Serializable 死锁
IsolationLevel.Unspecified 死锁
other ok.
https://msdn.microsoft.com/zh-cn/library/system.transactions.isolationlevel(v=vs.110).aspx

## TransactionScope
default:IsolationLevel.ReadCommitted



## BeginTransaction
Dapper:
default:IsolationLevel.ReadCommitted

Testing.......
