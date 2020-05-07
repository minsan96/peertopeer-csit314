using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FrontEnd.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PeerToPeerDTO;

namespace FrontEnd.Pages
{
    public class LoginModel : PageModel
    {
        protected readonly IApiClient _apiClient;

        public LoginModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public Users User { get; set; }

        public void OnGetDeleteExistingCookieAsync()
        {
            foreach (var cookie in HttpContext.Request.Cookies)
            {
                if (cookie.Key == "CurrentUser")
                {
                    Response.Cookies.Delete(cookie.Key);
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var username = User.UserName;
            var password = User.Password;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                return Page();
            }
            var login = await _apiClient.Login(username, password);
            if(login == null || string.IsNullOrEmpty(login.UserName))
            {
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                return Page();
            }
            var jsonuser = JsonConvert.SerializeObject(login);
            await this.SignInUser(login.UserName, false);
            Response.Cookies.Append("CurrentUser", jsonuser, new CookieOptions() {
                Expires = DateTime.Now.AddMinutes(30)
            });
            Response.Cookies.Append("CurrentUserType", login.UserType, new CookieOptions()
            {
                Expires = DateTime.Now.AddMinutes(30)
            });
            return RedirectToPage("Index");
        }

        private async Task SignInUser(string username, bool isPersistent)
        {
            // Initialization.  
            var claims = new List<Claim>();

            try
            {
                // Setting  
                claims.Add(new Claim(ClaimTypes.Name, username));
                var claimIdenties = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimPrincipal = new ClaimsPrincipal(claimIdenties);
                var authenticationManager = Request.HttpContext;

                // Sign In.  
                await authenticationManager.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, new AuthenticationProperties() { IsPersistent = isPersistent });
            }
            catch (Exception ex)
            {
                // Info  
                throw ex;
            }
        }
    }
}