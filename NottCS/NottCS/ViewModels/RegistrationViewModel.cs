﻿using System;
using System.Collections.Generic;
using System.Text;
using NottCS.Validations;
using System.Diagnostics;

namespace NottCS.ViewModels
{
    class RegistrationViewModel : BaseViewModel
    {
        private ValidatableObject<string> _name = new ValidatableObject<string>();
        public ValidatableObject<string> Name
        {
            get => _name;
            set => _name = value;
        }
        public RegistrationViewModel()
        {
            Title = "NottCS Login";
            AddValidationRules();
        }

        private void AddValidationRules()
        {
            _name.Validations.Add(new NotEmptyRule<string>() {ValidationMessage ="Name cannot be empty"});
        }
    }
}