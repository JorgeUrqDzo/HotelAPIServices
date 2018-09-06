using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Hotel.Entities;
using Hotel.Services.Interfaces;
using Hotel.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Services
{
    public class UsersService : BaseService
    {
        IRepository<User> userRepository;

        public UsersService(IUnitOfWork unitOfWork, IAppContext context)
            : base(unitOfWork, context)
        {
            userRepository = UnitOfWork.GetRepository<User>();
        }

        public void CreateUser(User user)
        {
            SetDefaults(user);

            ValidateNew(user);

            if (RolesIdentifiers.IsAdmin(user.Role))
            {
                user.InstanceId = null;
            }

            userRepository.Insert(user);

            UnitOfWork.SaveChanges();
        }

        public User GetUser(Guid id)
        {
            return userRepository.GetFirstOrDefault(
                predicate: u => (u.InstanceId == AppContext.InstanceId || u.InstanceId == null) && u.Id == id,
                disableTracking: false, include: i => i.Include(u => u.Role)
                //.Include(u => u.Instance)
            );
        }

        public void ValidateNew(User user)
        {
            ValidateRequiredFields(user);

            if (userRepository.Count(u => u.Email == user.Email && u.IsRemoved == false) != 0)
            {
                throw new ValidationException("The e-mail address is already registered for another user.");
            }

            var role = UnitOfWork.GetRepository<Role>().Find(user.RoleId);
            user.Role = role ?? throw new ValidationException("The User Role does not exist.");

//            if (UnitOfWork.GetRepository<Instance>().Count(i => i.Id == user.InstanceId) == 0)
//            {
//                throw new ValidationException("The Instance does not exist.");
//            }
        }

        public void ValidateEdit(User user)
        {
            ValidateRequiredFields(user);

            if (userRepository.Count(u => u.Id != user.Id && u.Email == user.Email && u.IsRemoved == false) != 0)
            {
                throw new ValidationException("The e-mail address is already registered for another user.");
            }

//            if (UnitOfWork.GetRepository<Instance>().Count(i => i.Id == user.InstanceId || user.InstanceId == null) ==
//                0)
//            {
//                throw new ValidationException("The Instance does not exist.");
//            }

            if (UnitOfWork.GetRepository<Role>().Count(r => r.Id == user.RoleId) == 0)
            {
                throw new ValidationException("The User Role does not exist.");
            }
        }

        public bool UpdateUser(User user)
        {
            var currentUserRoleChanged = false;
            var existingUser = GetUser(user.Id);

            if (AppContext.UserId == user.Id && existingUser.RoleId != user.RoleId)
            {
                currentUserRoleChanged = true;
            }

            if (existingUser == null)
            {
                throw new ValidationException("The user does not exist");
            }

            existingUser.FirstName = user.FirstName;
            existingUser.MiddleName = user.MiddleName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.RoleId = user.RoleId;
            existingUser.IsActive = user.IsActive;
            existingUser.UserName = user.Email;

            if (existingUser.Locked != user.Locked && user.Locked == false)
            {
                existingUser.LoginAttempts = 0;
            }

            existingUser.Locked = user.Locked;
            existingUser.UpdatedBy = AppContext.UserId;
            existingUser.UpdatedDate = DateTimeProvider.UtcNow;

            var userRole = UnitOfWork.GetRepository<Role>().Find(existingUser.RoleId);

            if (RolesIdentifiers.IsAdmin(userRole))
            {
                existingUser.InstanceId = null;
            }
            else
            {
                existingUser.InstanceId = AppContext.InstanceId;
            }

            ValidateEdit(existingUser);

            bool hasImplicitAccess = RolesIdentifiers.IsAdmin(userRole);

            UnitOfWork.SaveChanges();

            return currentUserRoleChanged;
        }

        public User GetUserByUserName(string userName)
        {
            return userRepository.GetFirstOrDefault(
                predicate: u => u.IsActive == true
                                && u.IsRemoved == false
                                && u.UserName == userName,
                disableTracking: false,
                include: i => i.Include(u => u.Role)
//                    .Include(u => u.Instance)
                );
        }

        public IEnumerable<User> GetAll()
        {
            var user = GetUser(AppContext.UserId);
            return userRepository.GetPagedList(predicate: u =>
                    (u.InstanceId == AppContext.InstanceId || u.InstanceId == null)
                    && u.IsActive == true && u.IsRemoved == false && u.Role.Order >= user.Role.Order,
                include: u => u.Include(r => r.Role), pageSize: 10000).Items;
        }

        public IEnumerable<User> SearchUsers(string searchCriteria)
        {
            var user = GetUser(AppContext.UserId);

            return userRepository.GetPagedList(predicate: u =>
                    (u.InstanceId == AppContext.InstanceId || u.InstanceId == null)
                    && u.IsActive == true
                    && u.IsRemoved == false
                    &&
                    (
                        string.IsNullOrWhiteSpace(searchCriteria)
                        || u.UserName.Contains(searchCriteria)
                        || u.FirstName.Contains(searchCriteria)
                        || u.MiddleName.Contains(searchCriteria)
                        || u.LastName.Contains(searchCriteria)
                    )
                    && u.Role.Order >= user.Role.Order,
                include: u => u.Include(r => r.Role), pageSize: 100000).Items;
        }

        private void ValidateRequiredFields(User user)
        {
            if (string.IsNullOrWhiteSpace(user.FirstName))
            {
                throw new ValidationException("First Name is required");
            }

            if (string.IsNullOrWhiteSpace(user.LastName))
            {
                throw new ValidationException("Last Name is required");
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new ValidationException("E-mail address is required");
            }

            if (user.RoleId == Guid.Empty)
            {
                throw new ValidationException("A User Role is required");
            }
        }

        private void SetDefaults(User user)
        {
            DateTime now = DateTimeProvider.UtcNow;

            user.Id = Guid.NewGuid();
            user.UserName = user.Email; //username must be the same as email
            user.CreatedDate = now;
            user.UpdatedDate = now;
            user.CreatedBy = AppContext.UserId;
            user.UpdatedBy = AppContext.UserId;
            user.InstanceId = AppContext.InstanceId;
            user.Password = null;
            user.IsRemoved = false;
            user.LastLogin = null;
            user.LastLoginIp = null;
            user.Locked = false;
            user.LoginAttempts = 0;
            user.ActivationToken = Guid.NewGuid();
            user.PasswordExpires = now.AddMonths(3);
        }
    }
}