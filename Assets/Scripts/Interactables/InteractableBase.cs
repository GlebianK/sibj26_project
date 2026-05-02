using UnityEngine;

public class InteractableBase : MonoBehaviour
{
    [SerializeField] protected bool isDebugging;
    [SerializeField] protected InteractableType interactableType;
    [SerializeField] protected PlayerForm allowedForm;

    protected bool isInteractableByHuman;

    public InteractableType Type { get { return interactableType; } }
    public bool IsInteractableByHuman { get { return isInteractableByHuman; } }

    public bool IsInteractable { get; protected set; }

    virtual protected void Awake()
    {
        IsInteractable = true;

        if (allowedForm == PlayerForm.Human)
            isInteractableByHuman = true;
        else
            isInteractableByHuman = false;
    }

    virtual public bool Interact()
    {
        if (!IsInteractable)
            return false;

        if (isDebugging)
            Debug.Log("This is the <color=yellow>base</color> Interact method!");

        return true;
    }

    virtual public bool Interact(GameObject newParent)
    {
        if (!IsInteractable)
            return false;

        if (isDebugging)
            Debug.Log("This is the <color=yellow>base</color> Interact method!");

        return true;
    }
}

[System.Serializable]
public enum InteractableType
{
    None,
    Evironment, // лестницы/уступы/вентил€ции, словом, персонаж подстраиваетс€ под объект
    Item,   // предметы: ключ-карты, бутылки?, проч.
    Movable,    // пресонаж подстраиваетс€ под объект, но объект двигаетс€ (€щики)
    Togglable  // интеракци€ без анимации персонажа (кнопки, компьютеры, рычаги, проч.)
}
