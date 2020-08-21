namespace BattleDyzx
{
    [System.Serializable]
    public class DyzkData
    {
        public int id { get; set; }
        public float radius { get; set; }
        public float weight { get; set; }
        public float spike { get; set; }
        public float balance { get; set; }
    }       

    [System.Serializable]
    public class Dyzk
    {
        public DyzkData dyzkData { get; set; }

        public Vector position;
        public Vector velocity;
        public Vector acceleration;

        public Vector normal;

        public float angle;             // in radians
        public float angularVelocity;   // in radians (per second)
    }
}