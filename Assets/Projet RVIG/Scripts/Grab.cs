using Leap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour {

    //leap motion controller
    [Tooltip("Must be in the scene")]
    public HandController handController;

    private Hand hand;
    private HandModel handModel;
    private bool grabingOther = false;
    private bool isGrabed = false;

    private int etape;

    private Vector3 lastObjectPosition;
    private Quaternion lastObjectRotation;
    private GameObject lastObject;

    public Quaternion Rotation
    {
        get;
        protected set;
    }
    public Vector3 Position
    {
        get;
        protected set;
    }

    //visible default leap hand model attributes
    public KeyCode visibleHandKey = KeyCode.V;

    public bool defaultVisibleHand = false;
    private bool visibleHand;

    //variables used for grab implant
    private bool grab = false;

    //variables used for grab implant
    //public bool isGrabable;
    public Color newColor = Color.red;
    private Color initialColor;
    private Quaternion rotationOffset;

    protected void Start()
    {
        Position = new Vector3();
        Position = Vector3.zero;
        visibleHand = defaultVisibleHand;

        Rotation = new Quaternion();
        Rotation = Quaternion.identity;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("isGrabable"))
        {
            lastObject = other.gameObject;
            lastObjectPosition = other.gameObject.transform.position;
            lastObjectRotation = other.gameObject.transform.rotation;
            Debug.Log("collision");
            if(other.gameObject.transform.childCount > 0 && other.name != "Lait" && other.name != "SaladierCollider" && other.name != "Farine" && other.name != "Sucre")
            {
                initialColor = other.gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color;
                other.gameObject.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", newColor);
            }
            else
            {
                initialColor = other.gameObject.transform.GetComponent<Renderer>().material.color;
                other.gameObject.transform.GetComponent<Renderer>().material.SetColor("_Color", newColor);
            }

            etape = GameStatus.etape;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("isGrabable")) {
            //other.gameObject.transform.GetComponent<Renderer>().material.SetColor("_Color", newColor);
            if (hand.GrabStrength >= 0.65 && grab == false)
            {
                rotationOffset = other.gameObject.transform.rotation * Quaternion.Inverse(this.transform.rotation);
                other.gameObject.transform.position = Position;
                other.gameObject.transform.rotation = Rotation* rotationOffset;
                visibleHand = false;
                grab = true;
            }
            if (grab == true && hand.GrabStrength >= 0.55)
            {
                other.gameObject.transform.position = Position;
                other.gameObject.transform.rotation = Rotation * rotationOffset;
                visibleHand = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("isGrabable"))
        {
            Debug.Log("Out");
            if (GameStatus.etape == etape)
            {          
                if(other.gameObject.transform.childCount>0 && other.name != "Lait" && other.name != "SaladierCollider" && other.name != "Farine" && other.name != "Sucre")
                {
                    other.gameObject.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", initialColor);
                }
                else
                {
                    other.gameObject.transform.GetComponent<Renderer>().material.SetColor("_Color", initialColor);
                }
            }
            visibleHand = true;
            grab = false;
            /*lastObject.transform.position = lastObjectPosition;
            lastObject.transform.rotation = lastObjectRotation;*/
        }
        
    }

    protected void Update()
    {
        UpdateTracker();
    }

    protected void UpdateTracker()
    {
        //get the 1st hand in the frame
        if (handController.GetAllGraphicsHands().Length != 0)
        {
            handModel = handController.GetAllGraphicsHands()[0];
            handModel.transform.GetComponentInChildren<SkinnedMeshRenderer>().enabled = visibleHand;
            hand = handModel.GetLeapHand();
            Position = handModel.GetPalmPosition();
            Rotation = handModel.GetPalmRotation();
        }

        //mask/display the graphical hand on key down
        if (Input.GetKeyDown(visibleHandKey))
        {
            var smr = handModel.transform.GetComponentInChildren<SkinnedMeshRenderer>();
            visibleHand = !visibleHand;
        }
    }
}
