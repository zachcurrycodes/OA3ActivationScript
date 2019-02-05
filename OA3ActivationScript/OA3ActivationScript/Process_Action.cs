using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OA3ActivationScript {
  class Process_Action {
    public string _OutputPath { get; set; }
    public string _DriveLetter { get; set; }
    public string[] fileNameSendToCMD { get; set; }
    public string[] fileServerToLocal { get; set; }


    internal void renameFile(string oldName, string newName) {
      try {
        oldName = System.IO.Path.Combine(oldName);
        newName = System.IO.Path.Combine(newName);
        File.Move(oldName, newName);
      } catch (IOException ex) {
        MessageBox.Show("Exception:\t \r\n\r\n" + ex);
      }
    }

    internal void moveTextToFile(string outputPath, string outputString, string processType) {
      string line = "";
      if (!File.Exists(outputPath)) {
        using (StreamWriter sw = File.CreateText(outputPath)) {
          sw.WriteLine("OA3 1st " + processType + " run at");
          sw.WriteLine("Date: " + DateTime.Now.ToString());
          sw.WriteLine(outputString);
        }
      } else {
        string outputString2 = "";
        int counter = 0;
        StreamReader reader2 = new StreamReader(outputPath);
        while ((line = reader2.ReadLine()) != null && counter < 1000) {
          outputString2 += line + "\r\n";
          counter++;
        }
        reader2.Close();
        File.WriteAllText(outputPath, String.Empty);
        using (StreamWriter sw = File.AppendText(outputPath)) {
          sw.WriteLine("OA3 " + processType + " Reran at");
          sw.WriteLine("Date: " + DateTime.Now.ToString());
          sw.WriteLine(outputString);
          sw.WriteLine();
          sw.WriteLine(outputString2);
        }
      }
    }

    internal void createStartUpBatch(string reason) {
      //create a batch file to run the OA3Activation.exe with 2 parameters
      string batchCommand = @"@ECHO OFF" + Environment.NewLine +
                            //@"title running" + Environment.NewLine +
                            @"CLS" + Environment.NewLine +
                            //@"@pushd %~dp0" + Environment.NewLine +
                            @"Echo wait to connect to the network" + Environment.NewLine +
                            @"TIMEOUT /T 10" + Environment.NewLine +
                            @":LOOP" + Environment.NewLine +
                            @"ping 172.19.0.1 -n 1 >null" + Environment.NewLine +
                            @"IF %ERRORLEVEL%==0 goto CONTINUE" + Environment.NewLine +
                            @"Echo %time% - Host Unreachable" + Environment.NewLine +
                            @"Echo Ctrl+""C"" then ""Y""+Enter will terminate the program" + Environment.NewLine +
                            @"TIMEOUT /T 10" + Environment.NewLine +
                            @"CLS" + Environment.NewLine +
                            @"GOTO LOOP" + Environment.NewLine +
                            @":CONTINUE" + Environment.NewLine +
                            //Environment.NewLine +
                            //@"start /wait ping -w 5000 172.19.0.1" + Environment.NewLine +
                            @"START \\zeus\Add_Software\_RunOA3\OA3ActivationScript\bin\Debug\OA3ActivationScript.exe """
                                    + _OutputPath + "\" \"" + _DriveLetter + "\" \"" + reason + "\"" + Environment.NewLine;// +
                            //@"pause" + Environment.NewLine;
                            //Environment.NewLine +
                            //@"popd";
      string startUpFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\OA3ActivationScript.bat";

      try {
        if (!File.Exists(startUpFolderPath)) {
          using (StreamWriter sw = File.CreateText(startUpFolderPath)) {
            sw.WriteLine(batchCommand);
          }
        } else {
          File.Delete(startUpFolderPath);
          using (StreamWriter sw = File.CreateText(startUpFolderPath)) {
            sw.WriteLine(batchCommand);
          }
        }
      } catch (IOException ex) {
        MessageBox.Show("Exception:\t \r\n\r\n" + ex);
      }

      string placeBackUpOnServer = _OutputPath;
      if (reason == "injected") {
        placeBackUpOnServer += @"\OA3ActivationScriptAfterInject.bat";
      } else {
        placeBackUpOnServer += @"\OA3ActivationScriptAfterClearKey.bat";
      }

      try {
        if (!File.Exists(placeBackUpOnServer)) {
          using (StreamWriter sw = File.CreateText(placeBackUpOnServer)) {
            sw.WriteLine(batchCommand);
          }
        } else {
          File.Delete(placeBackUpOnServer);
          using (StreamWriter sw = File.CreateText(placeBackUpOnServer)) {
            sw.WriteLine(batchCommand);
          }
        }
      } catch (IOException ex) {
        MessageBox.Show("Exception:\t \r\n\r\n" + ex);
      }
    }

    private void createLocalCopy(string from, string to) {
      try {
        try {
          if (File.Exists(from)) {
          } else {
            MessageBox.Show(from + " doesn't exist");
          }
        } catch (Exception ex) {
          ex.ToString();
        }
        File.Copy(from, to, true);
      } catch (Exception ex) {
        MessageBox.Show(ex.ToString());
      }
    }

    public void deleteLocalCopy(string[] localFilePaths) {
      for (int i = 0; i < localFilePaths.Length; i++) {
        try {
          if (File.Exists(localFilePaths[i])) {
            File.Delete(localFilePaths[i]);
          }
        } catch (Exception ex) {
          MessageBox.Show(ex.ToString());
        }
        try {
          fileNameSendToCMD[i] = null;
          fileServerToLocal[i] = null;
        } catch (Exception ex) {
          MessageBox.Show(ex.ToString());
        }
      }
    }

    public void setFileNames(string originalLocation, string newLocation) {
      for (int i = 0; i < fileServerToLocal.Length; i++) {
        if (fileServerToLocal[i] == null) {
          try {
            fileServerToLocal[i] = originalLocation;
            fileNameSendToCMD[i] = newLocation;

            createLocalCopy(fileServerToLocal[i], fileNameSendToCMD[i]);
            break;
          } catch (Exception ex) {
            MessageBox.Show(ex.ToString());
          }
        }
      }
    }
  }
}