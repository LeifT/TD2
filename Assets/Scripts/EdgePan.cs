using UnityEngine;

[ExecuteInEditMode]
public class EdgePan : MonoBehaviour
{
    private Rect _recDown;
    private Rect _recLeft;
    private Rect _recRight;
    private Rect _recUp;

    public float EdgeSize = 5;
    public float MoveSpeed = 10f;

    public GUISkin standardSkin = null;

    private void LateUpdate()
    {
        _recDown = new Rect(0, 0, Screen.width, EdgeSize);
        _recUp = new Rect(0, Screen.height - EdgeSize, Screen.width, EdgeSize);
        _recLeft = new Rect(0, 0, EdgeSize, Screen.height);
        _recRight = new Rect(Screen.width - EdgeSize, 0, EdgeSize, Screen.height);
    }

    private void OnGUI()
    {
        //GUI.skin = standardSkin;
        GUI.skin.box.stretchHeight = true;

        //GUI.skin.box.normal.background = standardSkin.box.normal.background;
        //GUI.Box(new Rect(EdgeSize,EdgeSize, Screen.width - EdgeSize * 2, Screen.height - EdgeSize * 2), "");
        GUI.Box(_recDown, "");
        GUI.Box(_recLeft, "");
        GUI.Box(_recRight, "");
        GUI.Box(_recUp, "");
    }

    //    private void OnDrawGizmos() {
    //        float height = (float)2.0 *  Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad) * 1;
    //        float width =  height * Screen.width / Screen.height;
    //        Debug.Log(Screen.width);
    //        //Debug.Log("Height: "+ height);
    //        Debug.Log("Width:" + width);
    //
    //        Gizmos.matrix = transform.localToWorldMatrix;
    //        //Gizmos.DrawWireCube(new Vector3(0,0,1), new Vector3(width,height,0));
    //
    //        //Gizmos.DrawWireCube(new Vector3(0, 0, 1), new Vector3(width - (width * 0.01f * 10f), height, 0));
    //    }

    private void Update()
    {
        if (_recDown.Contains(Input.mousePosition))
        {
            transform.Translate(0, 0, -MoveSpeed * Time.deltaTime, Space.World);
        }

        if (_recUp.Contains(Input.mousePosition))
        {
            transform.Translate(0, 0, MoveSpeed * Time.deltaTime, Space.World);
        }

        if (_recLeft.Contains(Input.mousePosition))
        {
            transform.Translate(-MoveSpeed * Time.deltaTime, 0, 0, Space.World);
        }

        if (_recRight.Contains(Input.mousePosition))
        {
            transform.Translate(MoveSpeed * Time.deltaTime, 0, 0, Space.World);
        }
    }
}