using Nordware.AddOn.MercadoDoAlarme.Core.BLL;
using SBO.Hub;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Nordware.AddOn.MercadoDoAlarme
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Application.Exit();
                return;
            }

            var sboApp = new SBOApp(args[0], $"{Application.StartupPath}\\Nordware.AddOn.MercadoDoAlarme.Core.dll");
            sboApp.InitializeApplication();

            InitializeBLL.Initialize();
            var oListener = new SBO.Hub.Services.Listener();
            var oThread = new Thread(new ThreadStart(oListener.startListener));
            oThread.IsBackground = true;
            oThread.Start();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run();
        }
    }
}
