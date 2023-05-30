namespace Game.Core.Stats
{
    public enum StatsType
    {
        Mock = 0,
        HoleComplete = 1,
        LongHoleComplete = 2,
        SawComplete = 3,
        FenceComplete = 4
    }

    public class StatisticsContainer
    {
        public StatsType Type;
        public float Amount;

        public StatisticsContainer(StatsType type, float amount)
        {
            Type = type;
            Amount = amount;
        }
    }
}