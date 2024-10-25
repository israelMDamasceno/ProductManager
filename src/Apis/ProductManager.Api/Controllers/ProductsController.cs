using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManager.Application.Commands;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Interfaces;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IRepository<Product> _repository;
    private readonly IMediator _mediator;

    public ProductController(IRepository<Product> repository, IMediator mediator)
    {
        this._mediator = mediator;
        this._repository = repository;
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var products = await _repository.GetAll(pageNumber, pageSize);
        if (!products.Any())
            return NotFound("No products found");
        return Ok(products);
    }

    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        var product = await _repository.Get(id);
        if (product == null)
            return NotFound("Product not found");
        return Ok(product);
    }

    [HttpGet("search")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByName([FromQuery] string name)
    {
        var products = await _repository.GetByName(name);
        if (!products.Any())
            return NotFound("No products found with that name");
        return Ok(products);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] CreatedProductCommand command)
    {
        if (command == null)
            return BadRequest("Invalid product data");
        
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateProductCommand command)
    {
        if (command == null || id != command.Id)
            return BadRequest("Invalid product data");
        
        var product = await _repository.Get(id);
        
        if (product == null)
            return NotFound("Product not found");
        
        command.DateUpdate = DateTime.Now;
        
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _repository.Get(id);
        if (product == null)
            return NotFound("Product not found");
        var obj = new DeletedProductCommand { Id = id };
        var result = await _mediator.Send(obj);
        return Ok(result);
    }
}
