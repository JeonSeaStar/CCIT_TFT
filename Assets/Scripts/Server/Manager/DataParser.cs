using System.Text;
using UnityEngine;

public class DataParser : MonoBehaviour
{
    #region json
    //데이터 수신
    public static T ReadJsonData<T>(byte[] buf)
    {
        var strByte = Encoding.Default.GetString(buf);
        //byte 배열을 string으로 변환
        return JsonUtility.FromJson<T>(strByte);
    }

    //데이터 전송
    public static byte[] DataToJsonData<T>(T obj)
    {
        var jsonData = JsonUtility.ToJson(obj);
        //string을 byte 배열로 변환
        return Encoding.UTF8.GetBytes(jsonData);
    }
    #endregion
}
