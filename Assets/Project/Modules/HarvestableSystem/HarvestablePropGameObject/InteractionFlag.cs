using System;
using UnityEngine;

public class InteractionFlag : MonoBehaviour
{
    [field: SerializeField] public bool canInteract { get; private set; }
    public Action OnInteractFlagUpdate;

    public void TryEnableInteraction()
    {
        canInteract = true;
        OnInteractFlagUpdate?.Invoke();
    }

    public void DisableInteraction()
    {
        canInteract = false;
        OnInteractFlagUpdate?.Invoke();
    }
}