using GameStore.Domain.Commons;

namespace GameStore.Domain.Entities.Files
{
    public class Image : Auditable
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
    }
}
