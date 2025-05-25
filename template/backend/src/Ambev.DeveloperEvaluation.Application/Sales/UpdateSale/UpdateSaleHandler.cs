using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Messaging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Handler for processing UpdateSaleCommand requests
/// </summary>
public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _publisher;
    
    /// <summary>
    /// Initializes a new instance of UpdateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="productRepository">The product repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="publisher">The EventPublisher instance</param>
    public UpdateSaleHandler(
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
    /// Handles the UpdateSaleCommand request
    /// </summary>
    /// <param name="command">The UpdateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated user details</returns>
    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (sale is null)
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

        // Atualiza dados de cliente e filial
        sale.UpdateCustomer(command.CustomerId, command.CustomerName);

        // Identifica produtos enviados na requisição
        var updatedProductIds = command.Items.Select(i => i.ProductId).ToHashSet();

        // Cancela itens que estavam na venda, mas não vieram na requisição
        var cancelledItems = new List<SaleItem>();
        foreach (var existingItem in sale.Items)
        {
            if (!updatedProductIds.Contains(existingItem.ProductId) && !existingItem.IsCancelled)
            {
                existingItem.Cancel();
                cancelledItems.Add(existingItem);
            }
        }

        // Adiciona ou atualiza os itens da venda com regras de desconto
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
                // Cancela item duplicado (caso já exista e não esteja cancelado)
                var alreadyExists = sale.Items.FirstOrDefault(i => i.ProductId == item.ProductId && !i.IsCancelled);
                if (alreadyExists is not null)
                    alreadyExists.Cancel();

                // Reinsere o item atualizado
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

        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        await _publisher.PublishAsync(new SaleModifiedEvent(sale.Id, sale.SaleNumber, sale.TotalAmount), cancellationToken);
        
        foreach (var cancelled in cancelledItems)
        {
            await _publisher.PublishAsync(new ItemCancelledEvent(sale.Id, cancelled.ProductId, cancelled.ProductName), cancellationToken);
        }

        var result = _mapper.Map<UpdateSaleResult>(updatedSale);
        return result;
    }
}