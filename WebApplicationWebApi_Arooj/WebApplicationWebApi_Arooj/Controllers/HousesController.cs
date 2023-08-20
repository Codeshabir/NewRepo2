using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationWebApi_Arooj.Data;
using WebApplicationWebApi_Arooj.Model;
using Microsoft.AspNetCore.Hosting;
using System.IO;



namespace WebApplicationWebApi_Arooj.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HousesController : ControllerBase
    {
        private readonly HouseRentingDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public HousesController(HouseRentingDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<House>>> GetHouses()
        {
            return await _context.Houses.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<House>> GetHouse(int id)
        {
            var house = await _context.Houses.FindAsync(id);

            if (house == null)
            {
                return NotFound();
            }

            return house;
        }

      


        private IFormFile CreateIFormFileFromBase64(string base64String, string fileName)
        {
            byte[] fileBytes = Convert.FromBase64String(base64String);
            return new FormFile(new MemoryStream(fileBytes), 0, fileBytes.Length, null, fileName);
        }


        [HttpPost]
        public async Task<ActionResult<House>> PostHouse(House houseDto)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(houseDto.Title) && !string.IsNullOrEmpty(houseDto.Description))
                {
                    string clientImagesFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "..", "Client", "wwwroot", "images");

                    List<string> imageUrls = new List<string>();

                    IFormFile formFile = CreateIFormFileFromBase64(houseDto.ImageInBase64, "dskdj.jpg");

                    var fileUpload = formFile;

                    if (!Directory.Exists(clientImagesFolder))
                    {
                        Directory.CreateDirectory(clientImagesFolder);
                    }

                    var fileName = Path.GetFileName(fileUpload.FileName);
                    fileName = fileName.Replace("'", "");
                    fileName = fileName.Replace(" ", "");
                    string fullFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + fileName;
                    string filePath = Path.Combine(clientImagesFolder, fullFileName);

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await fileUpload.CopyToAsync(fileStream);
                        }
                    }

                    imageUrls.Add("/images/" + fullFileName);

                    var house = new House()
                    {
                        Title = houseDto.Title,
                        Description = houseDto.Description,
                        Images = string.Join(",", imageUrls), // Store the list of image URLs as a comma-separated string
                        Rent = houseDto.Rent,
                        CategoryId = houseDto.CategoryId,
                        ImageInBase64 = "true"// Set the selected category
                    };

                    await _context.Houses.AddAsync(house);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetHouse", new { id = house.Id }, house);
                }
            }
            return BadRequest();
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> PutHouse(int id, House house)
        {
            if (id != house.Id)
            {
                return BadRequest();
            }

            _context.Entry(house).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HouseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHouse(int id)
        {
            var house = await _context.Houses.FindAsync(id);
            if (house == null)
            {
                return NotFound();
            }

            _context.Houses.Remove(house);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HouseExists(int id)
        {
            return _context.Houses.Any(e => e.Id == id);
        }


        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }
    }
}
        