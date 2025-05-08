using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControls : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private CardHand cardHand;

    public bool cursorActive;
    private GameObject selectedObject;

    private void Update()
    {
        if (!cursorActive)
            return;

        //find object under cursor
        RaycastHit hit;
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
            selectedObject = hit.collider.gameObject;

        if (selectedObject != null)
        {
            cardHand.SelectCard(selectedObject.GetComponent<Card>());
        }
    }
}
