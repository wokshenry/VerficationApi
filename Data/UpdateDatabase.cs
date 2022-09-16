using VerficationApi.Model;

namespace VerficationApi.Data
{
    public class UpdateDatabase
    {
        private readonly ApiContext db;
        public UpdateDatabase(ApiContext _db)
        {
            this.db = _db;
        }
        public void SaveEntries()
        {
            TelecomData telecomData = new TelecomData()
            {
                TelecomId=1,
                id = "e0bb93dc-f3fe-430e-625e-08da2cf7fc87",
                firstName = "RONALD",
                middleName= "ZAKAYO",
                surname= "GASHUMBA",
                gender= "Male",
                phoneNumber= "256703787352",
                nin= "CM9105210HLCG",
                status= "Active",
                createdAt= DateTime.Now,
            };
            var exists = db.TelecomData?.FirstOrDefault(o=> o.TelecomId== telecomData.TelecomId);
            if(exists == null)
            {
                db.TelecomData?.Add(telecomData);
                db.SaveChanges();
            }
        }
    }
}
