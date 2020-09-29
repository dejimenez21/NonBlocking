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
        private readonly ServerMode _mode;

        public ImagesController(ImageRepo repo)
        {
            _repo = repo;
            if(Environment.GetCommandLineArgs().Length > 1)
            {
                _mode = Environment.GetCommandLineArgs()[1] == "nb" ? ServerMode.NonBlocking : ServerMode.Blocking;
                System.Console.WriteLine(Environment.GetCommandLineArgs()[1]); 
            }
            else 
                _mode = ServerMode.Blocking;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ImageInfoSummary>>> GetAllImages()
        {
            IEnumerable<ImageInfo> source;
            if(_mode == ServerMode.Blocking)
            {
                source = _repo.GetAllImages(); 
            }
            else
            {
                source = await _repo.GetAllImagesAsync();
            }

            var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<ImageInfo, ImageInfoSummary>();
                    }); 
            IMapper mapper = config.CreateMapper();
            var result = mapper.Map<IEnumerable<ImageInfoSummary>>(source);
            
            return Ok(result);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageInfo>> GetTImageById(int id)
        {
            var image = new ImageInfo();
            var exist = false;
            if(_mode == ServerMode.Blocking)
            {
                exist = _repo.GetImage(id, out image); 
            }
            else
            {
                image = await _repo.GetImageAsync(id);
            }
            
            
            return Ok(image);
        }

        [HttpGet("serverMode")]
        public ActionResult<string> GetServerMode()
        {
            return Ok(_mode.ToString());
        }

    }
}