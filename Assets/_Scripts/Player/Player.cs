using System.Collections;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player player;

    [SerializeField] private TMP_Text moneyText;
    
    private float _money;

    public RaycastHit rayHit;

    public float money
    {
        get
        {
            return _money;
        }
        set
        {
            _money = value;
            StopCoroutine(nameof(GainMoney));
            StartCoroutine(nameof(GainMoney));
        }
    }

    public bool isPaused;

    private void Awake()
    {
        if (player != null)
        {
            Destroy(player);
        }
        else
        {
            player = this;
        }
    }

    public void StartGainingMoney(Component sender, object mmoney)
    {
        if (mmoney is float)
        {
            money += (float)mmoney;
        }
        StartCoroutine(nameof(GainMoney));
    }
    
    IEnumerator GainMoney()
    {
        moneyText.text = _money.ToString();
        moneyText.alpha = 0f;
        for (int i = 0; i < 10; i++)
        {
            if (moneyText.alpha >= 1f) break;

            moneyText.alpha += 0.1f;
            yield return new WaitForSeconds(0.02f);
        }

        moneyText.alpha = 1f;
        
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 10; i++)
        {
            if (moneyText.alpha <= 0f) break;

            moneyText.alpha -= 0.1f;
            yield return new WaitForSeconds(0.02f);
        }

        moneyText.alpha = 0f;
    }
    
    private void Start()
    {
        foreach (var audioSource in GetComponents<AudioSource>())
        {
            audioSource.volume = PlayerPrefs.GetFloat("Volume") / 100;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IPickable>() != null)
        {
            other.gameObject.GetComponent<IPickable>().PickUp();
        }
    }
}
