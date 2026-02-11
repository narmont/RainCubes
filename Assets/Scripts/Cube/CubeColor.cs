using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class CubeColor : MonoBehaviour
{
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        SetInitialColor();
    }

    public void ChangeColor()
    {
        _renderer.material.color = Random.ColorHSV(0f,1f,1f,1f,0.5f, 1f);
    }

    public void ResetColor()
    {
        SetInitialColor();
    }

    private void SetInitialColor()
    {
        _renderer.material.color = Color.black;
    }
}
