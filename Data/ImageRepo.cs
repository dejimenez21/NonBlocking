using System;
using System.IO;
using ImagesAPI.Models;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Linq;

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

        public List<ImageInfo> GetAllImages()
        {
            var Images = new List<ImageInfo>();
            foreach(var filepath in Directory.GetFiles(_path))
            {
                var file = new FileInfo(filepath);
                Images.Add(Parse(file));
            }
            //System.Console.WriteLine(Environment.GetCommandLineArgs()[1]);
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
                return true;
            }
        }

        ImageInfo Parse(FileInfo file)
        {
            ImageInfo image = new ImageInfo();
            image.Id = int.Parse(Path.GetFileNameWithoutExtension(file.FullName));
            image.MimeType = "image/" + (file.Extension.Substring(1) == "jpg" ? "jpeg" : file.Extension.Substring(1));
            image.Base64 = Convert.ToBase64String(File.ReadAllBytes(file.FullName));

            return image;
        }
    }
}