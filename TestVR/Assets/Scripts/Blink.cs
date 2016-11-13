using UnityEngine;
using System.Collections;

public class Blink : MonoBehaviour {

	public Vector3 startPoint;
	public Vector3 endPoint;
	public Vector3 startPoint2;
	public Vector3 endPoint2;
	public GameObject BlinkTop;
	public GameObject BlinkBot;
	public float _smoothTime;
	public float _separation;

    private Vector3 currentTargetPointTop;
    private Vector3 currentTargetPointBot;

    public float invisible;

    // Use this for initialization
    void Start ()
    {
		endPoint = BlinkTop.transform.localPosition;
		endPoint2 = BlinkBot.transform.localPosition;
		startPoint = new Vector3 (endPoint.x, endPoint.y + _separation, endPoint.z);
		startPoint2 = new Vector3 (endPoint2.x, endPoint2.y - _separation, endPoint2.z);
	}
	
	// Update is called once per frame
	void Update ()
    {
        float triggerRatio = (Input.GetAxis("Blink") + 1) / 2.0f;

        currentTargetPointTop = Vector3.Lerp(startPoint, endPoint, triggerRatio);
        currentTargetPointBot = Vector3.Lerp(startPoint2, endPoint2, triggerRatio);

        BlinkTop.transform.localPosition = Vector3.Lerp (BlinkTop.transform.localPosition, currentTargetPointTop, _smoothTime);//new Vector3 (endPoint.x, startPoint.y + ((endPoint.y - startPoint.y) * (Input.GetAxis("Vertical") + 1) /2), endPoint.z);
		BlinkBot.transform.localPosition = Vector3.Lerp (BlinkBot.transform.localPosition, currentTargetPointBot, _smoothTime);//new Vector3 (endPoint2.x, startPoint2.y + ((endPoint2.y - startPoint2.y) * (Input.GetAxis("Vertical") + 1) /2), endPoint2.z);

        invisible = triggerRatio;

        //Debug.Log (invisible);
    }


}
