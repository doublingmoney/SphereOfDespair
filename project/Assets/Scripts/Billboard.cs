using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform _cameraTranform;
    void Start()
    {
        _cameraTranform = Camera.main.transform;
    }
    void LateUpdate()
    {
        transform.rotation = _cameraTranform.rotation;
    }
}
