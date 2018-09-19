using UnityEngine;

[CreateAssetMenu(fileName = "Game Settings", menuName = "Custom Configs/Game Config")]
public class GameSettings : ScriptableObject
{
    [Range(0.1f, 1), Tooltip("Speed of holded Mesh")]
    public float DifficultyLevel = 0.5f;

    [Range(0.1f, 1)]
    public float MusicVolume = 0.5f;

    [Range(0.1f, 1)]
    public float EffectsVolume;
}
