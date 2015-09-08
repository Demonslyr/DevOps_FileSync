using FS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessMgmt.Log.Engines.Interfaces
{
    public interface ILogEngine
    {
        IEnumerable<History> CreateQuery();
        IEnumerable<History> LogTypeQuery(IEnumerable<History> query, SearchOptions options);
        IEnumerable<History> DateRangeQuery(IEnumerable<History> query, SearchOptions options);
        IEnumerable<History> FileOperationQuery(IEnumerable<History> query, SearchOptions options);
        IEnumerable<History> UserNameQuery(IEnumerable<History> query, SearchOptions options);
        List<History> ExecuteQuery(IEnumerable<History> query);
        bool CommitLog(History log);
        History CreateSyncLog(int interaction);
        History CreateFileChangeLog(int operation, string filePath);
        History CreateProgramInitLog(int operation, string filePath);
        History CreateErrorLog(string exception, int interaction, string filePath, int operation);

    }
}
