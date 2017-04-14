using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;


public class MainScript : MonoBehaviour {

    public static MainScript instance = null;

    public GameObject saladier;
    public Text instructionText;
    public GameObject lait;
    public GameObject oeuf1;
    public GameObject oeuf2;
    public GameObject oeuf3;
    public GameObject farine;
    public GameObject sucre;
    public GameObject huile;
    public GameObject rhum;
    public GameObject breakZone;

    private AudioSource source;

    //Success sound
    public AudioClip endSound;

    public float smoothing = 3f;

    private Vector3 positionLait; private Quaternion rotationLait;
    private Vector3 positionFarine; private Quaternion rotationFarine;

    public Color breakZoneColor = Color.blue;

    private List<string> instructions;
    private List<string> success;
    private List<GameObject> ingredientsOrder;
    private List<Vector3> ingredientsPosition;
    private List<Quaternion> ingredientsRotation;

    private bool musiqueLancee = false;

    public GameObject cam;

    private int etape;
    [SerializeField]
    private Color initialColor;
    [SerializeField]
    private Color selectionColor = Color.green;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {

        // Ajout des instructions
        instructions = new List<string>();
        success = new List<string>();

        instructions.Add("Ajoutez la farine dans le saladier");
        success.Add("Farine ajoutée !");
        instructions.Add("Ajoutez le sucre dans le saladier");
        success.Add("Sucre ajouté !");
        instructions.Add("Cassez l'oeuf sur la zone bleue puis ajoutez le premier oeuf");
        success.Add("Et de 1 !");
        instructions.Add("Cassez et ajoutez le second oeuf");
        success.Add("Et de 2 !");
        instructions.Add("Encore un oeuf !");
        success.Add("Et de 3 ! Les oeufs sont ajoutés !");
        instructions.Add("Ajoutez l'huile");
        success.Add("Huile ajoutée !");
        instructions.Add("Ajoutez le lait");
        success.Add("Lait ajouté !");
        instructions.Add("Ajoutez le rhum");
        success.Add("Rhum ajouté !");
        instructions.Add("Passons à la cuisson !");
        instructions.Add("Mettez le saladier devant la porte du four");
        success.Add("Magie, ça cuit! Plus que 10 secondes");
        success.Add("Encore 5 secondes..");
        success.Add("C'est cuit ! Bravo !");

        // Ajout des ingrédients dans l'ordre
        ingredientsOrder = new List<GameObject>();
        ingredientsPosition = new List<Vector3>();
        ingredientsRotation = new List<Quaternion>();

        ingredientsOrder.Add(farine);
        ingredientsOrder.Add(sucre);
        ingredientsOrder.Add(oeuf1);
        ingredientsOrder.Add(oeuf2);
        ingredientsOrder.Add(oeuf3);
        ingredientsOrder.Add(huile);
        ingredientsOrder.Add(lait);
        ingredientsOrder.Add(rhum);

        etape = GameStatus.etape;
            instructionText.text = instructions[etape];
            initialColor = ingredientsOrder[etape].GetComponent<Renderer>().material.GetColor("_Color");
            ingredientsOrder[etape].GetComponent<Renderer>().material.SetColor("_Color", selectionColor);

        breakZone.SetActive(false);

        // Mise en mémoire des rotations et positions des objets
        foreach(GameObject go in ingredientsOrder)
        {
            ingredientsPosition.Add(go.transform.position);
            ingredientsRotation.Add(go.transform.rotation);
        }
    }

    IEnumerator updateScene()
    {
        if(!(etape == 2 || etape == 3 || etape == 4))
        {
            
            ingredientsOrder[etape].SetActive(false);
            if(etape == 7)
            {
                ingredientsOrder[etape].transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", initialColor);
            }
            else
            {
                ingredientsOrder[etape].GetComponent<Renderer>().material.SetColor("_Color", initialColor);
            }
        }
        else
        {
            if (ingredientsOrder[etape].gameObject.activeSelf)
            {
                CoroutineSnap snapScript = (CoroutineSnap)ingredientsOrder[etape].gameObject.GetComponent(typeof(CoroutineSnap));
                snapScript.StartSnap();
            }
        }
        instructionText.text = success[etape];

        yield return new WaitForSeconds(2);

        etape = GameStatus.etape;

        switch (etape) {
            case 2:
                breakZone.SetActive(true);
            break;
            case 3:
                breakZone.GetComponent<Renderer>().material.SetColor("_Color", breakZoneColor);
                break;
            case 4:
                breakZone.GetComponent<Renderer>().material.SetColor("_Color", breakZoneColor);
                break;
            case 5:
                breakZone.SetActive(false);
                break;
            default:
                break;
        }

        instructionText.text = instructions[etape];

        if(etape == 7 || etape == 5)
        {
            initialColor = ingredientsOrder[etape].transform.GetChild(0).GetComponent<Renderer>().material.GetColor("_Color");
            ingredientsOrder[etape].transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", selectionColor);
        }
        if (etape == 6)
        {
            initialColor = ingredientsOrder[etape].transform.GetChild(0).GetComponent<Renderer>().material.GetColor("_Color");
            ingredientsOrder[etape].transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", selectionColor);
        }
        else
        {
            initialColor = ingredientsOrder[etape].GetComponent<Renderer>().material.GetColor("_Color");
            ingredientsOrder[etape].GetComponent<Renderer>().material.SetColor("_Color", selectionColor);
        }
    }

