using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web;
using System.Diagnostics;
using System.IO;
using TestBaseClassLibrary;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Configuration;
using ErrorLog;
using System.Xml;
using System.Xml.Serialization;
using System;

namespace ProwlAgent
{
    public partial class frmMain : Form
    {
        int retry = 0;
        DateTime StartupTime = DateTime.Now;
        Timer timer = new Timer();
        Label label = new Label();
        
        bool ran = false;
        public static string path = null;
        TestBaseClassLibrary.AppFramework.Agent.Listiner Agent = new TestBaseClassLibrary.AppFramework.Agent.Listiner();

        public frmMain()
        {
            try
            {
                //string keyName = @"SYSTEM\ControlSet001\services\eventlog\Application\Prowl Agent";
                //string keyName2 = @"SYSTEM\ControlSet001\services\eventlog\Application\ProwlAgent";
                //Microsoft.Win32.RegistryKey Key = Microsoft.Win32.Registry.LocalMachine;
                ////string[] keyvalues = Key.GetSubKeyNames();
                //try
                //{
                //    Key.DeleteSubKeyTree(keyName);
                //}
                //catch (Exception ex)
                //{
                //    WriteErrorToLog("ProwlAgent", "Application", "ProwlAgent Exception :" + retry + "", ex.ToString(), " Thrown in Main deleting first reg key", true, true);
                //}
                //try
                //{
                //    Key.DeleteSubKeyTree(keyName2);
                //}
                //catch (Exception ex)
                //{
                //    WriteErrorToLog("ProwlAgent", "Application", "ProwlAgent Exception :" + retry + "", ex.ToString(), " Thrown in Main deleting secound reg key", true, true);
                //}
                //Key.Close();
                InitializeComponent();
                TextReader reader = new StreamReader("AgentConfig.xml");
                XmlDocument SettingsFile = new XmlDocument();
                SettingsFile.Load("AgentConfig.xml");
                XmlNodeList NodeList = SettingsFile.SelectNodes("/config/server");

                try { TestBaseClassLibrary.AppFramework.Agent.Listiner.setServerIP(NodeList[0]["address"].InnerText); }
                catch (Exception ex) { WriteErrorToLog("ProwlAgent", "Application", "ProwlAgent Exception :" + retry + "", ex.ToString(), " Thrown in Main setServerIP", true, true); }



                //if (reader.Name == "address")
                //{

                //    TestBaseClassLibrary.AppFramework.Agent.Listiner.setServerIP(reader.Value);
                //}
                //else
                //{
                //    continue;
                //}
                // Do some work here on the data.
                //Console.WriteLine(reader.Name);

                //Console.ReadLine();


                starttimer();
                retry = 0;
            }
            catch (Exception ex)
            {
                WriteErrorToLog("ProwlAgent", "Application", "ProwlAgent Exception :" + retry + "", ex.ToString(), " Thrown in Main Unhandled Exception", true, true);
            }
        }

        public void starttimer()
        {
            try
            {
                
                Agent.StartAgent();
                retry = 0;
                //Agent.resetretry();
            }
            catch (Exception ex)
            {
                WriteErrorToLog("ProwlAgent", "Application", "ProwlAgent StartTimer Exception : --Retrying (" + retry + ")", ex.ToString(), "NetworkError", true, true);
                
                int wait = 60000 * retry;
                if (wait >= 900000)
                {
                    wait = 900000;
                }
                System.Threading.Thread.Sleep(wait);
                retry++;
                starttimer();
                
                
            }
            
        }

