using BusinessMgmt.Log.Engines.Interfaces;
using BusinessMgmt.Log.Manager.Interfaces;
using FS.Data;
using System;
using System.Collections.Generic;
//using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using DAL.DataAccess.Log.Interfaces;
using System.Diagnostics;


namespace BusinessMgmt.Log.Manager.Implementations
{
    public class LogManager : Interfaces.ILogManager
    {
        private ILogEngine _logEngine;
        public LogManager(ILogEngine logEngine) 
        {
            this._logEngine = logEngine;
        }

        public List<History> RunSampleQueries() 
        {
            throw new NotImplementedException();
            //return _logEngine.ExecuteQuery(_logEngine.DateRangeQuery(_logEngine.UserNameQuery(_logEngine.LogTypeQuery(_logEngine.CreateQuery(),5),"drmurtha"),-2,0));
        }

        private SearchOptions DecodeQuery(string[] args) 
        {
            throw new NotImplementedException();
        }
        public List<History> PerformQuery(SearchOptions options)
        {

            IEnumerable<History> result = _logEngine.CreateQuery();

            try
            {
                result = _logEngine.LogTypeQuery(result, options); 
                result = _logEngine.FileOperationQuery(result, options); 
                result = _logEngine.UserNameQuery(result, options); 
                result = _logEngine.DateRangeQuery(result, options);
            }
            catch (Exception)
            {
                
                throw;
            }

            return _logEngine.ExecuteQuery(result);
        }
        public bool CreateSyncLog(int interaction)
        {
            try
            {
                History result = _logEngine.CreateSyncLog(interaction);
                _logEngine.CommitLog(result);
                return true;
            }
            catch (Exception e)
            {
                CreateErrorLog(e.ToString() + " with interaction " + interaction, 8, null, 4);
                return false;
            }
        }

        public bool CreateFileChangeLog(int operation, string filePath)
        {
            try
            {
                History result = _logEngine.CreateFileChangeLog(operation, filePath);
                _logEngine.CommitLog(result);
                return true;
            }
            catch (Exception e)
            {
                CreateErrorLog(e.ToString() + " with operation " + operation + " at " + filePath, 9, filePath, 5);
                return false;
            }
        }

        public bool CreateProgramInitLog(int operation, string filePath) 
        {
            try
            {
                History result = _logEngine.CreateProgramInitLog(operation, filePath);
                _logEngine.CommitLog(result);
                return true;
            }
            catch (Exception e)
            {
                CreateErrorLog(e.ToString() + " with operation " + operation + " at " + filePath, 11, filePath, 5);
                return false;
            }
        }

        public bool CreateErrorLog(string exception, int interaction, string filePath, int operation)
        {
            try
            {
                History result = _logEngine.CreateErrorLog(exception, interaction, filePath, operation);
                _logEngine.CommitLog(result);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                // Make this bubble up to the WFE and maybe go to Windows Event Log
                return false;
            }
        }
    }
}
