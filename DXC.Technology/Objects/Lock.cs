using System;
using System.Collections.Generic;

namespace DXC.Technology.Objects
{
    /// <summary>
    /// Represents a lock object.
    /// </summary>
    public class Lock
    {
    }

    /// <summary>
    /// Represents a lock associated with a specific key.
    /// </summary>
    public class KeyedLock
    {
        #region Instance Fields

        /// <summary>
        /// The key associated with the lock.
        /// </summary>
        public long Key { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedLock"/> class with the specified key.
        /// </summary>
        /// <param name="key">The key for the lock.</param>
        public KeyedLock(long key)
        {
            Key = key;
        }

        #endregion
    }

    /// <summary>
    /// Manages a collection of keyed locks.
    /// </summary>
    public class KeyedLocks
    {
        #region Static Fields

        /// <summary>
        /// Static dictionary to store keyed locks.
        /// </summary>
        private static Dictionary<long, KeyedLock> locks = new Dictionary<long, KeyedLock>();

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Retrieves or creates a keyed lock for the given key.
        /// </summary>
        /// <param name="key">The key for the lock.</param>
        /// <returns>A keyed lock associated with the given key.</returns>
        public static KeyedLock GetKeyedLock(long key)
        {
            if (locks.Count > 500)
            {
                lock (locks)
                {
                    if (locks.Count > 500)
                    {
                        locks = new Dictionary<long, KeyedLock>();
                    }
                }
            }

            lock (locks)
            {
                if (locks.ContainsKey(key))
                    return locks[key];
                else
                {
                    var newLock = new KeyedLock(key);
                    locks.Add(key, newLock);
                    return locks[key];
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Represents a lock associated with a specific string key.
    /// </summary>
    public class KeyedStringLock
    {
        #region Instance Fields

        /// <summary>
        /// The key associated with the string lock.
        /// </summary>
        public string Key { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedStringLock"/> class with the specified key.
        /// </summary>
        /// <param name="key">The key for the string lock.</param>
        public KeyedStringLock(string key)
        {
            Key = key;
        }

        #endregion
    }

    /// <summary>
    /// Manages a collection of keyed string locks.
    /// </summary>
    public class KeyedStringLocks
    {
        #region Static Fields

        /// <summary>
        /// Static dictionary to store keyed string locks.
        /// </summary>
        private static Dictionary<string, KeyedStringLock> locks = new Dictionary<string, KeyedStringLock>();

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Retrieves or creates a keyed string lock for the given key.
        /// </summary>
        /// <param name="key">The key for the string lock.</param>
        /// <returns>A keyed string lock associated with the given key.</returns>
        public static KeyedStringLock GetKeyedLock(string key)
        {
            if (locks.Count > 500)
            {
                lock (locks)
                {
                    if (locks.Count > 500)
                    {
                        locks = new Dictionary<string, KeyedStringLock>();
                    }
                }
            }

            lock (locks)
            {
                if (locks.ContainsKey(key))
                    return locks[key];
                else
                {
                    var newLock = new KeyedStringLock(key);
                    locks.Add(key, newLock);
                    return locks[key];
                }
            }
        }

        #endregion
    }
}