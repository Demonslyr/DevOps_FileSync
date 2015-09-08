using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.DataAccess.Log.Implementations;
using DAL.DataAccess.Log.Interfaces;
using FS.Data;
using System.Diagnostics;

namespace BusinessMgmt.Log.Engines.Implementations
{
    public class LogEngine : Interfaces.ILogEngine
    {
        private ILogDataAccess _logDataAccess;
        public LogEngine(ILogDataAccess logDataAccess) 
        {
            this._logDataAccess = logDataAccess;
        }
        public bool CommitLog(History log)
        {
            _logDataAccess.CommitLogEntry(log);
            return true;
        }
        public List<History> ExecuteQuery(IEnumerable<History> query)
        {
            return _logDataAccess.ExecuteQuery(query);
        }
        public IEnumerable<History> CreateQuery()
        {
            //throw new NotImplementedException();
            return _logDataAccess.GetQueryObj();          
        }

        public IEnumerable<History> LogTypeQuery(IEnumerable<History> query, SearchOptions options)
        {
            //throw new NotImplementedException();
            if (options.LogTypeId == null) 
            {
                return query;
            }
            return query.Where(x => x.LogTypeId == options.LogTypeId.Value);
        }

        public IEnumerable<History> DateRangeQuery(IEnumerable<History> query, SearchOptions options)
        {
            DateTime _start;
            DateTime _end;

            //throw new NotImplementedException();
            if ((options.StartDate == null)&&(options.StopDate == null)) 
            {
                return query;
            }
            else if (options.StartDate == null)
            {
                _end = DateTime.Now.AddDays(options.StopDate.Value);
                return query.Where(x => x.LogDate <= _end);
            }
            else if (options.StopDate == null) 
            {
                _start = DateTime.Now.AddDays(options.StartDate.Value);
                return query.Where(x => x.LogDate >= _start);
            }
            else if ((options.StartDate == 0) && (options.StopDate == 0))//maybe not necessary
            {
                _start = DateTime.Now;                
                return query.Where(x => x.LogDate == _start);
            }
            else
            {
                _start = DateTime.Now.AddDays(options.StartDate.Value);
                _end = DateTime.Now.AddDays(options.StopDate.Value);
                return query.Where(x => x.LogDate >= _start)
                            .Where(x => x.LogDate <= _end);
            }
        }

        public IEnumerable<History> FileOperationQuery(IEnumerable<History> query, SearchOptions options)
        {
            //throw new NotImplementedException();
            if (options.FileOperationId == null)
            {
                return query;
            }
            return query.Where(x => x.FileOperationId == options.FileOperationId.Value);
        }

        public IEnumerable<History> UserNameQuery(IEnumerable<History> query, SearchOptions options)
        {
            //throw new NotImplementedException();
            if (options.UserName == "")//check case for empty string too
            {
                return query;
            } 
           return query.Where(x => x.UserName == options.UserName);
        }

        public History CreateSyncLog(int interaction)
        {
            //throw new NotImplementedException();
            try
            {
                var log = new History
                {
                    LogTypeId = 5,
                    InteractionTypeId = interaction,
                    Domain = Environment.UserDomainName,
                    UserName = Environment.UserName,
                    PCName = Environment.MachineName,
                    FileName = null,
                    FileOperationId = null,
                    ErrorMsg = null
                };
                return log;

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }

        }



        public History CreateFileChangeLog(int operation, string filePath)
        {
            //throw new NotImplementedException();
            try
            {
                var log = new History
                {
                    LogTypeId = 5,
                    InteractionTypeId = 9,
                    Domain = Environment.UserDomainName,
                    UserName = Environment.UserName,
                    PCName = Environment.MachineName,
                    FileName = filePath,
                    FileOperationId = operation,
                    ErrorMsg = null,
                };
                return log;

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }


        }

        public History CreateProgramInitLog(int operation, string filePath)
        {
            //throw new NotImplementedException();
            try
            {
                var log = new History
                {
                    LogTypeId = 2,
                    InteractionTypeId = 9,
                    Domain = Environment.UserDomainName,
                    UserName = Environment.UserName,
                    PCName = Environment.MachineName,
                    FileName = null,
                    FileOperationId = null,
                    ErrorMsg = null
                };

                return log;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }

        }

        public History CreateErrorLog(string exception, int interaction, string filePath, int operation)
        {
            //throw new NotImplementedException();
            try
            {
                var log = new History
                {
                    LogTypeId = 0,
                    InteractionTypeId = interaction,
                    Domain = Environment.UserDomainName,
                    UserName = Environment.UserName,
                    PCName = Environment.MachineName,
                    FileName = filePath,
                    FileOperationId = operation,
                    ErrorMsg = exception,
                };
                return log;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }


        }
    }
}

