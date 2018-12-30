using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using POSLibrary.Models;

namespace POSLibrary.Validations
{
    public class ProductValidator : AbstractValidator<ProductModel>
    {
        public ProductValidator()
        {
            RuleFor(prod => prod.Number).NotEmpty().WithMessage("Product Number is required");
            RuleFor(prod => prod.Name).NotEmpty().WithMessage("Product name is required")
                .Length(2, 100).WithMessage("Product name should be of 2-100 characters.");
            RuleFor(prod => prod.Quantity).NotNull().WithMessage("Quantity is required");
            RuleFor(prod => prod.UnitCostPrice).NotEmpty().WithMessage("Unit cost price is required");
            RuleFor(prod => prod.UnitSellingPrice).NotEmpty().WithMessage("Unit selling price is required");
        }


    }
}
