using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using System;

public class SaladierCollider : MonoBehaviour {

    public static bool brokenEgg = false;

    //private int etape;
    public GameObject lait;
    public GameObject oeuf1;
    public GameObject oeuf2;
    public GameObject oeuf3;
    public GameObject farine;
    public GameObject sucre;
    public GameObject huile;
    public GameObject rhum;

    public GameObject liquid;

    private AudioSource source;

    //Success sound
    public AudioClip successSound;

    //Couleur de la pate après avoir mis les oeufs
    private Color col = new Color(255.0f, 243.0f, 155.0f);

    // Use this for initialization
    void Start () {
        //etape = GameStatus.etape;
        source = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
	}

    bool isRotationDone(Collider other)
    {
        Quaternion rotation = other.gameObject.transform.rotation;
        if ((rotation.eulerAngles.x >= 60 && rotation.eulerAngles.x <= 300)|| (rotation.eulerAngles.z >= 60 && rotation.eulerAngles.z <= 300))
        {
            return true;
        }
        return false;
    }

    void OnTriggerStay(Collider other)
    {
        bool rotated = isRotationDone(other);
        Debug.Log(rotated);
        switch (GameStatus.etape)
        {
            case 0:
                if(other.gameObject.Equals(farine) && rotated)
                {
                    GameStatus.etape++;
                    source.PlayOneShot(successSound);

                    //Fill the bowl
                    MeshRenderer m = liquid.GetComponent<MeshRenderer>();
                    m.enabled = true;
                }
                break;
            case 1:
                if (other.gameObject.Equals(sucre) && rotated)
                {
                    GameStatus.etape++;
                    source.PlayOneShot(successSound);
                }
                break;
            case 2:
                if (other.gameObject.Equals(oeuf1) && brokenEgg)
                {
                    GameStatus.etape++;
                    brokenEgg = false;
                    source.PlayOneShot(successSound);

                    //Coloration en jaune
                    Renderer m = liquid.GetComponent<Renderer>();
                    
                    m.material.SetColor("_Color", col);
                }
                break;
            case 3:
                if (other.gameObject.Equals(oeuf2) && brokenEgg)
                {
                    GameStatus.etape++;
                    brokenEgg = false;
                    source.PlayOneShot(successSound);
                }
                break;
            case 4:
                if (other.gameObject.Equals(oeuf3) && brokenEgg)
                {
                    GameStatus.etape++;
                    brokenEgg = false;
                    source.PlayOneShot(successSound);
                }
                break;
            case 5:
                if (other.gameObject.Equals(huile) && rotated)
                {
                    GameStatus.etape++;
                    source.PlayOneShot(successSound);
                }
                break;
            case 6:
                if (other.gameObject.Equals(lait) && rotated)
                {
                    GameStatus.etape++;
                    source.PlayOneShot(successSound);
                }
                break;
            case 7:
                if (other.gameObject.Equals(rhum) && rotated)
                {
                    GameStatus.etape++;
                    other.gameObject.SetActive(false); //Suppression du rhum
                    source.PlayOneShot(successSound);
                }
                break;
            default:
                Debug.Log("End");
                break;
        }
    }
}
