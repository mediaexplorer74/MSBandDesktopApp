// Decompiled with JetBrains decompiler
// Type: SQLite.SQLite3
// Assembly: SQLite-net, Version=1.0.9.0, Culture=neutral, PublicKeyToken=null
// MVID: 3C087C9E-3E7A-4EB5-8100-289515B40443
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\SQLite_net.dll

using SQLitePCL;
using System;

namespace SQLite
{
  public static class SQLite3
  {
    public static SQLite3.Result Open(string filename, out sqlite3 db) => (SQLite3.Result) raw.sqlite3_open(filename, ref db);

    public static SQLite3.Result Open(
      string filename,
      out sqlite3 db,
      int flags,
      IntPtr zVfs)
    {
      return (SQLite3.Result) raw.sqlite3_open_v2(filename, ref db, flags, (string) null);
    }

    public static SQLite3.Result Close(sqlite3 db) => (SQLite3.Result) raw.sqlite3_close(db);

    public static SQLite3.Result BusyTimeout(sqlite3 db, int milliseconds) => (SQLite3.Result) raw.sqlite3_busy_timeout(db, milliseconds);

    public static int Changes(sqlite3 db) => raw.sqlite3_changes(db);

    public static sqlite3_stmt Prepare2(sqlite3 db, string query)
    {
      sqlite3_stmt sqlite3Stmt = (sqlite3_stmt) null;
      int num = raw.sqlite3_prepare_v2(db, query, ref sqlite3Stmt);
      if (num != 0)
        throw SQLiteException.New((SQLite3.Result) num, SQLite3.GetErrmsg(db));
      return sqlite3Stmt;
    }

    public static SQLite3.Result Step(sqlite3_stmt stmt) => (SQLite3.Result) raw.sqlite3_step(stmt);

    public static SQLite3.Result Reset(sqlite3_stmt stmt) => (SQLite3.Result) raw.sqlite3_reset(stmt);

    public static SQLite3.Result Finalize(sqlite3_stmt stmt) => (SQLite3.Result) raw.sqlite3_finalize(stmt);

    public static long LastInsertRowid(sqlite3 db) => raw.sqlite3_last_insert_rowid(db);

    public static string GetErrmsg(sqlite3 db) => raw.sqlite3_errmsg(db);

    public static int BindParameterIndex(sqlite3_stmt stmt, string name) => raw.sqlite3_bind_parameter_index(stmt, name);

    public static int BindNull(sqlite3_stmt stmt, int index) => raw.sqlite3_bind_null(stmt, index);

    public static int BindInt(sqlite3_stmt stmt, int index, int val) => raw.sqlite3_bind_int(stmt, index, val);

    public static int BindInt64(sqlite3_stmt stmt, int index, long val) => raw.sqlite3_bind_int64(stmt, index, val);

    public static int BindDouble(sqlite3_stmt stmt, int index, double val) => raw.sqlite3_bind_double(stmt, index, val);

    public static int BindText(sqlite3_stmt stmt, int index, string val, int n, IntPtr free) => raw.sqlite3_bind_text(stmt, index, val);

    public static int BindBlob(sqlite3_stmt stmt, int index, byte[] val, int n, IntPtr free) => raw.sqlite3_bind_blob(stmt, index, val);

    public static int ColumnCount(sqlite3_stmt stmt) => raw.sqlite3_column_count(stmt);

    public static string ColumnName(sqlite3_stmt stmt, int index) => raw.sqlite3_column_name(stmt, index);

    public static string ColumnName16(sqlite3_stmt stmt, int index) => raw.sqlite3_column_name(stmt, index);

    public static SQLite3.ColType ColumnType(sqlite3_stmt stmt, int index) => (SQLite3.ColType) raw.sqlite3_column_type(stmt, index);

    public static int ColumnInt(sqlite3_stmt stmt, int index) => raw.sqlite3_column_int(stmt, index);

    public static long ColumnInt64(sqlite3_stmt stmt, int index) => raw.sqlite3_column_int64(stmt, index);

    public static double ColumnDouble(sqlite3_stmt stmt, int index) => raw.sqlite3_column_double(stmt, index);

    public static string ColumnText(sqlite3_stmt stmt, int index) => raw.sqlite3_column_text(stmt, index);

    public static string ColumnText16(sqlite3_stmt stmt, int index) => raw.sqlite3_column_text(stmt, index);

    public static byte[] ColumnBlob(sqlite3_stmt stmt, int index) => raw.sqlite3_column_blob(stmt, index);

    public static int ColumnBytes(sqlite3_stmt stmt, int index) => raw.sqlite3_column_bytes(stmt, index);

