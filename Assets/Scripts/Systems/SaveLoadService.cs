using UnityEngine;

public enum SaveKey
{
    Settings,
    PlayerProgress
}

public static class SaveLoadService
{
    public static void Save<T>(SaveKey key, T data)
    {
        PlayerPrefs.SetString(key.ToString(), JsonUtility.ToJson(data));
    }

    public static T Load<T>(SaveKey key)
    {
        string stringKey = key.ToString();
        if (!PlayerPrefs.HasKey(stringKey))
            return default(T);

        return JsonUtility.FromJson<T>(
            PlayerPrefs.GetString(stringKey)
            );
    }
}