using DormMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DormMS.DTOs;
namespace DormMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly DormMsnContext _context;
        public NewsController(DormMsnContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _context.News
                .Where(x => x.Status == "ACTIVE")
                .OrderByDescending(x => x.IsImportant)
                .ThenByDescending(x => x.CreatedDate)
                .Select(x => new
                {
                    x.NewsId,
                    x.Title,
                    x.Summary,
                    x.CreatedDate,
                    x.IsImportant,
                    CreatedByName = _context.HostelUsers
                        .Where(u => u.UserId == x.CreatedBy)
                        .Select(u => u.Name)
                        .FirstOrDefault()
                })
                .ToList();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public IActionResult GetDetail(int id)
        {
            var data = _context.News
                .Where(x => x.NewsId == id)
                .Select(x => new
                {
                    x.NewsId,
                    x.Title,
                    x.Summary,
                    x.Content,
                    x.IsImportant,
                    x.CreatedDate,
                    CreatedByName = _context.HostelUsers
                        .Where(u => u.UserId == x.CreatedBy)
                        .Select(u => u.Name)
                        .FirstOrDefault()
                })
                .FirstOrDefault();
            if (data == null) return NotFound();
            return Ok(data);
        }

        [HttpPost]
        public IActionResult Create([FromBody] NewsCreateDto dto)
        {
            var news = new News
            {
                Title = dto.Title,
                Summary = dto.Summary,
                Content = dto.Content,
                IsImportant = dto.IsImportant,
                Status = "ACTIVE",
                CreatedBy = 1, // hardcode tạm
                CreatedDate = DateTime.Now,
                Type = "GENERAL"
            };
            _context.News.Add(news);
            _context.SaveChanges();
            return Ok(new { news.NewsId });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] NewsUpdateDto dto)
        {
            var news = _context.News.FirstOrDefault(x => x.NewsId == id);
            if (news == null) return NotFound();

            news.Title = dto.Title;
            news.Summary = dto.Summary;
            news.Content = dto.Content;
            news.IsImportant = dto.IsImportant;
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var news = _context.News.FirstOrDefault(x => x.NewsId == id);
            if (news == null) return NotFound();

            news.Status = "INACTIVE";
            _context.SaveChanges();
            return Ok();
        }
    }

}