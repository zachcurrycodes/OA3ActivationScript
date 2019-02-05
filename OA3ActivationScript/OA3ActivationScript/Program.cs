using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OA3ActivationScript
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args) {      
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      if (args.Length == 0) {
        Application.Run(new FrmMain(null, null, null));
      } else {
        if (args[2] == "injected") {
        new FrmMain(args[0], args[1], args[2]);
        } else {
          Application.Run(new FrmMain(args[0], args[1], args[2]));
        }
      }
    }
  }
}
