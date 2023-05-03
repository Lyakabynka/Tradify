namespace Tradify.Identity.Application.Features.User.Commands;

public class UpdateUserPasswordCommand
{
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}