        //void start_ResetAgent(object sender, EventArgs e)
        //{
        //    bool start = ResetAgent(sender, e);
        //}
       private void WriteToEventLog(string message)
{
           // Create the source, if it does not already exist.
        if(!EventLog.SourceExists("Application"))
        {
             //An event log source should not be created and immediately used.
             //There is a latency time to enable the source, it should be created
             //prior to executing the application that uses the source.
             //Execute this sample a second time to use the new source.
            EventLog.CreateEventSource("Application", "ProwlAgent");
            Console.WriteLine("CreatedEventSource");
            Console.WriteLine("Exiting, execute the application a second time to use the source.");
            // The source is created.  Exit the application to allow it to be registered.
            return;
        }

        // Create an EventLog instance and assign its source.
        EventLog myLog = new EventLog();
        myLog.Source = "ProwlAgent";

        // Write an informational entry to the event log.    
        myLog.WriteEntry(message, System.Diagnostics.EventLogEntryType.Information, 65530);
       
        }

//      bool ResetAgent(object sender, EventArgs e)
//       {

//           try
//           {
//               String UpdatePath = @"\\usqq1910\AgentUpdate\";
//               string[] files = System.IO.Directory.GetFiles(UpdatePath + @"LauncherUpdateFiles\");
//               string CurrentDirectory = System.IO.Directory.GetCurrentDirectory();
//               string ParentDirectory = System.IO.Directory.GetParent(CurrentDirectory).FullName;

//               //#if DEBUG
//               //    string absolute_path = System.IO.Path.GetFullPath(ParentDirectory + "\\Debug\\");
//               //#else
//               string absolute_path = System.IO.Path.GetFullPath(ParentDirectory + "\\bin\\");
//               //#endif
               
               
//               //String Path = @"C:\Program Files (x86)\Prowl Suite\bin\";


//               DateTime lastexecute = TestBaseClassLibrary.AppFramework.Agent.Listiner.lastexecute;
//               TimeSpan ts = DateTime.Now - lastexecute;
//               double diff = ts.TotalMinutes;
//               TimeSpan ts2 = DateTime.Now - StartupTime;
//               double startupdiff = ts2.TotalHours;

//               String updateversion = "0.0.0";
//               bool updateavailable = false;

//               try
//               {
//                   using (StreamReader sr = new StreamReader(UpdatePath + "UpdateVersion.txt"))
//                   {
//                       String line;
//                       while ((line = sr.ReadLine()) != null)
//                       {
//                           updateversion = line;
//                       }
//                   }
//               }
//               catch (Exception ex)
//               {
//                   WriteErrorToLog("ProwlAgent", "Application", "ProwlAgent ResetAgent 1 Exception : --Retrying (" + retry + ")", ex.ToString(), "NetworkError", true, true);
                   
//                   int wait = 60000 * retry;
//                   if (wait >= 900000)
//                   {
//                       wait = 900000;
//                   }
//                   System.Threading.Thread.Sleep(wait);
//                   retry++;
//                   bool results = ResetAgent(sender, e);
//                   if (results == true)
//                   {
//                       retry = 0;
//                   }
//               }
//               System.Version ProwlInfo = AssemblyName.GetAssemblyName(absolute_path + @"ProwlAgent.exe").Version;
//               if (updateversion != ProwlInfo.ToString())
//               {
//                   updateavailable = true;
//               }

//               if ((diff >= 15.000 && startupdiff >= 24.000) || (diff >= 11.000 && updateavailable == true) && (TestBaseClassLibrary.AppFramework.Agent.Listiner.available == true))
//           {
 
//               try
//               {

//                   foreach (string s in files)
//                   {
//                       try
//                       {
//                           TestBaseClassLibrary.AppFramework.JsonCall UnAvailJsonCall = new TestBaseClassLibrary.AppFramework.JsonCall();
//                           var hostEntry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
//                           var ip = (
//                                      from addr in hostEntry.AddressList
//                                      where addr.AddressFamily.ToString() == "InterNetwork"
//                                      select addr.ToString()
//                               ).FirstOrDefault();
//                           UnAvailJsonCall = new TestBaseClassLibrary.AppFramework.JsonCall(TestBaseClassLibrary.AppFramework.Agent.Listiner.ServerIP.ToString(), TestBaseClassLibrary.AppFramework.Agent.Listiner.ServerIP.ToString() + ":4445", ip + ":4446", DateTime.Now, "UNAVAILABLE", null);
//                           IPAddress destip = TestBaseClassLibrary.AppFramework.Agent.Listiner.ServerIP;
//                           RespondtoMessage(UnAvailJsonCall.ConvertToJson(), destip);
//                       }
//                       catch (Exception ex)
//                       {
//                           WriteErrorToLog("ProwlAgent", "Application", "ProwlAgent Exception : -- Retrying (" + retry + ")", ex.ToString(), "NetworkError", true, true);
                           
//                           int wait = 60000 * retry;
//                           if (wait >= 900000)
//                           {
//                               wait = 900000;
//                           }
//                           System.Threading.Thread.Sleep(wait);
//                           retry++;
//                           bool results = ResetAgent(sender, e);
//                           if (results == true)
//                           {
//                               retry = 0;
//                           }
//                       }


//                       // Use static Path methods to extract only the file name from the path.
//                       string fileName = System.IO.Path.GetFileName(s);
//                       string destFile = System.IO.Path.Combine(absolute_path, fileName);
//                       System.IO.File.Copy(s, destFile, true);
//                       WriteErrorToLog("ProwlAgent", "Application", "ProwlAgentLauncher Updated", "", "Trac", true, true);
                       

//                   }


//                   lastexecute = DateTime.Now;
//                   diff = 0;
//                   Process[] proc = Process.GetProcessesByName("java");
//                   foreach (Process prs in proc)
//                   {
//                       prs.Kill();
//                   }
//                   Process[] proc2 = Process.GetProcessesByName("Firefox");
//                   foreach (Process prs in proc2)
//                   {
//                       prs.Kill();
//                   }
//                   System.Threading.Thread.Sleep(10000);
//                   System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
////#if Debug
////                   System.IO.Directory.SetCurrentDirectory(@"E:\Workspace_cmnrs17\Core\Automation\PROWL Suite\Main\Source\ProwlAgentLauncher\ProwlAgentLauncher\bin\Debug");
////#else
//                   System.IO.Directory.SetCurrentDirectory(Application.StartupPath.ToString());
////#endif
                   
//                   info.FileName = @"ProwlAgentLauncher.exe";
//                   info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;
//                   System.Diagnostics.Process p;
//                   WriteErrorToLog("ProwlAgent", "Application", "ProwlAgent Restarting ", "", "Trac", true, true);
                   
                   
                           
//                   if (ran == false)
//                   {
                      
//                           System.GC.Collect();
//                           System.Threading.Thread.Sleep(90000);
//                           p = System.Diagnostics.Process.Start(info);
//                           TestBaseClassLibrary.AppFramework.Agent.Listiner.lastexecute = DateTime.Now;
//                           ran = true;
                      

//                   }
//                   System.Threading.Thread.Sleep(10000);

//               }
//               catch (Exception ex)
//               {
//                       WriteToEventLog("ProwlAgent", "Application", "ProwlAgent  ResetAgent 2 Exception : " + ex.ToString() + "--Retrying (" + retry + ")");
//                       int wait = 60000 * retry;
//                       if (wait >= 900000)
//                       {
//                           wait = 900000;
//                       }
//                       System.Threading.Thread.Sleep(wait);
//                       retry++;
//                       bool results = ResetAgent(sender, e);
//                       if (results == true)
//                       {
//                           retry = 0;
//                       }
                  
//               }
//               Agent.stopAgent();
//               Agent = null;
//               this.Close();
//           }
//           }
//           catch (Exception ex)
//           {
//               WriteErrorToLog("ProwlAgent", "Application", "ProwlAgent Exception : ", ex.ToString(), "NetworkError", true, true);
               
//           }
//           return true;
           
//       }
             private static string GetIP()
        {
            string strHostName = "";
            strHostName = System.Net.Dns.GetHostName();

            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

            IPAddress[] addr = ipEntry.AddressList;

            return addr[addr.Length - 1].ToString();

        }

