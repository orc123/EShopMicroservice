using FluentValidation;

namespace Ordering.Application.Orders.Commands.UpdateOrder;
public record UpdateOrderCommand(OrderDto Order) : ICommand<UpdateOrderResult>;

public record UpdateOrderResult(bool IsSuccess);

public class UpdateOrderCommandVAlidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandVAlidator()
    {
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Order.CustomerId).NotNull().WithMessage("CustomerId is required");
        RuleFor(x => x.Order.OrderItems).NotEmpty().WithMessage("CustomerId should not be empty");
    }
}