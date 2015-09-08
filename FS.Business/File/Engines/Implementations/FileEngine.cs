using BusinessMgmt.Log.Manager.Implementations;
using BusinessMgmt.Log.Manager.Interfaces;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BusinessMgmt.File.Engines.Implementations
{
    public class FileEngine : Interfaces.IFileEngine
    {


        private ILogManager _logManager;

        public FileEngine(ILogManager logManager)
        {
            this._logManager = logManager;
        }

        public FileEngine() 
        {
            //PreviewMode = false;
            AutoSync = false;
            FullSync = false;
        }
        //private bool PreviewMode { get; set; }
        private bool AutoSync { get; set; }
        private bool FullSync { get; set; }

        public void CheckTargetDirExists(String rootPath) 
        {
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }
        }

        public SyncOrchestrator SetSyncDirection(SyncOrchestrator agent) 
        {
            //throw new NotImplementedException();
            agent.Direction = SyncDirectionOrder.Upload;
            return agent;
        }

        public bool Synchronize(SyncOrchestrator agent) 
        {
            //throw new NotImplementedException();
            try
            {
                agent.Synchronize();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public SyncOrchestrator CreateSyncOrchestrator(FileSyncProvider source, FileSyncProvider destination) 
        {
            //throw new NotImplementedException();
            SyncOrchestrator agent = new SyncOrchestrator();
            agent.LocalProvider = source;
            agent.RemoteProvider = destination;
            return agent;
        }

        public bool DetectChanges(FileSyncProvider provider) 
        {
            //throw new NotImplementedException();
            try
            {
                provider = AttachDetectedChangeEventHandler(provider);
                provider.DetectChanges();
            }
            finally
            {
                // Release resources
                DisposeProvider(provider);

            }
            return true;
        }

        public FileSyncProvider SetPreviewModeFlag(FileSyncProvider provider, bool flag)
        {
            //throw new NotImplementedException();
            provider.PreviewMode = flag;
            return provider;
        }

        public FileSyncProvider CreateProvider(string RootPath, FileSyncScopeFilter filter, FileSyncOptions options) 
        {
            //throw new NotImplementedException();
            FileSyncProvider provider = null;

            try
            {
                provider = new FileSyncProvider(RootPath, filter, options);
            }
            catch (Exception e) 
            {
                throw e;
            }
            return provider;
        }

        public void DisposeProvider(FileSyncProvider provider)
        {
            //throw new NotImplementedException();
            if (provider != null)
                provider.Dispose();
        }

        public FileSyncOptions SetOptions() 
        {
           // throw new NotImplementedException();
            FileSyncOptions options = FileSyncOptions.ExplicitDetectChanges |
                                      FileSyncOptions.RecycleDeletedFiles |
                                      FileSyncOptions.RecyclePreviousFileOnUpdates |
                                      FileSyncOptions.RecycleConflictLoserFiles;
            return options;
        }

        public FileSyncScopeFilter CreateFilter()
        {
            //throw new NotImplementedException();
            FileSyncScopeFilter filter = new FileSyncScopeFilter();
            filter.FileNameExcludes.Add("*.metadata");
            return filter;
        }


        public FileSyncProvider AttachSkippedChangeEventHandler(FileSyncProvider provider)
        {
            //throw new NotImplementedException();
            provider.SkippedChange += new EventHandler<SkippedChangeEventArgs>(OnSkippedChange);
            return provider;        
        }
        public FileSyncProvider AttachDetectedChangeEventHandler(FileSyncProvider provider)
        {
            //throw new NotImplementedException();
            provider.DetectedChanges += new EventHandler<DetectedChangesEventArgs>(OnDetectedChange);
            return provider;
        }
        public FileSyncProvider AttachApplyingChangeEventHandler(FileSyncProvider provider)
        {
            //throw new NotImplementedException();
            provider.ApplyingChange += new EventHandler<ApplyingChangeEventArgs>(OnApplyingChange);
            return provider;
        }
        public FileSyncProvider AttachAppliedChangeEventHandler(FileSyncProvider provider) 
        {
            //throw new NotImplementedException();
            provider.AppliedChange += new EventHandler<AppliedChangeEventArgs>(OnAppliedChange);
            return provider;
        }

        private void OnDetectedChange(object sender, DetectedChangesEventArgs args)
        {
            //throw new NotImplementedException();
        }

        private void OnApplyingChange(object sender, ApplyingChangeEventArgs args)
        {
            //throw new NotImplementedException();
            switch (args.ChangeType)
            {
                case ChangeType.Create:

                    Console.WriteLine("Change type: CREATE");
                    break;
                case ChangeType.Delete:

                    Console.WriteLine("Change type: DELETE");
                    break;
                case ChangeType.Update:

                    Console.WriteLine("Change type: UPDATE");
                    break;
                case ChangeType.Rename:

                    Console.WriteLine("Change type: RENAME");
                    break;
            }
        }

        private void OnSkippedChange(object sender, SkippedChangeEventArgs args)
        {
            //throw new NotImplementedException();
            Console.WriteLine("-- Skipped applying " + args.ChangeType.ToString().ToUpper()
                  + " for " + (!string.IsNullOrEmpty(args.CurrentFilePath) ?
                                args.CurrentFilePath : args.NewFilePath) + " due to error");

            if (args.Exception != null)
                Console.WriteLine("   [" + args.Exception.Message + "]");

            _logManager.CreateErrorLog(Exception, 12, "dev.test", 5);
        }
         
        private void OnAppliedChange(object sender, AppliedChangeEventArgs args)
        {
            //throw new NotImplementedException();
            switch (args.ChangeType)
            {
                case ChangeType.Create:

                    break;
                case ChangeType.Delete:

                    break;
                case ChangeType.Update:

                    break;
                case ChangeType.Rename:

                    break;
            }
        }
        
    }
}
