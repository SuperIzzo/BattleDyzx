namespace BattleDyzx.Test
{
    public class CombatBase_TEST
    {
        protected Vector3D hitPoint = Vector3D.zero;
        protected Vector3D hitNormal = Vector3D.right;
        protected BattleGameDynamics battleDynamics = new BattleGameDynamics();

        protected DyzkState CreateDefaultDyzk()
        {
            DyzkState dyzk = new DyzkState
            {
                dyzkData = new DyzkData
                {
                    id = -1,            // invalid
                    maxRadius = 0.02f,  // 2cm
                    mass = 0.001f,      // ~10g     
                    saw = 0.1f,         // 10%
                    balance = 1.0f,     // 100%
                },

                angularVelocity = 1000,
            };

            return dyzk;
        }

        protected DyzkState CreateDyzkA()
        {
            DyzkState dyzkA = CreateDefaultDyzk();
            dyzkA.position = hitPoint + Vector3D.left * dyzkA.maxRadius;

            return dyzkA;
        }

        protected DyzkState CreateDyzkB()
        {
            DyzkState dyzkA = CreateDefaultDyzk();
            dyzkA.position = hitPoint + Vector3D.right * dyzkA.maxRadius;

            return dyzkA;
        }
    }
}