using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private float _healAmount = 10;
    [SerializeField] private bool rotate;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private GameObject collectEffect;

    void Update()
    {
        if (rotate)
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (collectSound)
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            if (collectEffect)
                Instantiate(collectEffect, transform.position, Quaternion.identity);

            var stats = other.GetComponent<PlayerStats>();
            stats.Heal(_healAmount);
            Destroy(gameObject);
        }
    }
}
