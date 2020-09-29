using System;
using System.IO;
using ImagesAPI.Models;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagesAPI.Data
{
    public class ImageRepo
    {
        private readonly string _path;

        public ImageRepo()
        { 
            var root = Directory.GetCurrentDirectory();
            _path = root + "\\Images";
        }

        public IEnumerable<ImageInfo> GetAllImages()
        {
            var Images = new List<ImageInfo>();

            foreach(var filepath in Directory.GetFiles(_path))
            {
                var file = new FileInfo(filepath);
                Images.Add(Parse(file));
            }
            
            return Images;
        }

        public async Task<IEnumerable<ImageInfo>> GetAllImagesAsync()
        {
            
            var tasks = new List<Task<ImageInfo>>();

            foreach(var filepath in Directory.GetFiles(_path))
            {
                var file = new FileInfo(filepath);
                tasks.Add(Task.Run(() => Parse(file)));
            }

            var Images = await Task.WhenAll(tasks);
            
            return Images;
        }

        public bool GetImage(int id, out ImageInfo image)
        {           
            image = new ImageInfo();
        
            var file = new FileInfo(Directory.GetFiles(_path).FirstOrDefault(x=>Path.GetFileNameWithoutExtension(x)==id.ToString()));

            if(!file.Exists){
                return false;
            }
            else{
                image = Parse(file);
                image.Base64 = GetBase64(image);
                return true;
            }
        }

        public async Task<ImageInfo> GetImageAsync(int id)
        {           
            var image = new ImageInfo();
            
            var file = new FileInfo(Directory.GetFiles(_path).FirstOrDefault(x=>Path.GetFileNameWithoutExtension(x)==id.ToString()));
 
            var loadBase64 = File.ReadAllBytesAsync(file.FullName);
            image = Parse(file);
            image.Base64 = Convert.ToBase64String(await loadBase64);
        
            return image;       
        }

        ImageInfo Parse(FileInfo file)
        {
            ImageInfo image = new ImageInfo();
            image.Id = int.Parse(Path.GetFileNameWithoutExtension(file.FullName));
            image.MimeType = "image/" + (file.Extension.Substring(1) == "jpg" ? "jpeg" : file.Extension.Substring(1));
            image.Location = file.FullName;
            image.Size = file.Length;
            
            return image;
        }

        string GetBase64(ImageInfo image)
        {
            var base64 = Convert.ToBase64String(File.ReadAllBytes(image.Location));
            return base64;
        }



    
    }
}