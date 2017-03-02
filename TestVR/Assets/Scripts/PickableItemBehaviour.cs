using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PickableItemBehaviour : MonoBehaviour {

    public GameObject pickItemPanel;
    public Text pickItemText;

    public GameEngineBehaviour GameEngine;

    public float distance;

    private GameObject currentPickableItemSeen;

    private float LastClickTime;

    // Use this for initialization
    void Start()
    {
        LastClickTime = Time.time-5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - LastClickTime > 5.0f)
        {
            if (CheckForPickableItem())
            {
                if (currentPickableItemSeen.GetComponent<DoorAndKey>() != null)
                {
                    pickItemText.text = currentPickableItemSeen.GetComponent<DoorAndKey>().pickableMessage;
                }
                else
                {
                    pickItemText.text = "Left click = Pick up Key";
                }

                pickItemPanel.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    if (currentPickableItemSeen.GetComponent<DoorAndKey>() != null)
                    {
                        if (currentPickableItemSeen.GetComponent<DoorAndKey>().OpenDoor())
                        {
                            pickItemPanel.SetActive(false);
                        }
                        else
                        {
                            pickItemText.text = "The door is locked";
                            StartCoroutine(WaitAndRemovePickItemPanel(2.0f));
                            LastClickTime = Time.time;
                        }
                    }
                    else
                    {
                        currentPickableItemSeen.SetActive(false);
                        GameEngine.HasKey = true;
                        pickItemPanel.SetActive(false);
                    }
                }
            }
            else
            {
                pickItemPanel.SetActive(false);
            }
        }
    }

    IEnumerator WaitAndRemovePickItemPanel(float timer)
    {
        yield return new WaitForSeconds(timer);
        pickItemPanel.SetActive(false);
    }

    private bool CheckForPickableItem()
    {
        Ray ray = new Ray(this.transform.position, this.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance))
        {
            if (hit.collider.tag.Equals("PickableItem"))
            {
                currentPickableItemSeen = hit.collider.gameObject;
                return true;
            }
        }
        currentPickableItemSeen = null;
        return false;
    }
}
