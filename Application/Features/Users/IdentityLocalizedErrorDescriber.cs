using Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Application.Features.Users
{
    public class IdentityLocalizedErrorDescriber : IdentityErrorDescriber
    {
        private readonly IStringLocalizer<ExceptionResource> localizer;

        public IdentityLocalizedErrorDescriber(IStringLocalizer<ExceptionResource> localizer)
        {
            this.localizer = localizer;
        }

        public override IdentityError DefaultError() { return new IdentityError { Code = nameof(DefaultError), Description = localizer["An unknown failure has occurred."] }; }
        public override IdentityError ConcurrencyFailure() { return new IdentityError { Code = nameof(ConcurrencyFailure), Description = localizer["Optimistic concurrency failure, object has been modified."] }; }
        public override IdentityError PasswordMismatch() { return new IdentityError { Code = nameof(PasswordMismatch), Description = localizer["Incorrect password."] }; }
        public override IdentityError InvalidToken() { return new IdentityError { Code = nameof(InvalidToken), Description = localizer["Invalid token."] }; }
        public override IdentityError LoginAlreadyAssociated() { return new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = localizer["A user with this login already exists."] }; }
        public override IdentityError InvalidUserName(string userName) { return new IdentityError { Code = nameof(InvalidUserName), Description = localizer["User name '{0}' is invalid, can only contain letters or digits.", userName] }; }
        public override IdentityError InvalidEmail(string email) { return new IdentityError { Code = nameof(InvalidEmail), Description = localizer["Email '{0}' is invalid.", email] }; }
        public override IdentityError DuplicateUserName(string userName) { return new IdentityError { Code = nameof(DuplicateUserName), Description = localizer["User Name '{0}' is already taken.", userName] }; }
        public override IdentityError DuplicateEmail(string email) { return new IdentityError { Code = nameof(DuplicateEmail), Description = localizer["Email '{0}' is already taken.", email] }; }
        public override IdentityError InvalidRoleName(string role) { return new IdentityError { Code = nameof(InvalidRoleName), Description = localizer["Role name '{0}' is invalid.", role] }; }
        public override IdentityError DuplicateRoleName(string role) { return new IdentityError { Code = nameof(DuplicateRoleName), Description = localizer["Role name '{0}' is already taken.", role] }; }
        public override IdentityError UserAlreadyHasPassword() { return new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = localizer["User already has a password set."] }; }
        public override IdentityError UserLockoutNotEnabled() { return new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = localizer["Lockout is not enabled for this user."] }; }
        public override IdentityError UserAlreadyInRole(string role) { return new IdentityError { Code = nameof(UserAlreadyInRole), Description = localizer["User already in role '{0}'.", role] }; }
        public override IdentityError UserNotInRole(string role) { return new IdentityError { Code = nameof(UserNotInRole), Description = localizer["User is not in role '{0}'.", role] }; }
        public override IdentityError PasswordTooShort(int length) { return new IdentityError { Code = nameof(PasswordTooShort), Description = localizer["Passwords must be at least {0} characters.", length] }; }
        public override IdentityError PasswordRequiresNonAlphanumeric() { return new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = localizer["Passwords must have at least one non alphanumeric character."] }; }
        public override IdentityError PasswordRequiresDigit() { return new IdentityError { Code = nameof(PasswordRequiresDigit), Description = localizer["Passwords must have at least one digit ('0'-'9')."] }; }
        public override IdentityError PasswordRequiresLower() { return new IdentityError { Code = nameof(PasswordRequiresLower), Description = localizer["Passwords must have at least one lowercase ('a'-'z')."] }; }
        public override IdentityError PasswordRequiresUpper() { return new IdentityError { Code = nameof(PasswordRequiresUpper), Description = localizer["Passwords must have at least one uppercase ('A'-'Z')."] }; }
    }
}
