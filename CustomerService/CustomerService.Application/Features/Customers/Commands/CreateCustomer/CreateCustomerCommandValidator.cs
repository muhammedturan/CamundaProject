using FluentValidation;

namespace CustomerService.Application.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ad boş olamaz")
            .MaximumLength(100).WithMessage("Ad en fazla 100 karakter olabilir");

        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage("Soyad boş olamaz")
            .MaximumLength(100).WithMessage("Soyad en fazla 100 karakter olabilir");

        RuleFor(x => x.CitizenId)
            .NotEmpty().WithMessage("TC Kimlik No boş olamaz")
            .Length(11).WithMessage("TC Kimlik No 11 haneli olmalıdır")
            .Matches(@"^\d+$").WithMessage("TC Kimlik No sadece rakam içermelidir");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz")
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}
