using Application.Common.Interfaces.Perception;
using Domain.Common.Errors;
using Domain.ReceiptAggregate;

namespace Application.Receipts.Queries;

public class GetReceiptHandler(
    IReceiptRepository receiptRepository,
    IUserRepository userRepository
) : IRequestHandler<GetReceiptQuery, ErrorOr<Receipt>>
{
    private readonly IReceiptRepository _receiptRepository = receiptRepository;
    private readonly IUserRepository _userRepository = userRepository;
    
    public async Task<ErrorOr<Receipt>> Handle(GetReceiptQuery query, CancellationToken cancellationToken)
    {
        var (userId, receiptId) = query;

        if (await _userRepository.CheckExistenceAsync(userId, cancellationToken) == false)
            return Errors.User.NotFound;
        
        Receipt? receipt = await _receiptRepository.GetAsync(receiptId, cancellationToken);
        if (receipt is null)
            return Errors.Receipt.NotFound;

        return receipt;
    }
}