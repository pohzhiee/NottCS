﻿using NottCS.Models;
using NottCS.Services.REST;
using NottCS.Validations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using NottCS.Services.Navigation;
using Xamarin.Forms;

namespace NottCS.ViewModels
{
    internal class AccountViewModel : BaseViewModel
    {
        #region ViewModalAdditionalClass

        public class UserDataObject
        {
            public string DataName { get; set; }
            public string DataValue { get; set; }
        }

        #endregion

        #region PageProperties
        private User _loginUser;
        private List<UserDataObject> _dataList;

        public User LoginUser
        {
            get => _loginUser;
            set => SetProperty(ref _loginUser, value);
        }

        public List<UserDataObject> DataList
        {
            get => _dataList;
            set => SetProperty(ref _dataList, value);
        }

        #endregion

        /// <summary>
        /// Constructor of AccountViewModel
        /// </summary>
        public AccountViewModel()
        {
            Title = "Account Page";
        }

        /// <summary>
        /// Sets the data for the page
        /// </summary>
        /// <param name="username">Username for the account data</param>
        private async Task<bool> SetPageDataAsync(string username)
        {
            var respondData = await BaseRestService.RequestGetAsync<User>(username);

            if (!respondData.Item1)
            {
                //TODO: Implement error notification
                UserDialogs.Instance.Alert("We're not able to log you in. Please try again.", "Login Error", "Ok");
                return false;
            }
            else
            {
                LoginUser = respondData.Item2;
                DataList = new List<UserDataObject>()
                {
                    new UserDataObject(){DataName = "Name", DataValue = LoginUser.Name},
                    new UserDataObject(){DataName = "Email", DataValue = LoginUser.Email},
                    new UserDataObject(){DataName = "Student ID", DataValue = LoginUser.StudentId},
                    new UserDataObject(){DataName = "Library Number", DataValue = LoginUser.LibraryNumber},
                    new UserDataObject(){DataName = "Date Joined", DataValue = LoginUser.DateJoined.ToLongDateString()}
                };
                return true;
            }
        }

        /// <summary>
        /// Initializes the page
        /// </summary>
        /// <param name="navigationData">Data passed from the previous page</param>
        /// <returns></returns>
        public override Task InitializeAsync(object navigationData)
        {

            try
            {
                var username = navigationData as string;
                var isSuccess = SetPageDataAsync(username).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return base.InitializeAsync(navigationData);

            //Debug.WriteLine("Initializing Account Page...");
            //if (navigationData is string username)
            //{
            //    Debug.WriteLine("Stage 2...");
            //    var isSuccess = SetPageDataAsync(username).GetAwaiter().GetResult();
            //    if (isSuccess)
            //    {
            //        Debug.WriteLine("Stage 3...");
            //        return base.InitializeAsync(navigationData);
            //    }
            //}
        }
    }
}
