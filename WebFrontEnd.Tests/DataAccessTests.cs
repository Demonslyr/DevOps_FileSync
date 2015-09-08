using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using Ninject;
using System.Linq;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Data.Entity;
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


namespace WebFrontEnd.Tests
{
    [TestClass]
    public class LogManagerTests
    {
        public static Registrator NinjectReg;

        [TestMethod]
        public void RegistratorTest()
        {
            NinjectReg = new Registrator();
        }

        [TestMethod]
        public void CreateSyncLogTest()
        {
            NinjectReg = new Registrator();
            var LogManager = NinjectReg.kernel.Get<LogManager>();

            try
            {
                LogManager.CreateSyncLog(4);
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                Assert.IsTrue(false);
            }


        }

        [TestMethod]
        public void CreateFileChangeLogTest()
        {
            NinjectReg = new Registrator();
            var LogManager = NinjectReg.kernel.Get<LogManager>();

            try
            {
                LogManager.CreateFileChangeLog(4, "dev/unit.test");
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                Assert.IsTrue(false);
            }

        }

        [TestMethod]
        public void CreateProgramInitLogTest()
        {
            NinjectReg = new Registrator();
            var LogManager = NinjectReg.kernel.Get<LogManager>();

            try
            {
                LogManager.CreateProgramInitLog(8, "dev/unit.test");
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                Assert.IsTrue(false);
            }

        }

        [TestMethod]
        public void CreateErrorLogTest()
        {
            NinjectReg = new Registrator();
            var LogManager = NinjectReg.kernel.Get<LogManager>();

            try
            {
                LogManager.CreateErrorLog("exception (unit test)", 8, "dev/unit.test", 4);
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                Assert.IsTrue(false);
            }

        }

        [TestMethod]
        public void MultiFilterLogQuery1()
        {
            NinjectReg = new Registrator();
            var LogManager = NinjectReg.kernel.Get<LogManager>();
            var options = new SearchOptions
            {
                UserName = Environment.UserName,
                StartDate = -10,
                StopDate = 1
            };
            var result = LogManager.PerformQuery(options);
            foreach (History a in result)
            {
                Debug.WriteLine(a.LogTime.ToString() + " " + a.LogDate.ToShortDateString() + ", " + a.LogType.Description.ToString() + ", " + a.InteractionType.Description.ToString() + ", " + a.Domain.ToString() + "\\" + a.UserName.ToString());
            }

        }

        [TestMethod]
        public void MultiFilterLogQuery2()
        {
            NinjectReg = new Registrator();
            var LogManager = NinjectReg.kernel.Get<LogManager>();
            var options = new SearchOptions
            {
                LogTypeId = 5,
                FileOperationId = 4,
                UserName = "cmbradley",
                StartDate = -6,
                StopDate = 1
            };
            var result = LogManager.PerformQuery(options);
            foreach (History a in result)
            {
                Debug.WriteLine(a.LogTime.ToString() + " " + a.LogDate.ToShortDateString() + ", " + a.LogType.Description.ToString() + ", " + a.InteractionType.Description.ToString() + ", " + a.Domain.ToString() + "\\" + a.UserName.ToString());
            }

        }

        [TestMethod]
        public void SyncLogEntry()
        {
            NinjectReg = new Registrator();
            var LogEngine = NinjectReg.kernel.Get<LogEngine>();
            var result = LogEngine.CreateQuery();
            Assert.IsInstanceOfType(result, typeof(IEnumerable<History>));

        }
    }

    [TestClass]
    public class DataAccessQueryTests
    {
        public static Registrator NinjectReg;

        [TestMethod]
        public void RegistratorTest()
        {
            NinjectReg = new Registrator();
        }

