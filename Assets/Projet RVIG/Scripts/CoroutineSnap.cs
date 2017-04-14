using UnityEngine;
using System.Collections;

public class CoroutineSnap : MonoBehaviour
{
    public float smoothing = 1f;
    public GameObject target;


    void Start()
    {
        
    }

    public void StartSnap()
    {
        StartCoroutine(MyCoroutine(target.transform));
    }


    IEnumerator MyCoroutine(Transform target)
    {
        while (Vector3.Distance(transform.position, target.position) > 0.9f)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, smoothing * Time.deltaTime);

            yield return null;
        }

        print("Reached the target.");

        this.gameObject.SetActive(false);

        print("MyCoroutine is now finished.");
    }
}