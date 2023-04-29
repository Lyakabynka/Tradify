using FluentValidation;
using FluentValidation.Results;
using LanguageExt.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Tradify.Identity.Application.Interfaces;
using Tradify.Identity.Application.Responses;
using Tradify.Identity.Application.Responses.Errors;
using Tradify.Identity.Application.Responses.Errors.Common;
using Tradify.Identity.Application.Responses.Types;
using Tradify.Identity.Application.Services;
using Unit = LanguageExt.Unit;

namespace Tradify.Identity.Application.Features.Auth.Commands;

public class LogoutCommand : IRequest<Result<Unit>> {}

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result<Unit>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly JwtProvider _jwtProvider;
    private readonly HttpContext _context;
    private readonly CookieProvider _cookieProvider;
    
    public LogoutCommandHandler(
        IApplicationDbContext dbContext,
        JwtProvider jwtProvider,
        IHttpContextAccessor accessor,
        CookieProvider cookieProvider)
    {
        _dbContext = dbContext;
        _jwtProvider = jwtProvider;
        _context = accessor.HttpContext!;
        _cookieProvider = cookieProvider;
    }
    
    public async Task<Result<Unit>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = _cookieProvider.GetRefreshTokenFromCookie(_context.Request);

        if (refreshToken is null)
        {
            var message = "Error on getting refresh token from cookie.";
            var error = new ValidationException(message, new[]
            {
                new ValidationFailure(nameof(refreshToken),message)
            });
            return new Result<Unit>(error);
        }

        var existingSession = await _dbContext.RefreshSessions
            .FirstOrDefaultAsync(cancellationToken);
        if (existingSession is null)
        {
            var message = "Refresh session by refresh token was not found.";
            var error = new ValidationException(message, new[]
            {
                new ValidationFailure(nameof(refreshToken),message)
            });
            return new Result<Unit>(error);
        }

        //delete refresh from database
        _dbContext.RefreshSessions.Remove(existingSession);

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        //delete from cookies
        _cookieProvider.DeleteJwtTokenFromCookies(_context.Response);
        _cookieProvider.DeleteRefreshTokenFromCookies(_context.Response);
        
        return Unit.Default;
    }
}
