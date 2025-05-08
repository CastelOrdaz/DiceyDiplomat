using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DiceyDialogue.Dialogue;

public class Player : MonoBehaviour
{
    [SerializeField] private CardHand cardHand;
    [SerializeField] private CardData[] cards;
    [SerializeField] PlayerMovement movement;
    [SerializeField] MouseControls mouseControls;
    [SerializeField] ConversationManager convoManager;
    [SerializeField] GameObject PauseScreen, WinScreen, LoseScreen;
    [SerializeField] GameObject DiceyHead;
    [SerializeField] Vector3 headSpin;

    public bool canInteract = true;
    public bool gamePaused = false;
    public bool gameOver = false;

    public bool GamePaused {  get { return gamePaused; } }

    private void Pause()
    {
        gamePaused = !gamePaused;
        PauseScreen.SetActive(gamePaused);

        canInteract = !gamePaused;
        movement.canMove = !gamePaused;
        mouseControls.cursorActive = !gamePaused;

        cardHand.handPaused = gamePaused;
        convoManager.convoPaused = gamePaused;
    }

    public void RestartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void Start()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            cardHand.AddCard(cards[i]);
        }
    }

    public void Update()
    {
        if (convoManager.State == ConvoState.Ended)
        {
            gameOver = true;
            PauseScreen.SetActive(false);
            if (convoManager.Win)
                WinScreen.SetActive(true);
            else
                LoseScreen.SetActive(true);
        }

        if (Input.GetButtonDown("Back") && !gameOver)
            Pause();

        if (!gamePaused && !gameOver)
        {
            DiceyHead.transform.Rotate(headSpin * Time.deltaTime);
        }

        //DebugToggleInteractability();
        //DebugAddCardToHand();
    }

    private void DebugToggleInteractability()
    {
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            canInteract = !canInteract;
            Debug.Log("Player Interaction: " + canInteract);
        }
    }

    private void DebugAddCardToHand()
    {
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            cardHand.AddCard(cards[0]);
        }
    }
}