    public static string ColumnString(sqlite3_stmt stmt, int index) => raw.sqlite3_column_text(stmt, index);

    public static byte[] ColumnByteArray(sqlite3_stmt stmt, int index) => SQLite3.ColumnBlob(stmt, index);

    public static SQLite3.ExtendedResult ExtendedErrCode(sqlite3 db) => (SQLite3.ExtendedResult) raw.sqlite3_extended_errcode(db);

    public enum Result
    {
      OK = 0,
      Error = 1,
      Internal = 2,
      Perm = 3,
      Abort = 4,
      Busy = 5,
      Locked = 6,
      NoMem = 7,
      ReadOnly = 8,
      Interrupt = 9,
      IOError = 10, // 0x0000000A
      Corrupt = 11, // 0x0000000B
      NotFound = 12, // 0x0000000C
      Full = 13, // 0x0000000D
      CannotOpen = 14, // 0x0000000E
      LockErr = 15, // 0x0000000F
      Empty = 16, // 0x00000010
      SchemaChngd = 17, // 0x00000011
      TooBig = 18, // 0x00000012
      Constraint = 19, // 0x00000013
      Mismatch = 20, // 0x00000014
      Misuse = 21, // 0x00000015
      NotImplementedLFS = 22, // 0x00000016
      AccessDenied = 23, // 0x00000017
      Format = 24, // 0x00000018
      Range = 25, // 0x00000019
      NonDBFile = 26, // 0x0000001A
      Notice = 27, // 0x0000001B
      Warning = 28, // 0x0000001C
      Row = 100, // 0x00000064
      Done = 101, // 0x00000065
    }

    public enum ExtendedResult
    {
      BusyRecovery = 261, // 0x00000105
      LockedSharedcache = 262, // 0x00000106
      ReadonlyRecovery = 264, // 0x00000108
      IOErrorRead = 266, // 0x0000010A
      CorruptVTab = 267, // 0x0000010B
      CannottOpenNoTempDir = 270, // 0x0000010E
      ConstraintCheck = 275, // 0x00000113
      NoticeRecoverWAL = 283, // 0x0000011B
      AbortRollback = 516, // 0x00000204
      ReadonlyCannotLock = 520, // 0x00000208
      IOErrorShortRead = 522, // 0x0000020A
      CannotOpenIsDir = 526, // 0x0000020E
      ConstraintCommitHook = 531, // 0x00000213
      NoticeRecoverRollback = 539, // 0x0000021B
      ReadonlyRollback = 776, // 0x00000308
      IOErrorWrite = 778, // 0x0000030A
      CannotOpenFullPath = 782, // 0x0000030E
      ConstraintForeignKey = 787, // 0x00000313
      IOErrorFsync = 1034, // 0x0000040A
      ConstraintFunction = 1043, // 0x00000413
      IOErrorDirFSync = 1290, // 0x0000050A
      ConstraintNotNull = 1299, // 0x00000513
      IOErrorTruncate = 1546, // 0x0000060A
      ConstraintPrimaryKey = 1555, // 0x00000613
      IOErrorFStat = 1802, // 0x0000070A
      ConstraintTrigger = 1811, // 0x00000713
      IOErrorUnlock = 2058, // 0x0000080A
      ConstraintUnique = 2067, // 0x00000813
      IOErrorRdlock = 2314, // 0x0000090A
      ConstraintVTab = 2323, // 0x00000913
      IOErrorDelete = 2570, // 0x00000A0A
      IOErrorBlocked = 2826, // 0x00000B0A
      IOErrorNoMem = 3082, // 0x00000C0A
      IOErrorAccess = 3338, // 0x00000D0A
      IOErrorCheckReservedLock = 3594, // 0x00000E0A
      IOErrorLock = 3850, // 0x00000F0A
      IOErrorClose = 4106, // 0x0000100A
      IOErrorDirClose = 4362, // 0x0000110A
      IOErrorSHMOpen = 4618, // 0x0000120A
      IOErrorSHMSize = 4874, // 0x0000130A
      IOErrorSHMLock = 5130, // 0x0000140A
      IOErrorSHMMap = 5386, // 0x0000150A
      IOErrorSeek = 5642, // 0x0000160A
      IOErrorDeleteNoEnt = 5898, // 0x0000170A
      IOErrorMMap = 6154, // 0x0000180A
    }

    public enum ConfigOption
    {
      SingleThread = 1,
      MultiThread = 2,
      Serialized = 3,
    }

    public enum ColType
    {
      Integer = 1,
      Float = 2,
      Text = 3,
      Blob = 4,
      Null = 5,
    }
  }
}
