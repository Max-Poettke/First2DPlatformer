using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour{

    public static GameMaster instance;
    public Vector2 lastCheckPointPos;
    public Text InputText;
    public Canvas Canvas;
    public Transform playerSpawn;
    public Transform player;
    private Door d;

    private void Start()
    {
        d = GameObject.FindGameObjectWithTag("Door").GetComponent<Door>();
    }



    void Awake(){
        if (instance == null){
        instance = this;
        DontDestroyOnLoad(instance);                
        DontDestroyOnLoad(Canvas);
        DontDestroyOnLoad(playerSpawn);
        } else {
            Destroy(gameObject);                            
        }

    }


}
