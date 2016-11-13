using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WatcherBehaviour : MonoBehaviour
{
    public GameObject player;
    public GameObject playerCamera;


    private Vector3 destination;

    public float patrolRadius;
    private Vector3 patrolCenter;


    private bool eyesOpened;


    public Transform raycastPointsParent;

    // Use this for initialization
    void Start()
    {
        patrolCenter = transform.position;
        SetNewDestination();

        eyesOpened = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(destination, transform.position) < 20.0f)
        {
            SetNewDestination();
        }
        CheckIfPlayerIsTooClose();
        CheckIfPlayerIsVisible();
    }

    public void CheckIfPlayerIsTooClose()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) < 2.0f)
        {
            eyesOpened = true;
        }
    }

    public void CheckIfPlayerIsVisible()
    {
        if (playerCamera.GetComponent<Blink>().invisible < 0.9f && eyesOpened)
        {
            foreach (Transform point in raycastPointsParent)
            {
                Ray r = new Ray(this.transform.position + Vector3.up, (point.position - this.transform.position));
                Debug.DrawRay(r.origin, (point.position - this.transform.position), Color.red, 0.1f, true);
                RaycastHit hit;
                if (Physics.Raycast(r, out hit, (point.position - this.transform.position).sqrMagnitude))
                {
                    if (hit.collider.tag.Equals("Player"))
                    {
                        PlayerIsSeen();
                    }
                }
            }
        }
    }

    /*
    void OnBecameVisible()
    {
        StartCoroutine(WaitAndSetEyesOpened(1.0f, false));
    }

    void OnBecameInvisible()
    {
        StartCoroutine(WaitAndSetEyesOpened(1.0f, true));
    }
    */

    IEnumerator WaitAndSetEyesOpened(float timer, bool open)
    {
        yield return new WaitForSeconds(timer);
        SetEyesOpened(open);
    }

    public void SetEyesOpened(bool open)
    {
        eyesOpened = open;
        this.GetComponent<Animator>().SetBool("Visible", open);
    }

    public void PlayerIsSeen()
    {
        MonsterBehaviour.lastSeenPosition = player.transform.position;

        this.GetComponent<AudioSource>().Stop();
        this.GetComponent<AudioSource>().Play();
    }

    public void MoveWatcher()
    {
        this.GetComponent<NavMeshAgent>().Resume();
    }

    public void StopWatcher()
    {
        this.GetComponent<NavMeshAgent>().Stop();
    }

    private void SetNewDestination()
    {
        Vector2 unitCircle = Random.insideUnitCircle;

        float distance = Random.Range(patrolRadius/2.0f, patrolRadius);
        Vector3 direction = new Vector3(unitCircle.x, 0, unitCircle.y);

        destination = patrolCenter + distance * direction;

        this.GetComponent<NavMeshAgent>().SetDestination(destination);
    }
}
