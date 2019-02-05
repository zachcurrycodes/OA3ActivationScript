using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace OA3ActivationScript
{
    class OA3_Action
    {
        private string _ConfigXml;
        public string _ReplaceModel { get; internal set; }
        public string _ReplaceSku { get; internal set; }
        public string _OutputPath { get; internal set; }
        public string _SelectedMotherBoardText { get; internal set; }
        public string _OA3ToolPath { get; internal set; }
        public string _DriveLetter { get; internal set; }
        public string _SerialNumber { get; internal set; }

        //private static string _Version = "OA3.v1";

        public string _WorkingDirectory = Path.GetFileName(Directory.GetCurrentDirectory());
        string[] fileNameSendToCMD = new string[3];
        string[] fileServerToLocal = new string[3];
        Process_Action pa = new Process_Action();

        internal void CreateConfigXml()
        {
            string sTemplate = "";
            sTemplate = File.ReadAllText(@".\OA3\OA3Template.cfg");
            //Replace function can't replace in place, using temp strings to replace
            string temp1 = sTemplate.Replace("ReplaceSKU", _ReplaceSku);
            string temp2 = temp1.Replace("ReplaceModel", _ReplaceModel);
            string temp3 = temp2.Replace("ReplaceBinPath", _OutputPath);
            _ConfigXml = temp3.Replace("ReplaceXMLPath", _OutputPath);
            string path = _OutputPath + @"\OA3.cfg";
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(_ConfigXml);
            }
        }

        internal void RunProcess(string fileNameCMD, string arguments, string logFileName, string processType)
        {
            ProcessStartInfo runStartInfo = new ProcessStartInfo();
            //Inject and ClearKey processes doesn't use the .\OA3\ working directory (run AFUWINx64.EXE)                                                                                                      IS THIS NEEDED?
            if (!(processType == "inject" || processType == "clearKey"))
            {
                runStartInfo.WorkingDirectory = @".\OA3\";
            }
            runStartInfo.FileName = fileNameCMD;
            runStartInfo.Arguments = arguments;
            Process runProcess = new Process();
            runProcess.StartInfo = runStartInfo;
            runProcess.StartInfo.UseShellExecute = false;
            runProcess.StartInfo.RedirectStandardInput = true;
            runProcess.StartInfo.RedirectStandardOutput = true;
            runStartInfo.CreateNoWindow = true;

            runProcess.Start();
            //runProcess.WaitForExit();

            string outputString = "\r\n" + fileNameCMD + " " + arguments + "\r\n\r\n";

            while (!runProcess.StandardOutput.EndOfStream)
            {
                outputString += runProcess.StandardOutput.ReadLine() + "\r\n";
            }

            runProcess.Close();
            string newPath = _OutputPath + @"\Logs" + logFileName;

            pa.moveTextToFile(newPath, outputString, processType);
        }

        internal void ClearKey()
        {
            string arguments = "";
            pa.fileServerToLocal = fileServerToLocal;
            pa.fileNameSendToCMD = fileNameSendToCMD;
            pa._DriveLetter = _DriveLetter;
            switch (_SelectedMotherBoardText)
            {
                case "ASUS":
                    pa.setFileNames(@".\OA3\SLPBuilder.exe", @"C:\\SLPBuilder.exe");
                    arguments = @"/clearoa30";
                    break;
                case "Gigabyte":
                    pa.setFileNames(@".\OA3\WinOA30_64.exe", @"C:\\WinOA30_64.exe");
                    pa.setFileNames(@".\OA3\Flash.dll", @"C:\\Flash.dll");
                    pa.setFileNames(@".\OA3\YccDrv.dll", @"C:\\YccDrv.dll");
                    arguments = @" /c";
                    break;
                case "ASRock, Clevo, MSI, Supermicro":
                    pa.setFileNames(@".\OA3\AFUWINx64.EXE", @"C:\\AFUWINx64.EXE");
                    pa.setFileNames(@".\OA3\amifldrv64.sys", @"C:\\amifldrv64.sys");
                    arguments = @"/OAD";
                    break;
                case "APEXX S3":
                    pa.setFileNames(@".\OA3\AFUWINx84 - ApexxS3\AFUWINx64.EXE", @"C:\\AFUWINx64.EXE");
                    pa.setFileNames(@".\OA3\AFUWINx84 - ApexxS3\amifldrv64.sys", @"C:\\amifldrv64.sys");
                    arguments = @"/OAD";
                    break;

            }

            if (File.Exists(fileNameSendToCMD[0]))
            {
                RunProcess(
                  fileNameSendToCMD[0],
                  arguments,
                  @"\00 clearkey_log.txt",
                  "clearKey");
            }
            else
            {
                MessageBox.Show(fileNameSendToCMD[0] + " doesn't exist");
            }

            pa._OutputPath = _OutputPath;
            pa._DriveLetter = _DriveLetter;
            pa.createStartUpBatch("clearKey");

            pa.deleteLocalCopy(fileNameSendToCMD);
        }

        internal void DefaultKey()
        {
            RunProcess(
              @"cscript",
              ConfigurationManager.AppSettings["defaultKeyCommand"].ToString(),
              @"\01 defaultkey_log.txt",
              "defaultKey");
        }

        internal void Assemble()
        {
            RunProcess(
              _OA3ToolPath,
              @"/Assemble /ConfigFile=""" + _OutputPath + @"\oa3.cfg""",
              @"\02 assemble_log.txt",
              "assemble");
        }

        internal void Inject()
        {
            if (File.Exists(_OutputPath + @"\OA3.bin"))
            {
                pa.fileServerToLocal = fileServerToLocal;
                pa.fileNameSendToCMD = fileNameSendToCMD;
                string arguments = "";
                switch (_SelectedMotherBoardText)
                {
                    case "ASUS":
                        pa.setFileNames(@".\OA3\SLPBuilder.exe", @"C:\\SLPBuilder.exe");
                        arguments = @"/oa30:" + _OutputPath + @"\OA3.bin";
                        break;
                    case "Gigabyte":
                        pa.setFileNames(@".\OA3\WinOA30_64.exe", @"C:\\WinOA30_64.exe");
                        pa.setFileNames(@".\OA3\Flash.dll", @"C:\\Flash.dll");
                        pa.setFileNames(@".\OA3\YccDrv.dll", @"C:\\YccDrv.dll");
                        arguments = @"/f """ + _OutputPath + @"\OA3.bin""";
                        break;
                    case "ASRock, Clevo, MSI, Supermicro":
                        pa.setFileNames(@".\OA3\AFUWINx64.EXE", @"C:\\AFUWINx64.EXE");
                        pa.setFileNames(@".\OA3\amifldrv64.sys", @"C:\\amifldrv64.sys");
                        arguments = @"/A""" + _OutputPath + @"\OA3.bin""";
                        break;
                }
                try
                {
                    RunProcess(
                      fileNameSendToCMD[0],
                      arguments,
                      @"\03 inject_log.txt",
                      "inject");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            pa._OutputPath = _OutputPath;
            pa._DriveLetter = _DriveLetter;
            pa.createStartUpBatch("injected");
            pa.renameFile(_OutputPath + @"\OA3.xml", _OutputPath + @"\OA3state2.xml");
            pa.deleteLocalCopy(fileNameSendToCMD);
        }

        internal void Validate()
        {
            RunProcess(
              _OA3ToolPath,
              @"/Validate",
              @"\04 validate_log.txt",
              "validate");

            MessageBox.Show("OA3 Validate Successful - Please Check Log for more details");
        }

        internal void Report()
        {
            RunProcess(
              _OA3ToolPath,
              @"/Report /ConfigFile=""" + _OutputPath + @"\OA3.cfg""",
              @"\05 report_log.txt",
              "report");

            try
            {
                if (File.Exists(_OutputPath + @"\OA3.xml"))
                {

                    pa.renameFile(_OutputPath + @"\OA3.xml", _OutputPath + @"\OA3state3.xml");

                    MessageBox.Show("OA3 Report Successful - Please Check Log for more details");
                    MessageBox.Show("OA3 Activation Successful & Completed");
                }
                else
                {
                    MessageBox.Show("OA3 Activation NOT SUCCESSFUL");
                }
                string startUpFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\OA3ActivationScript.bat";
                if (File.Exists(startUpFolderPath))
                {
                    File.Delete(startUpFolderPath);
                }
                startUpFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\null";
                if (File.Exists(startUpFolderPath))
                {
                    File.Delete(startUpFolderPath);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Exception:\t" + ex);
                MessageBox.Show("OA3 Activation NOT SUCCESSFUL");
            }
        }
    }
}