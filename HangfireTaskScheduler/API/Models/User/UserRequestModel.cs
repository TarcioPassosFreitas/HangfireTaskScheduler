using System.ComponentModel.DataAnnotations;

namespace HangfireTaskScheduler.API.Models.User;

public class UserRequestModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string FullName { get; set; } = null!;
}