using DAL.DataAccess.Log.Interfaces;
using FS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DAL.DataAccess.Log.Implementations
{
    public class LogDataAccess : ILogDataAccess
    {
        public IEnumerable<History> GetQueryObj() 
        {
            return new FileSyncLoggingEntities().Histories.AsEnumerable();
        }

        public bool CommitLogEntry(History log)
        {
            //throw new NotImplementedException();
            using (var db = new FileSyncLoggingEntities())
            {
                try
                {
                    db.Histories.Add(log);

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException dbEx)
                    {
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }

                return true;
            }
        }

        public List<History> ExecuteQuery(IEnumerable<History> result)
        {
            //throw new NotImplementedException();
            return result.ToList();
        }
    }
}
