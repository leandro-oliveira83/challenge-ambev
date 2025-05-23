using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

/// <summary>
/// Handler for processing UpdateProductCommand requests
/// </summary>
public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Initializes a new instance of UpdateProductHandler
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public UpdateProductHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Handles the UpdateProductCommand request
    /// </summary>
    /// <param name="command">The UpdateProduct command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created user details</returns>
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateProductCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var product = await _productRepository.GetByIdAsync(command.Id, cancellationToken);
        if (product is null)
            throw new KeyNotFoundException($"Product with ID {command.Id} not found");
        
        var existingProduct = await _productRepository.GetByTitleAsync(command.Title, cancellationToken);
        if (existingProduct is not null && existingProduct.Id != command.Id)
            throw new InvalidOperationException($"Product with title {command.Title} already exists");
        
        product.Change(command.Title, command.Price, command.Description, command.Category, command.Image, command.Rating);
        
        var updatedProduct = await _productRepository.UpdateAsync(product, cancellationToken);
        
        var result = _mapper.Map<UpdateProductResult>(updatedProduct);
        return result;
    }
    
}