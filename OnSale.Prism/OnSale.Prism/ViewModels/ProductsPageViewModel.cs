using Onsale.Common.Entities;
using Onsale.Common.Responses;
using Onsale.Common.Services;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace OnSale.Prism.ViewModels
{
    public class ProductsPageViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        private ObservableCollection<Product> _products;

        public ProductsPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _apiService = apiService;
            Title = "Products";
            LoadProductAsync();

        }

        public ObservableCollection<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        private async void LoadProductAsync()
        {
            //Internet Connection try
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Check the Internet Connection.", "Accept");
                return;
            }

            string urlApi = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListAsync<Product>(urlApi, "/api", "/Products");
            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            List<Product> listProducts = (List<Product>)response.Result;
            Products = new ObservableCollection<Product>(listProducts);


        }

    }
}
