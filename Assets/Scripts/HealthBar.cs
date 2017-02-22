using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public Color MaxColor = Color.green;
    public Color MinColor = Color.red;
    private float minValue = 0.0f;
    private float maxValue = 1.0f;
    private Image _image;
    
	void Start () {
	    if (_image == null) {
	        _image = gameObject.AddComponent<Image>();
            _image.hideFlags = HideFlags.HideInInspector;
	        _image.color = MaxColor;
	    }

	    transform.root.GetComponent<UnitHealth>().OnHealthChanged += UpdateHealthBar;
	}

    public void UpdateHealthBar(int current, int max) {
        SetHealthVisual((float)current/max);
    }
 
    public void SetHealthVisual(float healthNormalized) {
        gameObject.transform.localScale = new Vector3(healthNormalized, transform.localScale.y, transform.localScale.z);
        _image.color = Color.Lerp(MinColor, MaxColor, Mathf.Lerp(minValue, maxValue, transform.localScale.x));
    }
}
