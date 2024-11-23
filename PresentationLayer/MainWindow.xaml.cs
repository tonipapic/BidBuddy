using PresentationLayer.Helpers;
using System.Windows;
using System.Windows.Input;

namespace PresentationLayer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {}

        private void Window_KeyDown(object sender, KeyEventArgs e) {
            if(e.Key == Key.F1) {
                HelpProvider.ShowHelp();
            }
        }
    }
}
