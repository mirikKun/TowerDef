using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField] private Vector2Int _boardSize;
    [SerializeField] private GameBoard _board;
    [SerializeField] private GameTileContentFactory _contentFactory;
    [SerializeField] private EnemyFactory _enemyFactory;
    [SerializeField] private WarFactory _warFactory;

    [SerializeField] private GameScenario _scenario;
    [SerializeField, Range(10, 100)] private int _startingPlayerHealth = 100;

    private int _currentPlayerHealth;
    [SerializeField, Range(5, 30)] private float _prepareTime = 10;
    private bool _scenarioInProgress;
    private GameScenario.State _activeScenario;
    private GameBehaviorCollection _enemies = new GameBehaviorCollection();
    private GameBehaviorCollection _nonEnemies = new GameBehaviorCollection();

    [SerializeField] private Camera _camera;
    private Ray touchRay => _camera.ScreenPointToRay(Input.mousePosition);

    private TowerType _currentTowerType;
    private static Game _inctance;
    private bool _isPaused;

    private void OnEnable()
    {
        _inctance = this;
    }

    private void Start()
    {
        _board.Initialize(_boardSize, _contentFactory);
        BeginNewGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isPaused = !_isPaused;
            Time.timeScale = _isPaused ? 0 : 1;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            BeginNewGame();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentTowerType = TowerType.Laser;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentTowerType = TowerType.Mortar;
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            AlternativeHandleTouch();
        }

        if (_scenarioInProgress)
        {
            if (_currentPlayerHealth <= 0)
            {
                Debug.Log("Defeated");
                BeginNewGame();
            }

            if (!_activeScenario.Progress() && _enemies.IsEmpty)
            {
                BeginNewGame();

            }
        }


        Physics.SyncTransforms();
        _enemies.GameUpdate();
        _nonEnemies.GameUpdate();
        _board.GameUpdate();
    }

    public static void SpawnEnemy(EnemyFactory factory, EnemyType type)
    {
        GameTile spawnPoint = _inctance._board.GetSpawnPoint(Random.Range(0, _inctance._board.SpawnPointCount));
        Enemy enemy = factory.Get(type);
        enemy.SpawnOn(spawnPoint);
        _inctance._enemies.Add(enemy);
    }

    private void AlternativeHandleTouch()
    {
        GameTile tile = _board.GetTile(touchRay);
        if (tile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _board.ToggleSpawnPoint(tile);
            }
            else
            {
                _board.ToggleDestination(tile);
            }
        }
    }

    private void HandleTouch()
    {
        GameTile tile = _board.GetTile(touchRay);
        if (tile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _board.ToggleTower(tile, _currentTowerType);
            }
            else
            {
                _board.ToggleWall(tile);
            }
        }
    }

    public static Shell SpawnShell()
    {
        Shell shell = _inctance._warFactory.GetShell;
        _inctance._nonEnemies.Add(shell);
        return shell;
    }

    public static Explosion SpawnExplosion()
    {
        Explosion explosion = _inctance._warFactory.GetExplosion;
        _inctance._nonEnemies.Add(explosion);
        return explosion;
    }

    private void BeginNewGame()
    {
        _scenarioInProgress = false;
        if (_prepareRoutine != null)
        {
            StopCoroutine(_prepareRoutine);
        }

        _enemies.Clear();
        _nonEnemies.Clear();
        _board.Clear();
        _currentPlayerHealth = _startingPlayerHealth;
        _prepareRoutine = StartCoroutine(PrepareRoutine());
    }

    public static void EnemyReachDestination()
    {
        _inctance._currentPlayerHealth--;
    }

    private Coroutine _prepareRoutine;

    private IEnumerator PrepareRoutine()
    {
        yield return new WaitForSeconds(_prepareTime);
        _activeScenario = _scenario.Begin;
        _scenarioInProgress = true;
    }
}