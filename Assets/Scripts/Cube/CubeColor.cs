using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class CubeColor : MonoBehaviour
{
    private void Awake()
    {
        SetInitialColor();
    }

    public void ChangeColor()
    {
        GetComponent<Renderer>().material.color = Random.ColorHSV(0f,1f,1f,1f,0.5f, 1f);
    }

    public void ResetColor()
    {
        SetInitialColor();
    }

    private void SetInitialColor()
    {
        GetComponent <Renderer>().material.color = Color.black;
    }
}
