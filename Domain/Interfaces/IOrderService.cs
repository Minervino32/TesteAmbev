using Domain.Dtos;

namespace Domain.Interfaces
{
    public interface IOrderService
    {
        Task ProcessOrder(OrderDTO order);
    }
}
