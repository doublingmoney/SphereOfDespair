using UnityEngine;

namespace Impact.Demo
{
    public class UncapAngularVelocity : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Rigidbody>().maxAngularVelocity = Mathf.Infinity;
        }
    }
}

