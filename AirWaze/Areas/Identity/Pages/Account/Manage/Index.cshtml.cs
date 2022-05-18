// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AirWaze.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AirWaze.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AirWazeUser> _userManager;
        private readonly SignInManager<AirWazeUser> _signInManager;

        public IndexModel(
            UserManager<AirWazeUser> userManager,
            SignInManager<AirWazeUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [MinLength(1, ErrorMessage = "Minimum 1 character!")]
            [MaxLength(50, ErrorMessage = "Maximum 50 characters!")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "Streetname is required!")]
            public string StreetName { get; set; }

            [Range(0, int.MaxValue, ErrorMessage = "Housenumber error")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "Housenumber is required!")]
            public string HouseNumber { get; set; }

            [Range(0, int.MaxValue, ErrorMessage = "Bus error")]
            public string? Bus { get; set; }

            [Required(ErrorMessage = "Zipcode is Required")]
            [DataType(DataType.PostalCode)]
            public string Zipcode { get; set; }

            [MinLength(1, ErrorMessage = "Minimum 1 character!")]
            [MaxLength(50, ErrorMessage = "Maximum 50 characters!")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "City is required!")]
            public string City { get; set; }

            [MinLength(1, ErrorMessage = "Minimum 1 character!")]
            [MaxLength(50, ErrorMessage = "Maximum 50 characters!")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "Country is required!")]
            public string Country { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(AirWazeUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
