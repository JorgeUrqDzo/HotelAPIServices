using System;
using System.ComponentModel.DataAnnotations;
using Hotel.Entities;
using Hotel.Services;
using Hotel.Services.Exceptions;
using Hotel.Services.Interfaces;
using Hotel.WebApi.JWTProvider;
using Hotel.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using LoginModel = Hotel.WebApi.Models.LoginModel;

namespace Hotel.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Security")]
    public class SecurityController : BaseController
    {
        SecurityService securityService;
        UsersService usersService;
        IAppContext context;
        JwtTokenGenerator tokenGenerator;
        IApplicationSettingsService applicationSettingsService;

        public SecurityController(
            SecurityService authenticationService,
            UsersService usersService,
            IAppContext context,
            JwtTokenGenerator tokenGenerator,
            IApplicationSettingsService applicationSettingsService)
        {
            this.securityService = authenticationService;
            this.usersService = usersService;
            this.context = context;
            this.tokenGenerator = tokenGenerator;
            this.applicationSettingsService = applicationSettingsService;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            try
            {
                var user = securityService.Login(loginModel.Username, loginModel.Password);
                var token = tokenGenerator.GenerateSecurityToken(user);


                return Ok(
                    new
                    {
                        token = token.Value,
                        userName = user.UserName,
                        isGlobal = user.Role.Identifier == RolesIdentifiers.Admin
                    });
            }
            catch (ValidationException validationException)
            {
                return BadRequest(validationException.Message);
            }
            catch (PasswordExpiredException pwdExpiredException)
            {
                var user = usersService.GetUserByUserName(loginModel.Username);
                var token = tokenGenerator.GenerateSecurityToken(user);

                return Unauthorized(pwdExpiredException.Message, pwdExpiredException.ErrorCode,
                    new {Token = token.Value});
            }
            catch (UnauthorizedException unauthorizedException)
            {
                return Unauthorized(unauthorizedException.Message, unauthorizedException.ErrorCode);
            }
        }

        [AllowAnonymous]
        [HttpPost("Account/Activate")]
        public IActionResult UpdatePassword([FromBody] UpdatePasswordModel updatePasswordModel)
        {
            try
            {
                if (updatePasswordModel.ActivationToken == null)
                {
                    return BadRequest("Invalid activation token.");
                }

                securityService.ActivateUserAccount(updatePasswordModel.UserId,
                    updatePasswordModel.ActivationToken.Value, updatePasswordModel.NewPassword);
                return Ok(true);
            }
            catch (ValidationException validationException)
            {
                return BadRequest(validationException.Message);
            }
            catch
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost("ValidateActivationToken")]
        public IActionResult ValidateActivationToken([FromBody] Guid token)
        {
            try
            {
                var user = securityService.ValidateActivationToken(token);

                if (user != null)
                {
                    return Ok(user.Id);
                }
                else
                {
                    return Ok(null);
                }
            }
            catch
            {
                throw;
            }
        }

        [Authorize(Roles = RolesIdentifiers.Admin)]
        [HttpGet("GetInstances")]
        public IActionResult GetInstances()
        {
            try
            {
//                var instances = securityService.GetInstances();
//
//                if (instances != null)
//                {
//                    return new ObjectResult(instances.Select(i => new {i.Id, i.Name}).OrderBy(i => i.Name));
//                }

                return null;
            }
            catch
            {
                throw;
            }
        }


        [Authorize(Roles = RolesIdentifiers.Admin)]
        [HttpGet("UpdateInstance/{instanceId}")]
        public IActionResult UpdateInstance(Guid instanceId)
        {
            try
            {
                var user = this.usersService.GetUser(this.context.UserId);

                var token = tokenGenerator.GenerateSecurityToken(user, instanceId);

                return Ok(token.Value);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost("Account/ChangePassword")]
        public IActionResult ChangePassword([FromBody] UpdatePasswordModel updatePasswordModel)
        {
            try
            {
                securityService.ResetPassword(context.UserId,
                    updatePasswordModel.Password,
                    updatePasswordModel.NewPassword);

                return Ok(true);
            }
            catch (ValidationException validationException)
            {
                return BadRequest(validationException.Message);
            }
            catch
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpGet("Account/PasswordSettings")]
        public IActionResult GetPasswordComplexitySettings()
        {
            return Ok(applicationSettingsService.GetUserAccountSettings());
        }
    }
}