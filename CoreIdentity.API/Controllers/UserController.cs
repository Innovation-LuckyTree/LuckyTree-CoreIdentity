using CoreIdentity.Application.Requests.Users.Commands;
using CoreIdentity.Application.Requests.Users.Commands.AddUserRole;
using CoreIdentity.Application.Requests.Users.Commands.ResetUserPassword;
using CoreIdentity.Application.Requests.Users.Commands.UnlockUserAccount;
using CoreIdentity.Application.Requests.Users.Queries.GetLastActivity;
using CoreIdentity.Application.Requests.Users.Queries.GetLockedUserByUserId;
using CoreIdentity.Application.Requests.Users.Queries.GetLockedUsers;
using CoreIdentity.Application.Requests.Users.Queries.Getusers;
using CoreIdentity.Application.Requests.Users.Queries.UpdateUserInfo;
using CoreIdentity.Application.Requests.Users.Queries.UpdateUserPassword;
using CoreIdentity.Application.Requests.Users.Queries.UpdateUserPasswordById;
using Microsoft.AspNetCore.Mvc;

namespace CoreIdentity.API.Controllers;

/// <summary>
/// Users controller
/// </summary>
public class UsersController : ApiBaseController
{
    /// <summary>
    /// Get users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetUsersQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Create User
    /// </summary>
    /// <param name="request"></param>  
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> Put([FromBody]CreateUserCommand request, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Update User Information
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]UpdateUserInfoCommand request, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Add Role to User
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("role")]
    public async Task<IActionResult> AddUserRole([FromBody]AddUserRoleCommand request, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Reset User Password
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("password/reset")]
    public async Task<IActionResult> ResetUserPassword([FromBody]ResetUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Update User Password
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("password")]
    public async Task<IActionResult> UpdatePassword([FromBody]UpdateUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Update User Password but should be pass thru OTP process
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("password/update")]
    public async Task<IActionResult> UpdateUserPassword([FromBody]UpdateUserPasswordByIdCommand request, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Get locked users
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("locked/list")]
    public async Task<IActionResult> GetLockedUsers([FromQuery] GetLockedUsersQuery request, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Get locked user
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("locked")]
    public async Task<IActionResult> GetLockedUser([FromQuery] GetLockedUserByUserIdQuery request, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Unlock user
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("unlock")]
    public async Task<IActionResult> UnlockUser([FromBody] UnlockUserAccountCommand request, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Get user last activity
    /// </summary>
    /// <returns></returns>
    [HttpGet("last-activity/{UserId}")]
    public async Task<IActionResult> GetLastActivity(Guid UserId, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetLastActivityQuery(UserId), cancellationToken);
        return Ok(result);
    }
}    
