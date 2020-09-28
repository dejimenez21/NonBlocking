using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ImagesAPI.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using AutoMapper;
using ImagesAPI.Dtos;
using ImagesAPI.Data;
using Microsoft.AspNetCore.Cors;

namespace ImagesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : Controller
    {
        private readonly ImageRepo _repo;

        public ImagesController(ImageRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("")]
        public ActionResult<IEnumerable<ImageInfoSummary>> GetAllImages()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<ImageInfo, ImageInfoSummary>();
                }); 

            IMapper mapper = config.CreateMapper();
            var source = _repo.GetAllImages();
            var result = mapper.Map<IEnumerable<ImageInfoSummary>>(source);
           
            return Ok(result);
        }

        
        [HttpGet("{id}")]
        public ActionResult<ImageInfo> GetTImageById(int id)
        {
            var image = new ImageInfo();
            var exist = _repo.GetImage(id, out image);
            return Ok(image);
        }

        // [HttpPost("")]
        // public ActionResult<TModel> PostTModel(TModel model)
        // {
        //     return null;
        // }

        // [HttpPut("{id}")]
        // public IActionResult PutTModel(int id, TModel model)
        // {
        //     return NoContent();
        // }

        // [HttpDelete("{id}")]
        // public ActionResult<TModel> DeleteTModelById(int id)
        // {
        //     return null;
        // }
    }
}