        [TestMethod]
        public void NinjectLogManagerTest()
        {
            NinjectReg = new Registrator();
            var LogManager = NinjectReg.kernel.Get<LogManager>();
            //var result = LogManager.RunSampleQueries();
            //foreach (History a in result)
            //{
            //    Debug.WriteLine(a.LogTime.ToString() + " " + a.LogDate.ToShortDateString() + ", " + a.LogType.Description.ToString() + ", " + a.InteractionType.Description.ToString() + ", " + a.Domain.ToString() + "\\" + a.UserName.ToString());
            //}
        }

        [TestMethod]
        public void CreateQuery()
        {
            NinjectReg = new Registrator();
            var LogEngine = NinjectReg.kernel.Get<LogEngine>();
            var result = LogEngine.CreateQuery();
            Assert.IsInstanceOfType(result, typeof(IEnumerable<History>));

        }
        [TestMethod]
        public void DateQuery()
        {
            NinjectReg = new Registrator();
            var LogEngine = NinjectReg.kernel.Get<LogEngine>();
            var query = LogEngine.CreateQuery();
            var options = new SearchOptions
            {
                StartDate = -2,
                StopDate = 0
            };
            IEnumerable<History> result = LogEngine.DateRangeQuery(query,options);
            foreach (History x in result.ToList())
            {
                if (!((x.LogDate >= DateTime.Now.AddDays(-2) && (x.LogDate <= DateTime.Now.AddDays(0)))))
                {
                    Assert.IsFalse(true);
                    break;
                }
            }
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void FileOperationQuery()
        {
            NinjectReg = new Registrator();
            var LogEngine = NinjectReg.kernel.Get<LogEngine>();
            var query = LogEngine.CreateQuery();
            int operation = 4;
            var options = new SearchOptions {FileOperationId = 4 };
            try
            {
                IEnumerable<History> result = LogEngine.FileOperationQuery(query, options);
                foreach (History x in result.ToList())
                {
                    if (!(x.FileOperationId == operation))
                    {
                        Assert.IsFalse(true);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void UserNameQuery()
        {
            NinjectReg = new Registrator();
            var LogEngine = NinjectReg.kernel.Get<LogEngine>();
            var query = LogEngine.CreateQuery();
            var options = new SearchOptions { UserName = "drmurtha" };
            IEnumerable<History> result = LogEngine.UserNameQuery(query, options);
            foreach (History x in result.ToList())
            {
                if (!(x.UserName == "drmurtha"))
                {
                    Assert.IsFalse(true);
                    break;
                }
            }
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void LogTypeQuery()
        {
            NinjectReg = new Registrator();
            var LogEngine = NinjectReg.kernel.Get<LogEngine>();
            var query = LogEngine.CreateQuery();
            var options = new SearchOptions { LogTypeId = 5 };
            LogEngine.LogTypeQuery(query, options);
        }

        //[TestMethod]
        //public void CommitLogEntry()
        //{
        //    var LogEngine = NinjectReg.kernel.Get<LogEngine>();
        //    LogEngine.CommitLog();
        //}
        [TestMethod]
        public void ExecuteQuery()
        {
            NinjectReg = new Registrator();
            var LogEngine = NinjectReg.kernel.Get<LogEngine>();
            var query = LogEngine.CreateQuery();
            LogEngine.ExecuteQuery(query);
        }

        //[TestMethod]
        //public void SQLConnectTest()
        //{
        //    string sqllocalConnectionString = @"Data Source=VIN7D301566\FILESYNCLOGSRVR;Initial Catalog=FileSyncLogging;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //    SqlConnection LogSrvr = new SqlConnection(sqllocalConnectionString);
        //    bool ConnRes;

        //    try
        //    {
        //        LogSrvr.Open();
        //        ConnRes = true;
        //    }
        //    catch (Exception e)
        //    {
        //        ConnRes = false;
        //    }
        //    Assert.AreEqual(true, ConnRes);
        //}
    }

    [TestClass]
    public class DataAccessEntryTests
    {
        public static Registrator NinjectReg;

        [TestMethod]
        public void RegistratorTest()
        {
            NinjectReg = new Registrator();
        }


        [TestMethod]
        public void CommitEntryTest()
        {
            NinjectReg = new Registrator();
            var LDAL = NinjectReg.kernel.Get<LogEngine>();
            var log = LDAL.CreateErrorLog("Test", 0, "Test", 0);

            try
            {
                LDAL.CommitLog(log);
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                Assert.IsTrue(false);
            }

        }
        [TestMethod]
        public void ProgramInitEntryTest()
        {
            NinjectReg = new Registrator();
            var LogEngine = NinjectReg.kernel.Get<LogEngine>();
            try
            {
                LogEngine.CreateProgramInitLog(0, "Test");
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                Assert.IsTrue(false);
            }
        }
        [TestMethod]
        public void FileChangeEntryTest()
        {
            NinjectReg = new Registrator();
            var LogEngine = NinjectReg.kernel.Get<LogEngine>();
            try
            {
                LogEngine.CreateFileChangeLog(5, "Test");
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                Assert.IsTrue(false);
            }


        }
        [TestMethod]
        public void ErrorEntryTest()
        {
            NinjectReg = new Registrator();
            var LogEngine = NinjectReg.kernel.Get<LogEngine>();
            try
            {
                LogEngine.CreateErrorLog("Test", 0, "Test", 0);
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                Assert.IsTrue(false);
            }
        }
        [TestMethod]
        public void SyncEntryTest()
        {
            NinjectReg = new Registrator();
            var LogEngine = NinjectReg.kernel.Get<LogEngine>();

            try
            {
                LogEngine.CreateSyncLog(0);
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                Assert.IsTrue(false);
            }

        }

        [TestMethod]
        public void testThing()
        {
            NinjectReg = new Registrator();
            var LogEngine = NinjectReg.kernel.Get<LogEngine>();
            try
            {
                var log = LogEngine.CreateErrorLog("Test", 0, "Test", 0);
                LogEngine.CommitLog(log);
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                Assert.IsTrue(false);
            }

        }



    }

    [TestClass]
    public class FileEngineTests
    {
        public static Registrator NinjectReg;

        [TestMethod]
        public void RegistratorTest()
        {
            NinjectReg = new Registrator();
        }

        [TestMethod]
        public void CreateSyncFilterTest()
        {
            NinjectReg = new Registrator();
            var FileEngine = NinjectReg.kernel.Get<FileEngine>();
            var test = FileEngine.CreateFilter();
        }

        [TestMethod]
        public void CreateSyncOptionsTest()
        {
            NinjectReg = new Registrator();
            var FileEngine = NinjectReg.kernel.Get<FileEngine>();
            var test = FileEngine.SetOptions();
        }

        [TestMethod]
        public void AssignSyncDirectionsTest()
        {
            NinjectReg = new Registrator();
            var FileEngine = NinjectReg.kernel.Get<FileEngine>();
            var test = FileEngine.CreateSyncOrchestrator(FileEngine.CreateProvider(@"c:\testSource", FileEngine.CreateFilter(), FileEngine.SetOptions()), FileEngine.CreateProvider(@"c:\testDir", FileEngine.CreateFilter(), FileEngine.SetOptions()));
            test = FileEngine.SetSyncDirection(test);
        }

        [TestMethod]
        public void DisposeSyncProviderTest()
        {
            NinjectReg = new Registrator();
            var FileEngine = NinjectReg.kernel.Get<FileEngine>();
            var test = FileEngine.CreateProvider(@"c:\testSource", FileEngine.CreateFilter(), FileEngine.SetOptions());
        }

        [TestMethod]
        public void SetPreviewModeTest()
        {
            NinjectReg = new Registrator();
            var FileEngine = NinjectReg.kernel.Get<FileEngine>();
            var test = FileEngine.CreateProvider(@"c:\testSource", FileEngine.CreateFilter(), FileEngine.SetOptions());
            test = FileEngine.SetPreviewModeFlag(test, true);
            Assert.IsTrue(test.PreviewMode);
        }

        [TestMethod]
        public void CreateSyncProviderTest()
        {
            NinjectReg = new Registrator();
            var FileEngine = NinjectReg.kernel.Get<FileEngine>();
            var test = FileEngine.CreateProvider(@"c:\testSource", FileEngine.CreateFilter(), FileEngine.SetOptions());
        }

        [TestMethod]
        public void CreateSyncOrchestratorTest()
        {
            NinjectReg = new Registrator();
            var FileEngine = NinjectReg.kernel.Get<FileEngine>();
            FileEngine.CreateSyncOrchestrator(FileEngine.CreateProvider(@"c:\testSource", FileEngine.CreateFilter(), FileEngine.SetOptions()), FileEngine.CreateProvider(@"c:\testDir", FileEngine.CreateFilter(), FileEngine.SetOptions()));
        }


        [TestMethod]
        public void SyncTest()
        {
            NinjectReg = new Registrator();
            var FileEngine = NinjectReg.kernel.Get<FileEngine>();
        }

    }

    [TestClass]
    public class SyncTests
    {
        public static Registrator NinjectReg;
        [TestMethod]
        public void BigTest()
        {
            NinjectReg = new Registrator();
            var FileEngine = NinjectReg.kernel.Get<FileEngine>();

            FileSyncProvider sourceProvider = null;
            FileSyncProvider destinationProvider = null;

            try
            {
                sourceProvider = new FileSyncProvider(
                    @"C:\testSource", null, FileEngine.SetOptions());
                destinationProvider = new FileSyncProvider(
                    @"C:\targetDir", null, FileEngine.SetOptions());

                SyncOrchestrator agent = new SyncOrchestrator();
                agent.LocalProvider = sourceProvider;
                agent.RemoteProvider = destinationProvider;
                agent.Direction = SyncDirectionOrder.Upload; // Sync source to destination

                Console.WriteLine("Synchronizing changes to replica: " +
                    destinationProvider.RootDirectoryPath);
                //agent.Synchronize();
                Assert.IsTrue(true);
            }
            finally
            {
                // Release resources
                if (sourceProvider != null) sourceProvider.Dispose();
                if (destinationProvider != null) destinationProvider.Dispose();
            }
        }
    }

    [TestClass]
    public class FileManagerTests
    {
        public static Registrator NinjectReg;

        [TestMethod]
        public void FileManagerSyncTest()
        {
            NinjectReg = new Registrator();
            var FileEngine = NinjectReg.kernel.Get<FileEngine>();

            var sDir = @"C:\testSource";
            var tDir = @"C:\NewTar";
            var filter = FileEngine.CreateFilter();
            var options = FileEngine.SetOptions();
            FileEngine.CheckTargetDirExists(tDir);
            var SourceProvider = FileEngine.CreateProvider(sDir, filter, options);
            var DestinationProvider = FileEngine.CreateProvider(tDir, filter, options);
            FileEngine.DetectChanges(SourceProvider);
            SourceProvider = FileEngine.CreateProvider(sDir, filter, options);
            SourceProvider = FileEngine.SetPreviewModeFlag(SourceProvider, true);
            DestinationProvider = FileEngine.SetPreviewModeFlag(DestinationProvider, true);
            DestinationProvider = FileEngine.AttachApplyingChangeEventHandler(DestinationProvider);
            DestinationProvider = FileEngine.AttachAppliedChangeEventHandler(DestinationProvider);
            DestinationProvider = FileEngine.AttachSkippedChangeEventHandler(DestinationProvider);
            var agent = FileEngine.CreateSyncOrchestrator(SourceProvider,DestinationProvider);
            agent = FileEngine.SetSyncDirection(agent);
            FileEngine.Synchronize(agent);

        }
        [TestMethod]
        public void FileManagerDetectChangesSyncTest()
        {

        }

        [TestMethod]
        public void FileManagerAutoApproveSyncTest()
        {

        }
    }
   
}
