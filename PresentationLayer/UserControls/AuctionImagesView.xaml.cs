using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PresentationLayer.UserControls {
    /// <summary>
    /// Interaction logic for AuctionImagesView.xaml
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public partial class AuctionImagesView : UserControl {

        private List<BitmapImage> images = new List<BitmapImage>();
        private int currentImage;

        private BitmapImage noImagesImage;
        private bool allowEdit = false;


        public AuctionImagesView() {
            InitializeComponent();
        }

        public List<BitmapImage> Images {
            get => images;
            set {
                images = value;
                if(images == null) {
                    images = new List<BitmapImage>();
                }
                Update();
            }
        }

        public bool AllowEdit {
            get => allowEdit;
            set { allowEdit = value; Update(); }
        }

        private void Update() {

            if(allowEdit) {
                btnDelete.Visibility = Visibility.Visible;
                btnAdd.Visibility = Visibility.Visible;
            } else {
                btnDelete.Visibility = Visibility.Collapsed;
                btnAdd.Visibility = Visibility.Collapsed;
            }

            if(images.Count == 0) {

                lblImage.Content = "Nema slika";

                btnPreviousImage.IsEnabled = false;
                btnNextImage.IsEnabled = false;

                btnDelete.IsEnabled = false;
                imgCurrentImage.Source = noImagesImage;

                return;
            }

            if(currentImage < 0) {
                currentImage = 0;
            } else if(currentImage >= images.Count) {
                currentImage = images.Count - 1;
            }

            lblImage.Content = $"Slika {currentImage + 1} / {images.Count}";

            btnPreviousImage.IsEnabled = true;
            btnNextImage.IsEnabled = true;

            btnDelete.IsEnabled = true;
            imgCurrentImage.Source = images[currentImage];

        }

        private void AddImages() {

            var fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Filter = "Slike|*.jpg;*.jpeg;*.png";

            fileDialog.ShowDialog();

            string[] files = fileDialog.FileNames;
            if (files == null) return;

            List<BitmapImage> images = files.ToList().Select((file) => {
                return new BitmapImage(new Uri(file));
            }).ToList();

            if(this.images == null) {
                this.images = new List<BitmapImage>();
            }

            this.images.AddRange(images);
            currentImage = this.images.Count - 1;
            Update();

        }

        private void RemoveImage() {
            images.RemoveAt(currentImage);
            Update();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            noImagesImage = new BitmapImage(new Uri(@"/Resources/Images/nema-slike.png", UriKind.Relative));
            Update();
        }

        private void btnPreviousImage_Click(object sender, RoutedEventArgs e) {
            currentImage--;
            Update();
        }

        private void btnNextImage_Click(object sender, RoutedEventArgs e) {
            currentImage++;
            Update();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e) {
            AddImages();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e) {
            RemoveImage();
        }
    }
}
