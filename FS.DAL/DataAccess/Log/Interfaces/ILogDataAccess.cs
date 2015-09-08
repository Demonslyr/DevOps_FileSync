using FS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DataAccess.Log.Interfaces
{
    public interface ILogDataAccess
    {
        IEnumerable<History> GetQueryObj();
        bool CommitLogEntry(History log);
        List<History> ExecuteQuery(IEnumerable<History> result);

    }
}
