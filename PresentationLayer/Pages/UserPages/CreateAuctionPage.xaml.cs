using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Services;
using EntitiesLayer.Entities;
using PresentationLayer.Dialogs;
using PresentationLayer.Helpers;
using PresentationLayer.Navigation;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.Pages.UserPages {
    /// <summary>
    /// Interaction logic for CreateAuctionPage.xaml
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public partial class CreateAuctionPage : UserControl {

        private NavigationControl navigationControl;
        private AuctionService auctionService;
        private Category auctionCategory;

        private List<Category> categories;
        private ComboBoxHelper<ProductState> cbhProductState;
        private ComboBoxHelper<Region> cbhRegions;
        private Auction auction;

        public CreateAuctionPage(NavigationControl navigationControl, Auction auction = null) {
            InitializeComponent();
            this.navigationControl = navigationControl;
            auctionService = new AuctionService();

            cbhProductState = new ComboBoxHelper<ProductState>(cbProductState, (e) => e.Name);
            cbhRegions = new ComboBoxHelper<Region>(cbRegion, (e) => e.Name);

            this.auction = auction;
        }

        private void Init() {

            auctionImagesView.AllowEdit = true;

            var categoryService = new CategoryService();
            var regionSerivce = new RegionService();
            var productStateService = new ProductStateService();

            categories = categoryService.GetAllCategories();
            cbhProductState.DataSource = productStateService.GetAllStates();
            cbhRegions.DataSource = regionSerivce.GetAllRegions();

            if(auction != null) {
                btnCreateOrUpdate.Content = "Uredi aukciju";
                FillAuctionData();
            } else {
                btnCreateOrUpdate.Content = "Kreiraj aukciju";
            }

        }

        private void FillAuctionData() {

            // set basic info

            txtName.Text = auction.Name;
            txtDescription.Text = auction.Description;

            dpEndDate.SelectedDate = auction.EndDate;
            txtEndTime.Text = auction.EndDate.ToString("HH:mm");

            txtMinimalBidPrice.Text = auction.MinimalBidPrice.ToString();
            if(auction.InstantBuyPrice.HasValue) {
                cbEnableInstantBuy.IsChecked = true;
                txtInstantBuyPrice.Text = auction.InstantBuyPrice.ToString();
            }

            // select product state, region and category

            cbhProductState.SelectItem((e) => e.ProductStateId == auction.ProductStateId);
            cbhRegions.SelectItem((e) => e.RegionId == auction.RegionId);
            
            auctionCategory = auction.Category;
            UpdateAuctionCategory();

            // load auction images

            AuctionImageService auctionImageService = new AuctionImageService();
            BitmapImageHelper bitmapImageHelper = new BitmapImageHelper();

            var auctionImages = auctionImageService.GetImagesForAuction(auction.AuctionId);

            var bitmapImages = bitmapImageHelper.ConvertToBitmapImages(auctionImages);
            auctionImagesView.Images = bitmapImages;

        }

        private void CreateOrUpdateAuction() {

            if(auction == null) {
                CreateAuction();
            } else {
                UpdateAuction();
            }

        }

        private void CreateAuction() {

            var newAuction = new Auction();

            try {

                CheckInput(newAuction);

                auctionService.CreateAuction(newAuction);
                AddAuctionImages(newAuction);
                
                navigationControl.PopPage();

            } catch (InvalidInputException e) {
                MessageBoxes.ShowWarning("Kreiranje aukcije", e.Message);
            } catch (Exception) {
                MessageBoxes.ShowError("Kreiranje aukcije", "Nepoznata greška");
            }

        }

        private void UpdateAuction() {

            try {

                CheckInput(auction);
                auctionService.CheckAuction(auction);

                if (auction.AuctionStateId == AuctionState.NotSold) {
                    auction.AuctionStateId = AuctionState.Active;
                }

                auctionService.UpdateAuction(auction);
                UpdateAuctionImages(auction);

                navigationControl.PopPage("edit");

            } catch (InvalidInputException e) {
                MessageBoxes.ShowWarning("Uređivanje aukcije", e.Message);
            } catch (Exception) {
                MessageBoxes.ShowError("Uređivanje aukcije", "Nepoznata greška");
            }

        }

        private void CheckInput(Auction auction) {

            string name = txtName.Text.Trim();
            string description = txtDescription.Text.Trim();

            if (name == "") {
                throw new InvalidInputException("Unesite naziv aukcije!");
            }

            DateTime? endDate = dpEndDate.SelectedDate;
            if (!endDate.HasValue) {
                throw new InvalidInputException("Postavite datum završetka aukcije!");
            }

            string endTimeText = txtEndTime.Text.Trim();
            if (endTimeText == "") {
                throw new InvalidInputException("Postavi vrijeme završetka aukcije!");
            }

            (int hour, int minute) = DateTimeHelper.ParseTime(endTimeText);

            DateTime endDateTime = new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day, hour, minute, 0);

            string minimalBidPriceText = txtMinimalBidPrice.Text.Trim();
            if (minimalBidPriceText == "") {
                throw new InvalidInputException("Postavite cijenu početne ponude!");
            }

            if (!decimal.TryParse(minimalBidPriceText, out decimal minPrice)) {
                throw new InvalidInputException("Cijena početne ponude nije broj!");
            }

            auction.Name = name;
            auction.Description = description;

            auction.EndDate = endDateTime;

            auction.MinimalBidPrice = minPrice;

            if (cbEnableInstantBuy.IsChecked ?? false) {

                string instantBuyPriceText = txtInstantBuyPrice.Text.Trim();
                if (instantBuyPriceText == "") {
                    throw new InvalidInputException("Postavite cijenu instant kupnje!");
                }

                if (!decimal.TryParse(instantBuyPriceText, out decimal instantBuyPrice)) {
                    throw new InvalidInputException("Cijena instant kupnje nije broj!");
                }

                auction.InstantBuyPrice = instantBuyPrice;
            }

            auction.CreatorId = AuthenticationService.LoggedUser.UserId;

            ProductState productState = cbhProductState.CurrentItem() ?? throw new InvalidInputException("Odaberite stanje proizvoda!");
            auction.ProductStateId = productState.ProductStateId;

            Category category = auctionCategory ?? throw new InvalidInputException("Odaberite kategoriju proizvoda!");
            auction.CategoryId = category.CategoryId;

            Region region = cbhRegions.CurrentItem() ?? throw new InvalidInputException("Odaberite regiju!");
            auction.RegionId = region.RegionId;

            int imageCount = auctionImagesView.Images.Count;
            if (imageCount > 10) {
                throw new InvalidInputException("Maksimalno možete staviti 10 slika za proizvod!");
            }

        }

        private void AddAuctionImages(Auction auction) {

            try {

                var auctionImageService = new AuctionImageService();
                var bitmapImageHelper = new BitmapImageHelper();

                var images = auctionImagesView.Images;

                if (images.Count == 0) {
                    return;
                }

                var auctionImages = bitmapImageHelper.ConvertToAuctionImages(images, auction.AuctionId);
                auctionImageService.AddImages(auctionImages);

            }catch(Exception) {
                MessageBoxes.ShowError("Slike aukcije", "Postavljanje slika nije uspjelo!");
            }

        }

        private void UpdateAuctionImages(Auction auction) {

            try {

                var auctionImageService = new AuctionImageService();
                var bitmapImageHelper = new BitmapImageHelper();

                auctionImageService.DeleteAllImagesForAuction(auction.AuctionId);

                var images = auctionImagesView.Images;

                if (images.Count == 0) {
                    return;
                }

                var auctionImages = bitmapImageHelper.ConvertToAuctionImages(images, auction.AuctionId);
                auctionImageService.AddImages(auctionImages);

            } catch (Exception) {
                MessageBoxes.ShowError("Slike aukcije", "Ažuriranje slika nije uspjelo!");
            }

        }

        private void UpdateAuctionCategory() {

            if (auctionCategory != null) {
                btnCategory.Content = auctionCategory.Name;
            }

        }

        private void SelectCateogry() {

            var dialog = new CategoryPickerDialog("Odaberite kategoriju", categories);
            dialog.ShowDialog();

            auctionCategory = dialog.SelectedCategory;
            UpdateAuctionCategory();

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            Init();
        }

        private void btnCategory_Click(object sender, RoutedEventArgs e) {
            SelectCateogry();
        }

        private void cbEnableInstantBuy_Checked(object sender, RoutedEventArgs e) {
            txtInstantBuyPrice.IsEnabled = true;
        }

        private void cbEnableInstantBuy_Unchecked(object sender, RoutedEventArgs e) {
            txtInstantBuyPrice.IsEnabled = false;
        }

        private void btnCreateOrUpdate_Click(object sender, RoutedEventArgs e) {
            CreateOrUpdateAuction();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) {
            navigationControl.PopPage();
        }

    }
}
