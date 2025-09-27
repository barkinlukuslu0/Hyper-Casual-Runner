using UnityEngine;
using UnityEngine.UI;

public class DailyReward : MonoBehaviour
{
    public bool initalized;
    public long rewardGivingTimeTicks;

    public GameObject rewardMenu;
    public Text remainingText;

    public void InitializeDailyReward()
    {
        if (PlayerPrefs.HasKey("lastDailyReward"))
        {
            rewardGivingTimeTicks = long.Parse(PlayerPrefs.GetString("lastDailyReward")) + 864000000000;
            long currenTime = System.DateTime.Now.Ticks;
            if (currenTime >= rewardGivingTimeTicks)
            {
                GiveReward();
            }
        }
        else
        {
            GiveReward();
        }
        initalized = true;
    }

    void Update()
    {
        if(initalized)
        {
            if(LevelController.Current.startMenu.activeInHierarchy)
            {
                long currenTime = System.DateTime.Now.Ticks;
                long remaningTime = rewardGivingTimeTicks - currenTime;

                if (remaningTime <= 0)
                {
                    GiveReward();
                }
                else
                {
                    System.TimeSpan timeSpan = System.TimeSpan.FromTicks(remaningTime);
                    remainingText.text = string.Format("{0}:{1}:{2}", timeSpan.Hours.ToString("D2"), timeSpan.Minutes.ToString("D2"), timeSpan.Seconds.ToString("D2"));
                }
            }
        }
    }

    public void GiveReward()
    {
        LevelController.Current.GiveMoneyToPlayer(100);
        rewardMenu.SetActive(true);
        PlayerPrefs.SetString("lastDailyReward", System.DateTime.Now.Ticks.ToString());
        rewardGivingTimeTicks = long.Parse(PlayerPrefs.GetString("lastDailyReward")) + 864000000000;
    }

    public void TapToReturnButton()
    {
        rewardMenu.SetActive(false);
    }
}
