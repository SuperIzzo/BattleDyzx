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
        public float mass { get; set; }
        public float saw { get; set; }
        public float balance { get; set; }
        public float size { get; set; }
        public float maxSpeed { get; set; }
        public float maxRPM { get; set; }
    }

    public struct DyzkCollisionData
    {
        public bool isInCollision;
        public Vector2D preservedForce;
        public Vector2D knockbackForce;
        public Vector2D tangentForce;
        public Vector2D finalForce;
    }

    /// <summary> Runtime dyzk state. </summary>
    /// <remarks>
    ///     This data represents the dyzk state at runtime during a battle.
    /// </remarks>
    [System.Serializable]
    public class DyzkState
    {
        public DyzkData dyzkData { get; set; }
        public int id
        {
            get => dyzkData.id;
            set { dyzkData.id = value; }
        }

        public float maxRadius
        {
            get => dyzkData.maxRadius;
            set { dyzkData.maxRadius = value; } 
        }

        public float saw
        {
            get => dyzkData.saw;
            set { dyzkData.saw = value; }
        }

        public float balance
        {
            get => dyzkData.balance;
            set { dyzkData.balance = value; }
        }

        public float mass
        {
            get => dyzkData.mass;
            set { dyzkData.mass = value; }
        }

        public float size
        {
            get => dyzkData.size;
            set { dyzkData.size = value; }
        }

        public float maxSpeed
        {
            get => dyzkData.maxSpeed;
            set { dyzkData.maxSpeed = value; }
        }

        public float maxRPM
        {
            get => dyzkData.maxRPM;
            set { dyzkData.maxRPM = value; }
        }

        public Vector3D position;
        public Vector3D velocity;
        public Vector3D acceleration;
        public Vector3D control;

        public Vector3D normal = Vector3D.up;
        public Vector3D gravity = Vector3D.down;
        public float ground = 0.0f;

        public float angle;             // in radians
        public float angularVelocity;   // in radians (per second)

        public float speed;

        public DyzkCollisionData collisionDebug;

        public float RPM
        {
            get { return angularVelocity * 60 / (Math.PI * 2); }
            set { angularVelocity = value / 60 * (Math.PI * 2); }
        }
    }
}