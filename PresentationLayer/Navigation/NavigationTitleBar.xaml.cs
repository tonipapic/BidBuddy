using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.Navigation {
    /// <summary>
    /// Interaction logic for NavigationTitleBar.xaml
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public partial class NavigationTitleBar : UserControl {

        private NavigationControl navigationControl;

        public NavigationTitleBar(NavigationControl navigationControl) {
            InitializeComponent();
            this.navigationControl = navigationControl;
        }

        public void UpdateTitleBar(List<UserControl> pages) {

            int pageCount = pages.Count;
            if(pageCount > 1) {
                btnGoBack.Visibility = Visibility.Visible;
            } else {
                btnGoBack.Visibility = Visibility.Collapsed;
            }

            string title = "";

            for(int i = 0; i < pages.Count; i++) {
                var pageExtra = pages[i].Tag as NavigationPageExtra;

                if (i != 0) title += " > ";
                title += pageExtra.Title;
            }

            lblTitle.Content = title;

        }

        private void btnGoBack_Click(object sender, RoutedEventArgs e) {
            navigationControl.PopPage();
        }

    }
}
