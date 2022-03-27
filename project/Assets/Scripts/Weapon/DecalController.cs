using UnityEngine;

public class DecalController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float timeToDestroy = 5f;
    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }
}

