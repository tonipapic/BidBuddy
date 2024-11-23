using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.Helpers {

    public delegate object DisplayProperty<T>(T t);

    internal class DataGridColumn<T> {
        internal string Title { get; set; }
        internal Type Type { get; set; }
        internal DisplayProperty<T> DisplayPropertyCallback { get; set; }
    }

    /// <summary>
    /// Makes it easier to work with DataGrid.
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataGridHelper<T> where T : class {

        private DataGrid dataGrid;
        private List<DataGridColumn<T>> columns;

        public DataGridHelper(DataGrid dataGrid) {
            this.dataGrid = dataGrid;
            columns = new List<DataGridColumn<T>>();
        }

        /// <summary>
        /// Adds column to DataGrid.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="callback"></param>
        public void AddColumn(string title, DisplayProperty<T> callback) {
            AddColumn(title, null, callback);
        }

        /// <summary>
        /// Adds column to DataGrid with custom type.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="type"></param>
        /// <param name="callback"></param>
        public void AddColumn(string title, Type type, DisplayProperty<T> callback) {
            columns.Add(new DataGridColumn<T>() {
                Title = title,
                Type = type,
                DisplayPropertyCallback = callback,
            });
        }

        public List<T> DataSource {
            set {
                if (value == null) ClearData();
                else SetData(value);
            }
        }

        public void SetData(List<T> source) {

            DataTable dataTable = new DataTable();

            // first column is used to retrieve selected object
            dataTable.Columns.Add("ObjectRef", typeof(T));

            foreach(var column in columns) {
                var added = dataTable.Columns.Add(column.Title);
                if(column.Type != null) {
                    added.DataType = column.Type;
                }
            }

            foreach (T obj in source) {

                object[] values = new object[columns.Count + 1];
                values[0] = obj;
                
                for (int i = 0; i < columns.Count; i++) {
                    values[i + 1] = columns[i].DisplayPropertyCallback(obj);
                }

                dataTable.Rows.Add(values);

            }

            dataGrid.ItemsSource = dataTable.DefaultView;

            // hide object ref column
            dataGrid.Columns[0].Visibility = Visibility.Hidden;
        }

        public void ClearData() {
            dataGrid.ItemsSource = null;
        }

        /// <summary>
        /// Retrieves selected item or null.
        /// </summary>
        /// <returns></returns>
        public T CurrentItem() {
            DataRowView item = dataGrid.SelectedItem as DataRowView;
            if (item == null) return null;

            return item.Row.ItemArray[0] as T;
        }

    }
}
