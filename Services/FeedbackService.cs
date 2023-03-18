using SWP391.Project.Entities;
using SWP391.Project.Models.Dtos.Feedback;
using SWP391.Project.Repositories;
 
namespace SWP391.Project.Services
{
    public interface IFeedbackService
    {
        Task<bool> AddFeedbackAsync(Guid accountId, AddFeedbackDto input);
    }
 
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IOrderRepository _orderRepository;
 
        public FeedbackService(IOrderRepository orderRepository,
                               IFeedbackRepository feedbackRepository)
        {
            _orderRepository = orderRepository;
            _feedbackRepository = feedbackRepository;
        }
 
        public async Task<bool> AddFeedbackAsync(Guid accountId, AddFeedbackDto input)
        {
            var existingOrder = await _orderRepository.GetSingleAsync(predicate: order => order.ShipmentAddress!.Account!.Id != accountId
                && order.Id == input.OrderId
            );
 
            if (existingOrder == null)
            {
                return false;
            }
 
            FeedbackEntity newFeedback = new()
            {
                Content = input.Content,
                Rate = input.Rate,
                Order = existingOrder
            };
 
            return await _feedbackRepository.AddAsync(entity: newFeedback);
        }
    }
}
