using Onsale.Common.Entities;
using Onsale.Common.Responses;
using Onsale.Common.Services;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace OnSale.Prism.ViewModels
{
    public class ProductsPageViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        private bool _isRunning;
        private ObservableCollection<Product> _products;
        private string _search;
        private List<Product> _listProducts;
        private DelegateCommand _searchCommand; 

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


        public bool IsRunning
        {
            get { return _isRunning; }
            set { SetProperty(ref _isRunning , value); }
        }

        public string Search
        {
            get => _search;
            set 
            { 
                SetProperty(ref _search, value);
                ShowProducts();
            }
        }

        public DelegateCommand SearchCommand => _searchCommand ?? (_searchCommand = new DelegateCommand(ShowProducts));

        private void ShowProducts()
        {
            if (string.IsNullOrEmpty(Search))
            {
                Products = new ObservableCollection<Product>(_listProducts);
            }
            else
            {
                Products = new ObservableCollection<Product>(_listProducts.Where(p => p.Name.ToLower().Contains(Search.ToLower())));
            }
        }

        private async void LoadProductAsync()
        {
            //Internet Connection try
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Check the Internet Connection.", "Accept");
                return;
            }

            IsRunning = true;

            string urlApi = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListAsync<Product>(urlApi, "/api", "/Products");
            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

           _listProducts = (List<Product>)response.Result;
            ShowProducts();

            IsRunning = false;

        }

    }
}
