[System.Serializable]
public class UserGameData
{
    public int level;
    public float experience;
    public int gold;
    public int jewel;


    public void Reset()
    {
        level = 1;
        experience = 0;
        gold = 0;
        jewel = 0;

    }
}
