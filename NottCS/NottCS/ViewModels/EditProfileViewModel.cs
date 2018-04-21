﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NottCS.Helpers;
using NottCS.Models;
using NottCS.Services;
using NottCS.Services.Navigation;
using NottCS.Services.REST;
using NottCS.Validations;
using Xamarin.Forms;

namespace NottCS.ViewModels
{
    class EditProfileViewModel:BaseViewModel
    {
        private readonly List<string> _courseList = new List<string>();
        private ObservableCollection<string> _suggestions = new ObservableCollection<string>();

        #region PublicMethodsWithPrivateBackingFields
        private bool _isValidStudentID = true;
        private bool _isValidLibraryNumber = true;

        private bool _isValidCourse = true;
        private string _selectedYearOfStudy;

        #endregion
        private User CurrentUser
        {
            get => GlobalUserData.CurrentUser;
            set => SetProperty(ref GlobalUserData.CurrentUser, value);
        }

        public ValidatableObject<string> StudentID { get; set; } = new ValidatableObject<string>()
        {
            Validations =
            {
                new StringNotEmptyRule(),
                new StringNumericRule()
            }
        };
        public ValidatableObject<string> LibraryNumber { get; set; } = new ValidatableObject<string>()
        {
            Validations =
            {
                new StringNotEmptyRule(),
                new StringNumericRule()
            }
        };
        public ValidatableObject<string> Course { get; set; } = new ValidatableObject<string>()
        {
            Validations = { new StringNotEmptyRule(), new StringAlphaNumericRule() }
        };

        public ObservableCollection<string> Suggestions
        {
            get => _suggestions;
            set => SetProperty(ref _suggestions, value);
        }

        public ObservableCollection<string> YearOfStudy { get; set; } = new ObservableCollection<string>
        {
            "1",
            "2",
            "3",
            "4",
            "Other"
        };

        public string SelectedYearOfStudy
        {
            get => _selectedYearOfStudy;
            set => SetProperty(ref _selectedYearOfStudy, value);
        }

        public ICommand UpdateProfileCommand => new Command(async () => await UpdateProfile());
        public ICommand CourseItemSelectedCommand => new Command(CourseItemSelected);
        public ICommand CourseTextChangedCommand => new Command(CourseTextChanged);

        private bool Validate()
        {
            _isValidStudentID = StudentID.Validate();
            if (!_isValidStudentID)
            {
                Acr.UserDialogs.UserDialogs.Instance.Alert($"Student ID {StudentID.Errors.First()}");
            }
            _isValidLibraryNumber = LibraryNumber.Validate();
            if (!_isValidLibraryNumber)
            {
                Acr.UserDialogs.UserDialogs.Instance.Alert($"Library Number {LibraryNumber.Errors.First()}");
            }
            _isValidCourse = Course.Validate();
            if (!_isValidCourse)
            {
                Acr.UserDialogs.UserDialogs.Instance.Alert($"Course {Course.Errors.First()}");
            }
            bool result = _isValidStudentID && _isValidLibraryNumber && _isValidCourse;

            DebugService.WriteLine($"{_isValidStudentID} {_isValidLibraryNumber} {_isValidCourse}");
            return result;
        }

        private void AddCoursesToList()
        {
            _courseList.Add("Foundation in Engineering");
            _courseList.Add("Mechanical Engineering");
            _courseList.Add("Mechatronic Engineering");
            _courseList.Add("Electrical and Electronic Engineering");
            _courseList.Add("Chemical Engineering");
            _courseList.Add("Chemical Engineering with Environmental Engineering");
            _courseList.Add("Civil Engineering");
            _courseList.Add("Applied Mathematics");
            _courseList.Add("Computer Science");
            _courseList.Add("Computer Science with Artificial Intelligence");
            _courseList.Add("Computer Science and Management Studies");
            _courseList.Add("Software Engineering");
            _courseList.Add("Foundation in Computer Science");
        }

        private async Task UpdateProfile()
        {
            IsBusy = true;

            bool isValid = Validate();
            if (isValid)
            {
                CurrentUser.StudentId = StudentID.Value;
                CurrentUser.LibraryNumber = LibraryNumber.Value;
                CurrentUser.Course = Course.Value;
                CurrentUser.YearOfStudy = SelectedYearOfStudy;

                var requestUpdate = await RestService.RequestUpdateAsync(CurrentUser);
                if (requestUpdate == "OK")
                {
                    var userData = await RestService.RequestGetAsync<User>("");
                    DebugService.WriteLine($"Respond from server is : {userData.Item1}");
                    if (userData.Item1 == "OK")
                    {
                        await NavigationService.NavigateToAsync<HomeViewModel>();
                    }
                    else
                    {
                        Acr.UserDialogs.UserDialogs.Instance.Alert(requestUpdate, "Server Error", "OK");
                    }
                }
                else
                {
                    Acr.UserDialogs.UserDialogs.Instance.Alert(requestUpdate, "Server Error", "OK");
                }
            }
            IsBusy = false;
        }
        private void CourseItemSelected(object param)
        {
            if (!(param is string s)) return;
            DebugService.WriteLine($"{s} is selected");
            Course.Value = s;
        }

        private void CourseTextChanged(object courseEntryParameter)
        {
            if (!(courseEntryParameter is TextChangedEventArgs args)) return;
            var courseEntryString = args.NewTextValue;
            Suggestions.Clear();
            foreach (string course in _courseList)
            {
                if (course.ToUpper().Contains(courseEntryString.ToUpper()) && Suggestions.Count < 5 && course != courseEntryString)
                {
                    Suggestions.Add(course);
                }
            }
        }
        public override Task InitializeAsync(object navigationData)
        {
            DebugService.WriteLine(GlobalUserData.CurrentUser.StudentId);
            DebugService.WriteLine(GlobalUserData.CurrentUser.LibraryNumber);
            DebugService.WriteLine(GlobalUserData.CurrentUser.Course);
            DebugService.WriteLine(GlobalUserData.CurrentUser.YearOfStudy);
            DebugService.WriteLine(GlobalUserData.CurrentUser.Name);
            return base.InitializeAsync(navigationData);
        }
        public EditProfileViewModel()
        {
            Title = "Edit Profile";
            AddCoursesToList();
            StudentID.Value = GlobalUserData.CurrentUser.StudentId;
            LibraryNumber.Value = GlobalUserData.CurrentUser.LibraryNumber;
            Course.Value = GlobalUserData.CurrentUser.Course;
            SelectedYearOfStudy = GlobalUserData.CurrentUser.YearOfStudy;
        }
    }
}
