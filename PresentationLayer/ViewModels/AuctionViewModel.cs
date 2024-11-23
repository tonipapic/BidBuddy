using EntitiesLayer.Entities;
using PresentationLayer.Navigation;

namespace PresentationLayer.ViewModels
{
    public class AuctionViewModel : ViewModelBase
    {
        public Auction Auction { get; set; }
        public NavigationControl NavigationControl { get; set; }

        public AuctionViewModel(NavigationControl navigationControl,Auction auction)
        {
            NavigationControl = navigationControl;
            Auction = auction;
        }
    }
}
