using Onsale.Common.Entities;
using Onsale.Common.Helpers;
using Onsale.Common.Responses;
using Onsale.Common.Services;
using OnSale.Prism.ItemViewModel;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace OnSale.Prism.ViewModels
{
    public class ProductsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private bool _isRunning;
        private ObservableCollection<ProductItemViewModel> _products;
        private string _search;
        private List<Product> _listProducts;
        private DelegateCommand _searchCommand;

        public ProductsPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.Products;
            LoadProductAsync();

        }

        public ObservableCollection<ProductItemViewModel> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }


        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
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

        private async void LoadProductAsync()
        {


            //Internet Connection try
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            IsRunning = true;
            string urlApi = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListAsync<Product>(urlApi, "/api", "/Products");
            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            _listProducts = (List<Product>)response.Result;
            ShowProducts();

            IsRunning = false;


        }

        private void ShowProducts()
        {
            if (string.IsNullOrWhiteSpace(Search))
            {
                Products = new ObservableCollection<ProductItemViewModel>(_listProducts.Select(p => new ProductItemViewModel(_navigationService)
                {
                    Category = p.Category,
                    Description = p.Description,
                    Id = p.Id,
                    IsActive = p.IsActive,
                    IsStarred = p.IsStarred,
                    Name = p.Name,
                    Price = p.Price,
                    ProductImages = p.ProductImages,
                }).ToList());
            }
            else
            {
                Products = new ObservableCollection<ProductItemViewModel>(_listProducts.Select(p => new ProductItemViewModel(_navigationService)
                {
                    Category = p.Category,
                    Description = p.Description,
                    Id = p.Id,
                    IsActive = p.IsActive,
                    IsStarred = p.IsStarred,
                    Name = p.Name,
                    Price = p.Price,
                    ProductImages = p.ProductImages,
                }).Where(p => p.Name.ToLower().Contains(Search.ToLower())).ToList());
            }
        }


    }
}
