﻿using AutoMapper;
using Tradify.Identity.Application.Features.User.Commands;
using Tradify.Identity.Application.Common.Mappings;

namespace Tradify.Identity.RestAPI.Models;

public class RegisterRequestModel : IMappable
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string? Phone { get; set; }
    public string? HomeAddress { get; set; }
    
    public DateOnly BirthDate { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<RegisterRequestModel, RegisterCommand>();
    }
}