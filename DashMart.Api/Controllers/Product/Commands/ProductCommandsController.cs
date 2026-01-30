using DashMart.Api.Controllers.Abstraction;
using DashMart.Api.Controllers.Product.Requests;
using DashMart.Application.Products.Commands;
using DashMart.Application.Products.Commands.Category;
using DashMart.Application.Products.Commands.ProductImageCommands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DashMart.Api.Controllers.Product.Commands
{
    [Route("products/")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class ProductCommandsController(IMediator _mediator) : BaseController
    {

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return ToActionResult(result); 

        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteProduct( Guid Id, CancellationToken cancellationToken)
        {
            var command = new DeleteProductCommand(Id);

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }



        [HttpPatch("{Id}/stock/insert")]
        public async Task<IActionResult> InsertProductStockQuantity
            (Guid Id, [FromBody] UpdateStockQuantityRequest request, CancellationToken cancellationToken)
        {
            var command = new InsertProductStockQuantityCommand(Id, request.Quantity);

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }



        [HttpPatch("{Id}/stock/reduce")]
        public async Task<IActionResult> ReduceStockQuantity
            (Guid Id, [FromBody] UpdateStockQuantityRequest request, CancellationToken cancellationToken)
        {
            var command = new ReduceStockQuantityCommand(Id, request.Quantity);

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }



        [HttpPatch("{Id}/details")]
        public async Task<IActionResult> UpdateProductDetails
            (Guid Id, [FromBody] UpdateProductDetailsRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateProductDetailsCommand(Id, request.Name, request.Description, request.HowToUseNote);

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }



        [HttpPatch("{Id}/price")]
        public async Task<IActionResult> UpdateProductPrice
            (Guid Id, [FromBody] UpdateProductPriceRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateProductPriceCommand(Id, request.Price);

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }



        [HttpPatch("{Id}/sku")]
        public async Task<IActionResult> UpdateProductSKU
            (Guid Id, [FromBody] UpdateProductSKURequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateProductSKUCommand(Id, request.SKU);
            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }



        [HttpPatch("{Id}/weight")]
        public async Task<IActionResult> UpdateProductWeight
            (Guid Id, [FromBody] UpdateProductWeightRequest request,  CancellationToken cancellationToken)
        {
            var command = new UpdateProductWeightCommand(Id, request.Weight);

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }



        [HttpPost("{Id}/images")]
        public async Task<IActionResult> AddProductImages
            (Guid Id, [FromBody] AddProductImagesRequest request, CancellationToken cancellationToken)
        {
            var command = new AddProductImagesCommand(Id, request.ImagePaths);
            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }


        [HttpDelete("{Id}/image/{imageId}")]
        public async Task<IActionResult> DeleteProductImage
            (Guid Id, Guid imageId, CancellationToken cancellationToken)
        {
            var request = new DeleteProductImageCommand(Id, imageId);

            var result = await _mediator.Send(request, cancellationToken);

            return ToActionResult(result);
        }


        [HttpPatch("{Id}/image/{imageId}")]
        public async Task<IActionResult> UpdateProductImage
            (Guid Id,Guid imageId, [FromBody] UpdateProductImageRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateProductImageCommand(Id, imageId, request.ImagePath);

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }


        [HttpPost("{Id}/category/{categoryId}")]
        public async Task<IActionResult> AddProductCategory
            (Guid Id, Guid categoryId, CancellationToken cancellationToken)
        {
            var request = new AddProductCategoryCommand(categoryId, Id);

            var result = await _mediator.Send(request, cancellationToken);

            return ToActionResult(result);
        }


        [HttpDelete("{Id}/category/{categoryId}")]
        public async Task<IActionResult> RemoveProductFromCategory
            (Guid Id, Guid categoryId, CancellationToken cancellationToken)
        {
            var request = new RemoveProductFromCategoryCommand(categoryId, Id);

            var result = await _mediator.Send(request, cancellationToken);

            return ToActionResult(result);
        }


        [HttpPut("{Id}/currentCategory/{currentCategoryId}/newCategory/{newCategoryId}")]
        public async Task<IActionResult> UpdateProductCategory
            (Guid Id, Guid currentCategoryId,Guid newCategoryId, CancellationToken cancellationToken)
        {
            var request = new UpdateProductCategoryCommand(Id, currentCategoryId, newCategoryId);

            var result = await _mediator.Send(request, cancellationToken);

            return ToActionResult(result);
        }


    }
}
