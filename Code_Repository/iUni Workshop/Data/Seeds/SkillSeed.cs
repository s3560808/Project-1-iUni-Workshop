using iUni_Workshop.Models.JobRelatedModels;

namespace iUni_Workshop.Data.Seeds
{
    public class SkillSeed
    {
        public static Skill[] skills = new Skill[]
        {
            new Skill{ Id = 1,Name = "Java", NormalizedName = "Java".ToUpper(), Status = SkillStatus.InUse},
            new Skill{ Id = 2,Name = "JavaScript", NormalizedName = "JavaScript".ToUpper(), Status = SkillStatus.InUse},
            new Skill{ Id = 3,Name = "C", NormalizedName = "C".ToUpper(), Status = SkillStatus.InUse},
            new Skill{ Id = 4,Name = "C++", NormalizedName = "C++".ToUpper(), Status = SkillStatus.InUse},
            new Skill{ Id = 5,Name = "C#", NormalizedName = "C#".ToUpper(), Status = SkillStatus.InUse},
            new Skill{ Id = 6,Name = "SQL", NormalizedName = "SQL".ToUpper(), Status = SkillStatus.InUse},
            new Skill{ Id = 7,Name = "PHP", NormalizedName = "PHP".ToUpper(), Status = SkillStatus.InUse},
            new Skill{ Id = 8,Name = "CSS", NormalizedName = "CSS".ToUpper(), Status = SkillStatus.InUse},
            new Skill{ Id = 9,Name = "HTML", NormalizedName = "HTML".ToUpper(), Status = SkillStatus.InUse},
            new Skill{ Id = 10, Name = "Asp.Net", NormalizedName = "Asp.Net".ToUpper(), Status = SkillStatus.InUse},
            new Skill{ Id = 11,Name = "Laravel", NormalizedName = "Laravel".ToUpper(), Status = SkillStatus.InUse},
            new Skill{ Id = 12,Name = "Request1", NormalizedName = "Request1".ToUpper(), Status = SkillStatus.InRequest},
            new Skill{ Id = 13,Name = "Request2", NormalizedName = "Request2".ToUpper(), Status = SkillStatus.InRequest},
            new Skill{ Id = 14,Name = "NoLonger1", NormalizedName = "NoLonger1".ToUpper(), Status = SkillStatus.NoLongerUsed},
            new Skill{ Id = 15,Name = "NoLonger2", NormalizedName = "NoLonger2".ToUpper(), Status = SkillStatus.NoLongerUsed}
        };
    }
}