using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Hotel.Entities;
using Hotel.Services.Exceptions;
using Hotel.Services.Helpers;
using Hotel.Services.Interfaces;
using Hotel.Services.Settings;
using Hotel.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Hotel.Services
{
  public class SecurityService : BaseService
    {
        protected CustomSettings Settings { get; private set; }
        IRepository<User> userRepository;
        UsersService UsersService;
        UserAccountSettings accountSettings;

        public SecurityService(IUnitOfWork unitOfWork, CustomSettings settings, IAppContext context, IApplicationSettingsService applicationSettingsService, UsersService usersService)
        : base(unitOfWork, context)
        {
            Settings = settings;
            UsersService = usersService;
            userRepository = UnitOfWork.GetRepository<User>();
            accountSettings = applicationSettingsService.GetUserAccountSettings();
        }

        public User Login(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                throw new ValidationException("User Name and Password cannot be empty.");
            }

            var pass = SecurityCrypto.SerializeAndEncrypt(password, Settings.CryptoKey);

            var user = userRepository.GetFirstOrDefault(u => u.UserName == userName && u.Password == pass && u.IsActive == true && u.IsRemoved == false, null, i => i.Include(u => u.Role), true);

            if (user == null)
            {
                user = userRepository.GetFirstOrDefault(u => u.UserName == userName, null, null, true);

                if (user == null)
                    throw new ValidationException("The User Name or Password are Invalid.");

                if (user.Locked)
                    throw new UnauthorizedException("The User is Locked. Please contact the Administrator.", "user_locked");

                UpdateLoginAttempts(user);
            }

            if (!user.IsActive)
                throw new UnauthorizedException("The User is not Active. Please contact the Administrator.", "user_inactive");

            if (user.PasswordExpires.HasValue && user.PasswordExpires.Value < DateTimeProvider.UtcNow)
                throw new PasswordExpiredException();

            //Update user last login information
            user.LastLogin = DateTimeProvider.UtcNow;
            user.LastLoginIp = AppContext.UserIpAddress;
            user.LoginAttempts = 0;
            userRepository.Update(user);
            UnitOfWork.SaveChanges();

            return user;
        }

        public void UpdateLoginAttempts(User user)
        {
            user.UpdatedDate = DateTimeProvider.UtcNow;
            user.LoginAttempts = user.LoginAttempts + 1;

            if (user.LoginAttempts == accountSettings.NumFailedLoginAttemptsLockout)
            {
                user.Locked = true;
                userRepository.Update(user);
                UnitOfWork.SaveChanges();
                throw new ValidationException("The User has been locked. Please contact the Administrator.");
            }

            userRepository.Update(user);
            UnitOfWork.SaveChanges();
            throw new ValidationException("The Password is Invalid. The User will be locked after " + accountSettings.NumFailedLoginAttemptsLockout + " invalid attempts (" + user.LoginAttempts + ")");
        }

        public void ActivateUserAccount(Guid userID, Guid activationToken, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ValidationException("Password cannot be empty. Please try again.");
            }

            var user = userRepository.Find(userID);

            if (user == null)
            {
                throw new ValidationException("The user you are trying to activate is not valid.");
            }


            if (user.ActivationToken == null || string.IsNullOrEmpty(user.Password) == false)
            {
                throw new ValidationException("The user has been already activated.");
            }

            if (user.ActivationToken != activationToken)
            {
                throw new ValidationException("You are not authorized to perform this action."); //must be forbidden
            }

            ChangeUserPassword(user, password);
            user.ActivationToken = null;
            UnitOfWork.SaveChanges();
        }

        public User ValidateActivationToken(Guid token)
        {
            return UnitOfWork.GetRepository<User>()
                                  .GetFirstOrDefault(u => u.ActivationToken != null && u.ActivationToken == token, null, null, true);
        }

