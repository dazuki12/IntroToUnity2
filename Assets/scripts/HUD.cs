using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public int score = 0;
    public TMP_Text ScoreTxtComponent;
    public GameObject Filler;
    public GameObject soulImage;
    public int souls = 3;
    public float soulImageWidth = 12f;
    Image FillerComponent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
       
        FillerComponent = Filler.GetComponent<Image>();
    }
    // Update is called once per frame
    

    public void UpdateHealthBar(float health, float maxHealth)
    {
        if (health <= 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().ResetHealth();

            souls--;
            UpdateSouls();
        }
        FillerComponent.fillAmount = health/maxHealth;
        
    }

    public void UpdateSouls()
    {
        RectTransform rT = soulImage.GetComponent<RectTransform>();
        rT.sizeDelta = new Vector2(souls * soulImageWidth, rT.sizeDelta.y);
    }
    public void UpdateScoreTxt()
    {
        ScoreTxtComponent.text = "Score:" + score.ToString();
    }

    public void HurtPlayer()
    {
        
        souls--;
        UpdateSouls();
    }
    
}