using Ardalis.Result;
using HangfireTaskScheduler.Core.Aggregate.UserAggregate;
using HangfireTaskScheduler.Core.Interfaces.Repository;
using HangfireTaskScheduler.Core.Interfaces.Service;
using System.Text.RegularExpressions;

namespace HangfireTaskScheduler.Core.Services;

public class UserService : IUserService
{
    private readonly IEmailSenderService _emailSenderService;
    private readonly IUserRepository _userRepository;

    public UserService(
        IEmailSenderService emailSenderService,
        IUserRepository userRepository)
    {
        _emailSenderService = emailSenderService;
        _userRepository = userRepository;
    }

    public async Task<Result<User>> AddAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            var validEmail = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            var isValid = Regex.IsMatch(user.Email, validEmail);

            if (!isValid)
            {
                var errors = new List<ValidationError> { new ValidationError
                {
                    Identifier = "Email",
                    ErrorMessage = "Email inválido",
                    ErrorCode = "InvalidEmail",
                    Severity = ValidationSeverity.Error
                }};

                return Result<User>.Invalid(errors);
            }

            var validPassword = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()]).{8,}$";

            var isPasswordValid = Regex.IsMatch(user.Password, validPassword);

            if (!isPasswordValid)
            {
                var errors = new List<ValidationError>
                {
                    new ValidationError
                    {
                        Identifier = "Password",
                        ErrorMessage = "The password is invalid. It must contain at least one lowercase character, one uppercase character, one digit, one special character, and be at least 8 characters long.",
                        ErrorCode = "InvalidPassword",
                        Severity = ValidationSeverity.Error
                    }
                };

                return Result<User>.Invalid(errors);
            }

            user.Password = User.GenerateHash(user.Password);

            var result = await _userRepository.AddAsync(user, cancellationToken);

            if (result != null)
                await _emailSenderService.SendEmailAsync("t.passos.2017.2@gmail.com", user.FullName, user.Email, "Welcome", "Welcome to our application", "4855647");

            return Result<User>.Success(result);
        }
        catch
        {
            return Result.Error();
        }
    }
}