using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Color MaxColor = Color.green;
    public Color MinColor = Color.red;
    private float minValue = 0.0f;
    private float maxValue = 1.0f;

    //public Image image;

    private Image _image;

	// Use this for initialization
	void Start () {
	    if (_image == null) {
	        _image = gameObject.AddComponent<Image>();
            _image.hideFlags = HideFlags.HideInInspector;
	        _image.color = MaxColor;
	    }

	    transform.root.GetComponent<UnitHealth>().OnHealthDecresed += Aoeu;
	}

    public void Aoeu(int current, int max) {
        SetHealthVisual((float)current/(float)max);
    }
 
    public void SetHealthVisual(float healthNormalized) {
        gameObject.transform.localScale = new Vector3(healthNormalized, transform.localScale.y, transform.localScale.z);
        _image.color = Color.Lerp(MinColor, MaxColor, Mathf.Lerp(minValue, maxValue, transform.localScale.x));
    }
}