        public void RespondtoMessage(String Text2Send, IPAddress Destination)
        {
            try
            {
                TcpClient client = new TcpClient();

                IPEndPoint serverEndPoint = new IPEndPoint(Destination, 4446);

                client.Connect(serverEndPoint);

                System.IO.StreamWriter clientStream = new System.IO.StreamWriter(client.GetStream());

                //ASCIIEncoding encoder = new ASCIIEncoding();
                TestBaseClassLibrary.AppFramework.JsonCall JsonCall = new TestBaseClassLibrary.AppFramework.JsonCall(Text2Send);
                clientStream.WriteLine(JsonCall.ConvertToJson());
                clientStream.Flush();
                clientStream = null;
                client.Close();
                System.GC.Collect();
                System.Threading.Thread.Sleep(10000);
            }
            catch (Exception ex)
            {
                WriteErrorToLog("ProwlAgent", "Application", "ProwlAgent Exception : ", ex.ToString(), "NetworkError", true, true);
                
            }
            //byte[] buffer = encoder.GetBytes(Text2Send);

            //clientStream.Write(buffer, 0, buffer.Length);
            //clientStream.Flush();
            //client.Close();
        }
      public void WriteToEventLog(string sSource, string sLog, string comment)
        {
            string sEvent;
            sEvent = comment;

            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);

