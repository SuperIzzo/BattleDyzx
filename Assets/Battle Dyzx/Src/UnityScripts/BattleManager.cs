using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleDyzx
{
    /// <summary>
    /// Battle manager is a mono behaviour that sets up the battle scene.
    /// </summary>
    /// <remarks>
    /// It is responsible for setting up the arena the players and AI and
    /// spawning in all dyzx and other objects.
    /// </remarks>
    public class BattleManager : MonoBehaviour
    {
        [SerializeField]
        private Dyzk dyzkPrefab;

        [SerializeField]
        private Arena arenaPrefab;

        [SerializeField]
        private int numDyzx;

        BattleGameState battleState;
        BattleGameDynamics battleDynamics;

        static BattleManager battleManager;
        
        void Start()
        {
            SetupState();            
            SetupDynamics();
        }

        private void SetupState()
        {
            // Create the state
            battleState = new BattleGameState();

            // Create the arena data (testing only)
            float arenaRes = 1024 * 1;

            IArenaReliefTopology reliefTopology;
            reliefTopology = new BumpyGenArenaHeightTopology(arenaRes, arenaRes, 256, 8);
            reliefTopology = new ArenaHeightTopologyMemoizer(reliefTopology);

            IArenaNormalTopology normalTopology;
            normalTopology = new ReliefBasedNormalTopology(reliefTopology);
            battleState.CreateArena(reliefTopology, normalTopology);

            Arena arena = FindObjectOfType<Arena>();
            if (!arena)
            {
                arena = Instantiate(arenaPrefab);
            }
            arena.arenaState = battleState.arena;

            // Create the dyzx data (testing only)
            float halfArena = arenaRes / 2;
            for (int i = 0; i < numDyzx; i++)
            {
                float angle = Math.PI * 0.25f + Math.PI * 2 * i / numDyzx;
                float spawnX = halfArena + Math.Cos(angle) * halfArena;
                float spawnY = halfArena + Math.Sin(angle) * halfArena;
                float spawnZ = arenaRes;

                Vector spawnLocation = new Vector(spawnX, spawnY, spawnZ);
                
                DyzkState dyzkState = battleState.CreateDyzk(null);
                dyzkState.position = spawnLocation;
                dyzkState.RPM = 1000;
                dyzkState.speed = 100;

                Dyzk dyzk = Instantiate(dyzkPrefab);
                dyzk.dyzkState = dyzkState;
            }
        }

        private void SetupDynamics()
        {
            battleDynamics = new BattleGameDynamics();
            battleState.dynamicsTimeStep = Time.fixedDeltaTime;
            battleState.gravity = new Vector(0, 0, -1800);
        }

        void FixedUpdate()
        {
            battleDynamics.Tick(battleState);
        }
    }
}