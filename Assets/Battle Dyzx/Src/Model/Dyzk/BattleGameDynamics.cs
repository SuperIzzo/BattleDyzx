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

            foreach( DyzkState dyzk in state.dyzx )
            {
                dyzk.angle += dyzk.angularVelocity * dt;

                dyzk.position += dyzk.velocity * dt;
                dyzk.velocity += dyzk.acceleration * dt;

                Vector normalForce = dyzk.normal * dyzk.normal.Dot(dyzk.acceleration);
                dyzk.acceleration = state.gravity;
                dyzk.acceleration -= normalForce;

                dyzk.normal = state.arena.SampleNormal(dyzk.position.x, dyzk.position.y);

                dyzk.acceleration += dyzk.control * dyzk.speed;
            }            
        }
    }
}