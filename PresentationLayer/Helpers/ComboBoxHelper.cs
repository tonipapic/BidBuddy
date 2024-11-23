using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.Helpers {

    /// <summary>
    /// Makes it easier to work with ComboBox.
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ComboBoxHelper<T> where T : class {

        private ComboBox comboBox;
        private DisplayProperty<T> displayPropertyCallback;

        public ComboBoxHelper(ComboBox comboBox, DisplayProperty<T> displayPropertyCallback) {
            this.comboBox = comboBox;
            this.displayPropertyCallback = displayPropertyCallback;
        }

        public List<T> DataSource {
            set {
                if (value == null) ClearData();
                else SetData(value);
            }
        }

        public void SetData(List<T> source) {

            var items = source.Select((e) => {
                var item = new ComboBoxItem();
                item.Tag = e;
                item.Content = displayPropertyCallback(e);
                item.HorizontalContentAlignment = HorizontalAlignment.Left;
                item.VerticalContentAlignment = VerticalAlignment.Center;

                return item;
            });

            comboBox.ItemsSource = items;

        }

        public void ClearData() {
            comboBox.ItemsSource = null;
        }

        /// <summary>
        /// Selects item that meets the predicate.
        /// </summary>
        /// <param name="predicate"></param>
        public void SelectItem(Predicate<T> predicate) {
            foreach(ComboBoxItem item in comboBox.Items) {
                T obj = item.Tag as T;
                if(predicate(obj)) {
                    comboBox.SelectedItem = item;
                    return;
                }
            }
        }

        /// <summary>
        /// Deselects current item.
        /// </summary>
        public void DeselectCurrentItem()
        {
            comboBox.SelectedIndex = -1;
        }

        /// <summary>
        /// Retrieves selected item or null.
        /// </summary>
        /// <returns></returns>
        public T CurrentItem() {
            ComboBoxItem item = comboBox.SelectedItem as ComboBoxItem;
            if (item == null) return null;

            return item.Tag as T;
        }

    }
}
