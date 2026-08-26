using DirectoryService.Contracts.Dtos;
using FluentValidation;

namespace DirectoryService.Core.Departments;

public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentDto>
{
  public  CreateDepartmentValidator()
  {
    RuleFor(x => x.Name)
      .NotNull()
      .NotEmpty()
      .MaximumLength(100)
      .WithMessage("Некорректное имя");
    
    RuleFor(x => x.Slug)
      .NotNull()
      .NotEmpty()
      .MaximumLength(50)
      .WithMessage("Некорректный slug");

    RuleFor(d => d.locationIds)
      .NotNull()
      .NotEmpty()
      .WithMessage("Необходимо указать локации");
  }
}