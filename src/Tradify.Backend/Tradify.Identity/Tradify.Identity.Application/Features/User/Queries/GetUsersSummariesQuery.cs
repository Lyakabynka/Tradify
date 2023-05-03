using AutoMapper;
using AutoMapper.QueryableExtensions;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tradify.Identity.Application.Common.Mappings;
using Tradify.Identity.Application.Interfaces;

namespace Tradify.Identity.Application.Features.User.Queries;

public class UserSummaryResponseModel
{
    public string UserName { get; set; }
    public string AvatarPath { get; set; }
}

public class GetUsersSummariesQuery : IRequest<Result<IEnumerable<UserSummaryResponseModel>>>, IRequest
{
    public IEnumerable<int> UsersIds { get; set; }
}

public class GetUsersSummariesQueryHandler : IRequestHandler<GetUsersSummariesQuery, Result<IEnumerable<UserSummaryResponseModel>>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetUsersSummariesQueryHandler(IApplicationDbContext dbContext) =>
    _dbContext = dbContext;

    public async Task<Result<IEnumerable<UserSummaryResponseModel>>> Handle(GetUsersSummariesQuery request, CancellationToken cancellationToken)
    {
        var userSummaryResponseModels = await _dbContext.Users
            .Where(u => request.UsersIds.Any(id => id == u.Id))
            .Include(u=>u.UserData)
            .Select(u=>
                new UserSummaryResponseModel()
                {
                    UserName = u.UserName,
                    
                    AvatarPath = u.UserData.AvatarPath,
                })
            .ToListAsync(cancellationToken);
        
        return userSummaryResponseModels;
    }
}