using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace HelmoBilite_RC_LM.Models;

public class Utilisateur : IdentityUser
{
    [AllowNull]
    public string Picture { get; set; }
}