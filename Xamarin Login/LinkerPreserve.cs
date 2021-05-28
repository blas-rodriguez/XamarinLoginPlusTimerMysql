using Akavache.Sqlite3;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin_Login
{
    public static class LinkerPreserve
    {
        static LinkerPreserve()
        {
            var persistentName = typeof(SQLitePersistentBlobCache).FullName;
            var encryptedName = typeof(SQLiteEncryptedBlobCache).FullName;
        }
    }
}