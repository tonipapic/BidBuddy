using EntitiesLayer.Entities;
using System.Collections.Generic;
using System.Windows.Controls;

namespace PresentationLayer.Helpers {

    /// <summary>
    /// Used to populate TreeView with list of categories.
    /// Each category has sub categories.
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public class CategoriesTreeViewHelper {

        private TreeView treeView;

        public CategoriesTreeViewHelper(TreeView treeView) {
            this.treeView = treeView;
        }

        public List<Category> DataSource {
            set {
                if (value == null) ClearData();
                else SetData(value);
            }
        }

        public void SetData(List<Category> categories) {

            treeView.Items.Clear();

            var categoryQueue = new Queue<Category>();
            var treeItemQueue = new Queue<TreeViewItem>();

            foreach (var category in categories) {
                var item = CreateTreeItem(category);
                treeView.Items.Add(item);

                categoryQueue.Enqueue(category);
                treeItemQueue.Enqueue(item);
            }

            while(categoryQueue.Count > 0) {

                var category = categoryQueue.Dequeue();
                var item = treeItemQueue.Dequeue();

                foreach (var subCategory in category.SubCategories) {
                    var subItem = CreateTreeItem(subCategory);
                    item.Items.Add(subItem);

                    categoryQueue.Enqueue(subCategory);
                    treeItemQueue.Enqueue(subItem);
                }

            }

        }

        public void ClearData() {
            treeView.Items.Clear();
        }

        /// <summary>
        /// Retrieves selected category or null.
        /// </summary>
        /// <returns></returns>
        public Category CurrentItem() {
            var item = treeView.SelectedItem as TreeViewItem;
            if(item == null) return null;

            return item.Tag as Category;
        }

        /// <summary>
        /// Unselects current category.
        /// </summary>
        public void Unselect() {
            var selected = treeView.SelectedItem as TreeViewItem;
            if (selected != null) {
                selected.IsSelected = false;
            }
        }

        private TreeViewItem CreateTreeItem(Category category) {
            var item = new TreeViewItem();
            item.Header = category.Name;
            item.Tag = category;
            return item;
        }

    }
}
