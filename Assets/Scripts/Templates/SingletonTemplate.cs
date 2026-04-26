using UnityEngine;

/// <summary>
/// Шаблон для синглтонов
/// </summary>
public class SingletonTemplate : MonoBehaviour
{
    public static SingletonTemplate Instance { get; private set; }

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
}
