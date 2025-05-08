using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer InteractPrompt;

    [SerializeField] protected float InteractCooldown;
    protected float interactCooldownTime;

    private bool interactable = true;
    private bool targeted = false;

    public bool IsInteractable()
    {
        return interactable;
    }

    public void ToggleTargeted(bool state)
    {
        targeted = state;
    }

    public virtual void Interact(Player player)
    {
        interactCooldownTime = InteractCooldown;
    }

    protected void DisplayPrompt()
    {
        InteractPrompt.enabled = interactable && targeted;
    }

    protected void UpdateInteractCooldown()
    {
        if (interactCooldownTime > 0)
        {
            interactable = false;

            interactCooldownTime -= Time.deltaTime;
            if (interactCooldownTime <= 0)
            {
                InteractCooldownEnd();
            }
        }
    }

    protected virtual void InteractCooldownEnd()
    {
        interactable = true;
    }

    public void Update()
    {
        UpdateInteractCooldown();
        DisplayPrompt();

        //DebugToggleInteractability();
    }

    private void DebugToggleInteractability()
    {
        if (interactable && Input.GetKeyDown(KeyCode.Alpha9))
        {
            interactable = false;
            Debug.Log(gameObject.name + "is intractable");
        }
        if (!interactable && Input.GetKeyDown(KeyCode.Alpha0))
        {
            interactable = true;
            Debug.Log(gameObject.name + "is not intractable");
        }
    }
}
