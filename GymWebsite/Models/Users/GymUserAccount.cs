using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymWebsite.Models.Users;

public class GymUserAccount : IdentityUser {
    [Unicode]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Unicode]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    public DateTime RegistrationDateTime { get; set; }
}
