using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{

    public int ID;
    private void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("Tutorial trigger enter collider = " + collider.name);
        if (collider.CompareTag("Player"))
        {
            GameManager.Instance.ShowTutorial(ID, true);
        }

    }

    private void OnTriggerExit(Collider collider)
    {
        //Debug.Log("Tutorial trigger exit collider = " + collider.name);
        if (collider.CompareTag("Player"))
        {
            GameManager.Instance.ShowTutorial(ID, false);
        }
    }
}
