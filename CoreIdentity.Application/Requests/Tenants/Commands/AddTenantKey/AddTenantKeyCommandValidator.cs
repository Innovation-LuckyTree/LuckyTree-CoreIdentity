using FluentValidation;

namespace CoreIdentity.Application.Requests.Tenants.Commands.AddTenantKey;

public class AddTenantKeyCommandValidator : AbstractValidator<AddTenantKeyCommand>
{
    public AddTenantKeyCommandValidator()
    {
        RuleFor(o => o.TenantId)
            .NotEmpty();
    }
}
