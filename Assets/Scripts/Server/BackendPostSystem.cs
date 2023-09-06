using System.Collections.Generic;
using UnityEngine;
using BackEnd;


/// <summary>
/// ���� ������ ���� ����Ʈ�� �ҷ����� ��ũ��Ʈ
/// </summary>
public class BackendPostSystem : MonoBehaviour
{
    private List<PostData> postList = new List<PostData>();

    private void Update()
    {
        if(Input.GetKeyDown("1"))
        {
            PostListGet(PostType.Admin);
        }
    }

    public void PostListGet(PostType postType)
    {
        Backend.UPost.GetPostList(postType, callback =>
         {
             if(!callback.IsSuccess())
             {
                 Debug.LogError($"���� �ҷ����� �� ������ �߻��߽��ϴ�. : {callback}");
                 return;
             }

             Debug.Log($"���� ����Ʈ �ҷ����� ��û�� �����߽��ϴ�. : {callback}");

             //  JSON ������ �Ľ� ����
             try
             {
                 LitJson.JsonData jsonData = callback.GetFlattenJSON()["postList"];

                 //�޾ƿ� �������� ������ 0�̸� �����Ͱ� ���� ��
                 if(jsonData.Count <= 0)
                 {
                     Debug.LogWarning("�������� ����ֽ��ϴ�.");
                     return;
                 }

                 //�Ź� ���� ����Ʈ�� �ҷ��� �� postList �ʱ�ȭ
                 postList.Clear();

                 //���� ���� ������ ��� ���� ���� �ҷ�����
                 for(int i = 0; i < jsonData.Count; ++i)
                 {
                     PostData post = new PostData();

                     post.title = jsonData[i]["title"].ToString();
                     post.content = jsonData[i]["content"].ToString();
                     post.inDate = jsonData[i]["inDate"].ToString();
                     post.expirationDate = jsonData[i]["expirationDate"].ToString();

                     // ����� �Բ� �߼۵� ��� ������ ���� �ҷ�����
                     foreach(LitJson.JsonData itemJson in jsonData[i]["items"])
                     {
                         //���� �Բ� �߼۵� �������� ��Ʈ �̸��� "��ȭ��Ʈ" �� �� �Ƹ� �� �׽�Ʈ ��ȭ��Ʈ�ϵ�?
                         if(itemJson["chartName"].ToString() == Constants.GOODS_CHART_NAME)
                         {
                             string itmeName = itemJson["item"]["itemName"].ToString();
                             int itemCount = int.Parse(itemJson["itemCount"].ToString());

                             //���� ���Ե� �������� ���� �� �϶�
                             //�̹� postReward�� �ش� ������ ������ ������ ���� �߰�
                             if(post.postReward.ContainsKey(itmeName))
                             {
                                 post.postReward[itmeName] += itemCount;
                             }
                             //postReward�� ���� ������ �����̸� ��� �߰�
                             else
                             {
                                 post.postReward.Add(itmeName, itemCount);
                             }

                             post.isCanReceive = true;
                         }
                         else
                         {
                             Debug.LogWarning($"���� �������� �ʴ� ��Ʈ �����Դϴ�. : {itemJson["charName"].ToString()}");

                             post.isCanReceive = false;
                         }
                     }

                     postList.Add(post);
                 }

                 //������ ������ ��� ����(postList) ���� ���
                 for( int i = 0; i < postList.Count; ++i)
                 {
                     Debug.Log($"{i}��° ����\n{postList[i].ToString()}");
                 }
             }
             //JSON ������ �Ľ� ����
             catch (System.Exception e)
             {
                 Debug.LogError(e);
             }
         });
    }
}
