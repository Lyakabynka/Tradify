﻿using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tradify.Identity.Application.Mappings;
using Tradify.Identity.Application.Responses;
using Tradify.Identity.Application.Responses.Errors;

namespace Tradify.Identity.RestAPI.Results;

public class ApiResult<TEntity> : ActionResult, IMappable
{
    public TEntity? Data { get; set; }
    public Error? Error { get; set; }
    
    public override async Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;

        response.ContentType = "application/json";
        response.StatusCode = Error == null ? (int)HttpStatusCode.OK : (int)Error.StatusCode;

        await response.WriteAsJsonAsync(this);
    }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Result<TEntity>, ApiResult<TEntity>>();
    }
}