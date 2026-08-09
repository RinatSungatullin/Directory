using DirectoryService.Contracts.Dtos;
using FluentValidation;

namespace DirectoryService.Core.Locations;

public class CreateLocationValidator : AbstractValidator<CreateLocationDto>
{
  public CreateLocationValidator()
  {
    RuleFor(x => x.Name)
      .NotNull()
      .NotEmpty()
      .MaximumLength(50)
      .WithMessage("Имя некорректно");
    
    RuleFor(x => x.Address.City)
      .NotNull()
      .NotEmpty()
      .MaximumLength(100)
      .WithMessage("Адрес некорректен");
    
    RuleFor(x => x.Address.Street)
      .NotNull()
      .NotEmpty()
      .MaximumLength(100)
      .WithMessage("Адрес некорректен");
    
    RuleFor(x => x.Address.Building)
      .NotNull()
      .NotEmpty()
      .MaximumLength(100)
      .WithMessage("Адрес некорректен");
    
    RuleFor(x => x.Address.OfficeNumber)
      .NotNull()
      .NotEmpty()
      .MaximumLength(10)
      .WithMessage("Адрес некорректен");
  }
}