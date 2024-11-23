namespace PresentationLayer.Navigation {

    public delegate void OnPageResult(string key, Bundle bundle);

    /// <summary>
    /// Extra info that is stored as a Tag in page.
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public class NavigationPageExtra {

        public string Title { get; set; }
        public OnPageResult OnPageResultCallback { get; set; }

    }

}
