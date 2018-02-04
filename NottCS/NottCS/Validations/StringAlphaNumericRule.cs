﻿using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("NottCSTest")]
namespace NottCS.Validations
{
    internal class StringAlphaNumericRule : IValidationRule<string>
    {
        public string ValidationMessage { get; set; }

        public bool Check(string value)
        {
            if (value == null)
            {
                return false;
            }
            var str = value;
            Regex r = new Regex("^[a-zA-Z0-9]*$");
            return r.IsMatch(str);
        }
    }
}