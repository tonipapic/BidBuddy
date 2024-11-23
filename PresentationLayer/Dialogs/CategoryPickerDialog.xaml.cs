using EntitiesLayer.Entities;
using PresentationLayer.Helpers;
using System.Collections.Generic;
using System.Windows;

namespace PresentationLayer.Dialogs {
    /// <summary>
    /// Interaction logic for CategoryPickerDialog.xaml
    /// </summary>
    public partial class CategoryPickerDialog : Window {

        private CategoriesTreeViewHelper tvhCategories;
        private List<Category> categories;

        public Category SelectedCategory { get; set; }

        public CategoryPickerDialog(string title, List<Category> categories) {
            InitializeComponent();
            Title = title;
            this.categories = categories;
   
            tvhCategories = new CategoriesTreeViewHelper(tvCategories);
        }

        private void LoadCategories() {
            tvhCategories.DataSource = categories;
        }

        private void PickCategory() {
            SelectedCategory = tvhCategories.CurrentItem();
        }

        private void UpdateSelection() {

            Category selectedCategory = tvhCategories.CurrentItem();

            if(selectedCategory == null) {
                btnPick.IsEnabled = false;
                return;
            }

            btnPick.IsEnabled = true;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            LoadCategories();
            UpdateSelection();
        }

        private void btnPick_Click(object sender, RoutedEventArgs e) {
            PickCategory();
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void tvCategories_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            UpdateSelection();
        }
    }
}
