namespace BattleDyzx
{
    /// <summary> Persistent dyzk data. </summary>
    /// <remarks>
    ///     DyzkData is invariable dyzk information that is true outside of battle.
    /// This data will be generated from a dyzk image.
    /// </remarks>
    [System.Serializable]
    public class DyzkData
    {
        public int id { get; set; }
        public float maxRadius { get; set; }
        public float weight { get; set; }
        public float spike { get; set; }
        public float balance { get; set; }
        public float size { get; set; }
        public float maxSpeed { get; set; }
        public float maxRPM { get; set; }
    }       

    /// <summary> Runtime dyzk state. </summary>
    /// <remarks>
    ///     This data represents the dyzk state at runtime during a battle.
    /// </remarks>
    [System.Serializable]
    public class DyzkState
    {
        public DyzkData dyzkData { get; set; }

        public Vector3D position;
        public Vector3D velocity;
        public Vector3D acceleration;
        public Vector3D control;

        public Vector3D normal = Vector3D.up;
        public float ground = 0.0f;

        public float angle;             // in radians
        public float angularVelocity;   // in radians (per second)

        public float speed;

        public bool isInCollision = false;

        public float RPM
        {
            get { return angularVelocity * 60 / (Math.PI * 2); }
            set { angularVelocity = value / 60 * (Math.PI * 2); }
        }
    }
}