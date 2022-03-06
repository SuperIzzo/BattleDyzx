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
        private int numDummies;

        private List<DyzkController> _dyzkControllers = new List<DyzkController>();

        private BattleGameState battleState;
        private BattleGameDynamics battleDynamics;

        private static BattleManager _instance;
        public static BattleManager instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<BattleManager>();
                }
                return _instance;
            }
        }

        public delegate void DyzkControllerEvent(DyzkController dyzkController);
        public event DyzkControllerEvent OnDyzkControllerAdded;

        private void Awake()
        {
            _instance = this;
        }

        private void Start()
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
            reliefTopology = new BumpyGenArenaHeightTopology(arenaRes, arenaRes, 200, 8);
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
            int numDyzx = numPlayers + numAI + numDummies;
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
                    playerController.controllerName = "P" + (i + 1);
                    playerController.controllerId = i;
                    playerController.gamepadId = i;                    
                    AddDyzkController(playerController);
                }
                else if (i < numPlayers + numAI)
                {
                    var aiController = dyzk.gameObject.AddComponent<DyzkAIController>();
                    aiController.controllerId = i;
                    aiController.controllerName = "AI";
                    AddDyzkController(aiController);
                }
            }
        }

        private Dyzk CreateRandomDyzk(Vector3D spawnLocation)
        {
            DyzkDatabase dyzkDB = ConfigManager.dyzkDatabase;

            int dyzkIdx = Random.Range(0, dyzkDB.dyzkTextures.Count);
            Texture2D dyzkTexture = dyzkDB.dyzkTextures[dyzkIdx];

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
            battleState.gravity = new Vector3D(0, 0, -4);
        }

        private void FixedUpdate()
        {
            battleDynamics.Tick(battleState);
        }

        private void AddDyzkController(DyzkController dyzkController)
        {
            while(_dyzkControllers.Count <= dyzkController.controllerId)
            {
                _dyzkControllers.Add(null);
            }

            _dyzkControllers[dyzkController.controllerId] = dyzkController;
            OnDyzkControllerAdded?.Invoke(dyzkController);
        }

        public DyzkController GetDyzkController(int controllerId)
        {
            return controllerId >= 0 && controllerId < _dyzkControllers.Count ? _dyzkControllers[controllerId] : null;
        }
    }
}