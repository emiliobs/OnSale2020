﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace OnSale.Prism.ViewModels
{
    public class OnSaleMasterDetailPageViewModel : ViewModelBase
    {
        public OnSaleMasterDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {

        }
    }
}
