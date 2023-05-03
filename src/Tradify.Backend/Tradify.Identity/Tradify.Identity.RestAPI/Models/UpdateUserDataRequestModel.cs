namespace Tradify.Identity.RestAPI.Models;

public class UpdateUserDataRequestModel
{
    public int UserId { get; set; }
    
    public string AvatarPath { get; set; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string? Phone { get; set; }
    public string? HomeAddress { get; set; }
    
    public DateOnly BirthDate { get; set; }
}