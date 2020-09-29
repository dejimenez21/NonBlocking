namespace ImagesAPI.Models
{
    public class ImageInfo
    {
        public int Id { get; set; }
        public string MimeType { get; set; }
        public string Base64 { get; set; }
        public string Location { get; set; }
        public long Size { get; set; }
    }



    public enum ServerMode
    {
        Blocking,
        NonBlocking
    }

}