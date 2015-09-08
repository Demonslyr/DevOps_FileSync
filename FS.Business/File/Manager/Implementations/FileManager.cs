using System.Runtime.InteropServices;
using BusinessMgmt.File.Engines.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Ninject;
using System.Linq;
using System.Diagnostics;
using System.Collections;

using BusinessMgmt.File.Engines.Implementations;
using BusinessMgmt.File.Engines.Interfaces;
using BusinessMgmt.File.Manager.Implementations;
using BusinessMgmt.File.Manager.Interfaces;
using BusinessMgmt.Log.Engines.Implementations;
using BusinessMgmt.Log.Engines.Interfaces;
using BusinessMgmt.Log.Manager.Implementations;
using BusinessMgmt.Log.Manager.Interfaces;
using BusinessMgmt.Timer.Manager.Implementations;
using BusinessMgmt.Timer.Manager.Interfaces;
using BusinessMgmt.Timer.Engines.Implementations;
using BusinessMgmt.Timer.Engines.Interfaces;
using DAL.DataAccess.Log.Implementations;
using DAL.DataAccess.Log.Interfaces;
using FS.Data;
using System.Collections.Generic;
using Microsoft.Synchronization.Files;
using Microsoft.Synchronization;

namespace BusinessMgmt.File.Manager.Implementations
{

    public class FileManager : Interfaces.IFileManager
    {
        private IFileEngine _fileEngine;
        public FileManager(IFileEngine fileEngine) 
        {
            this._fileEngine = fileEngine;
        }

        public bool Sync(string sDir, string tDir, FileSyncScopeFilter filter, FileSyncOptions options)
        {
            try
            {
                _fileEngine.CheckTargetDirExists(tDir);
                var SourceProvider = _fileEngine.CreateProvider(sDir, filter, options);
                var DestinationProvider = _fileEngine.CreateProvider(tDir, filter, options);
                _fileEngine.DetectChanges(SourceProvider);
                SourceProvider = _fileEngine.CreateProvider(sDir, filter, options);
                SourceProvider = _fileEngine.SetPreviewModeFlag(SourceProvider, true);
                DestinationProvider = _fileEngine.SetPreviewModeFlag(DestinationProvider, true);
                DestinationProvider = _fileEngine.AttachApplyingChangeEventHandler(DestinationProvider);
                DestinationProvider = _fileEngine.AttachAppliedChangeEventHandler(DestinationProvider);
                DestinationProvider = _fileEngine.AttachSkippedChangeEventHandler(DestinationProvider);
                var agent = _fileEngine.CreateSyncOrchestrator(SourceProvider, DestinationProvider);
                agent = _fileEngine.SetSyncDirection(agent);
                _fileEngine.Synchronize(agent);
                return true;
            }
            catch (Exception e)
            {
                // log teh error mon
                return false;
            }
        }

    }
}
