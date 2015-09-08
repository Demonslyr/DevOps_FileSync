using Microsoft.Synchronization;
using Microsoft.Synchronization.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessMgmt.File.Engines.Interfaces
{
    public interface IFileEngine
    {
        void CheckTargetDirExists(String rootPath);

        bool DetectChanges(FileSyncProvider provider);

        SyncOrchestrator CreateSyncOrchestrator(FileSyncProvider source, FileSyncProvider destination);
        SyncOrchestrator SetSyncDirection(SyncOrchestrator agent);
        bool Synchronize(SyncOrchestrator agent);

        FileSyncProvider CreateProvider(string RootPath, FileSyncScopeFilter filter, FileSyncOptions options);
        void DisposeProvider(FileSyncProvider provider);
        
        FileSyncOptions SetOptions();
        FileSyncScopeFilter CreateFilter();
        FileSyncProvider SetPreviewModeFlag(FileSyncProvider provider, bool flag);

        FileSyncProvider AttachSkippedChangeEventHandler(FileSyncProvider provider);
        FileSyncProvider AttachDetectedChangeEventHandler(FileSyncProvider provider);
        FileSyncProvider AttachApplyingChangeEventHandler(FileSyncProvider provider);
        FileSyncProvider AttachAppliedChangeEventHandler(FileSyncProvider provider);

    }
}
