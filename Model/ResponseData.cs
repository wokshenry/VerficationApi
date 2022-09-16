namespace VerficationApi.Model
{
    public class ResponseData
    {
        public int ResponseId { get; set; }
        public string? firstName { get; set; }
        public string? middleName { get; set; }
        public string? surname { get; set; }
        public string? gender { get; set; }
        public string? phoneNumber { get; set; }
        public string? nin { get; set; }
        public string? status { get; set; }
        public string? referenceId { get; set; }
        public DateTime? processedAt { get; set; }
        public int? errorCode { get; set; }
        public string? errorMessage { get; set; }
        public string? PhoneNumberValidity { get; set; }
        public string? id { get; set; }
        public DateTime? createdAt { get; set; }
    }
}
