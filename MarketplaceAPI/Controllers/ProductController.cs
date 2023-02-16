using AutoMapper;
using AutoMapper.QueryableExtensions;
using MarketplaceAPI.Data;
using MarketplaceAPI.DTOs;
using MarketplaceAPI.Entity;
using MarketplaceAPI.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly DataContext _context;
        private readonly IMapper mapper;

        public ProductController(DataContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }


        [HttpPost]
        public async Task<ActionResult> PostProduct(CreateProductDto createProductDto)
        {
            if (createProductDto == null) return BadRequest();

            var userName = User.GetUserame();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);


            var categorie = GetorCreateCategorie(createProductDto.Categorie);

            var product = new Product()
            {
                Title = createProductDto.Title,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                Created = DateTime.Now,
                Categorie = categorie,
                User = user,
            };

            _context.Products.Add(product);

            if (await _context.SaveChangesAsync() > 0) return Ok("Product saved");

            return BadRequest();
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            return await _context.Products
                .Include(x => x.Categorie)
                .Include(x => x.Photos)
                .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromQuery] CreateProductDto productDto)
        {
            var product = _context.Products.SingleOrDefault(x => x.Id == id);
            if (product == null) return NotFound();

            var categorie = GetorCreateCategorie(productDto.Categorie);

            product.Title = productDto.Title;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.Categorie = categorie;

            if (await _context.SaveChangesAsync() > 0) return Ok();

            return BadRequest();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = _context.Products.SingleOrDefault(x => x.Id == id);
            if (product == null) return NotFound();
            _context.Products.Remove(product);
            if (await _context.SaveChangesAsync() > 0) return Ok();

            return BadRequest();
        }

        private Categorie GetorCreateCategorie(string categorieName)
        {
           var categorie = _context.Categories.FirstOrDefault
                                    (x => x.Name == categorieName.ToLower());

           if (categorie != null) return categorie;

            var NewCategorie = new Categorie { Name = categorieName.ToLower() };
           _context.Categories.Add(NewCategorie);

            return NewCategorie;
        }
    }
}
