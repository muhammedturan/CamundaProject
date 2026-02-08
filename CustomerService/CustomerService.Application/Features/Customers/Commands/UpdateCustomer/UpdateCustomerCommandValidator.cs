using FluentValidation;

namespace CustomerService.Application.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Musteri ID bos olamaz");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ad bos olamaz")
            .MaximumLength(100).WithMessage("Ad en fazla 100 karakter olabilir");

        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage("Soyad bos olamaz")
            .MaximumLength(100).WithMessage("Soyad en fazla 100 karakter olabilir");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Gecerli bir e-posta adresi giriniz")
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}
