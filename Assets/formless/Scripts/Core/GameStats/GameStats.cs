public class GameStats
{
    public int EnemiesKilled { get; private set; }
    public int ClearedRooms { get; private set; }
    public int HeartsCollected { get; private set; }
    public int KeysCollected { get; private set; }
    public float PlayTime { get; private set; }

    private bool _isTrackingTime;

    public void StartTrackingTime()
    {
        _isTrackingTime = true;
        PlayTime = 0;
    }

    public void StopTrackingTime()
    {
        _isTrackingTime = false;
    }

    public void UpdateTime(float deltaTime)
    {
        if (_isTrackingTime)
        {
            PlayTime += deltaTime;
        }
    }

    public void EnemyKilled()
    {
        EnemiesKilled++;
    }

    public void RoomCleared()
    {
        ClearedRooms++;
    }

    public void HeartCollected()
    {
        HeartsCollected++;
    }

    public void KeyCollected()
    {
        KeysCollected++;
    }

    public void ResetStats()
    {
        EnemiesKilled = 0;
        ClearedRooms = 0;
        HeartsCollected = 0;
        KeysCollected = 0;
        PlayTime = 0;
    }
}
