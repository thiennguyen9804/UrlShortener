using IdGen;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Base64Extension;
using UrlShortener.Context;
using UrlShortener.Models;

namespace UrlShortener.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class UrlShortenController : ControllerBase
    {
        private readonly UrlContext _context;
        private readonly IIdGenerator<long> _generator;
        private const long Base62Min7Chars = 56_800_235_584L;

        public UrlShortenController(UrlContext context, IIdGenerator<long> generator)
        {
            _context = context;
            _generator = generator;
        }

        [HttpPost("data/shorten")]
        public async Task<IActionResult> Shorten([FromQuery] string longUrl)
        {
            long id = _generator.CreateId();
            string idBase62 = id.ToBase62();
            var domain = $"{Request.Scheme}://{Request.Host}";
            var shortenedUrl = $"{domain}/api/v1/{idBase62}";
            Url url = new()
            {
                Id = id,
                LongUrl = longUrl,
                ShortUrl = shortenedUrl
            };
            _context.Urls.Add(url);
            _context.SaveChangesAsync();
            return Ok(idBase62);
        }

        [HttpGet("{idBase62}")]
        public async Task<IActionResult> Access([FromRoute] string idBase62)
        {
            var id = Base62Extension.FromBase62(idBase62);
            var urlEntity = await _context.Urls.FindAsync(id);
            if(urlEntity == null)
            {
                return NotFound();
            }
            var longUrl = urlEntity.LongUrl;
            return RedirectPermanent(longUrl);
        }
    }
}
