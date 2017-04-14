using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakZoneCollider : MonoBehaviour
{

    public GameObject oeuf1;
    public GameObject oeuf2;
    public GameObject oeuf3;
    public Color disabledColor = Color.black;

    private AudioSource source;

    //Success sound
    public AudioClip breakSound;

    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnTriggerEnter(Collider other)
    {
        switch (GameStatus.etape)
        {
            case 2:
                if (other.gameObject.Equals(oeuf1))
                {
                    source.PlayOneShot(breakSound);
                }
                break;
            case 3:
                if (other.gameObject.Equals(oeuf2))
                {
                    source.PlayOneShot(breakSound);
                }
                break;
            case 4:
                if (other.gameObject.Equals(oeuf3))
                {
                    source.PlayOneShot(breakSound);
                }
                break;
            default:
                break;
        }

    }

    void OnTriggerStay(Collider other)
    {
        switch (GameStatus.etape)
        {
            case 2:
                if (other.gameObject.Equals(oeuf1))
                {
                    SaladierCollider.brokenEgg = true;
                    this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", disabledColor);
                }
                break;
            case 3:
                if (other.gameObject.Equals(oeuf2))
                {
                    SaladierCollider.brokenEgg = true;
                    this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", disabledColor);
                }
                break;
            case 4:
                if (other.gameObject.Equals(oeuf3))
                {
                    SaladierCollider.brokenEgg = true;
                    this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", disabledColor);
                }
                break;
            default:
                Debug.Log("End of eggs");
                break;
        }
    }
}