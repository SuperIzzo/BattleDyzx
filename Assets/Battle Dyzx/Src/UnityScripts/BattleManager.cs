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
        private int numPlayers;

        [SerializeField]
        private int numAI;

        [SerializeField]
        private DyzkDatabase dyzkDatabase;

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
            reliefTopology = new BumpyGenArenaHeightTopology(arenaRes, arenaRes, 100, 8);
            reliefTopology = new ArenaHeightTopologyMemoizer(reliefTopology);

            IArenaNormalTopology normalTopology;
            normalTopology = new ReliefBasedNormalTopology(reliefTopology);
            battleState.CreateArena(reliefTopology, normalTopology, 1.0f);

            Arena arena = FindObjectOfType<Arena>();
            if (!arena)
            {
                arena = Instantiate(arenaPrefab);
            }
            arena.arenaState = battleState.arena;
            
            float arenaHalfWidth = battleState.arena.width * 0.5f;
            float arenaHalfHeight = battleState.arena.width * 0.5f;
            float arenaDepth = battleState.arena.depth;

            // Create the dyzx data (testing only)
            int numDyzx = numPlayers + numAI;
            for (int i = 0; i < numDyzx; i++)
            {
                float angle = Math.PI * 0.25f + Math.PI * 2 * i / numDyzx;
                float spawnX = arenaHalfWidth + Math.Cos(angle) * arenaHalfWidth;
                float spawnY = arenaHalfHeight + Math.Sin(angle) * arenaHalfHeight;
                float spawnZ = arenaDepth;

                Vector3D spawnLocation = new Vector3D(spawnX, spawnY, spawnZ);

                Dyzk dyzk = CreateRandomDyzk(spawnLocation);

                if (i < numPlayers)
                {
                    var playerController = dyzk.gameObject.AddComponent<DyzkPlayerController>();
                    playerController.controllerId = i;
                }
                else
                {
                    var aiController = dyzk.gameObject.AddComponent<DyzkAIController>();
                }    
            }
        }

        Dyzk CreateRandomDyzk(Vector3D spawnLocation)
        {
            int dyzkIdx = Random.Range(0, dyzkDatabase.dyzkTextures.Count);
            Texture2D dyzkTexture = dyzkDatabase.dyzkTextures[dyzkIdx];

            IImageData imageData = new Texture2DImageData(dyzkTexture);
            DyzkImageAnalysis analysis = new DyzkImageAnalysis(imageData, 0.05f);

            DyzkData dyzkData = analysis.CreateDyzkData();
            DyzkState dyzkState = battleState.CreateDyzk(dyzkData);            
            dyzkState.position = spawnLocation;
            dyzkState.speed = dyzkData.maxSpeed;
            dyzkState.RPM = dyzkData.maxRPM;

            Dyzk dyzk = Instantiate(dyzkPrefab);
            dyzk.dyzkState = dyzkState;
            dyzk.dyzkTexture = dyzkTexture;

            return dyzk;
        }

        private void SetupDynamics()
        {
            battleDynamics = new BattleGameDynamics();
            battleState.dynamicsTimeStep = Time.fixedDeltaTime;
            battleState.gravity = new Vector3D(0, 0, -1800);
        }

        void FixedUpdate()
        {
            battleDynamics.Tick(battleState);
        }
    }
}