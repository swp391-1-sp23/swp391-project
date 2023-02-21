using SWP391.Project.Entities;
using SWP391.Project.Models.Dtos.Feedback;
using SWP391.Project.Repositories;

namespace SWP391.Project.Services
{
    public interface IFeedbackService
    {
        Task<bool> AddFeedbackAsync(AddFeedbackDto input);
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

        public async Task<bool> AddFeedbackAsync(AddFeedbackDto input)
        {
            FeedbackEntity newFeedback = new()
            {
                Content = input.Content,
                Rate = input.Rate,
                Order = await _orderRepository.GetByIdAsync(entityId: input.OrderId)
            };

            return await _feedbackRepository.AddAsync(entity: newFeedback);
        }
    }
}