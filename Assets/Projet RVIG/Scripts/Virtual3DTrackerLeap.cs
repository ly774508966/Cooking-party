using Leap;
using System.Collections;
using UnityEngine;

/// <summary>
/// Script simplifié pour RVIG
/// Fournit la position d'une main (1ère main gauche ou droite reconnue) à chaque frame grâce au HandController
/// Utilise cette position pour déplacer l'objet courant
/// Une touche permet d'afficher/masquer la main virtuelle d'origine
/// </summary>
public class Virtual3DTrackerLeap : MonoBehaviour
{
    //leap motion controller
    [Tooltip("Must be in the scene")]
    public HandController handController;

    private Hand hand;
    private HandModel handModel;

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

    protected void Start()
    {
        Position = new Vector3();
        Position = Vector3.zero;
        visibleHand = defaultVisibleHand;

        Rotation = new Quaternion();
        Rotation = Quaternion.identity;
    }

    protected void Update()
    {
        UpdateTracker();
        this.transform.position = Position;
        this.transform.rotation = Rotation;
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