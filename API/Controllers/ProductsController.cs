using System;
using Core;
using Core.Entitites;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IproductRepository repo) : ControllerBase
{
 

    [HttpGet]
    //returns a list of products
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts()
    {
        return  Ok(await repo.GetProductsAsync());
    }
    [HttpGet("{id:int}")] //api/products/2
    // returns a product with the requested Id, if not found return notFound()
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetProductByIdAsync(id);
        if (product == null) return NotFound();
        return product;
    }

    [HttpPost]
    //create a new product 
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.AddProduct(product);
        if (await repo.SaveChangesAsync())
        {
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }
        return BadRequest("problem creating product");
    }

    [HttpPut("{id:int}")]
    //update a product  if it exists
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !ProductExists(id))
        {
            return BadRequest("Cannot update this product");

        }
        repo.UpdateProduct(product);
        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem updating the product");
    }

    //checks if the product exists in the database 
    private bool ProductExists(int id)
    {
        return repo.ProductExists(id);
    }

    [HttpDelete("{id:int}")]
    //this method deletes a product that matches the given id
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetProductByIdAsync(id);
        if (product == null) return NotFound();

        repo.DeleteProduct(product);
        repo.UpdateProduct(product);
        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem deleting the product");

    }
}
