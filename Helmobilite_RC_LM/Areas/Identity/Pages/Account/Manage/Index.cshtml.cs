// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using HelmoBilite_RC_LM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helmobilite_RC_LM.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Utilisateur> _userManager;
        private readonly SignInManager<Utilisateur> _signInManager;
        private readonly HelmoBiliteRcLmContext _context;

        public IndexModel(
            UserManager<Utilisateur> userManager,
            SignInManager<Utilisateur> signInManager,HelmoBiliteRcLmContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
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

        public string Role { get; set; }
        
        [Display(Name = "Photo")]
        public IFormFile PhotoFile { get; set; }

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

            public Client client { get; set; }
            public Dispatcher dispatcher { get; set; }
            public Camionneur driver { get; set; }
        }

        private async Task LoadAsync(Utilisateur user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            Role =  _userManager.GetRolesAsync(user).Result[0];
            Input = new InputModel
            {
                client = null,
                dispatcher = null,
                driver = null
            };
            switch (Role)
            {
                //Réccupération des données du client
                case "Client":
                    Input.client = (Client) user;
                    Input.client.CompagnyAdress = await _context.Adresses.FindAsync(Input.client.IdAdresse);
                    break;
                case "Dispatcher":
                    Input.dispatcher = (Dispatcher) user;
                    break;
                case "Camionneur":
                    Input.driver = (Camionneur) user;
                    break;
            }

            Username = userName;


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
            var role = _userManager.GetRolesAsync(user).Result[0];
            if (role =="Client")
            {
                if (CheckClientEdit((Client)user))
                {
                    await _userManager.UpdateAsync(user);
                    
                }
                else
                {
                    await LoadAsync(user);
                    return Page(); 
                }
            }
            else if (role == "Dispatcher")
            {
                if (CheckDispatcherEdit((Dispatcher)user))
                {
                    await _userManager.UpdateAsync(user);
                }
                else
                {
                    await LoadAsync(user);
                    return Page(); 
                }
            }
            else if (role == "Camionneur")
            {
                if (CheckDriverEdit((Camionneur)user))
                {
                    await _userManager.UpdateAsync(user);
                }
                else
                {
                    await LoadAsync(user);
                    return Page(); 
                }
            }
            else
            {
                await LoadAsync(user);
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        private bool CheckDriverEdit(Camionneur user)
        {
            var edit = false;
            if (user.Matricule != Input.driver.Matricule)
            {
                user.Matricule = Input.driver.Matricule;
                edit = true;
            }

            if (user.Prenom != Input.driver.Prenom)
            {
                user.Prenom = Input.driver.Prenom;
                edit = true;
            }

            if (user.Nom != Input.driver.Nom)
            {
                user.Nom = Input.driver.Nom;
                edit = true;
            }

            if (user.DateDeNaissance != Input.driver.DateDeNaissance)
            {
                user.DateDeNaissance = Input.driver.DateDeNaissance;
                edit = true;
            }

            if (user.Permis != Input.driver.Permis)
            {
                user.Permis = Input.driver.Permis;
                edit = true;
            }

            return edit;
        }

        private bool CheckDispatcherEdit(Dispatcher user)
        {
            var edit = false;
            if (user.Matricule != Input.dispatcher.Matricule)
            {
                user.Matricule = Input.dispatcher.Matricule;
                edit = true;
            }

            if (user.Prenom != Input.dispatcher.Prenom)
            {
                user.Prenom = Input.dispatcher.Prenom;
                edit = true;
            }

            if (user.Nom != Input.dispatcher.Nom)
            {
                user.Nom = Input.dispatcher.Nom;
                edit = true;
            }

            if (user.DateDeNaissance != Input.dispatcher.DateDeNaissance)
            {
                user.DateDeNaissance = Input.dispatcher.DateDeNaissance;
                edit = true;
            }

            if (user.NiveauEtude != Input.dispatcher.NiveauEtude)
            {
                user.NiveauEtude = Input.dispatcher.NiveauEtude;
                edit = true;
            }

            return edit;
        }

        private bool CheckClientEdit(Client user)
        {
            var edit = false;
            if (user.CompanyName != Input.client.CompanyName)
            {
                user.CompanyName = Input.client.CompanyName;
                edit = true;
            }
            var co = _context.Adresses.Find(user.IdAdresse);
            if (co != Input.client.CompagnyAdress)
            {
                user.CompagnyAdress = Input.client.CompagnyAdress;
                _userManager.UpdateAsync(user);
                //enlever l'ancienne adresse
                _context.Adresses.Remove(co);
                user.IdAdresse = Input.client.CompagnyAdress.Id;
                edit = true;
            }

            return edit;
        }
    }
}
