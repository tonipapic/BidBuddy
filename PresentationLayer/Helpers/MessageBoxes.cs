using System.Windows;

namespace PresentationLayer.Helpers {

    /// <summary>
    /// Makes it easier to work with MessageBox.
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public static class MessageBoxes {

        /// <summary>
        /// Shows error message box with OK button.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        public static void ShowError(string title, string text) {
            MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Shows warning message with OK button.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        public static void ShowWarning(string title, string text) {
            MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// Shows info message with OK button.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        public static void ShowInfo(string title, string text) {
            MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Shows confirm message with OK and CANCEL buttons.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static MessageBoxResult Confirm(string title, string text) {
            return MessageBox.Show(text, title, MessageBoxButton.OKCancel, MessageBoxImage.Question);
        }

    }
}
