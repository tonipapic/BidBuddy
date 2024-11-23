using BusinessLogicLayer.Services;
using EntitiesLayer.Entities;
using PresentationLayer.Navigation;
using PresentationLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PresentationLayer.Pages.UserPages
{
    /// <summary>
    /// Interaction logic for AuctionsLoadingPage.xaml
    /// </summary>
    public partial class AuctionsLoadingPage : UserControl
    {
        private NavigationControl navigationControl;
        private DispatcherTimer progressTimer = new DispatcherTimer();
        public AuctionsLoadingPage(NavigationControl navigationControl)
        {
            this.navigationControl = navigationControl;
            InitializeComponent();
            InitializeMyTimer();
        }

        private void InitializeMyTimer()
        {
            progressTimer.Interval = TimeSpan.FromMilliseconds(1);
            progressTimer.Tick += new EventHandler(IncreaseProgressBar);
            progressTimer.Start();
        }
        private void IncreaseProgressBar(object sender, EventArgs e)
        {
            prgBarAuctions.Value += 1;
            if (prgBarAuctions.Value == prgBarAuctions.Maximum)
            {
                progressTimer.Stop();
                prgBarAuctions.IsIndeterminate = true;
            }
        }
    }
}
