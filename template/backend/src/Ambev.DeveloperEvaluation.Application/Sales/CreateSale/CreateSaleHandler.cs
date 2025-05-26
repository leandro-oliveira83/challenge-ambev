using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Messaging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSaleCommand requests
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _publisher;
    
    /// <summary>
    /// Initializes a new instance of CreateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="productRepository">The product repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="publisher">The EventPublisher instance</param>
    public CreateSaleHandler(
        ISaleRepository saleRepository, 
        IProductRepository productRepository,
        IMapper mapper, 
        IEventPublisher publisher)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _mapper = mapper;
        _publisher = publisher;
    }
    
    /// <summary>
    /// Handles the CreateSaleCommand request
    /// </summary>
    /// <param name="command">The CreateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created user details</returns>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        var sale = new Sale(
            saleNumber: Guid.NewGuid().ToString("N")[..10],
            date: DateTime.UtcNow,
            customerId: command.CustomerId,
            customerName: command.CustomerName,
            branchId: command.BranchId,
            branchName: command.BranchName
        );
        
        foreach (var item in command.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            if (product is null)
            {
                throw new ValidationException([
                    new("Items", $"Product with ID '{item.ProductId}' does not exist in the system.")
                ]);
            }
            
            try
            {
                sale.AddItem(
                    productId: item.ProductId,
                    productName: item.ProductName,
                    quantity: item.Quantity,
                    unitPrice: item.UnitPrice
                );
            }
            catch (Exception ex)
            {
                throw new ValidationException([
                    new("Items", $"Error adding item '{item.ProductName}': {ex.Message}")
                ]);
            }
        }

        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
        
        await _publisher.PublishAsync(new SaleCreatedEvent(sale.Id, sale.SaleNumber, sale.TotalAmount), cancellationToken);
        
        var result = _mapper.Map<CreateSaleResult>(createdSale);
        return result;
    }
    
}