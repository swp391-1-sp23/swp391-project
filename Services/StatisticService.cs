using SWP391.Project.Repositories;

namespace SWP391.Project.Services
{
    public interface IStatisticService { }

    public class StatisticService : IStatisticService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public StatisticService(IProductRepository productRepository,
                                IOrderRepository orderRepository,
                                IFeedbackRepository feedbackRepository,
                                IBrandRepository brandRepository,
                                IAccountRepository accountRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _feedbackRepository = feedbackRepository;
            _brandRepository = brandRepository;
            _accountRepository = accountRepository;
        }
    }
}