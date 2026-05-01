using UniRx;
using UnityEngine;

/// <summary>
/// Глобальные переменные и события
/// </summary>
public static class Blackboard
{
    public static ReactiveCommand StartGame = new();
    public static ReactiveCommand EndGame = new();
    public static ReactiveCommand Quit = new();

    public static ReactiveProperty<GameState> GameStateProperty = new();

    //PLAYER
    public static ReactiveProperty<int> LastCheckpointIndex = new();
    public static ReactiveProperty<InteractableBase> SelectedInteractable = new(); //да, грязно!
    public static ReactiveProperty<PlayerState> PlayerStateProperty = new();
}
