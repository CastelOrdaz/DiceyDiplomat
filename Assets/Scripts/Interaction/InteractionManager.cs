using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float minInteractDist, maxInteractDist;

    private InteractableObject target;

    private void SetTarget(InteractableObject newTarget)
    {
        if (target == newTarget)
            return;

        //untarget current target
        if (target != null)
        {
            target.ToggleTargeted(false);
        }

        //target new target
        if (newTarget != null)
        {
            newTarget.ToggleTargeted(true);
        }

        target = newTarget;
    }

    private void ClearFarTarget()
    {
        if (target == null)
            return;

        if (Vector3.Distance(transform.position, target.transform.position) > maxInteractDist)
        {
            SetTarget(null);
        }
    }

    private void InteractWithTarget()
    {
        if (!player.canInteract || target == null)
            return;

        if (Input.GetButtonDown("Interact"))
        {
            target.Interact(player);
        }
    }

    private void LookForInteractables()
    {
        if (!player.canInteract)
            return;

        //collect colliders within interaction distance
        Collider[] hits = Physics.OverlapSphere(transform.position, minInteractDist);
        if (hits.Length == 0)
            return;

        for (int i = 0; i < hits.Length; i++)
        {
            //skip object its its not an intreactable object
            if (hits[i].tag != "Interactable")
                continue;

            InteractableObject interactObj = hits[i].GetComponent<InteractableObject>();
            //skip object if its not able to be interacted with
            if (!interactObj.IsInteractable())
                continue;

            //set object to target if there is no current target
            if (target == null)
            {
                SetTarget(interactObj);
                continue;
            }

            //set object to target if its closer than current target
            if (Vector3.Distance(transform.position, interactObj.transform.position) < Vector3.Distance(transform.position, target.transform.position))
            {
                SetTarget(interactObj);
            }
        }
    }

    private void Update()
    {
        ClearFarTarget();
        InteractWithTarget();
    }

    private void FixedUpdate()
    {
        LookForInteractables();
    }
}
