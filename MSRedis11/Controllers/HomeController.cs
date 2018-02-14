using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

//
// Can't find
// 1. GetAll method
//

namespace MSRedis11.Controllers
{
    public class Content
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(Id);
                    writer.Write(Text);
                }
                return m.ToArray();
            }
        }

        public static Content Desserialize(byte[] data)
        {
            Content result = new Content();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    result.Id = reader.ReadInt32();
                    result.Text = reader.ReadString();
                }
            }
            return result;
        }
    }
    public class HomeController : Controller
    {
        private readonly IDistributedCache _cache;
        private DistributedCacheEntryOptions _cacheOptions;
        private Content Desserialize(byte[] data)
        {
            Content result = new Content();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    result.Id = reader.ReadInt32();
                    result.Text = reader.ReadString();
                }
            }
            return result;
        }

        public HomeController(IDistributedCache cache)
        {
            this._cache = cache;
            var serverStartTimeString = DateTime.Now.ToString();
            byte[] val = Encoding.UTF8.GetBytes(serverStartTimeString);
            _cacheOptions = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(30));
            //_cacheOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);            
        }

        public IActionResult Index()
        {
            Content content = new Content()
            {
                Id = 5,
                Text = "this is a bunch of text that we want to store"
            };
            _cache.Set("content" + content.Id.ToString(), content.Serialize(), _cacheOptions);

            Content cn = Desserialize(_cache.Get("content" + content.Id.ToString()));
            
            return View();
        }

        [HttpPost]
        [Route("api/Home/Add")]
        public async Task<IActionResult> Add([FromBody]Content newEntry)
        {
            await _cache.SetAsync("content" + newEntry.Id.ToString(), newEntry.Serialize(), _cacheOptions);
            return Ok();
        }

        [Route("api/Home/Seed/{num}/{start}")]
        public async Task<IActionResult> Seed(int num, int start)
        {            
            for (int idx = start; idx < start + num; idx++)
            {
                Content newEntry = new Content()
                {
                    Id = idx,
                    Text = string.Format("this is content for ID {0}", idx),
                };
                await _cache.SetAsync("content" + newEntry.Id.ToString(), newEntry.Serialize(), _cacheOptions);                
            }

            return Ok();
        }

        [Route("api/Home/SeedBigData/{Id}/{numBytes}")]
        public async Task<IActionResult> SeedBigData(int Id, int numBytes)
        {
            StringBuilder sb = new StringBuilder(numBytes);
            for (int idx = 0; idx < numBytes; idx++)
            {
                sb.Append('i');
            }            
            Content newEntry = new Content()
            {
                Id = Id,
                Text = sb.ToString(),
            };
            await _cache.SetAsync("content" + newEntry.Id.ToString(), newEntry.Serialize(), _cacheOptions);            
            return Ok();
        }

        [HttpDelete]
        [Route("api/Home/{Id}")]
        public async Task<IActionResult> Remove(string Id)
        {
            await _cache.RemoveAsync("content" + Id);            
            return NoContent();            
        }
        
        [Route("api/Home/{key}")]
        public async Task<IActionResult> Get(string key)
        {
            try
            {                
                Content cn = Desserialize(await _cache.GetAsync("content" + key));
                return Ok(cn);
            }
            catch(Exception)
            {                
            }
            return NotFound();            
        }

        [Route("api/Home")]
        public IActionResult GetAll()
        {
            // Cannot find an implementation
            return NotFound();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