            EventLog.WriteEntry(sSource, sEvent);
            EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 234);

        }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="SSource">ProwlAgent, EBizPerformance, Logging, ProwlCrawl, ProwlFramework, ProwlServer, ProwlTest</param>
      /// <param name="Slog">What Log in Windows Event Viewer Generaly Application</param>
      /// <param name="Comment">Any Comment you may have</param>
      /// <param name="Exception">Send in the ex.toString()</param>
      /// <param name="EventType">DatabaseError, GeneralError, NetworkError, ProwlError, Trac</param>
        private void WriteErrorToLog(String SSource, String Slog, String Comment, String Exception, String EventType, bool SubmitLog, bool Info)
        {
            LogData.LogSource Source;
            LogData.EventType ET;

            switch (EventType.ToLower())
            {
                case "databaseerror":
                    ET = LogData.EventType.DatabaseError;
                    break;
                case "generalerror":
                    ET = LogData.EventType.GeneralError;
                    break;
                case "networkerror":
                    ET = LogData.EventType.NetworkError;
                    break;
                case "prowlerror":
                    ET = LogData.EventType.ProwlError;
                    break;
                case "trac":
                    ET = LogData.EventType.Trac;
                    break;
                default:
                    ET = LogData.EventType.ProwlError;
                    break;
            }
            switch (SSource.ToLower())
            {
                case "prowlagent":
                    Source = LogData.LogSource.ProwlAgent;
                    break;
                case "ebizperformance":
                    Source = LogData.LogSource.EBizPerformance;
                    break;
                case "logging":
                    Source = LogData.LogSource.Logging;
                    break;
                case "prowlcrawl":
                    Source = LogData.LogSource.ProwlCrawl;
                    break;
                case "prowlframework":
                    Source = LogData.LogSource.ProwlFramework;
                    break;
                case "prowlserver":
                    Source = LogData.LogSource.ProwlServer;
                    break;
                case "prowltest":
                    Source = LogData.LogSource.ProwlTest;
                    break;
                default:
                    Source = LogData.LogSource.ProwlAgent;
                    break;


            }
            
            Log LogException = new Log(Source, Comment + Exception.ToString(), ET, SubmitLog, Info);
            LogException.CommitLogDB(Info);
            WriteToEventLog(SSource, Slog, Comment + Exception.ToString());


        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void frmMain_Load_1(object sender, System.EventArgs e)
        {

        }

        
    }
}
