using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Management;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections;
using System.Security.Principal;

namespace OA3ActivationScript
{
    public partial class FrmMain : Form
    {
        private int _minheight;
        private int _iSpecial;
        private string _MappedDrive;
        private string _ipAddress;
        private string _boxxFamily;
        private string _SerialNumber;
        private string _ProduKeyPath;
        private string _OA3ToolPath;
        private string _OutputPath;

        public class KeyPair
        {
            public string key;
            public string value;
        }
        public FrmMain(string outputPath, string driveLetter, string function)
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                MessageBox.Show("The program must be run as Admin" + Environment.NewLine
                  + Environment.NewLine +
                  "Please restart the program" + Environment.NewLine
                  + Environment.NewLine +
                  "If clearKey or injection process has already started. A batch file has been created on the server");
                ErrorAndExit("The program must be run as Admin" + Environment.NewLine
                  + Environment.NewLine +
                  "Please restart the program" + Environment.NewLine
                  + Environment.NewLine +
                  "If clearKey or injection process has already started. A batch file has been created on the server",

                  "Allow to run as Administrator",

                  "No Admin Privileaes Allowed");
            }
            else
            {
                try
                {
                    SetCurrentDirectory();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                if (function == "injected")
                {
                    driveLetter = driveLetter.Substring(0, 1);
                    try
                    {
                        CheckNetworkPathAvailability("reboot");
                        _OA3ToolPath = ConfigurationManager.AppSettings["OA3toolPath"].ToString();
                        OA3_Action oaAction = new OA3_Action();
                        oaAction._OutputPath = outputPath;
                        oaAction._OA3ToolPath = _OA3ToolPath;
                        oaAction.Validate();
                        oaAction.Report();
                        FrmMain_Close("", null);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Problem caught in frmMain()" + Environment.NewLine + Environment.NewLine + ex);
                    }
                }
                else if (function == "clearKey")
                {
                    SetCurrentDirectory();
                    InitializeComponent();
                    btnClearKey.Visible = false;
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
                else
                {
                    InitializeComponent();
                }
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            //TopMost = true;
            WindowState = FormWindowState.Maximized;

            bool foundCorrectIp = false;

            //Check networking
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                IPInterfaceProperties props = ni.GetIPProperties();
                //foreach()

                string aD = ni.Description;
                IPInterfaceProperties iPp = ni.GetIPProperties();
                iPp.DhcpServerAddresses.Count();

                _ipAddress = Dns.GetHostAddresses(Dns.GetHostName()).Where(address => address.AddressFamily == AddressFamily.InterNetwork).First().ToString();

                //iPp.DhcpServerAddresses.First<>;
                string DhcpAddress = ConfigurationManager.AppSettings["DhcpAddress"];
                foreach (System.Net.IPAddress address in iPp.DhcpServerAddresses)
                {
                    if (DhcpAddress == address.ToString())
                    {
                        foundCorrectIp = true;
                    }
                }
            }

            if (foundCorrectIp)
            {
                lblNetwork.BackColor = Color.Green;
                lblNetwork.ForeColor = Color.White;
            }
            else
            {
                lblNetwork.BackColor = Color.Red;
                ErrorAndExit("No network interface is connected to the production network.", "No appropriate network connections", "Wrong Network Connection");
            }

            //Check network path availability
            CheckNetworkPathAvailability("loadUp");

            //Add Radio Buttons
            List<KeyPair> osOptions = new List<KeyPair>();
            List<KeyPair> mbOptions = new List<KeyPair>();
            foreach (string s in ConfigurationManager.AppSettings.AllKeys)
            {
                if ((s.Length > 3) && (s.Substring(0, 3) == "OS_"))
                {
                    osOptions.Add(new KeyPair { key = s.Substring(3, s.Length - 3), value = ConfigurationManager.AppSettings[s] });
                }
                else if ((s.Length > 3) && (s.Substring(0, 3) == "MB_"))
                {
                    mbOptions.Add(new KeyPair { key = s.Substring(3, s.Length - 3), value = ConfigurationManager.AppSettings[s] });
                }
            }
            int osMinHeight = (osOptions.Count + 1) * 20 + 150;
            int otherMinHeight = (mbOptions.Count + 1) * 20 + 335;
            if (osMinHeight > otherMinHeight)
            {
                _minheight = osMinHeight;
            }
            else
            {
                _minheight = otherMinHeight;
            }

            //Add Radio buttons
            string sSpecial = ConfigurationManager.AppSettings["SpecialRadioButtons"];

            bool res = int.TryParse(sSpecial, out _iSpecial);
            if (res == false)
            {
                _iSpecial = 0;
            }

            for (int i = 0; i < osOptions.Count; i++)
            {
                KeyPair kp = osOptions[i];
                RadioButton rdo = new RadioButton();
                rdo.Name = kp.key;
                rdo.Text = kp.value;
                rdo.AutoSize = false;
                rdo.Height = 17;
                rdo.Width = 400;
                rdo.TextAlign = ContentAlignment.MiddleLeft;
                Point p = new Point();
                if (i < osOptions.Count - _iSpecial)
                {
                    //put these at the top
                    p.X = 10;
                    p.Y = (i + 1) * 20;
                }
                else
                {
                    //put these at the bottom
                    p.X = 10;
                    p.Y = gbOsRadio.Height - 20 * (i + 1 - _iSpecial);
                }
                rdo.Location = p;
                //if (i == 2) {                                                                                                               //for testing only
                //	rdo.Checked = true;                                                                                                       //
                //}                                                                                                                           //
                gbOsRadio.Controls.Add(rdo);
            }
            for (int i = 0; i < mbOptions.Count; i++)
            {
                KeyPair kp = mbOptions[i];
                RadioButton rdo = new RadioButton();
                rdo.Name = kp.key;
                rdo.Text = kp.value;
                rdo.AutoSize = false;
                rdo.Height = 17;
                rdo.Width = 400;
                rdo.TextAlign = ContentAlignment.MiddleLeft;
                Point p = new Point();
                p.X = 10;
                p.Y = (i + 1) * 20;
                rdo.Location = p;

                ////Auto Select the motherboard radio button                                                                                            //TODO auto select 2nd radio button group
                //ProcessStartInfo runStartInfo = new ProcessStartInfo();
                //runStartInfo.FileName = "cmd.exe";
                //runStartInfo.Arguments = "/c wmic baseboard get manufacturer";
                //Process runProcess = new Process();
                //runProcess.StartInfo = runStartInfo;
                //runProcess.StartInfo.UseShellExecute = false;
                //runProcess.StartInfo.RedirectStandardInput = true;
                //runProcess.StartInfo.RedirectStandardOutput = true;
                //runStartInfo.CreateNoWindow = true;
                //runProcess.Start();

                //string ee = "";
                //while (!runProcess.StandardOutput.EndOfStream) {
                //  ee = runProcess.StandardOutput.ReadLine().Trim();

                //  if (i == 0 && ee == "ASUS") {
                //    rdo.Checked = true;
                //    break;
                //  } else if (i == 1 && ee == "Gigabyte") {
                //    rdo.Checked = true;
                //    break;
                //  } else if (i == 2) {
                //    rdo.Checked = true;
                //    break;
                //  }
                //}

                //runProcess.Close();
                gbMB.Controls.Add(rdo);
            }
            try
            {
                Point p = new Point();
                p.X = 445;
                p.Y = ((mbOptions.Count + 1) * 20) + 115;
                gbActions.Location = p;
            }
            catch (Exception ex)
            {
                dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.Red;
                ErrorAndExit("Error moving group box." + ex.InnerException, "Error moving group box", "Interface Issue");
            }

            gbMB.Height = (mbOptions.Count + 1) * 20 + 10;
            dataGridView1.Rows[0].Cells[6].Value = _ipAddress;
            try
            {
                ManagementObjectSearcher mgmtObjSearch = new ManagementObjectSearcher(@"root\cimv2", "Select * from Win32_ComputerSystem");
                foreach (ManagementObject obj in mgmtObjSearch.Get())
                {
                    dataGridView1.Rows[0].Cells[1].Value = obj["Manufacturer"];
                    string strModel = obj["Model"].ToString();
                    dataGridView1.Rows[0].Cells[2].Value = strModel;

                    switch (strModel.Substring(0, 5).ToUpper())
                    {
                        case "APEXX":
                            _boxxFamily = "APEXX";
                            break;
                        case "RENDE":
                            _boxxFamily = "RenderBOXX";
                            break;
                        case "GOBOX":
                            _boxxFamily = "GoBOXX";
                            break;
                        default:
                            _boxxFamily = "APEXX";
                            break;
                    }
                    dataGridView1.Rows[0].Cells[0].Value = _boxxFamily;
                }
            }
            catch (ManagementException ex)
            {
                dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.Red;
                ErrorAndExit("Unable to gather system manufacturer WMI information." + ex.InnerException, "Unable to gather WMI Information", "WMI Manufacturer Error");
            }

            try
            {
                ManagementObjectSearcher mgmtObjSearch = new ManagementObjectSearcher(@"root\cimv2", "Select * from Win32_BIOS");
                foreach (ManagementObject obj in mgmtObjSearch.Get())
                {
                    _SerialNumber = obj["SerialNumber"].ToString();
                    dataGridView1.Rows[0].Cells[3].Value = _SerialNumber;
                    dataGridView1.Rows[0].Cells[7].Value = obj["Manufacturer"];
                }
            }
            catch (ManagementException ex)
            {
                dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.Red;
                ErrorAndExit("Unable to gather BIOS WMI information." + ex.InnerException, "Unable to gather WMI Information", "WMI BIOS Error");
            }

            //////Check for valid Serial Number - starts with B																																																//TODO uncomment when put in use
            //if (_SerialNumber.Substring(0, 1) != "B") {
            //  ErrorAndExit(@"Serial Number must start with a ""B""", "Invalid Serial Number", @"Invalid Serial Number (B)");
            //}

            //////Check for valid Serial Number B followed by 6 numbers
            //string compare = _SerialNumber.Substring(1, _SerialNumber.Length - 1);
            //if (!(Regex.IsMatch(compare, @"^[0-9]*$") && compare.Length == 6)) {
            //  ErrorAndExit(@"Serial Number must have 6 digits", "Invalid Serial Number", "Invalid Serial Number");
            //}

            try
            {
                ManagementObjectSearcher mgmtObjSearch = new ManagementObjectSearcher(@"root\cimv2", "Select * from Win32_SystemEnclosure");
                foreach (ManagementObject obj in mgmtObjSearch.Get())
                {
                    dataGridView1.Rows[0].Cells[4].Value = obj["SerialNumber"];
                }
            }
            catch (ManagementException ex)
            {
                dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.Red;
                ErrorAndExit("Unable to gather Enclosure WMI information." + ex.InnerException, "Unable to gather WMI Information", "WMI Enclosure Error");
            }

            try
            {
                ManagementObjectSearcher mgmtObjSearch = new ManagementObjectSearcher(@"root\cimv2", "Select * from Win32_Processor");
                int numCores = 0;
                foreach (ManagementObject obj in mgmtObjSearch.Get())
                {
                    int theseCores = new int();
                    bool isCoreCountAnInt = int.TryParse(obj["NumberOfCores"].ToString(), out theseCores);
                    if (isCoreCountAnInt == false)
                    {
                        theseCores = 0;
                    }
                    numCores = numCores + theseCores;
                }
                dataGridView1.Rows[0].Cells[5].Value = numCores;
            }
            catch (ManagementException ex)
            {
                dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.Red;
                ErrorAndExit("Unable to gather Processor WMI information." + ex.InnerException, "Unable to gather WMI Information", "WMI Processor Error");
            }

            if (_MappedDrive != null)
            {
                //Determine whether the directory for the product family exists
                if (!(Directory.Exists(_MappedDrive + @"\" + _boxxFamily)))
                {
                    Directory.CreateDirectory(_MappedDrive + @"\" + _boxxFamily);
                }
                //Determine whether the directory for the specific serial number exists
                _OutputPath = _MappedDrive + @"\" + _boxxFamily + @"\" + _SerialNumber;
                //_OutputPath = _MappedDrive + @"\""" + _boxxFamily + @"""\""" + _SerialNumber + "\"";
                if (!(Directory.Exists(_OutputPath)))
                {
                    Directory.CreateDirectory(_OutputPath);
                }
                else
                {
                    if (btnClearKey.Visible == true)
                    {
                        DialogResult dialog = MessageBox.Show(
                                      "Serial Number already exists in the System" + Environment.NewLine + "Do you want to run OA3 again?",
                                      "Run OA3 again?",
                                      MessageBoxButtons.YesNo);

                        //grab text in all files already created, create txt files and place in log folder, then delete original files
                        if (dialog == DialogResult.Yes)
                        {
                            RerunOA3();
                        }
                        else if (dialog == DialogResult.No)
                        {
                            MessageBox.Show("Exiting application" + Environment.NewLine + "Click OK to exit",
                                            "Exiting application",
                                            MessageBoxButtons.OK);
                            Application.Exit();
                        }
                    }
                }
            }

            //Determine whether the directory for the specific serial number exists
            if (Directory.Exists(_OutputPath))
            {
                if (!(Directory.Exists(_OutputPath + @"\Logs")))
                {
                    Directory.CreateDirectory(_OutputPath + @"\Logs");
                }
            }

            //check to make sure the paths exist
            try
            {
                _ProduKeyPath = ConfigurationManager.AppSettings["ProduKeyPath"].ToString();
                _OA3ToolPath = ConfigurationManager.AppSettings["OA3toolPath"].ToString();
            }
            catch (ManagementException ex)
            {
                dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.Red;
                ErrorAndExit("Tool paths not available." + ex.InnerException, "Tool paths not available", "Tool Path Error");
            }
        }

        private void CheckNetworkPathAvailability(string state)
        {
            ManagementScope scope = new ManagementScope("root\\CIMV2");
            SelectQuery sQuery = new SelectQuery("Select * from Win32_LogicalDisk Where DriveType = 4");
            ManagementObjectSearcher search = new ManagementObjectSearcher(sQuery);
            char newDriveLetter = new char();
            bool bFound = false;
            for (char c = 'Z'; c >= 'D'; c--)
            {
                bool bNotFound = true;
                string sCheck = c.ToString() + ":";
                foreach (ManagementObject queryObj in search.Get())
                {
                    if (sCheck == queryObj["DeviceID"].ToString())
                    {
                        bNotFound = false;
                    }
                }
                bFound = bNotFound;
                if (bFound)
                {
                    newDriveLetter = c;
                    break;
                }
            }
            if (newDriveLetter == '\0')
            {
                ErrorAndExit("No available network drive letters." + Environment.NewLine + "Please unmap drives manually", "No available letters.", "Network Drives Full");
            }
            else
            {
                // tries to connect to an alias
                ArrayList shareNames = new ArrayList();
                shareNames.Add(@"\\ZEUS\Add_Software\Log\");
                shareNames.Add(@"\\ZEUS1\Add_Software\Log\");
                shareNames.Add(@"\\ZEUS2\Add_Software\Log\");
                shareNames.Add(@"\\ZEUS3\Add_Software\Log\");
                shareNames.Add(@"\\ZEUS4\Add_Software\Log\");
                shareNames.Add(@"\\boxxintranet\engineering\TempCharlesTest\");

                string sPassword = null;
                string sUserName = null;
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MapPassword"].ToString()))
                {
                    sPassword = ConfigurationManager.AppSettings["MapPassword"].ToString();
                }
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MapUserName"].ToString()))
                {
                    sUserName = ConfigurationManager.AppSettings["MapUserName"].ToString();
                }
                DriveSettings ds = new DriveSettings();

                int returnValue = 0;
                foreach (string mapFolderPath in shareNames)
                {
                    returnValue = ds.MapNetworkDrive(newDriveLetter.ToString(), mapFolderPath, sUserName, sPassword);
                    if (returnValue == 0)
                    {
                        if (state == "loadUp")
                        {
                            lblShare.BackColor = Color.Green;
                            lblShare.ForeColor = Color.White;
                            lblShare.Text = "Shared Folder: " + newDriveLetter.ToString() + @":\";
                        }
                        _MappedDrive = newDriveLetter.ToString() + ":";
                        //MessageBox.Show("Connected to:\t" + mapFolderPath);
                        break;
                    }
                }
                if (returnValue != 0)
                {
                    if (state == "loadUp")
                    {
                        lblShare.BackColor = Color.Red;
                    }
                    MessageBox.Show(
                        "return value:\t" + returnValue + Environment.NewLine +
                        "Mapped Drive:\t" + _MappedDrive?.ToString());
                    ErrorAndExit("Unable to connect to shared drive.", "Unable to connect to shared drive", "Shared Drive Error");
                }
            }
        }

        private void RerunOA3()
        {
            OA3_Action oaAction = new OA3_Action();
            oaAction._OutputPath = _OutputPath;
            //oaAction._SelectedMotherBoardText = gbMB.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked).Text;
            //oaAction.ClearKey();
            if (!(Directory.Exists(_OutputPath + @"\Logs")))
            {
                Directory.CreateDirectory(_OutputPath + @"\Logs");
            }
            //TODO condense this 1/17/18
            string[] filesArray = Directory.GetFiles(_OutputPath);
            List<string> fileNameArray = new List<string>();

            foreach (string fullPath in filesArray)
            {
                string fileName = Path.GetFileName(fullPath);
                try
                {
                    string from = System.IO.Path.Combine(fullPath);
                    string to = System.IO.Path.Combine(_OutputPath + @"\Logs\" + fileName);
                    File.Move(from, to);
                    fileNameArray.Add(to);
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex);
                }
            }

            foreach (string fullFilePath in fileNameArray)
            {
                string fileName = Path.GetFileName(fullFilePath);
                string directory = Path.GetDirectoryName(fullFilePath);
                string outputString = "";
                string line;

                try
                {
                    //After file move, append txt to already existing file if exists & rename .bin file with timestamp
                    if (Path.GetFileName(fullFilePath) != "OA3.bin")
                    {
                        StreamReader reader1 = new StreamReader(fullFilePath);
                        while ((line = reader1.ReadLine()) != null)
                        {
                            outputString += line + "\r\n";
                        }

                        reader1.Close();
                        File.Delete(fullFilePath);

                        string outputPath = directory + @"\" + fileName + "_OA3Log.txt";
                        Process_Action pa = new Process_Action();
                        pa.moveTextToFile(outputPath, outputString, "moveFiles");
                    }
                    else
                    {
                        string newFileName = directory + @"\"
                            + fileName + "_"
                            + DateTime.Now.Month.ToString() + "-"
                            + DateTime.Now.Day.ToString() + "-"
                            + DateTime.Now.Year.ToString() + " Time_"
                            + DateTime.Now.Hour.ToString() + "_"
                            + DateTime.Now.Minute.ToString() + "_"
                            + DateTime.Now.Millisecond.ToString()
                            + Path.GetExtension(fileName);
                        File.Move(fullFilePath, newFileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }


            }
            //oaAction.DefaultKey();
        }

        private void ErrorAndExit(string message, string windowTitle, string reason)
        {
            DialogResult dialog = MessageBox.Show(message + Environment.NewLine + "Click OK to exit", windowTitle, MessageBoxButtons.OK);
            LogErrorExit(message, windowTitle, reason);
            if (_MappedDrive == null)
            {
                DialogResult dialog2 = MessageBox.Show("Check drive to see log files"
                  + Environment.NewLine + "Log Server unavailable",
                  windowTitle,
                  MessageBoxButtons.OK);
            }
            Application.Exit();
        }

        private void LogErrorExit(string message, string windowTitle, string reason)
        {
            string line = "";
            string logDirectory = "";
            string logPath = "";
            string zeusMapped = ConfigurationManager.AppSettings["MapFolderPath"];
            ////string zeusMapped = @"\\boxxintranet\engineering\TempCharlesTest\"; //testing purposes

            if (_MappedDrive != null)
            {
                logDirectory = _MappedDrive + @"\";
            }
            else
            {
                logDirectory = Path.GetPathRoot(Directory.GetCurrentDirectory()) + @"\Log Errors\";
            }
            string logString = "";

            logString += "Mapped Folder:\t" + zeusMapped + @"Log Errors\" + "\r\n";
            logString += "Mapped Drive:\t" + _MappedDrive?.ToString() + "\r\n";
            logString += "IP Address:\t" + _ipAddress?.ToString() + "\r\n";
            logString += "Boxx Family:\t" + _boxxFamily?.ToString() + "\r\n";
            logString += "Serial Number:\t" + _SerialNumber?.ToString() + "\r\n";
            logString += "Product Path:\t" + _ProduKeyPath?.ToString() + "\r\n";
            logString += "OA3ToolPath:\t" + _OA3ToolPath?.ToString() + "\r\n";
            logString += "Output Path:\t" + _OutputPath?.ToString() + "\r\n";

            if (!(Directory.Exists(logDirectory)))
            {
                Directory.CreateDirectory(logDirectory);
            }
            logPath = logDirectory + reason + " " + DateTime.Now.ToString("yyyyMMddHHmmss") + @".txt";
            if (!File.Exists(logPath))
            {
                try
                {
                    using (StreamWriter sw = File.CreateText(logPath))
                    {
                        sw.WriteLine(reason);
                        sw.WriteLine("Window Title:\t" + windowTitle);
                        sw.WriteLine("Date:\t\t" + DateTime.Now.ToString());
                        sw.WriteLine();
                        sw.WriteLine(logString);
                        sw.WriteLine();
                        sw.WriteLine("Message:" + Environment.NewLine + message);
                    }
                }
                catch (Exception ex)
                {
                    DialogResult dialog = MessageBox.Show(ex.ToString()
                                  + Environment.NewLine + "Click OK to exit",
                                  "Did not log error", MessageBoxButtons.OK);
                }
            }
            else
            {
                string outputString2 = "";
                int counter = 0;
                StreamReader reader2 = new StreamReader(logPath);
                while ((line = reader2.ReadLine()) != null && counter < 1000)
                {
                    outputString2 += line + "\r\n";
                    counter++;
                }
                reader2.Close();
                File.WriteAllText(logPath, String.Empty);
                using (StreamWriter sw = File.AppendText(logPath))
                {
                    sw.WriteLine(reason);
                    sw.WriteLine("Window Title:\t " + windowTitle);
                    sw.WriteLine("Message:\t " + message);
                    sw.WriteLine("Date: " + DateTime.Now.ToString());
                    sw.WriteLine();
                    sw.WriteLine(outputString2);
                }
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmMain_Resize_1(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            int gbHeight = gbOsRadio.Height;
            if (control.Size.Height > (gbHeight + 150))
            {
                //The window is bigger, make the GB bigger
                gbOsRadio.Size = new Size(gbOsRadio.Width, control.Size.Height - 150);
            }
            else if (control.Size.Height == (gbHeight + 150))
            {
                //the height is right, do nothing.
            }
            else
            {
                //The window is smaller, but not too small, shrink the gb
                if (control.Size.Height >= _minheight)
                {
                    gbOsRadio.Size = new Size(gbOsRadio.Width, control.Size.Height - 150);
                }
                //The window is too small, make the window bigger, and reset the gb to the min height
                else
                {
                    control.Size = new Size(dataGridView1.Width + 40, _minheight);
                    gbOsRadio.Size = new Size(gbOsRadio.Width, control.Size.Height - 150);
                }
            }
            if (control.Size.Width < 890)
            {
                control.Size = new Size(890, control.Size.Height);
            }

            for (int i = 0; i < gbOsRadio.Controls.Count; i++)
            {
                Point p = new Point();
                if (i < gbOsRadio.Controls.Count - _iSpecial)
                {
                    //put these at the top
                    p.X = 10;
                    p.Y = (i + 1) * 20;
                }
                else
                {
                    //put these at the bottom
                    p.X = 10;
                    p.Y = gbOsRadio.Height - 20 * (i + 1 - _iSpecial);
                }
                gbOsRadio.Controls[i].Location = p;
            }
        }

        private void FrmMain_Close(object sender, FormClosingEventArgs e)
        {
            int exitCode = new int();

            if (!String.IsNullOrEmpty(_MappedDrive))
            {
                exitCode = DriveSettings.DisconnectNetworkDrive(_MappedDrive, true);
            }

            if (exitCode != 0)
            {
                MessageBox.Show("Exiting with code: " + exitCode + Environment.NewLine + Environment.NewLine + "PLEASE UNMAP DRIVE MANUALLY", "Exit Code", MessageBoxButtons.OK);
            }
        }

        private void BtnProductKey_Click(object sender, EventArgs e)
        {
            //if (validateForm(this)) {
            try
            {
                //if breaks here, check if the exe is missing
                using (Process exeProcess = Process.Start(_ProduKeyPath))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch (ManagementException ex)
            {
                dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.Red;
                ErrorAndExit("Unable to run ProduKey." + ex.InnerException, "Unable to run ProduKey", "ProduKey Error");
            }
            //}
        }

        private bool ValidateForm(Form form)
        {
            bool valid = true;
            var checkedButton = gbOsRadio.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            if (checkedButton == null)
            {
                gbOsRadio.BackColor = Color.LightPink;
                valid = false;
            }
            else
            {
                gbOsRadio.BackColor = SystemColors.Control;
            }
            checkedButton = gbMB.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            if (checkedButton == null)
            {
                gbMB.BackColor = Color.LightPink;
                valid = false;
            }
            else
            {
                gbMB.BackColor = SystemColors.Control;
            }
            if (!valid)
            {
                MessageBox.Show("Please select a value for all required fields", "Form validation error", MessageBoxButtons.OK);
            }
            return valid;
        }

        private void BtnClearKey_Click(object sender, EventArgs e)
        {
            if (ValidateForm(this))
            {
                OA3_Action oaAction = new OA3_Action();
                oaAction._SelectedMotherBoardText = gbMB.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked).Text;
                oaAction._OutputPath = _OutputPath;
                oaAction._DriveLetter = _MappedDrive;
                oaAction.ClearKey();
                //close all programs and restart the computer
                Restart();
            }
        }

        private void BtnDefault_Click(object sender, EventArgs e)
        {
            if (ValidateForm(this))
            {
                OA3_Action oaAction = new OA3_Action();
                oaAction._OutputPath = _OutputPath;
                oaAction.DefaultKey();
            }
        }

        private void RunOA3()
        {
            OA3_Action oaAction = new OA3_Action();
            if (ValidateForm(this))
            {
                oaAction._ReplaceSku = gbOsRadio.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked).Name.ToString();
                oaAction._SelectedMotherBoardText = gbMB.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked).Text;
                oaAction._ReplaceModel = _boxxFamily;
                oaAction._OutputPath = _OutputPath;
                oaAction.CreateConfigXml();
                oaAction._OA3ToolPath = _OA3ToolPath;

                if (cbDefaultKey.Checked)
                {
                    oaAction.DefaultKey();
                }
                oaAction.Assemble();
                oaAction._DriveLetter = _MappedDrive;
                oaAction._SerialNumber = _SerialNumber;
                oaAction.Inject();
                //oaAction.Validate();
                //oaAction.Report();
                //close all programs and restart the computer
                Restart();
            }
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (File.Exists(_OutputPath + @"\OA3.cfg"))
            {
                DialogResult dialog = MessageBox.Show(
                  "OA3 has already run." + Environment.NewLine + "Do you want to run OA3 again?",
                  "Run OA3 again?",
                  MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    RerunOA3();
                    RunOA3();
                }
            }
            else
            {
                RunOA3();
            }
        }

        private static void Restart()
        {
            ProcessStartInfo proc = new ProcessStartInfo();
            proc.WindowStyle = ProcessWindowStyle.Hidden;
            proc.FileName = "cmd";
            proc.Arguments = @"/C shutdown -f -r -t 5 -c ""System will restart and continue the OA3 Process""";
            Process.Start(proc);
        }

        private static void SetCurrentDirectory()
        {
            Directory.SetCurrentDirectory(@"\\zeus\Add_Software\_RunOA3\OA3ActivationScript\bin\Debug\");
        }
    }
}
