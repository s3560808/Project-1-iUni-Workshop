using iUni_Workshop.Models.SchoolModels;

namespace iUni_Workshop.Data.Seeds
{
    public class SchoolSeed
    {
        public static School[] schools = new School[]
        {
            new School{Id = 1, SchoolName = "RMIT", NormalizedName = "RMIT".ToUpper(), DomainExtension = "rmit.edu.au", SuburbId = 1, Status = SchoolStatus.InUse},
            new School{Id = 2, SchoolName = "RMIT", NormalizedName = "RMIT".ToUpper(), DomainExtension = "rmit.edu.au", SuburbId = 2, Status = SchoolStatus.InUse},
            new School{Id = 3, SchoolName = "Melbourne University", NormalizedName = "Melbourne University".ToUpper(), DomainExtension = "unimelb.edu.au", SuburbId = 3, Status = SchoolStatus.InUse},
            new School{Id = 4, SchoolName = "Melbourne University", NormalizedName = "Melbourne University".ToUpper(), DomainExtension = "unimelb.edu.au", SuburbId = 5, Status = SchoolStatus.InRequest},
            new School{Id = 5, SchoolName = "Melbourne University", NormalizedName = "Melbourne University".ToUpper(), DomainExtension = "unimelb.edu.au", SuburbId = 6, Status = SchoolStatus.NoLongerUsed},
        };
    }
}