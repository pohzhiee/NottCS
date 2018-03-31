﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using NottCS.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using NottCS.Services;
using NottCS.Services.Navigation;
using NottCS.Views;

namespace NottCS.ViewModels
{
    class EventRegistrationFormsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> TemporaryForms { get; set; } = new ObservableCollection<Item>()
        {
            new Item()
            {
                FilledInformation ="Name"
            },
            new Item()
            {
                FilledInformation="Student ID"
            },
            new Item()
            {
                FilledInformation="OWA"
            },
            new Item()
            {
                FilledInformation="Stuff"
            }
        };

        #region EventRegistrationNavigation
        public ICommand SignUpCommand => new Command(async () => await Navigate());
        private async Task Navigate()
        {
            try
            {
                DoNothingForNow();
                await NavigationService.NavigateToAsync<EventRegistrationSuccessViewModel>();
                DebugService.WriteLine("Button pressed");
            }
            catch (Exception e)
            {
                DebugService.WriteLine(e.Message);
            }
        }
        #endregion
        public void DoNothingForNow()
        {

            foreach (var item in TemporaryForms)
            {
                DebugService.WriteLine(item.Information);
            }
        }
        //public override Task InitializeAsync(object navigationData)
        //{
        //    if (navigationData is Item s)
        //    {
        //        DebugService.WriteLine("Finished Navigation");
        //    }
        //    return base.InitializeAsync(navigationData);
        //}
        #region Disable ItemSelectedCommand
        public ICommand DisableItemSelectedCommand => new Command(DisableItemSelected);
        public void DisableItemSelected()
        {
        }
        #endregion
        #region EventRegistrationFormsViewModel Constructor

        public EventRegistrationFormsViewModel()
        {

        }
        #endregion
    }
}