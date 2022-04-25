
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterStats : MonoBehaviour
{
    [SerializeField]
    private Image HealthBar;
    float maxHealth = 100;
    public float power = 10;
    int KillScore = 200;

    public float currentHealth { get; private set;}
    
    // Start is called before the first frame update
    void Start()
    {
       // HealthBar = GetComponent<Image>();
        currentHealth = maxHealth;
        
    }

    public void ChangeHealth (float value)
    {
        //currentHealth += value;
        currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
        Debug.Log("Current Health" + currentHealth + "/" + maxHealth);

        if (transform.CompareTag("Enemy"))
        {
            transform.Find("Canvas").GetChild(1).GetComponent<Image>().fillAmount = currentHealth / maxHealth;
        }
        else if (transform.CompareTag("Player"))
        {
            LevelManager.instance.MainCanvas.Find("PnlStats").Find("ImgHealthBar").GetComponent<Image>().fillAmount = currentHealth / maxHealth;
            LevelManager.instance.MainCanvas.Find("PnlStats").Find("TextHealth").GetComponent<TextMeshProUGUI>().text = 
            string.Format("{0:0.##}%", (currentHealth / maxHealth) *100); 
        }

        if (currentHealth <= 0)
             Die();
    }

    void Die()
    {
        if (transform.CompareTag("Player"))
        {

        }
        else if (transform.CompareTag("Enemy"))
        {
            LevelManager.instance.score += KillScore;
            Destroy(gameObject);
              Instantiate(LevelManager.instance.Particles[2], transform.position, transform.rotation);
        }
    }

    
}
