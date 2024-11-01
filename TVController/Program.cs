using Microsoft.Win32;
using System.Windows.Forms;

namespace TVController
{
    internal static class Program
    {
        private static Test _test;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var notify = new NotifyIcon();
            _test = new Test();
            notify.Icon = _test.Icon;
            notify.Text = "TV Controller";
            notify.Visible = true;
            var context = new ContextMenuStrip();
            context.Items.Add("Exit", null, OnExit);
            notify.ContextMenuStrip = context;
            AppDomain.CurrentDomain.ProcessExit += ApplicationOnApplicationExit;
            Application.Run(_test);
            Application.ApplicationExit += ApplicationOnApplicationExit;

        }

        private static void OnExit(object? sender, EventArgs e)
        {
            Application.Exit();
        }

        private static void ApplicationOnApplicationExit(object? sender, EventArgs e)
        {
            _test.TvDown();
        }
    }
}