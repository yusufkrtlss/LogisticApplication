namespace LogisticMVC.DTOs.Company
{
    public class GetAllCompaniesDTO
    {
        public string id { get; set; }
        public string companyName { get; set; }
        public string taxNumber { get; set; }
        public string address { get; set; }
        public int firmNumber { get; set; }
        //public bool isDeleted { get; set; }
        //public DateTime lastUpdated { get; set; }
        //public string lastUpdatedBy { get; set; }
    }
}
