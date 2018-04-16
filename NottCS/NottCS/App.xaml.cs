﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Microsoft.Identity.Client;
using NottCS.Services.Navigation;
using NottCS.Views;
using Xamarin.Forms;
using NottCS.Models;
using NottCS.Services;

namespace NottCS
{
    //TODO:Code behind for dynamic elements (like event additional parameter, event dates)
    //TODO:Pull to refresh does not reload UI (Club only)
    //TODO:Account details disable item selected
    //TODO:Change admin panel icon
    //TODO:Change EventAdditionalParameter to Event only (1 big model), redo CreateEventPage
    //TODO:Add ability to edit profile
    //TODO:Fix report issue UI
    //TODO:Disable notification
    //TODO:Iphone camera for QR code unstable
    //TODO: implement my club(wait wen kang)
    //TODO: implement event for club(wait wen kang)
    //TODO: refactor REST to probably use RestSharp
    //TODO: refactor club page for readability
    public partial class App : Application
	{
	    public static PublicClientApplication ClientApplication { get; private set; }
	    public static readonly string[] Scopes = { "User.Read" };
	    public static UIParent UiParent = null;
        public static AuthenticationResult MicrosoftAuthenticationResult = null;
        public App ()
		{
            DebugService.WriteLine("App is starting up...");
			InitializeComponent();
            InitializeDialogService();
		    ClientApplication =
		        new PublicClientApplication("81a5b712-2ec4-4d3f-9324-211f60d0a0c9")
		        {
		            RedirectUri = "msal81a5b712-2ec4-4d3f-9324-211f60d0a0c9://auth"
		        };
//		    MainPage = new NavigationPage(new MediaTestPage());
		    MainPage = new ContentPage();
		}

	    private static Task InitNavigation()
	    {
            DebugService.WriteLine("Trying to start startup service");
	        return StartupService.InitializeAsync();
	    }

        protected override async void OnStart ()
		{
            // Handle when your app starts
//		    Stopwatch stopwatch = new Stopwatch();
//		    stopwatch.Start();
            await InitNavigation();
//		    Debug.WriteLine($"Init navigation took {stopwatch.ElapsedMilliseconds}ms");
//            stopwatch.Stop();

        }

        protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

	    private void InitializeDialogService()
	    {
            
	    }
	}
}
