using BusinessLogicLayer.Services;
using EntitiesLayer.Entities;
using PresentationLayer.Helpers;
using PresentationLayer.Navigation;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.Pages {
    /// <summary>
    /// Interaction logic for CategoriesPage.xaml
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public partial class CategoriesPage : UserControl {

        private NavigationControl navigationControl;
        private CategoryService categoryService;

        private CategoriesTreeViewHelper tvhCategories;
        private Category selectedCategory;

        public CategoriesPage(NavigationControl navigationControl) {
            InitializeComponent();
            this.navigationControl = navigationControl;
            categoryService = new CategoryService();
            tvhCategories = new CategoriesTreeViewHelper(tvCategories);
        }

        private void Refresh() {
            try {
                var categories = categoryService.GetAllCategories();
                tvhCategories.DataSource = categories;
            }catch(Exception e) {
                MessageBoxes.ShowError("Dohvaćanje kategorija", "Dohvaćanje kategorija nije uspjelo! " + e.Message);
            }
        }

        private void UpdateSelection() {

            var category = selectedCategory;

            if(category == null) {

                gbAddCategory.Header = "Dodaj kategoriju";
                gbEditCategory.Visibility = Visibility.Collapsed;

                txtAddCategoryName.Text = "";
                txtEditCategoryName.Text = "";

                btnAddCategory.Visibility = Visibility.Visible;
                btnAddSubCategory.Visibility = Visibility.Collapsed;

            } else {

                gbAddCategory.Header = "Dodaj podkategoriju";
                gbEditCategory.Visibility = Visibility.Visible;

                txtAddCategoryName.Text = "";
                txtEditCategoryName.Text = selectedCategory.Name;

                btnAddCategory.Visibility = Visibility.Collapsed;
                btnAddSubCategory.Visibility = Visibility.Visible;

            }

        }

        private void AddCategory() {

            string categoryName = txtAddCategoryName.Text.Trim();
            if (categoryName == "") return;

            var category = new Category();
            category.Name = categoryName;
            category.ParentId = null;

            try {
                categoryService.AddCategory(category);
                Refresh();

                txtAddCategoryName.Text = "";
            }catch(Exception e) {
                MessageBoxes.ShowError("Dodavanje kategorije", "Dodavanje kategorije nije uspjelo! " + e.Message);
            }

        }

        private void AddSubCategory() {

            string categoryName = txtAddCategoryName.Text.Trim();
            if (categoryName == "") return;

            var parentCategory = selectedCategory;

            var subCategory = new Category();
            subCategory.Name = categoryName;
            subCategory.ParentId = parentCategory.CategoryId;

            try {
                categoryService.AddCategory(subCategory);
                Refresh();

                txtAddCategoryName.Text = "";
            }catch(Exception e) {
                MessageBoxes.ShowError("Dodavanje podkategorije", "Dodavanje podkategorije nije uspjelo! " + e.Message);
            }

        }

        private void UpdateCategory() {

            string categoryName = txtEditCategoryName.Text.Trim();
            if (categoryName == "") return;

            selectedCategory.Name = categoryName;

            try {
                categoryService.UpdateCategory(selectedCategory);
                Refresh();
            }catch(Exception e) {
                MessageBoxes.ShowError("Uređivanje kategorije", "Uređivanje kategorije nije uspjelo! " + e.Message);
            }

        }

        private void DeleteCategory() {

            MessageBoxResult result = MessageBoxes.Confirm(
                "Brisanje kategorije",
                "Pri brisanju kategorije, svim aukcijama koje koriste tu kategoriju, kategorija će se promijeniti u nepoznatu kategoriju. " +
                "Ova radnja se ne može poništiti. " +
                "Želite li nastaviti?");

            if (result == MessageBoxResult.OK) {
                try {
                    categoryService.DeleteCategory(selectedCategory);
                    Refresh();
                } catch (Exception e) {
                    MessageBoxes.ShowError("Brisanje kategorije", "Brisanje kategorije nije uspjelo! " + e.Message);
                }
            }


        }

        private void CancelEdit() {
            tvhCategories.Unselect();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e) {
            Refresh();
            UpdateSelection();
        }

        private void tvCategories_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e) {
            selectedCategory = tvhCategories.CurrentItem();
            UpdateSelection();
        }

        private void btnAddCategory_Click(object sender, System.Windows.RoutedEventArgs e) {
            AddCategory();
        }

        private void btnSave_Click(object sender, System.Windows.RoutedEventArgs e) {
            UpdateCategory();
        }

        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e) {
            CancelEdit();
        }

        private void btnDelete_Click(object sender, System.Windows.RoutedEventArgs e) {
            DeleteCategory();
        }

        private void btnAddSubCategory_Click(object sender, System.Windows.RoutedEventArgs e) {
            AddSubCategory();
        }

        private void btnRefresh_Click(object sender, System.Windows.RoutedEventArgs e) {
            Refresh();
        }
    }
}
