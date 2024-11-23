using System.Collections.Generic;
using System.Windows.Automation;
using System.Windows.Controls;

namespace PresentationLayer.Navigation {

    /// <summary>
    /// Main class for handling navigation.
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public class NavigationControl {

        private ContentControl mainContentControl;
        private ContentControl titleBarContentControl;
        private ContentControl menuContentControl;

        private NavigationTitleBar titleBar;
        private NavigationMenu navMenu;

        private List<UserControl> pages = new List<UserControl>();
        private UserControl currentPage = null;

        public NavigationControl(ContentControl ccMain, ContentControl ccTitleBar, ContentControl ccMenu) {
            mainContentControl = ccMain;
            titleBarContentControl = ccTitleBar;
            menuContentControl = ccMenu;

            titleBar = new NavigationTitleBar(this);
            navMenu = new NavigationMenu(this);
        }

        /// <summary>
        /// Sets the page to display.
        /// Removes the current page and all previous pages.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="title"></param>
        public void SetPage(UserControl page, string title) {
            pages.Clear();
            PushPage(page, title);
        }

        /// <summary>
        /// Sets the page to display.
        /// Does not remove current page and previous pages.
        /// User can go back to previous pages.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="title"></param>
        public void PushPage(UserControl page, string title) {

            GetPageExtra(page).Title = title;
            AutomationProperties.SetAutomationId(page, "currentPage");
            AutomationProperties.SetName(page, title);

            pages.Add(page);
            currentPage = page;

            mainContentControl.Content = page;
            titleBar.UpdateTitleBar(pages);

        }

        /// <summary>
        /// Removes current page. Displays previous page or nothing.
        /// Parameters key and bundle are used to return data to previous page.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="bundle"></param>
        public void PopPage(string key = "", Bundle bundle = null) {

            if (pages.Count < 2) {
                pages.Clear();
                currentPage = null;

                mainContentControl.Content = null;
                titleBar.UpdateTitleBar(pages);
                return;
            }

            int pageToRemove = pages.Count - 1;
            int pageToShow = pageToRemove - 1;

            pages.RemoveAt(pageToRemove);
            currentPage = pages[pageToShow];

            mainContentControl.Content = pages[pageToShow];
            titleBar.UpdateTitleBar(pages);

            CurrentPageExtra.OnPageResultCallback?.Invoke(key, bundle);

        }

        /// <summary>
        /// Updates page's title.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="title"></param>
        public void UpdatePageTitle(UserControl page, string title) {
            var extra = GetPageExtra(page);
            extra.Title = title;
            titleBar.UpdateTitleBar(pages);
        }

        /// <summary>
        /// It is called when we return to the page with PopPage method.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="callback"></param>
        public void SetOnPageResult(UserControl control, OnPageResult callback) {
            var extra = GetPageExtra(control);
            extra.OnPageResultCallback = callback;
        }

        /// <summary>
        /// Displays navigation menu and titlebar.
        /// </summary>
        public void DisplayNavigationUI() {
            titleBarContentControl.Content = titleBar;
            menuContentControl.Content = navMenu;
        }

        /// <summary>
        /// Hides navigation menu and titlebar.
        /// </summary>
        public void HideNavigationUI() {
            titleBarContentControl.Content = null;
            menuContentControl.Content = null;
        }

        public NavigationMenu NavigationMenu { get => navMenu; }

        private NavigationPageExtra CurrentPageExtra { get => GetPageExtra(currentPage); }

        private NavigationPageExtra GetPageExtra(UserControl page) {

            NavigationPageExtra extra = page.Tag as NavigationPageExtra;

            if(extra == null) {
                extra = new NavigationPageExtra();
                page.Tag = extra;
            }

            return extra;
        }

    }
}
