using System.Collections.Generic;
using UnityEngine;
using BackEnd;


/// <summary>
/// 콘솔에 들록한 모든 차트 데이터를 불러오는 스크립트
/// </summary>
public class BackendChartData
{
    //레벨별 레벨업 필요 경험치와 보상
    public static List<LevelChartData> levelChart;

    static BackendChartData()
    {
        levelChart = new List<LevelChartData>();
    }

    public static void LoadAllChart()
    {
        LoadLevelChart();
    }

    public static void LoadLevelChart()
    {
        //차트 파일의 UUID 혹은 id로 해당 id 값을 가지는 차트 정보를 불러온다
        Backend.Chart.GetChartContents(Constants.LEVEL_CHART, callback =>
        {
            if(callback.IsSuccess())
            {
                //JSON 데이터 파싱 성공
                try
                {
                    LitJson.JsonData jsonData = callback.FlattenRows();

                    //받아온 데이터의 개수가 0이면 데이터가 없는것
                    if(jsonData.Count <= 0 )
                    {
                        Debug.LogWarning("데이터가 존재하지 않습니다.");
                    }
                    else
                    {
                        for (int i = 0; i < jsonData.Count; ++i)
                        {
                            LevelChartData newChart = new LevelChartData();
                            newChart.level = int.Parse(jsonData[i]["level"].ToString());
                            newChart.maxExperience = int.Parse(jsonData[i]["maxExperience"].ToString());
                            newChart.rewardGold = int.Parse(jsonData[i]["rewardGold"].ToString());

                            levelChart.Add(newChart);

                            Debug.Log($"Level : {newChart.level}, Max Exp : {newChart.maxExperience}, Reward Gold : {newChart.rewardGold}");
                        }
                    }
                }
                //JSON 데이터 파싱 실패
                catch(System.Exception e)
                {
                    //Try-catch 에러 출력
                    Debug.LogError(e);
                }
            }
            else
            {
                Debug.LogError($"{Constants.LEVEL_CHART}의 차트 불러오기에 에러 발생 : {callback}");
            }
        });
    }
}

[System.Serializable]
public class LevelChartData
{
    public int level;
    public int maxExperience;
    public int rewardGold;
}
