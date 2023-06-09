﻿using AutoMapper;
using FluentValidation;
using LanguageExt.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tradify.Identity.Application.Common.Extensions;

namespace Tradify.Identity.RestAPI.Controllers;

[ApiController]
public class ApiControllerBase : ControllerBase
{
    private ISender _mediator;
    private IMapper _mapper;
    
    protected ISender Mediator =>
        _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    protected IMapper Mapper =>
        _mapper ??= HttpContext.RequestServices.GetRequiredService<IMapper>();

    protected internal async Task<IActionResult> RequestAsync<TValue>(
        IRequest<Result<TValue>> request)
    {
        // non generic / generic
        var result = await Mediator.Send(request);

        return result.Match<IActionResult>(b =>
        {
            // ReSharper disable once HeapView.PossibleBoxingAllocation
            return Ok(b);
        },exception =>
        {  
            return exception switch
            {
                ValidationException validationException => BadRequest(validationException.ToProblemDetails()),
                _ => StatusCode(500),
            };
        });
    }
    
}