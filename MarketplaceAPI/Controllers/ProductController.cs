using AutoMapper;
using AutoMapper.QueryableExtensions;
using MarketplaceAPI.Data;
using MarketplaceAPI.DTOs;
using MarketplaceAPI.Entity;
using MarketplaceAPI.Extentions;
using MarketplaceAPI.Helpers;
using MarketplaceAPI.Interfaces;
using MarketplaceAPI.Services;
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
        private readonly IPhotoService photoService;

        public ProductController(DataContext context, IMapper mapper, IPhotoService photoService)
        {
            _context = context;
            this.mapper = mapper;
            this.photoService = photoService;
        }


        [HttpPost]
        public async Task<ActionResult<int>> PostProduct( CreateProductDto createProductDto )
        {
            if (createProductDto == null) return BadRequest();

            var userName = User.GetUserame();
            if(userName == null) return NotFound("user name not found");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if(user == null) return NotFound("user not Found");

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

            if (await _context.SaveChangesAsync() > 0) return Ok(product.Id);

            return BadRequest();
        }


        [HttpPost("AddPhoto/{productId}")]
        public async Task<ActionResult> AddProductPhoto(IFormFile file,int productId)
        {
            if (productId == null) return BadRequest();

            var product = await _context.Products.FirstOrDefaultAsync(u => u.Id == productId);

            if (product == null) return NotFound("product not Found");

            var userName = User.GetUserame();

            if (userName == null) return NotFound("user name not found");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null) return NotFound("user not Found");

            var result = await photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo()
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };

            product.Photos.Add(photo);

            if (await _context.SaveChangesAsync() > 0) return Ok("photo saved");

            return BadRequest();
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] ProductParams productParams)
        {

            var query = _context.Products
                .Include(x => x.Categorie)
                .Include(x => x.Photos)
                .Include(x => x.User)
                .AsQueryable();

            //UserName
            if (productParams.UserName != null)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == productParams.UserName);
                if (user == null) return NotFound("user not Found");
                query = query.Where(x => x.User == user);
            }
            //MinPrice
            if (productParams.MinPrice.HasValue)
                query = query.Where(x => x.Price > productParams.MinPrice);
            //MaxPrice
            if (productParams.MaxPrice.HasValue)
                query = query.Where(x => x.Price < productParams.MaxPrice);
            //Categirie
            if (productParams.Categorie != null)
                query = query.Where(x => x.Categorie.Name == productParams.Categorie);
            //State
            if(productParams.State != null)
                query = query.Where(x => x.User.State == productParams.State);

            var count = query.Count();
            //var items = query.Skip( (productParams.pageNumber -1 ) * productParams.pageSize )
                //.Take(productParams.pageSize);

            //var products = await items.ProjectTo<ProductDto>(mapper.ConfigurationProvider).ToListAsync();

            var products = await PageList<ProductDto>.CreateAsync(
                query.ProjectTo<ProductDto>(mapper.ConfigurationProvider),
                productParams.pageNumber,
                productParams.pageSize
                );

            Response.AddPaginationHeader(
                new PaginationHeader(
                    products.CurrentPage,
                    products.PageSize,
                    products.TotalCount,
                    products.TotalPages)
                );
            return Ok( products );
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProducts(int id)
        {
            var userName = User.GetUserame();
            if (userName == null) return NotFound("user name not found");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null) return NotFound("user not Found");

            return await _context.Products
                .Include(x => x.Categorie)
                .Include(x => x.Photos)
                .Include(x => x.User)
                .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpGet("myproducts")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetmyProducts()
        {
            var userName = User.GetUserame();
            if (userName == null) return NotFound("user name not found");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null) return NotFound("user not Found");

            return await _context.Products
                .Include(x => x.Categorie)
                .Include(x => x.Photos)
                .Where(x=> x.User == user)
                .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, CreateProductDto productDto)
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
