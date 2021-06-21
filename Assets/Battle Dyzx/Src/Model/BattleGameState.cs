using System.Collections.Generic;

namespace BattleDyzx
{
    public class BattleGameState
    {
        public List<DyzkState> dyzx = new List<DyzkState>();
        public ArenaState arena;
        public float dynamicsTimeStep;
        public Vector gravity;

        public DyzkState CreateDyzk(DyzkData dyzkData)
        {
            var state = new DyzkState();
            state.dyzkData = dyzkData;
            dyzx.Add(state);
            return state;
        }

        public ArenaState CreateArena(IArenaReliefTopology relief, IArenaNormalTopology normal)
        {
            arena = new ArenaState();
            arena.reliefTopology = relief;
            arena.normalTopology = normal;
            return arena;
        }
    }
}