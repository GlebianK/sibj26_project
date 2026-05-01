using UnityEngine;

public class InteractableBase : MonoBehaviour
{
    [SerializeField] protected bool isDebugging;
    [SerializeField] protected InteractableType interactableType;

    public InteractableType Type { get { return interactableType; } }

    public bool IsInteractable { get; private set; }

    virtual protected void Awake()
    {
        IsInteractable = true;
    }

    virtual public void Interact()
    {
        if (!IsInteractable)
            return;

        if (isDebugging)
            Debug.Log("This is the <color=yellow>base</color> Interact method!");
    }

    [System.Serializable]
    public enum InteractableType
    {
        Evironment, // лестницы/уступы/вентил€ции, словом, персонаж подстраиваетс€ под объект
        Item,   // предметы: ключ-карты, бутылки?, проч.
        Movable,    // пресонаж подстраиваетс€ под объект, но объект двигаетс€ (€щики)
        Togglable  // интеракци€ без анимации персонажа (кнопки, компьютеры, рычаги, проч.)
    }
}
