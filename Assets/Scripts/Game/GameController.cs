using Az7.Extensions;
using UnityEngine;
using UniRx;
using Az7.UI;
using Cysharp.Threading.Tasks;
using System.Threading;

public class GameController : MonoBehaviour
{
    private const string DEBUG_NAME = "<color=#2495e2>Game Controller</color>";

    public static GameController Instance { get; private set; }

    [SerializeField] private bool _initOnStart = true;

    [Header("Debug")]
    [SerializeField] private bool _skipMenu = true;
    [SerializeField] private int _startCheckpointIndex = 0;

    private void Initialize()
    {
        //Первичные инициализации при загрузке сцены
        //Пробрасывание зависимостей

        Blackboard.StartGame.Subscribe(_ =>
        {
            //if (Blackboard.GameStateProperty.Value == GameState.Prepared)
            StartFromLastCheckpoint(Application.exitCancellationToken).Forget();
        }).AddTo(this);

        Blackboard.EndGame.Subscribe(_ =>
        {
            if (Blackboard.GameStateProperty.Value == GameState.Running)
                EndGameAsync(Application.exitCancellationToken).Forget();
        }).AddTo(this);

        Blackboard.Quit.Subscribe(_ =>
        {
            QuitGame();
        }).AddTo(this);

        DebugMessage("Initialized, prepare new game");
        PrepareNewGameAsync(Application.exitCancellationToken).Forget();
    }

    private async UniTaskVoid PrepareNewGameAsync(CancellationToken token)
    {
        //Подготовка и конфигурация сцены, обновление состояний

        CheckpointsController.Instance.ResetAllPoints();
        Blackboard.LastCheckpointIndex.Value = _startCheckpointIndex;

        if (_skipMenu)
        {
            CurtainSingle.Instance.HideImmidiate();
            UI_ControllerSingle.Instance.ShowViewImmidiate(UI_ViewKey.InteractablesHint);
            var playerSpawnPosition = CheckpointsController.Instance.GetSpawnPosition();
            PlayerController.Instance.Setup(playerSpawnPosition);
            Blackboard.GameStateProperty.Value = GameState.Running;
            PlayerController.Instance.AllowMovement = true;
            return;
        }

        //Показываем главное меню
        UI_ControllerSingle.Instance.ShowViewImmidiate(UI_ViewKey.MainMenu);

        //Открываем занавес
        await CurtainSingle.Instance.HideAsync(token);

        Blackboard.GameStateProperty.Value = GameState.Prepared;

        DebugMessage("New game prepared");
    }

    private async UniTaskVoid StartFromLastCheckpoint(CancellationToken token)
    {
        //Выключаем управление
        PlayerController.Instance.AllowMovement = false;

        //Закрываем занавес
        await CurtainSingle.Instance.ShowAsync(token);

        //Типа идёт загрузка

        //Скрываем главное меню
        UI_ControllerSingle.Instance.HideViewImmidiate(UI_ViewKey.MainMenu);
        UI_ControllerSingle.Instance.ShowViewImmidiate(UI_ViewKey.InteractablesHint);

        //Перемещаем плаера
        var playerSpawnPosition = CheckpointsController.Instance.GetSpawnPosition();
        PlayerController.Instance.Setup(playerSpawnPosition);
        PlayerController.Instance.AllowMovement = true;

        //Открываем занавес
        await CurtainSingle.Instance.HideAsync(token);

        //Включаем управление

        Blackboard.GameStateProperty.Value = GameState.Running;

        DebugMessage("New game started");
    }

    private async UniTaskVoid EndGameAsync(CancellationToken token)
    {
        Blackboard.GameStateProperty.Value = GameState.None;

        //Выключаем игроку управление

        //Процедуры по завершению игры
        //Большая надпись YOU DIED или типа того

        //Закрываем занавес
        await CurtainSingle.Instance.ShowAsync(token);

        DebugMessage("Game ended, prepare new game");
        PrepareNewGameAsync(Application.exitCancellationToken).Forget();
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning($"Instance of {GetType()} already created!");
        }
    }

    private void Start()
    {
        if (_initOnStart) Initialize();
    }

    private void DebugMessage(string message)
    {
        Debug.Log($"{DEBUG_NAME}: {message}");
    }
}
