using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TVController
{
    public partial class Test : Form
    {
        public Test()
        {
            InitializeComponent();
            this.Hide();
            SystemEvents.PowerModeChanged += SystemEventsOnPowerModeChanged;
            SystemEvents.SessionEnding +=SystemEventsOnSessionEnding;
            TvActive();
        }

        private void SystemEventsOnSessionEnding(object sender, SessionEndingEventArgs e)
        {
            TvDown();
        }

        private void SystemEventsOnPowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case PowerModes.Suspend:
                    TvDown();
                    break;
                case PowerModes.Resume:
                    TvActive();
                    break;
            }
        }

        public async void TvActive()
        {
            try
            {
                var tcp = new TcpClient();
                await tcp.ConnectAsync(new IPEndPoint(IPAddress.Parse("192.168.1.99"), 47777));
                tcp.Client.Send(Encoding.Default.GetBytes("UpTv"));
                tcp.Client.Dispose();

            }
            catch (Exception e)
            {
                Thread.Sleep(1000);
                TvActive();
            }
          
        }

        public async void TvDown()
        {
            try
            {
                var tcp = new TcpClient();
                await tcp.ConnectAsync(new IPEndPoint(IPAddress.Parse("192.168.1.99"), 47777));
                tcp.Client.Send(Encoding.Default.GetBytes("DownTv"));
                tcp.Client.Dispose();
            }
            catch (Exception e)
            {
               Thread.Sleep(1000);
               TvDown();
            }
          

        }
    }
}
