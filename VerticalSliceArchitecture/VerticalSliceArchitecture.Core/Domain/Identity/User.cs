using System;
using System.Collections.Generic;
using VerticalSliceArchitecture.Core.Domain.Games;
using VerticalSliceArchitecture.Core.Domain.Shared;
using VerticalSliceArchitecture.Core.Exceptions;
using VerticalSliceArchitecture.Core.Helpers;

namespace VerticalSliceArchitecture.Core.Domain.Identity
{
    public class User: BaseEntity<Guid>
    {
        private readonly List<Session> _sessions = new List<Session>();
        public  IEnumerable<Session> Sessions => _sessions.AsReadOnly();
        public  ICollection<Game> Games { get; private set;}

        public string Email { get; private set; }
        public string Login { get; private set; }
        public string PasswordHash { get; private set; }
        public string PasswordSalt { get; private set; }
        public UserRole UserRole { get; private set; }       
        public bool IsDeactivated { get; private set; }
       
        private User()
        {
        }

        public User(string email,string login,UserRole userRole,string passwordHash,string passwordSalt) : this()
        {
            SetEmail(email);
            SetLogin(login);
            UserRole = userRole;
            SetPasswordAndSalt(passwordHash, passwordSalt);
        }
        public void ReActivateUser()
        {
            if (!IsDeactivated)
            {
                return;
            }

            IsDeactivated = false;
        }
        public void DeactivateUser()
        {
            if (IsDeactivated)
            {
                return;
            }

            IsDeactivated = true;
        }
        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new DomainException(DomainExceptionCodes.InvalidEmail,
                   ExceptionMessageHelpers.NotEmpty(nameof(Email)));
            }

            if (Email == email)
            {
                return;
            }

            if (email.Length > 255)
            {
                throw new DomainException(DomainExceptionCodes.InvalidEmail,
                   ExceptionMessageHelpers.NoLongerThen(nameof(Email), 255));
            }

            Email = email.Trim().ToLowerInvariant();
        }       

        public void SetLogin(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                throw new DomainException(DomainExceptionCodes.InvalidLogin,
                   ExceptionMessageHelpers.NotEmpty(nameof(Login)));
            }

            if (Login == login)
            {
                return;
            }

            if (login.Length > 50)
            {
                throw new DomainException(DomainExceptionCodes.InvalidLogin,
                    ExceptionMessageHelpers.NoLongerThen(nameof(Login), 50));
            }

            Login = login;
        }
        public void SetPasswordAndSalt(string password, string salt)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new DomainException(DomainExceptionCodes.InvalidPassword,
                   ExceptionMessageHelpers.NotEmpty(nameof(PasswordHash)));
            }

            if (string.IsNullOrWhiteSpace(salt))
            {
                throw new DomainException(DomainExceptionCodes.InvalidSalt,
                   ExceptionMessageHelpers.NotEmpty(nameof(PasswordSalt)));
            }

            if (PasswordHash == password && PasswordSalt == salt)
            {
                return;
            }

            if (password.Length > 64)
            {
                throw new DomainException(DomainExceptionCodes.InvalidLogin,
                   ExceptionMessageHelpers.NoLongerThen(nameof(PasswordHash), 64));
            }

            if (salt.Length > 64)
            {
                throw new DomainException(DomainExceptionCodes.InvalidLogin,
                   ExceptionMessageHelpers.NoLongerThen(nameof(PasswordSalt), 64));
            }

            PasswordHash = password;
            PasswordSalt = salt;
        }                      
        public void SetPasswordHash(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
            {
                throw new DomainException(DomainExceptionCodes.InvalidPassword,
                   ExceptionMessageHelpers.NotEmpty(nameof(PasswordHash)));
            }

            if (PasswordHash == hash )
            {
                return;
            }

            if (hash.Length > 64)
            {
                throw new DomainException(DomainExceptionCodes.InvalidLogin,
                  ExceptionMessageHelpers.NoLongerThen(nameof(PasswordHash), 64));
            }
         
            PasswordHash = hash;
        }
       


    }
}