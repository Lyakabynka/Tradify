﻿namespace Tradify.Identity.Application.Responses.Errors.Common
{
    public enum ErrorCode
    {
        Unknown,
        
        UserNotFound,
        PasswordInvalid,
        
        RefreshSessionNotFound,
        UserByRefreshSessionNotFound,
        
        RefreshInCookiesNotFound,
        RefreshParseError,
        
        UserNameAlreadyExists,
        EmailAlreadyExists,
    }
}
