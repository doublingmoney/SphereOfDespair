using UnityEngine;

public class AcidTrigger : MonoBehaviour
{
    [SerializeField] private float _damage = 0.1f;
    [SerializeField] private AudioSource _hitSFX;

    private float soundTimer = 0;
    [SerializeField] private float soundFrequence = 2f;

    private void Start()
    {
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.GetComponent<CharacterStats>() != null)
        {
            CharacterStats colliderStats = other.GetComponent<CharacterStats>();
            colliderStats.Damage(_damage);

            if(soundTimer <= 0)
            {
                soundTimer = soundFrequence;
                _hitSFX.Play();
            } else { soundTimer -= Time.deltaTime; }
            

        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<Rigidbody>().angularDrag = 8f;

        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<Rigidbody>().angularDrag = 0.1f;
        }
    }

}
