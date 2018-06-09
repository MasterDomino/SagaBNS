using SagaBNS.Core;
using System;
using System.Data;
using System.Data.Common;

namespace SagaBNS.Entity.Helpers
{
    public static class DataAccessHelper
    {
        #region Members

        private static SagaBNSContext _context;

        #endregion

        #region Properties

        private static SagaBNSContext Context => _context ?? (_context = CreateContext());

        #endregion

        #region Methods

        /// <summary>
        /// Begins and returns a new transaction. Be sure to commit/rollback/dispose this transaction
        /// or use it in an using-clause.
        /// </summary>
        /// <returns>A new transaction.</returns>
        public static DbTransaction BeginTransaction()
        {
            // an open connection is needed for a transaction
            if (Context.Database.Connection.State == ConnectionState.Broken || Context.Database.Connection.State == ConnectionState.Closed)
            {
                Context.Database.Connection.Open();
            }

            // begin and return new transaction
            return Context.Database.Connection.BeginTransaction();
        }

        /// <summary>
        /// Creates new instance of database context.
        /// </summary>
        public static SagaBNSContext CreateContext() => new SagaBNSContext();

        /// <summary>
        /// Disposes the current instance of database context.
        /// </summary>
        public static void DisposeContext()
        {
            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }
        }

        public static bool Initialize()
        {
            using (SagaBNSContext context = CreateContext())
            {
                try
                {
                    context.Database.Initialize(true);
                    context.Database.Connection.Open();

                    Logger.Info("Database Initialized");
                }
                catch (Exception ex)
                {
                    Logger.LogEventError("DATABASE_INITIALIZATION", "Database Error", ex);

                    //Logger.LogEventError("DATABASE_INITIALIZATION", "Database not up 2 date");
                    return false;
                }
                return true;
            }
        }

        #endregion
    }
}