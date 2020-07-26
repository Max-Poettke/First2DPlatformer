using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Door : MonoBehaviour{

    public int levelToLoad;
    public int collisionCheck = 0;
    public UnityEngine.Vector2 doorSpawn;

    private GameMaster gm;

    void Start(){

        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();

    }


    void OnTriggerEnter2D(Collider2D col){

        if (col.CompareTag("Player")){
            gm.InputText.text = ("[E] to enter");
             if (Input.GetKeyDown("e")){
                gm.lastCheckPointPos = doorSpawn;   //gm.playerSpawn.position;                  
                SceneManager.LoadScene(levelToLoad);
            }         
        }

    }

    void OnTriggerStay2D(Collider2D col){

        if (col.CompareTag("Player")){
            collisionCheck = 1;
            if (Input.GetKeyDown("e")){
                gm.lastCheckPointPos = doorSpawn;   //gm.playerSpawn.position;                    
                SceneManager.LoadScene(levelToLoad);
                gm.InputText.text = (" ");
            }
        }
        

    }

    void OnTriggerExit2D(Collider2D col){
        if (col.CompareTag("Player")){
            gm.InputText.text = (" ");
            collisionCheck = 0;
        }
    }
}
