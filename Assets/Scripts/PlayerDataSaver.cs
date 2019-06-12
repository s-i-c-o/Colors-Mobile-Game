using UnityEngine;

public class PlayerDataSaver : MonoBehaviour
{
    public PlayerSaveData playerData;

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SavePlayerData(PlayerSaveData data)
    {
        PlayerPrefs.SetInt("first_place_score", data.firstPlaceScore);
        PlayerPrefs.SetInt("second_place_score", data.secondPlaceScore);
        PlayerPrefs.SetInt("third_place_score", data.thirdPlaceScore);
        PlayerPrefs.SetInt("recent_score", data.recentScore);

        PlayerPrefs.SetString("first_place_initials", data.firstPlaceInitials);
        PlayerPrefs.SetString("second_place_initials", data.secondPlaceInitials);
        PlayerPrefs.SetString("third_place_initials", data.thirdPlaceInitials);

        PlayerPrefs.Save();
    }

    public static PlayerSaveData LoadPlayerData()
    {
        PlayerSaveData loadedData = new PlayerSaveData();

        loadedData.firstPlaceScore = PlayerPrefs.GetInt("first_place_score");
        loadedData.secondPlaceScore = PlayerPrefs.GetInt("second_place_score");
        loadedData.thirdPlaceScore = PlayerPrefs.GetInt("third_place_score");
        loadedData.recentScore = PlayerPrefs.GetInt("recent_score");

        loadedData.firstPlaceInitials = PlayerPrefs.GetString("first_place_initials");
        loadedData.secondPlaceInitials = PlayerPrefs.GetString("second_place_initials");
        loadedData.thirdPlaceInitials = PlayerPrefs.GetString("third_place_initials");


        return loadedData;
    }
}
