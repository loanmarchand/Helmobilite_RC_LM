// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Helmobilite_RC_LM.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using HelmoBilite_RC_LM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Helmobilite_RC_LM.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Utilisateur> _signInManager;
        private readonly UserManager<Utilisateur> _userManager;
        private readonly IUserStore<Utilisateur> _userStore;
        private readonly IUserEmailStore<Utilisateur> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<Utilisateur> userManager,
            IUserStore<Utilisateur> userStore,
            SignInManager<Utilisateur> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

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
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public IEnumerable<SelectListItem> PermisSelectList { get; set; }

        
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            
            [Display(Name = "Nom de l'entreprise")]
            public string ClientNomEntreprise { get; set; }
            public Adresse ClientAdresse { get; set; }
            public UtilisateurHelmo UtilisateurHelmo { get; set; }
            [Display(Name = "Niveau d'étude")]
            public string DispatcherNiveauEtude { get; set; }
            public string ChauffeurPermis { get; set; }
            public string Role { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (CheckInput())
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);
                await _userManager.AddToRoleAsync(user, Input.Role);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private bool CheckInput()
        {
            
            if (Input.Role == "client")
            {
                if (Input.ClientNomEntreprise == null || Input.ClientAdresse == null)
                {
                    ModelState.AddModelError(string.Empty, "Veuillez remplir tous les champs");
                    return false;
                }
            }
            else if (Input.Role == "dispatcher")
            {
                if (Input.UtilisateurHelmo == null || Input.DispatcherNiveauEtude == null)
                {
                    ModelState.AddModelError(string.Empty, "Veuillez remplir tous les champs");
                    return false;
                }
            }
            else if (Input.Role == "chauffeur")
            {
                if (Input.UtilisateurHelmo == null || Input.ChauffeurPermis == null)
                {
                    ModelState.AddModelError(string.Empty, "Veuillez remplir tous les champs");
                    return false;
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Veuillez remplir tous les champs");
                return false;
            }

            return true;
        }

        private Utilisateur CreateUser()
        {
            try
            {
                switch (Input.Role)
                {
                    case "client":
                    {
                        var user = Activator.CreateInstance<Client>();
                        user.CompanyName = Input.ClientNomEntreprise;
                        user.CompagnyAdress = Input.ClientAdresse;
                        return user;
                    }
                    case "dispatcher":
                    {
                        var user = Activator.CreateInstance<Dispatcher>();
                        user.Nom = Input.UtilisateurHelmo.Nom;
                        user.Prenom = Input.UtilisateurHelmo.Prenom;
                        user.Matricule = Input.UtilisateurHelmo.Matricule;
                        user.NiveauEtude = Input.DispatcherNiveauEtude;
                        return user;
                    }
                    case "chauffeur":
                    {
                        var user = Activator.CreateInstance<Camionneur>();

                        user.Nom = Input.UtilisateurHelmo.Nom;
                        user.Prenom = Input.UtilisateurHelmo.Prenom;
                        user.Matricule = Input.UtilisateurHelmo.Matricule;
                        user.Permis = Input.ChauffeurPermis;
                        return user;
                    }
                    default:
                        return new Utilisateur();
                }
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(Utilisateur)}'. " +
                    $"Ensure that '{nameof(Utilisateur)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<Utilisateur> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<Utilisateur>)_userStore;
        }
    }
}
