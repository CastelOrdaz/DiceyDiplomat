using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversableObject : InteractableObject
{
    [SerializeField] private ConversationManager convoManager;
    [SerializeField] private GameObject[] overworldObjects;
    [SerializeField] private Speaker[] speakers = new Speaker[2];

    public override void Interact(Player player)
    {
        base.Interact(player);

        convoManager.StartConvo(overworldObjects, speakers, speakers[0].DefaultDialogue);
    }
}
