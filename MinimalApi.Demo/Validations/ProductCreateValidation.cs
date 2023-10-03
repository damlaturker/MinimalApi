using FluentValidation;
using MinimalApi.Demo.Models.DTO;

namespace MinimalApi.Demo.Validations
{
    public class ProductCreateValidation: AbstractValidator<ProductCreateDto>
    {
        public ProductCreateValidation()
        {
            RuleFor(model=>model.Name).NotEmpty();
            RuleFor(model=>model.Price).InclusiveBetween(1,100);
        }
    }
}
