using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] private GameObject _replacement;
    [SerializeField] private float _breakForce = 20;
    [SerializeField] private float _collisionMultiplier = 1;
    [SerializeField] private bool _broken;

    private void OnCollisionEnter(Collision collision)
    {
        if (_broken) return;
        if (collision.relativeVelocity.magnitude >= _breakForce)
        {
            _broken = true;

            GameObject replacement = Instantiate(_replacement, this.transform.position, this.transform.rotation);
            Rigidbody[] rbs = replacement.GetComponentsInChildren<Rigidbody>();

            foreach (var rb in rbs)
            {
                rb.AddExplosionForce(collision.relativeVelocity.magnitude * _collisionMultiplier, collision.contacts[0].point, 1, 0);
            }

            Destroy(gameObject);
        }
    }
}
