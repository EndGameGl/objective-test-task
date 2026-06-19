using System;
using System.Collections.Generic;
using System.Text;

namespace ConcurrentIncrementer;

internal static class ServerImplementation
{
    private static int _counter = 0;
    private static readonly ReaderWriterLockSlim _rwls = new();

    public static void AddToCount(int value)
    {
        try
        {
            _rwls.EnterWriteLock();
            _counter += value;
        }
        finally
        {
            _rwls.ExitWriteLock();
        }
    }

    public static int GetCount()
    {
        try
        {
            _rwls.EnterReadLock();
            return _counter;
        }
        finally
        {
            _rwls.ExitReadLock();
        }
    }
}
