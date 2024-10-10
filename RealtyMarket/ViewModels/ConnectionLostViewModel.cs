using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers.Commands;
using RealtyMarket.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RealtyMarket.ViewModels
{
    public class ConnectionLostViewModel : ObservableObject
    {
        private bool _isLoading = false;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand ReturnToPageAsyncCommand { get; }

        private string _returnPage;

        public string ReturnPage
        {
            get => _returnPage;
            set => SetProperty(ref _returnPage, value);
        }

        private readonly ConnectivityService _connectivityService;

        public ConnectionLostViewModel(ConnectivityService connectivityService, 
            string returnPage)
        {
            ReturnPage = returnPage;
            _connectivityService = connectivityService;
            ReturnToPageAsyncCommand = new AsyncCommand(ReturnToPageAsync);
        }

        public async Task ReturnToPageAsync()
        {
            IsLoading = true;

            if (_connectivityService.IsConnectedToInternet())
            {
                await Task.Run(async () =>
                    await Shell.Current.GoToAsync($"{_returnPage}"));
            }

            Application.Current.Dispatcher.Dispatch(() =>
            {
                IsLoading = false;
            });
        }
    }
}