    IEnumerator preGrabSaladier()
    {
        etape = GameStatus.etape;
        instructionText.text = instructions[etape];

        yield return new WaitForSeconds(2);

        saladier.transform.position = new Vector3(85, 26.9F, -10F);

        Camera camera = Camera.main;
        camera.transform.position = new Vector3(90F, 30F, -40F);
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = new Vector3(20.547F, 0F, 0F);
        camera.transform.rotation = rotation;

        //Positionnement de la radio
        GameObject radio = GameObject.Find("Radio");
        radio.transform.position = new Vector3(97, 23.2F, -8);


        instructionText.text = instructions[etape+1];
        GameStatus.etape++;

    }

    IEnumerator grabSaladier()
    {

        yield return new WaitForSeconds(2);
        etape = GameStatus.etape;
        instructionText.text = instructions[etape + 1];
        yield return new WaitForSeconds(2);
    }

    IEnumerator cuisson()
    {
        etape = GameStatus.etape;
        instructionText.text = success[8];
        yield return new WaitForSeconds(10);
        instructionText.text = success[9];
        yield return new WaitForSeconds(5);
        instructionText.text = success[10];

        //Arret de la musique
        GameObject radio = GameObject.Find("Radio");
        AudioSource sourceRadio = radio.GetComponent<AudioSource>();
        sourceRadio.enabled = false;


        //Lancement de la musique de fin
        if (!musiqueLancee)
        {
            source = GetComponent<AudioSource>();
            source.PlayOneShot(endSound);
            musiqueLancee = true;
        }



    }

 
    IEnumerator auxReset (int ingIndex)
    {
        ingredientsOrder[ingIndex].GetComponent<Collider>().isTrigger = true;
        ingredientsOrder[ingIndex].GetComponent<Rigidbody>().detectCollisions = false;
        ingredientsOrder[ingIndex].GetComponent<Rigidbody>().velocity = Vector3.zero;
        ingredientsOrder[ingIndex].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        Vector3 deltaPosition = new Vector3(0, 0.1f, 0);
        while (Vector3.Distance(ingredientsOrder[ingIndex].transform.position, ingredientsPosition[ingIndex]) > 0.9f || Quaternion.Angle(ingredientsOrder[ingIndex].transform.rotation, ingredientsRotation[ingIndex]) > 10)
        {
            ingredientsOrder[ingIndex].transform.position = Vector3.Lerp(ingredientsOrder[ingIndex].transform.position, ingredientsPosition[ingIndex]+deltaPosition, smoothing * Time.deltaTime*3);
            ingredientsOrder[ingIndex].transform.rotation = Quaternion.Lerp(ingredientsOrder[ingIndex].transform.rotation, ingredientsRotation[ingIndex], smoothing * Time.deltaTime*3);
            //farine.transform.rotation = Quaternion.Lerp(farine.transform.rotation, rotationFarine, smoothing * Time.deltaTime);
            yield return null;
        }

       
        //ingredientsOrder[ingIndex].transform.rotation = ingredientsRotation[ingIndex];
        ingredientsOrder[ingIndex].GetComponent<Collider>().isTrigger = false;
        ingredientsOrder[ingIndex].GetComponent<Rigidbody>().detectCollisions = true;
    }
    

    void OnTriggerExit(Collider other)
    {
        string objectName = other.gameObject.transform.name;
        switch (objectName)
        {
            case "Farine":
                StartCoroutine(auxReset(0));
                break;
            case "Sucre":
                StartCoroutine(auxReset(1));
                break;
            case "Oeuf1":
                StartCoroutine(auxReset(2));
                break;
            case "Oeuf2":
                StartCoroutine(auxReset(3));
                break;
            case "Oeuf3":
                StartCoroutine(auxReset(4));
                break;
            case "Huile":
                StartCoroutine(auxReset(5));
                break;
            case "Lait":
                StartCoroutine(auxReset(6));
                break;
            case "Rhum":
                StartCoroutine(auxReset(7));
                break;
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown("escape"))
        {//When a key is pressed down it see if it was the escape key if it was it will execute the code
            Application.Quit(); // Quits the game
        }
        //foreach(GameObject go in ingredientsOrder) {
        //    if(!go.GetComponent<Renderer>().isVisible)
        //    {
        //        StartCoroutine(resetObject(go.transform.name));
        //    }
        //}
        //Hidden feature
        if (Vector3.Distance(rhum.transform.position, cam.transform.position) < 8.0f)
        { 
            cam.GetComponent<Blur>().enabled = true;
            instructionText.text = "Il est un peu tôt pour prendre l'apéro...";
        }
        else
        {
            cam.GetComponent<Blur>().enabled = false;
        }

        //Debug.Log(GameStatus.etape);
        if (GameStatus.etape != etape)
        {
            if (GameStatus.etape < 8)
            {
                StartCoroutine(updateScene());
            }
            if (GameStatus.etape == 8)
            {
                StartCoroutine(preGrabSaladier());
            }
            if (GameStatus.etape == 9) //Ajout du collider
            {
                GameObject saladier = GameObject.Find("SaladierBowl");
                saladier.AddComponent<Rigidbody>();
                saladier.AddComponent<BoxCollider>();
                GameStatus.etape++;
            }
            if (GameStatus.etape == 10)
            {
                StartCoroutine(grabSaladier());
            }
            if (GameStatus.etape == 11)
            {
                StartCoroutine(cuisson());
            }
        }    
    }
    

}