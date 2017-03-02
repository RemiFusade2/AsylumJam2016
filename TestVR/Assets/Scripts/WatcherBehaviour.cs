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


    public AudioSource footStepsSource;
    public List<AudioClip> footstepsClips;

    public AudioSource breathSource;
    public List<AudioClip> breathClips;

    public AudioSource spotyouSource;
    public List<AudioClip> spotyouClips;
    public AudioClip screamClips;

    public static int spotCount;



    // Use this for initialization
    void Start()
    {
        patrolCenter = transform.position;
        SetNewDestination();

        eyesOpened = true;

        StartCoroutine(WaitAndPlayBreathing(Random.Range(5.0f,15.0f)));
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(destination, transform.position) < 20.0f)
        {
            SetNewDestination();
        }
        //CheckIfPlayerIsTooClose();
        CheckIfPlayerIsVisible();
    }

    public void CheckIfPlayerIsTooClose()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) < 5.0f)
        {
            SetEyesOpened(false);
            StartCoroutine(WaitAndSetEyesOpened(2.0f, true));
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
                float distanceSight = 40;
                if (Physics.Raycast(r, out hit, distanceSight))
                {
                    if (hit.collider.tag.Equals("Player"))
                    {
                        PlayerIsSeen();
                    }
                }
            }
        }
    }

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

    private float lastTimePlayerWasSeen;

    public void PlayerIsSeen()
    {
        if (Time.time - lastTimePlayerWasSeen > 2.0f)
        {
            // player has been seen
            // monster is warned

            // if it's the third time player is seen, monster has to run !

            lastTimePlayerWasSeen = Time.time;
            if (spotCount >= 3)
            {
                // trigger alarm
                spotCount = 0;
                breathSource.Stop();

                spotyouSource.Stop();
                spotyouSource.clip = screamClips;
                spotyouSource.Play();

                SetEyesOpened(false);
                StartCoroutine(WaitAndSetEyesOpened(5.0f, true));

                MonsterBehaviour.gofast = true; // run, Monster, run !
            }
            else
            {
                spotCount++;

                spotyouSource.Stop();
                spotyouSource.clip = spotyouClips[Random.Range(0, spotyouClips.Count)];
                spotyouSource.Play();

                SetEyesOpened(false);
                StartCoroutine(WaitAndSetEyesOpened(2.0f, true));
            }

            MonsterBehaviour.lastSeenPosition = player.transform.position;
            MonsterBehaviour.lastSeenPositionUpdateTime = Time.time;
        }
    }

    private Coroutine footstepsCoroutine;

    IEnumerator WaitAndPlayFootsteps (float timer)
    {
        yield return new WaitForSeconds(timer);

        int index = Random.Range(0, footstepsClips.Count);

        footStepsSource.Stop();
        footStepsSource.clip = footstepsClips[index];
        footStepsSource.Play();

        footstepsCoroutine = StartCoroutine(WaitAndPlayFootsteps(footStepsSource.clip.length));
    }

    IEnumerator WaitAndPlayBreathing(float timer)
    {
        yield return new WaitForSeconds(timer);

        int index = Random.Range(0, breathClips.Count);
        //Debug.Log("play breathing : " + index);

        breathSource.Stop();
        breathSource.clip = breathClips[index];
        breathSource.Play();
        
        StartCoroutine(WaitAndPlayBreathing(Random.Range(2.0f, 5.0f)));
    }

    public void MoveWatcher()
    {
        this.GetComponent<NavMeshAgent>().Resume();

        if (footstepsCoroutine == null)
        {
            footstepsCoroutine = StartCoroutine(WaitAndPlayFootsteps(0.0f));
        }
    }

    public void StopWatcher()
    {
        if (this.GetComponent<NavMeshAgent>().isActiveAndEnabled)
        {
            this.GetComponent<NavMeshAgent>().Stop();
        }

        if (footstepsCoroutine != null)
        {
            StopCoroutine(footstepsCoroutine);
            footstepsCoroutine = null;
            footStepsSource.Stop();
        }
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
