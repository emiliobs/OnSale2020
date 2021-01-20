﻿using Onsale.Common.Entities;
using Prism.Navigation;

namespace OnSale.Prism.ViewModels
{
    public class ProductDetailPageViewModel : ViewModelBase
    {
        private Product _product;

        public ProductDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Product Details";
        }

        public Product Product 
        {
            get => _product; 
            set => SetProperty(ref _product, value); }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            //hew recived the parameter of the page previos:
            if (parameters.ContainsKey("product"))
            {
                Product = parameters.GetValue<Product>("product");
                Title = Product.Name;

            }
            base.OnNavigatedTo(parameters);
        }
    }
}