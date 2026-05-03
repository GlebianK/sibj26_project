using System.Collections;
using UnityEngine;

public class InteractableBreakable : InteractableBase
{
    [Tooltip("0 - default state")]
    [SerializeField] protected GameObject[] objectStates;
    [SerializeField] protected float disappearTimer = 7.5f;

    override protected void Awake()
    {
        base.Awake();

        for (int i = 0; i < objectStates.Length; i++)
        {
            if (i == 0)
                objectStates[i].SetActive(true);
            else
                objectStates[i].SetActive(false);
        }
    }

    override public bool Interact()
    {
        InteractionManager.Instance.CompleteInteraction();

        if (isDebugging)
            Debug.Log("Breakable: <color=yellow>Trying to interact... </color>");

        if (!base.Interact())
            return false;

        if (isDebugging)
            Debug.Log("This is the <color=yellow>InteractableBreakable's</color> Interact method ;)");

        ChangeState();

        return true;
    }

    virtual protected void ChangeState()
    {
        if (isDebugging)
            Debug.Log($"{gameObject.name}: <color=green>changing state here =)</color>");

        foreach (GameObject obj in objectStates)
        {
            obj.SetActive(!obj.activeInHierarchy);
            if (obj.activeInHierarchy && obj.TryGetComponent<Collider>(out Collider col))
            {
                //col.enabled = false;
                StartCoroutine(Disappear((obj, col)));
            }
        }

        if (isDebugging)
            Debug.Log($"Interaction with type {Blackboard.SelectedInteractable.Value.Type}: <color=green>success!</color>");

        IsInteractable = false;
    }

    private IEnumerator Disappear((GameObject objectToDestroy, Collider colToDisappear) data)
    {
        yield return new WaitForSeconds(3f);
        data.colToDisappear.enabled = false;
        yield return new WaitForSeconds(disappearTimer);
        Destroy(data.objectToDestroy);
        yield return null;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
