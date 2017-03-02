using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Blink : MonoBehaviour {
    

	public GameObject BlinkTop;
	public GameObject BlinkBot;
    public GameObject alphaPanel;

    public GameObject alphaPanelHiddenText;

	public float _smoothTime;
    
    private float _targetInvisible;

    public float invisible;
    private bool takeInput;

    public void ForceInvisible(float newValue)
    {
        invisible = newValue;
        _targetInvisible = newValue;
    }

    // Use this for initialization
    void Start ()
    {
        _targetInvisible = invisible = 0;
        takeInput = true;
    }

	// Update is called once per frame
	void Update ()
    {

        if(takeInput)
        {
            _targetInvisible -= Input.GetAxis("Blink") / 2.0f;
        }
        _targetInvisible = _targetInvisible < 0 ? 0 : (_targetInvisible > 1 ? 1 : _targetInvisible);

        invisible += Mathf.Sign(_targetInvisible - invisible) * 0.01f;

        alphaPanelHiddenText.SetActive(invisible >= 0.95f);

        BlinkTop.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (Screen.height+10) * invisible);
        BlinkBot.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (Screen.height+10) * invisible);
        alphaPanel.GetComponent<Image>().color = new Color(0,0,0,invisible*invisible);
    }

    public void ForceClose()
    {
        _targetInvisible = 1.0f;
        takeInput = false;
    }

}
