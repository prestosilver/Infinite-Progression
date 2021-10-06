using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayGamesScript
{
    // Use this for initialization
    public static void Start()
    {
#if UNITY_ANDROID
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        SignIn();
#endif
    }

    public static void SignIn()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result) =>{ });
#endif
    }

    public static void UnlockAchievement(string id)
    {
#if UNITY_ANDROID
        Social.ReportProgress(id, 100, success => { });
#endif
    }

    public static void IncrementAchievement(string id, int stepsToIncrement)
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.IncrementAchievement(id, stepsToIncrement, success => { });
#endif
    }

    public static void ShowAchievementsUI()
    {
#if UNITY_ANDROID
        Debug.Log("Achivements");
        Social.ShowAchievementsUI();
#endif
    }

    public static void AddScoreToLeaderboard(string leaderboardId, long score)
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.ReportScore(score, leaderboardId, success => { });
#endif
    }

    public static void ShowLeaderboardsUI()
    {
#if UNITY_ANDROID
        Debug.Log("Leaderboard");
        PlayGamesPlatform.Instance.ShowLeaderboardUI();
#endif
    }
}
