﻿using RKeeperWaiter.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using WaiterMobile.Views;
using Xamarin.Forms;

namespace WaiterMobile.ViewModels
{
    public class GuestOrdersViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand GoToBack { get; private set; }
        public ICommand GuestCountChanged { get; private set; }
        public ICommand CreateOrder { get; private set; }

        public Order Order { get; private set; }
        public OrderType SelectedType { get; set; }
        public IEnumerable<OrderType> Types => App.Waiter.OrderTypes;
        public string PageLabel { get; set; }
        public string GuestCount { get; set; }

        public GuestOrdersViewModel(Order newOrder) 
        {
            Order = newOrder;

            GoToBack = new Command(OnGoToBack);
            GuestCountChanged = new Command(OnGuestCountChanged);
            CreateOrder = new Command(OnCreateOrder);
        }

        private void OnGoToBack()
        {
            Shell.Current.Navigation.PopAsync(true);
        }

        private void OnGuestCountChanged()
        {
            int guestCount;

            if (int.TryParse(GuestCount, out guestCount) == true && guestCount <= 100)
            {
                return;
            }

            if (GuestCount.Length == 0)
            {
                return;
            }

            GuestCount = GuestCount.Remove(GuestCount.Length - 1);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GuestCount)));
        }

        private void OnCreateOrder()
        {
            int guestCount = Convert.ToInt32(GuestCount);

            Order.Type = SelectedType;

            App.Waiter.CreateNewOrder(Order, guestCount);

            PageRemover.Remove(3);
            Shell.Current.Navigation.PushAsync(new Orders(), false);
        }
    }
}
