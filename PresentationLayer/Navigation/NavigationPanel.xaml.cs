using PresentationLayer.Pages;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.Navigation {
    /// <summary>
    /// Interaction logic for NavigationPanel.xaml
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public partial class NavigationPanel : UserControl {

        private NavigationControl navigationControl;

        public NavigationPanel() {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            navigationControl = new NavigationControl(ccMain, ccTitleBar, ccNavigationMenu);
            navigationControl.SetPage(new LoginPage(navigationControl), "Prijava");
        }

    }
}
