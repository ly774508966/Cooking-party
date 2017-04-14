using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using System;

public class OvenCollider : MonoBehaviour
{

    //private int etape;
    public GameObject preparation;

    // Use this for initialization
    void Start()
    {
        //etape = GameStatus.etape;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerStay(Collider other)
    {
        if (GameStatus.etape == 10)   //Si on est à la dernière étape
        {
            other.gameObject.SetActive(false); //Suppression de la préparation
            GameStatus.etape++;
        }
        
    }
}
