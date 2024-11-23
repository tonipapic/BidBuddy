using System.Diagnostics;

namespace PresentationLayer.Helpers {

    /// <summary>
    /// Used to give user documentation.
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public static class HelpProvider {

        /// <summary>
        /// Shows user documentation.
        /// </summary>
        public static void ShowHelp() {
            Process.Start(@".\UserHelp\User Documentation.pdf");
        }

    }
}
