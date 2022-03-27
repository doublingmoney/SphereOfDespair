using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    //public static HUDManager Instance;
    [SerializeField] private TextMeshProUGUI _currentVelocityUI;
    
    [SerializeField] private TextMeshProUGUI _enemyKC;

    [SerializeField] private TextMeshProUGUI _dashCD;

    //[SerializeField] private Text _overheatUI;
    [SerializeField] private TextMeshProUGUI _overheatUI;
    [SerializeField] private Slider overheatSlider;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider healthSlider;

    [SerializeField] private Image _DashCDFill;

    private PlayerController _playerController;
    private CharacterStats _playerStats;
    private GameObject _player;
    private GunVersion2 _weapon;

    /*
    private void Awake()
    {
        Instance = this;
    }
    */

    void Awake()
    {
        _player = GameObject.Find("Player");
        _playerController = _player.GetComponent<PlayerController>();
        _playerStats = _player.GetComponent<CharacterStats>();
        _weapon = _player.GetComponent<GunVersion2>();
    }
    private void Start()
    {
        //Debug.Log(GameEvents.Instance);
        GameEvents.Instance.onGoalUpdate += GoalUpdate;
    }

    // Update is called once per frame
    void Update()
    {
        _currentVelocityUI.text = ("" + _playerController.currentVelocity.ToString());

        _overheatUI.text = string.Format("{0} / {1}", _weapon._batteryRemaining, _weapon._batterySize);
        overheatSlider.value = _weapon._batteryRemaining;

        healthSlider.value = _playerStats.currHealth;
        int roundHealth = Mathf.CeilToInt(_playerStats.currHealth);
        healthText.text = (roundHealth.ToString() + " / 100");

        DCD();
    }

    private void GoalUpdate(int id, int current, int goal)
    {
        _enemyKC.text = string.Format("{0}/{1}", current, goal);
    }

    private void DCD()
    {
        if (_playerController._dashRemainingCD <= 0.0f)
        {
            _dashCD.gameObject.SetActive(false);
            _DashCDFill.fillAmount = 0.0f;
        }
        else
        {
            _dashCD.gameObject.SetActive(true);
            _dashCD.text = (_playerController._dashRemainingCD.ToString());
            _DashCDFill.fillAmount = _playerController._dashRemainingCD / _playerController._dashCooldown;
        }
    }
}
