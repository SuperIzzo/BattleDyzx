using System.Collections.Generic;

namespace BattleDyzx
{
    public class BattleGameDynamics
    {        
        public void Tick( BattleGameState state )
        {
            UpdateDyzx( state );
        }

        private void UpdateDyzx( BattleGameState state )
        {
            float dt = state.dynamicsTimeStep;

            // Update physics
            foreach( DyzkState dyzk in state.dyzx )
            {
                dyzk.angle += dyzk.angularVelocity * dt;

                dyzk.position += dyzk.velocity * dt;
                dyzk.velocity += dyzk.acceleration * dt;
                dyzk.velocity *= 0.995f; // friction

                Vector3D normalForce = dyzk.normal * dyzk.normal.Dot(dyzk.acceleration);
                dyzk.acceleration = state.gravity;
                dyzk.acceleration -= normalForce;

                dyzk.normal = state.arena.SampleNormalScaled(dyzk.position.x, dyzk.position.y);
                dyzk.ground = state.arena.SampleElevationScaled(dyzk.position.x, dyzk.position.y);

                dyzk.position.z = Math.Max(dyzk.position.z, dyzk.ground);

                dyzk.acceleration += dyzk.control * dyzk.speed;

                dyzk.isInCollision = false;
            }

            // Detect Collisions
            for (int i=0; i<state.dyzx.Count; i++)
            {
                for (int j = i+1; j < state.dyzx.Count; j++)
                {
                    DyzkState dyzkA = state.dyzx[i];
                    DyzkState dyzkB = state.dyzx[j];

                    // In the first phase we just need to check
                    // circle x circle collision on the XY plane
                    float xDist = dyzkA.position.x - dyzkB.position.x;
                    float yDist = dyzkA.position.y - dyzkB.position.y;
                    float distSquared = xDist * xDist + yDist * yDist;

                    float radiiSquared = dyzkA.dyzkData.maxRadius + dyzkB.dyzkData.maxRadius;
                    radiiSquared *= radiiSquared;
                    
                    if (distSquared > radiiSquared)
                    {
                        // No collision the dyzx are far apart
                        continue;
                    }

                    dyzkA.isInCollision = true;
                    dyzkB.isInCollision = true;
                }
            }
        }
    }
}