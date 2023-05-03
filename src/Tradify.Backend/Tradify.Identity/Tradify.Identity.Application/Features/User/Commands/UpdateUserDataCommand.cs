using FluentValidation;
using FluentValidation.Results;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tradify.Identity.Application.Interfaces;
using Unit = LanguageExt.Unit;

namespace Tradify.Identity.Application.Features.User.Commands;

public class UpdateUserDataCommand : IRequest<Result<Unit>>
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

public class UpdateUserDataCommandHandler : IRequestHandler<UpdateUserDataCommand, Result<Unit>>
{
    private readonly IApplicationDbContext _dbContext;
    
    public UpdateUserDataCommandHandler(IApplicationDbContext dbContext) =>
        (_dbContext) = (dbContext);

    public async Task<Result<Unit>> Handle(UpdateUserDataCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.UserDatas
            .AsTracking()
            .FirstOrDefaultAsync(u=>u.Id == request.UserId,cancellationToken);

        var countAlternatedUserDatas = await _dbContext.UserDatas.Where(ud => ud.Id == request.UserId)
            .ExecuteUpdateAsync(s=>
                s.SetProperty(ud=>ud.AvatarPath, ud=>request.AvatarPath)
                .SetProperty(ud=>ud.FirstName, ud=>request.FirstName)
                .SetProperty(ud=>ud.LastName, ud=>request.LastName)
                .SetProperty(ud=>ud.MiddleName, ud=>request.MiddleName)
                .SetProperty(ud=>ud.Phone, ud=>request.Phone)
                .SetProperty(ud=>ud.HomeAddress, ud=>request.HomeAddress)
                .SetProperty(ud=>ud.BirthDate, ud=>request.BirthDate)
                .SetProperty(ud=>ud.UpdatedAt, ud=>DateTime.UtcNow),
                cancellationToken);
        
        if (countAlternatedUserDatas == 0)
        {
            var message = "User with given id was not found.";
            var error = new ValidationException(new[]
            {
                new ValidationFailure(nameof(request.UserId),message)
            });
            return new Result<Unit>(error);
        }

        return Unit.Default;
    }
}