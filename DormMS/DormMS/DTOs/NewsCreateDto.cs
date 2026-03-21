namespace DormMS.DTOs
{
    public class NewsCreateDto
    {
        public string Title { get; set; }
        public string? Summary { get; set; }
        public string Content { get; set; }
        public bool IsImportant { get; set; }
    }
}