//        public IEnumerable<Instance> GetInstances()
//        {
//            return UnitOfWork.GetRepository<Instance>().GetPagedList(pageSize: 1000).Items;
//        }

        public void ResetPassword(Guid userId, string password, string newPassword)
        {

            if (userId != AppContext.UserId)
            {
                throw new ValidationException("You are not authorized to perform this action."); //must be forbidden
            }

            var user = UsersService.GetUser(userId);

            if (user == null)
            {
                throw new ValidationException("User does not exist.");
            }

            if (CheckPassword(user, EncryptPassword(password)) == false)
            {
                throw new ValidationException("The value for Current Password you provided is incorrect. Please re-enter your Current Password");
            }

            if (user.Locked)
            {
                throw new ValidationException("The account is locked due to previous failed login attempts. Please contact the Administrator."); //must be forbidden
            }

            ChangeUserPassword(user, newPassword);
            UnitOfWork.SaveChanges();
        }

        private void ChangeUserPassword(User user, string newPassword)
        {
            ValidatePasswordComplexity(newPassword);

            var encryptedPassword = EncryptPassword(newPassword);

            ValidatePasswordHistory(user, encryptedPassword);

            string currentPassword = user.Password;

            UpdatePassword(user, encryptedPassword);

            if (string.IsNullOrEmpty(currentPassword) == false)
            {
                UpdateUserPasswordHistory(user.Id, currentPassword);
            }
        }

        private bool CheckPassword(User user, string encryptedPassword)
        {
            return user.Password == encryptedPassword;
        }

        private string EncryptPassword(string password)
        {
            return SecurityCrypto.SerializeAndEncrypt(password, Settings.CryptoKey);
        }

        private void UpdatePassword(User user, string encryptedPassword)
        {
            user.Password = encryptedPassword;
            user.PasswordExpires = DateTimeProvider.UtcNow.AddDays(accountSettings.PasswordExpirationDuration);
            user.UpdatedBy = user.Id;
            user.UpdatedDate = DateTimeProvider.UtcNow;
        }

        private void ValidatePasswordComplexity(string password)
        {
            var passwordValidator = new PasswordComplexityValidator(accountSettings, password);
            if (passwordValidator.IsValid() == false)
            {
                throw new ValidationException("The password does not meet the complexity rules.");
            }
        }

        private void ValidatePasswordHistory(User user, string encryptedPassword)
        {
            if (accountSettings.PasswordNumPrevAllowed == 0)
            {
                return;
            }

            if(user.Password == encryptedPassword)
            {
                throw new ValidationException("The new password cannot be the same as the current one.");
            }

            var history = UnitOfWork
                            .GetRepository<UserPasswordHistoryEntry>()
                            .GetPagedList(
                                predicate: h => h.UserId == user.Id,
                                orderBy: o => o.OrderByDescending(h => h.UpdatedDate),
                                pageSize: accountSettings.PasswordNumPrevAllowed
                            ).Items;

            if (history.Any(h => h.Password == encryptedPassword))
            {
                throw new ValidationException($"Your may not reuse any of your last {accountSettings.PasswordNumPrevAllowed} passwords.");
            }
        }

        private void UpdateUserPasswordHistory(Guid userId, string encryptedPassword)
        {
            var repository = UnitOfWork.GetRepository<UserPasswordHistoryEntry>();

            var historyEntries = repository.GetPagedList(
                                                predicate: h => h.UserId == userId,
                                                orderBy: o => o.OrderByDescending(h => h.UpdatedDate),
                                                pageSize: 1000,
                                                disableTracking: false
                                            ).Items;

            while (historyEntries.Count >= accountSettings.PasswordNumPrevAllowed)
            {
                var last = historyEntries[historyEntries.Count - 1];

                historyEntries.RemoveAt(historyEntries.Count - 1);
                repository.Delete(last);
            }

            repository.Insert(new UserPasswordHistoryEntry
            {
                UserId = userId,
                Password = encryptedPassword,
                UpdatedDate = DateTime.UtcNow
            });
        }
    }
}