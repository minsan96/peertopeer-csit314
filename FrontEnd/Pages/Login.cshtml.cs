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
            Response.Cookies.Append("CurrentUser", jsonuser, new CookieOptions() {
                Expires = DateTime.Now.AddMinutes(30)
            });
            return RedirectToPage("Index");
        }
    }
}