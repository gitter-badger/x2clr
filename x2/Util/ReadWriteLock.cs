﻿// Copyright (c) 2013-2015 Jae-jun Kang
// See the file LICENSE for details.

using System;
using System.Threading;

namespace x2
{
    /// <summary>
    /// Provides a disposable read lock.
    /// </summary>
    public class ReadLock : IDisposable
    {
        private ReaderWriterLockSlim rwlock;

        /// <summary>
        /// Initializes a new instance of the ReadLock class to acquire a read
        /// lock based on the specified ReaderWriterLockSlim object.
        /// </summary>
        public ReadLock(ReaderWriterLockSlim rwlock)
        {
            this.rwlock = rwlock;
            rwlock.EnterReadLock();
        }

        /// <summary>
        /// Releases the read lock held by this object.
        /// </summary>
        public void Dispose()
        {
            rwlock.ExitReadLock();
        }
    }

    /// <summary>
    /// Provides a disposable read lock that is upgradable.
    /// </summary>
    public class UpgradeableReadLock : IDisposable
    {
        private ReaderWriterLockSlim rwlock;

        /// <summary>
        /// Initializes a new instance of the UpgradeableReadLock class to
        /// acquire an upgradeable read lock based on the specified
        /// ReaderWriterLockSlim object.
        /// </summary>
        public UpgradeableReadLock(ReaderWriterLockSlim rwlock)
        {
            this.rwlock = rwlock;
            rwlock.EnterUpgradeableReadLock();
        }

        /// <summary>
        /// Releases the upgradeable read lock held by this object.
        /// </summary>
        public void Dispose()
        {
            rwlock.ExitUpgradeableReadLock();
        }
    }

    /// <summary>
    /// Provides a disposable write lock.
    /// </summary>
    public class WriteLock : IDisposable
    {
        private ReaderWriterLockSlim rwlock;

        /// <summary>
        /// Initializes a new instance of the WriteLock class to acquire a write
        /// lock based on the specified ReaderWriterLockSlim object.
        /// </summary>
        public WriteLock(ReaderWriterLockSlim rwlock)
        {
            this.rwlock = rwlock;
            rwlock.EnterWriteLock();
        }

        /// <summary>
        /// Releases the write lock held by this object.
        /// </summary>
        public void Dispose()
        {
            rwlock.ExitWriteLock();
        }
    }
}
