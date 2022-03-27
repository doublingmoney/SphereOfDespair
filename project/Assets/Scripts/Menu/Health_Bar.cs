using UnityEngine;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHP(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(float Health)
    {
        slider.value = Health;
    }

}
