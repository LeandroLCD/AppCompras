using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;

namespace AppCompras.Models.Autentication
{
    public class User : Product.Invoice
    {
        [JsonIgnore]
        public string LocalId { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public List<Rol> Roles { get; set; }

        public List<string> ProducFavorite { get; set; }

        [JsonIgnore]
        public string VerifyEmail { get; set; }

        public string Password { get; set; }

        [JsonIgnore]
        public string VerifyPassword { get; set; }

        #region Reading properties
        [JsonIgnore]
        public bool IsValidPasswor => !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(VerifyPassword) && Password == VerifyPassword ? true : false;

        [JsonIgnore]
        public bool IsValidEmail => IsEmail(Email) && IsEmail(VerifyEmail) && Email == VerifyEmail ? true : false;

        [JsonIgnore]
        public bool IsValidPhone => Phone != null && Phone.Length == 12 ? IsPhonenumber(Phone) : false;

        [JsonIgnore]
        public bool IsValidDomain => IsDomainValid(Email);

        public Rol Rol
        {
            get => default;
            set
            {
            }
        }

        #endregion

        #region Methods
        public bool IsEmail(string email)
        {
            if (email != null && new EmailAddressAttribute().IsValid(email))
            {
                return true;
            }
            return false;
        }

        public bool IsPhonenumber(string phoneNumber)
        {
            var match = Regex.Match(phoneNumber, @"^\+[0-9]{11}");
            if (match.Success)
                return true;
            else
                return false;
        }

        public bool IsDomainValid(string email)
        {

            try
            {
                var domain = email.Split('@');
                var mxRecords = Dns.GetHostEntry(domain[1]);
                return mxRecords.HostName == domain[1] ? true : false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        #endregion
    }
}
