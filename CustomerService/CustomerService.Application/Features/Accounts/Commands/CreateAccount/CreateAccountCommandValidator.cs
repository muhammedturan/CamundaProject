using FluentValidation;

namespace CustomerService.Application.Features.Accounts.Commands.CreateAccount;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Müşteri ID boş olamaz");

        RuleFor(x => x.AccountType)
            .IsInEnum().WithMessage("Geçerli bir hesap türü seçiniz");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Para birimi boş olamaz")
            .Length(3).WithMessage("Para birimi 3 karakter olmalıdır");
    }
}
