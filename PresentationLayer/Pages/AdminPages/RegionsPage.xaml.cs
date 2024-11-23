using BusinessLogicLayer.Services;
using EntitiesLayer.Entities;
using PresentationLayer.Helpers;
using PresentationLayer.Navigation;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.Pages {
    /// <summary>
    /// Interaction logic for RegionsPage.xaml
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public partial class RegionsPage : UserControl {

        private NavigationControl navigationControl;
        private RegionService regionService;

        private DataGridHelper<Region> dghRegions;
        private Region selectedRegion;

        public RegionsPage(NavigationControl navigationControl) {
            InitializeComponent();
            this.navigationControl = navigationControl;
            regionService = new RegionService();

            dghRegions = new DataGridHelper<Region>(dgRegions);
            dghRegions.AddColumn("ID", (e) => e.RegionId);
            dghRegions.AddColumn("Naziv", (e) => e.Name);

        }

        private void Refresh() {
            try {
                List<Region> regions = regionService.GetAllRegions();
                dghRegions.DataSource = regions;
            } catch(Exception e) {
                MessageBoxes.ShowError("Regije", "Dohvaćanje regija nije uspjelo! " + e.Message);
            }
        }

        private void UpdateSelection() {

            var region = selectedRegion;

            if(region == null) {

                gbEditRegion.Header = "Dodaj regiju";
                txtRegionName.Text = "";

                btnAddRegion.Visibility = Visibility.Visible;
                btnSave.Visibility = Visibility.Collapsed;
                btnCancel.Visibility = Visibility.Collapsed;
                btnDelete.Visibility = Visibility.Collapsed;

            } else {

                gbEditRegion.Header = "Uredi regiju";
                txtRegionName.Text = region.Name;

                btnAddRegion.Visibility = Visibility.Collapsed;
                btnSave.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Visible;
                btnDelete.Visibility = Visibility.Visible;

            }

        }

        private void AddRegion() {

            string regionName = txtRegionName.Text.Trim();
            if (regionName == "") {
                MessageBoxes.ShowWarning("Dodavanje regije", "Unesite naziv regije!");
                return;
            }

            var region = new Region();
            region.Name = regionName;

            try {
                regionService.AddRegion(region);
                Refresh();

                txtRegionName.Text = "";
            } catch(Exception e) {
                MessageBoxes.ShowError("Dodavanje regije", "Dodavanje regije nije uspjelo! " + e.Message);
            }

        }

        private void UpdateRegion() {

            string regionName = txtRegionName.Text.Trim();
            if (regionName == "") {
                MessageBoxes.ShowWarning("Uređivanje regije", "Unesite naziv regije!");   
                return;
            }

            selectedRegion.Name = regionName;

            try {
                regionService.UpdateRegion(selectedRegion);
                Refresh();
            } catch (Exception e) {
                MessageBoxes.ShowError("Uređivanje regije", "Uređivanje regije nije uspjelo! " + e.Message);
            }

        }

        private void DeleteRegion() {

            MessageBoxResult result = MessageBoxes.Confirm(
                "Brisanje regije",
                "Pri brisanju regije, svim aukcijama koje koriste tu regiju, regija će se promijeniti u nepoznatu regiju. " +
                "Ova radnja se ne može poništiti. " +
                "Želite li nastaviti?");

            if(result == MessageBoxResult.OK) {
                try {
                    regionService.DeleteRegion(selectedRegion);
                    Refresh();
                } catch (Exception e) {
                    MessageBoxes.ShowError("Brisanje regije", "Brisanje regije nije uspjelo! " + e.Message);
                }
            }

        }

        private void CancelEdit() {
            dgRegions.UnselectAll();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            Refresh();
            UpdateSelection();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e) {
            
        }

        private void dgRegions_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            selectedRegion = dghRegions.CurrentItem();
            UpdateSelection();
        }

        private void btnAddRegion_Click(object sender, RoutedEventArgs e) {
            AddRegion();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e) {
            UpdateRegion();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) {
            CancelEdit();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e) {
            DeleteRegion();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e) {
            Refresh();
        }
    }
}
