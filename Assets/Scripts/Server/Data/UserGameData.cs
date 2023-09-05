[System.Serializable]
public class UserGameData
{
    public int level;
    public float experience;
    public int gold;
    public int jewel;
    public int dailyBestScore; //랭킹 테스트용으로 제작함


    public void Reset()
    {
        level = 1;
        experience = 0;
        gold = 0;
        jewel = 0;
        dailyBestScore = 0;
    }
}
