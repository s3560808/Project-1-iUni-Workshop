using iUni_Workshop.Models.StateModels;

namespace iUni_Workshop.Data.Seeds
{
    public class StateSeed
    {
        public static State[] states = new State[]{
            new State {Id = 1, Name = "ACT"},
            new State {Id = 2, Name = "NSW"},
            new State {Id = 3, Name = "NT"},
            new State {Id = 4, Name = "QLD"},
            new State {Id = 5, Name = "SA"},
            new State {Id = 6, Name = "TAS"},
            new State {Id = 7, Name = "VIC"},
            new State {Id = 8, Name = "WA"},
            new State {Id = 9, Name = "JBT"}
        };
    }
}