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
                if (cookie.Key == "CurrentUserType")
                {
                    Response.Cookies.Delete(cookie.Key);
                }
                if (cookie.Key == "CurrentUserName")
                {
                    Response.Cookies.Delete(cookie.Key);
                }
                if (cookie.Key == "CurrentUserID")
                {
                    Response.Cookies.Delete(cookie.Key);
                }
                if (cookie.Key.Contains("Upvote"))
                {
                    Response.Cookies.Delete(cookie.Key);
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var login = new Users();
            try
            {
                var username = User.UserName;
                var password = User.Password;
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                    return Page();
                }

                login = await _apiClient.Login(username, password);
           
                if(login == null || string.IsNullOrEmpty(login.UserName))
                {
                    ModelState.AddModelError(string.Empty, "This user does not exist.");
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
                Response.Cookies.Append("CurrentUserName", login.FirstName + " " + login.LastName, new CookieOptions()
                {
                    Expires = DateTime.Now.AddMinutes(30)
                });
                Response.Cookies.Append("CurrentUserID", login.ID.ToString(), new CookieOptions()
                {
                    Expires = DateTime.Now.AddMinutes(30)
                });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("400"))
                {
                    ModelState.AddModelError(string.Empty, "Incorrect username and password.");
                    return Page();
                }
                else if (ex.Message.Contains("404"))
                {
                    ModelState.AddModelError(string.Empty, "This user does not exist.");
                    return Page();
                }
                else if(ex.Message.Contains("500"))
                {
                    ModelState.AddModelError(string.Empty, "Internal Server Error.");
                    return Page();
                }
            }
            if(login.UserType == "Admin")
            {
                return RedirectToPage("Index");
            }
            else
            {
                return RedirectToPage("QPages/Index");
            }
            
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