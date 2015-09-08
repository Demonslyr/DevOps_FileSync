using BusinessMgmt.Log.Engines.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FS.Data;

namespace BusinessMgmt.Log.Manager.Interfaces
{
    public interface ILogManager
    {
        List<History> RunSampleQueries();
        List<History> PerformQuery(SearchOptions options);
        bool CreateErrorLog(string exception, int interaction, string filePath, int operation);
        bool CreateProgramInitLog(int operation, string filePath);

    }
}
