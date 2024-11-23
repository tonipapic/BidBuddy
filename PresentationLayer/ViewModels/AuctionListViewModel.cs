using EntitiesLayer.Entities;
using PresentationLayer.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PresentationLayer.ViewModels
{
    public class AuctionListViewModel : ViewModelBase
    {
        private NavigationControl navigationControl;
        private ObservableCollection<AuctionViewModel> recommendedAuctions;

        private ObservableCollection<AuctionViewModel> auctionViewModels;

        public ObservableCollection<AuctionViewModel> AuctionViewModels
        {
            get => auctionViewModels;
            set => auctionViewModels = value;
        }
        public ObservableCollection<AuctionViewModel> RecommendedModels { get => recommendedAuctions; set => recommendedAuctions = value; }

        public AuctionListViewModel()
        {
            ObservableCollection<AuctionViewModel> newViewModels =
                new ObservableCollection<AuctionViewModel>();
            AuctionViewModels = newViewModels;
        }
        public AuctionListViewModel(NavigationControl navigationControl, List<Auction> auctions, List<Auction> recommended)
        {
            this.navigationControl = navigationControl;
            ObservableCollection<AuctionViewModel> newViewModels =
                new ObservableCollection<AuctionViewModel>();
            ObservableCollection<AuctionViewModel> newRecommendedModels =
                new ObservableCollection<AuctionViewModel>();

            foreach (var item in auctions)
            {
                newViewModels.Add(new AuctionViewModel(navigationControl, item));
            }
            foreach (var item in recommended)
            {
                newRecommendedModels.Add(new AuctionViewModel(navigationControl, item));
            }
            AuctionViewModels = newViewModels;
            RecommendedModels = newRecommendedModels;
        }

        public void AddRecommendations(List<Auction> auctions)
        {
            ObservableCollection<AuctionViewModel> newRecommendedModels =
                new ObservableCollection<AuctionViewModel>();
            foreach (var item in auctions)
            {
                newRecommendedModels.Add(new AuctionViewModel(navigationControl, item));
            }
            RecommendedModels = newRecommendedModels;
        }
    }
}
