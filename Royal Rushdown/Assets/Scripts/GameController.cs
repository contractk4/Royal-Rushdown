﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	// Use this for initialization
    public static float globalSpeed;
    public enum GameState
    {
        PreGame, InGame, PostGame
    }
   
    public static GameState gameState;
    public EventSystem eventSystem;
    
    public GameObject restartButton;
    private Text restartText;
    public GameObject mainMenuButton;
    private Text mainMenuText;
    public Font defaultFont;
    public Font activeFont;
    private GameObject[] deathScreenUI;
    private void Awake()
    {
        globalSpeed = 1f;
        gameState = GameState.InGame;
    }
    void Start () {
        restartText = restartButton.transform.parent.GetComponent<Text>();
        mainMenuText = mainMenuButton.transform.parent.GetComponent<Text>();
        deathScreenUI = GameObject.FindGameObjectsWithTag("DeathScreenUI");
        
	}
	
	// Update is called once per frame
	void Update () {
        switch (gameState)
        {
            case (GameState.PreGame):
                break;
            case (GameState.InGame):
                break;
            case (GameState.PostGame):
                updateDeathScreen();
                break;
        }
	}
    public void revealDeathScreen()
    {
        eventSystem.SetSelectedGameObject(restartButton);
        for(int i = 0; i < deathScreenUI.Length; ++i)
        {
            deathScreenUI[i].GetComponent<GraphicColorLerp>().startColorChange();
        }
    }
    public void updateDeathScreen()
    {
        if(eventSystem.currentSelectedGameObject == restartButton)
        {
            restartText.font = activeFont; 
        } else
        {
            restartText.font = defaultFont;
        }
        if(eventSystem.currentSelectedGameObject == mainMenuButton)
        {
            mainMenuText.font = activeFont;
        } else
        {
            mainMenuText.font = defaultFont;
        }
    }
    public void restartButtonPress()
    {
        SceneManager.LoadScene("GameplayScene");
    }
    public void mainMenuButtonPress()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public int getGameState()
    {
        return (int)gameState;
    }
    public void setGameState(int state)
    {
        gameState = (GameState)state;
        switch ((GameState)state)
        {
            case (GameState.PreGame):
                break;
            case (GameState.InGame):
                break;
            case (GameState.PostGame):
                revealDeathScreen();
                break;
        }
    }
}
