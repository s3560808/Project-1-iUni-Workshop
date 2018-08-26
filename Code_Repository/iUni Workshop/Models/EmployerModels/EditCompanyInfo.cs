namespace iUni_Workshop.Models.EmployerModels
{
    public class EditCompanyInfo
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int? PostCode { get; set; }
        public string SuburbName { get; set; }
        public string ContactEmail { get; set; }
        public string BriefDescription { get; set; }
        public string ABN { get; set; }
        public bool? Certificated { get; set; }
        public bool? RequestionCertification { get; set; }
        public string PhoneNumber { get; set; }
    }
}