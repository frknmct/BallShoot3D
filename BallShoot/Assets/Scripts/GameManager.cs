using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("BALL SETTINGS")]
    [SerializeField] private GameObject[] Balls;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private float ballPower;
    private int currentBallIndex;
    [SerializeField] private Animator ballShooterAnim;
    [SerializeField] private ParticleSystem ballShootEffect;
    [SerializeField] private ParticleSystem[] ballEffects;
    private int currentBallEffectIndex;
    [SerializeField] private AudioSource[] ballSounds;
    private int currentBallVoiceIndex;

    [Header("LEVEL SETTINGS")] 
    [SerializeField]private int targetBallCount;
    [SerializeField]private int currentBallCount;
    private int scoredBallCount;
    [SerializeField] private Slider levelSlider;
    [SerializeField] private TextMeshProUGUI remainedBallCountText;
    
    [Header("UI SETTINGS")] 
    [SerializeField] private GameObject[] Panels;
    [SerializeField] private TextMeshProUGUI StarCount;
    [SerializeField] private TextMeshProUGUI WinLevelCount;
    [SerializeField] private TextMeshProUGUI LoseLevelCount;
    
    [Header("OTHER SETTINGS")]
    [SerializeField] private Renderer bucketTransparent;
    private float bucketFirstValue;
    private float bucketStepValue;
    [SerializeField] private AudioSource[] otherVoices;

    private string LevelName;
    void Start()
    {
        currentBallEffectIndex = 0;
        currentBallVoiceIndex = 0;
        LevelName = SceneManager.GetActiveScene().name;
        bucketFirstValue = .5f;
        bucketStepValue = .25f / targetBallCount;
        levelSlider.maxValue = targetBallCount;
        remainedBallCountText.text = currentBallCount.ToString();
    }
    public void BallScored()
    {
        scoredBallCount++;
        levelSlider.value = scoredBallCount;

        bucketFirstValue -= bucketStepValue;
        bucketTransparent.material.SetTextureScale("_MainTex",new Vector2(1f,bucketFirstValue));
        
        ballSounds[currentBallVoiceIndex].Play();
        currentBallVoiceIndex++;
        
        if (currentBallVoiceIndex == ballSounds.Length - 1)
        {
            currentBallVoiceIndex = 0;
        }
        
        if (scoredBallCount == targetBallCount)
        {
            Time.timeScale = 0;
            otherVoices[1].Play();
            PlayerPrefs.SetInt("Level",SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("Star",PlayerPrefs.GetInt("Star") + 15);
            StarCount.text = PlayerPrefs.GetInt("Star").ToString();
            WinLevelCount.text = "Level : " + LevelName;
            Panels[1].SetActive(true);
        }

        int count=0;
        foreach (var item in Balls)
        {
            if (item.activeInHierarchy)
                count++;
        }

        if (count == 0)
        {
            if (currentBallCount == 0 && scoredBallCount != targetBallCount)
            {
                Lose();
            }
        
            if ((currentBallCount + scoredBallCount) < targetBallCount)
            {
                Lose();
            }
        }
        
        
        
    }
    public void BallMissed()
    {
        int count=0;
        foreach (var item in Balls)
        {
            if (item.activeInHierarchy)
                count++;
        }

        if (count == 0)
        {
            if (currentBallCount == 0)
            {
                Lose();
            }
            if ((currentBallCount + scoredBallCount) < targetBallCount)
            {
                Lose();
            }
        }
    }

    public void StopGame()
    {
        Panels[0].SetActive(true);
        Time.timeScale = 0;
    }
    public void ButtonOperationsForPanels(string operation)
    {
        switch (operation)
        {
            case "Continue":
                Time.timeScale = 1;
                Panels[0].SetActive(false);
                break;
            case "Exit":
                Application.Quit();
                break;
            case "Settings":
                break;
            case "Retry":
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            case "Next":
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            
        }
    }
    public void ParcEffect(Vector3 _position,Color _color)
    {
        ballEffects[currentBallEffectIndex].transform.position = _position;
        var main = ballEffects[currentBallEffectIndex].main;
        main.startColor = _color;
        ballEffects[currentBallEffectIndex].gameObject.SetActive(true);
        currentBallEffectIndex++;
        
        if (currentBallEffectIndex == ballEffects.Length - 1)
        {
            currentBallEffectIndex = 0;
        }

    }
    void Lose()
    {
        Time.timeScale = 0;
        otherVoices[0].Play();
        LoseLevelCount.text = "Level : " + LevelName;
        Panels[2].SetActive(true);
    }

    public void BallShoot()
    {
        if (Time.timeScale != 0)
        {
            currentBallCount--;
                remainedBallCountText.text = currentBallCount.ToString();
                ballShooterAnim.Play("BallShooter");
                ballShootEffect.Play();
                otherVoices[2].Play();
                Balls[currentBallIndex].transform.SetPositionAndRotation(firePoint.transform.position,firePoint.transform.rotation);  
                Balls[currentBallIndex].SetActive(true);
                Balls[currentBallIndex].GetComponent<Rigidbody>().AddForce(Balls[currentBallIndex].transform.TransformDirection(90,90,0) * ballPower,ForceMode.Force);
            
                if (Balls.Length - 1 == currentBallIndex)
                    currentBallIndex = 0;
                else
                {
                    currentBallIndex++;
                }
        }
    }
}
