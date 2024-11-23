using BusinessLogicLayer.Services;
using EntitiesLayer.Entities;
using PresentationLayer.ApplicationTheme;
using PresentationLayer.Dialogs;
using PresentationLayer.Helpers;
using PresentationLayer.Navigation;
using PresentationLayer.UserControls;
using PresentationLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PresentationLayer.Pages
{
    enum AuctionSorting
    {
        Recent,
        Ending,
        MostWanted,
        Cheap,
        Valuable
    }
    /// <summary>
    /// Interaction logic for AuctionsPage.xaml
    /// </summary>
    public partial class AuctionsPage : UserControl
    {

        private AuctionSorting sorting = AuctionSorting.Recent;
        private RecommendationService recommendationsService;
        private NavigationControl navigationControl;
        private ComboBoxHelper<Region> cbhRegions;
        private AuctionService auctionService;
        private List<Category> categories;
        private Category selectedCategory;

        private AuctionsPage(NavigationControl navigationControl)
        {
            recommendationsService = new RecommendationService();
            this.navigationControl = navigationControl;
            auctionService = new AuctionService();
            categories = new CategoryService().GetAllCategories();

            InitializeComponent();
            cbhRegions = new ComboBoxHelper<Region>(cmbPickRegion, x => x.Name);
        }

        public static async Task<AuctionsPage> CreateAuctionsPageAsync(NavigationControl navigationControl)
        {
            AuctionsPage page = new AuctionsPage(navigationControl);
            Task pageTask = page.SetDataContext();
            await pageTask;
            return page;
        }

        private async void SearchAuctions(object sender, RoutedEventArgs e = null)
        {
            await SetDataContext();
        }

        private Filter<Auction> GetFilters()
        {
            Filter<Auction> filter = new Filter<Auction>();
            string auctionName = txtBoxAuctioName.Text.ToLower() ?? string.Empty;
            string auctionAuthor = txtBoxAuthor.Text ?? string.Empty;
            DateTime untilDate = dpUntil.SelectedDate ?? DateTime.Now.AddYears(1);
            Region selectedRegion = cbhRegions.CurrentItem();
            bool isVerified = chkBoxIsVerified.IsChecked ?? false;

            filter.AddFilter(x => x.EndDate > DateTime.Now);
            filter.AddFilter(x => x.EndDate <= untilDate);
            filter.AddFilter(x => x.User.IsVerified == isVerified || x.User.IsVerified);

            filter.AddFilter(
                x => x.Name.ToLower().Contains(auctionName) ||
                String.IsNullOrEmpty(auctionName));

            filter.AddFilter(
                x => x.User.Username.ToLower().Contains(auctionAuthor) ||
                String.IsNullOrEmpty(auctionAuthor));

            if (selectedRegion != null)
                filter.AddFilter(x => x.Region.RegionId == selectedRegion.RegionId);
            if (selectedCategory != null)
                filter.AddFilter(x => x.Category.CategoryId == selectedCategory.CategoryId);

            return filter;
        }



        private Task<List<Auction>> FetchAuctions()
        {
            Filter<Auction> filter = new Filter<Auction>();
            filter.AddFilter(x => x.EndDate > DateTime.Now);
            return auctionService.GetAuctionsFilteredAsync(filter);
        }

        private Task<List<Auction>> FetchRecommended()
        {
            return recommendationsService.GetUsersRecommendedAuctions(AuthenticationService.LoggedUser, 6);
        }

        private async Task SetDataContext()
        {
            var auctionsTask = FetchAuctions();
            var auctions = await auctionsTask;
            var recommendedTask = FetchRecommended();
            var recommended = await recommendedTask;
            auctions = ApplySorting(auctions);
            DataContext = new AuctionListViewModel(navigationControl, auctions, recommended);
        }

        private List<Auction> ApplySorting(List<Auction> auctions)
        {
            switch (sorting)
            {
                case AuctionSorting.Recent:
                    auctions = auctions.OrderBy(x => x.CreationDate).ToList();
                    break;
                case AuctionSorting.Ending:
                    auctions = auctions.OrderBy(x => x.EndDate).ToList();
                    break;
                case AuctionSorting.MostWanted:
                    auctions = auctions.OrderBy(x => x.AuctionBids.Count).ToList();
                    break;
                case AuctionSorting.Cheap:
                    auctions = auctions.OrderBy(x => x.GetCurrentBid()).ToList();
                    break;
                case AuctionSorting.Valuable:
                    auctions = auctions.OrderByDescending(x => x.GetCurrentBid()).ToList();
                    break;
                default:
                    break;
            }

            return auctions;
        }

        private void SetTextForeground(ContentControl control)
        {
            var color = new SolidColorBrush(AppColors.BackgroundWhite);
            control.Foreground = color;
            foreach (var child in DescendantFinder.FindVisualChildren<TextBlock>(control))
            {
                if (AncestorFinder.FindAncestor<Button>(child) == null)
                    child.Foreground = color;
            }
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            var page = sender as AuctionsPage;
            if (page != null)
            {
                page.SetTextForeground(expFilters);
                page.SetTextForeground(expSorting);
                var regionService = new RegionService();
                page.cbhRegions.DataSource = regionService.GetAllRegions();
            }
        }

        private void ClearFilters(object sender, RoutedEventArgs e)
        {
            txtBoxAuctioName.Text = null;
            txtBoxAuthor.Text = null;
            dpUntil.SelectedDate = null;
            selectedCategory = null;

            chkBoxIsVerified.IsChecked = false;
            cbhRegions.DeselectCurrentItem();
            UpdateCategoryButton();
            SearchAuctions(this);
        }

        private void UpdateSortingOption(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var sorting = sender as TextBlock;
            var page = AncestorFinder.FindAncestor<AuctionsPage>(sorting);
            page.SetSelectedSorting(sorting);
            page.SearchAuctions(page);

        }

        private void SetSelectedSorting(TextBlock sort)
        {

            var foreground = new SolidColorBrush(AppColors.BackgroundBlack);
            sort.Foreground = new SolidColorBrush(AppColors.BackgroundWhite);

            sorting = (AuctionSorting)Enum.Parse(typeof(AuctionSorting), sort.Tag.ToString());

            foreach (var child in DescendantFinder.FindVisualChildren<TextBlock>(expSorting))
            {
                if (child.Tag != sort.Tag)
                {
                    child.Foreground = foreground;
                }
            }

        }

        private void SelectCategory(object sender, RoutedEventArgs e)
        {
            SelectCateogry();
            var page = AncestorFinder.FindAncestor<AuctionsPage>(sender as Button);
            page.UpdateCategoryButton();
        }

        private void UpdateCategoryButton()
        {
            string buttonText = "Odaberi";
            if (selectedCategory != null)
                buttonText = selectedCategory.Name;
            if (btnCategory != null)
                btnCategory.Content = buttonText;
        }

        private void SelectCateogry()
        {
            var dialog = new CategoryPickerDialog("Odaberite kategoriju", categories);
            dialog.ShowDialog();
            selectedCategory = dialog.SelectedCategory;
        }
    }
}
