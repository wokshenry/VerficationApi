using System.Xml.Linq;

namespace VerficationApi.Model
{
    public class RequestData
    {
        public int Id { get; set; }
        public string? referenceId { get; set; }
        public string? nin { get; set; }
        public string? phoneNumber { get; set; }
        public DateTime? processedAt { get; set; }
    }
}
