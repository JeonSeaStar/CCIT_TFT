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
        //���� �� �ִ� Ȥ�� �߼��� �Ϸ�� ������ �ִ��� ����Ʈ ��
        if(Input.GetKeyDown("1"))
        {
            PostListGet(PostType.Admin);
        }
        //���� �� �ִ� index��° ���� ����
        else if(Input.GetKeyDown("2"))
        {
            PostReceive(PostType.Admin, 0);
        }
        else if(Input.GetKeyDown("3"))
        {
            PostRecevieAll(PostType.Admin);
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
                             Debug.LogWarning($"���� �������� �ʴ� ��Ʈ �����Դϴ�. : {itemJson["chartName"].ToString()}");

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

    //���� ���� ����
    public void PostReceive(PostType postType, int index)
    {
        if(postList.Count <= 0)
        {
            Debug.LogWarning("���� �� �ִ� ������ �������� �ʽ��ϴ�. Ȥ�� ���� ����Ʈ �ҷ����⸦ ���� ȣ�����ּ���.");
            return;
        }

        if(index >= postList.Count)
        {
            Debug.LogError($"�ش� ������ �������� �ʽ��ϴ�. : ��û index{index} / ���� �ִ� ���� : {postList.Count}");
            return;
        }

        Debug.Log($"{postType.ToString()}�� {postList[index].inDate} ��������� �䱸�մϴ�.");

        Backend.UPost.ReceivePostItem(postType, postList[index].inDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"{postType.ToString()}�� {postList[index].inDate} ������� �� ������ �߻��߽��ϴ�. : {callback}");
                return;
            }

            Debug.Log($"{postType.ToString()}�� {postList[index].inDate} ������ɿ� �����߽��ϴ�. : {callback}");

            postList.RemoveAt(index);

            //���� ������ �������� ���� ��
            if(callback.GetFlattenJSON()["postItems"].Count > 0)
            {
                //������ ����
                SavePostToLocal(callback.GetFlattenJSON()["postItems"]); //������ ������ �� �������� Ű ���� Items�� �ƴ�["postItems"] �̴�
                //�÷��̾��� ��ȭ ������ ������ ������Ʈ
                BackendGameData.Instance.GameDataUpdate();
            }

            //������ ������ �� JsonData�� "postItems"�� ���� ���� ���� �Ѵ� %%%%�߿�%%%%
        });
    }

    //���� ��Ʈ�� ������ ������ �����ϰ� ���󰡾��� JSON �����Ϳ� �����ϴ� �޼���
    public void SavePostToLocal(LitJson.JsonData item)
    {
        //JSON ������ �Ľ� ����
        try
        {
            //%%JSON �����͸� �ٷ궧 ������ ��%% ��Ʈ�� �̸��� ������ �̸��� ����ؾ� �Ѵ�. ��ҹ���, string�� �̸����� �����ؾ� ��
            foreach (LitJson.JsonData itemJson in item)
            {
                //��Ʈ ���� �̸�(*.xlsx)�� Backend Console�� ����� ��Ʈ �̸�
                string chartFileName = itemJson["item"]["chartFileName"].ToString();
                string chartName = itemJson["chartName"].ToString();

                //GoodsChart.xlsx�� ����� ù��° �� �̸�
                int itemId = int.Parse(itemJson["item"]["itemId"].ToString());
                string itemName = itemJson["item"]["itemName"].ToString();
                string itemInfo = itemJson["item"]["ItemInfo"].ToString();

                //������ �߼��� �� �ۼ��ϴ� ������ ����
                int itemCount = int.Parse(itemJson["itemCount"].ToString());

                //�������� ���� ��ȭ�� ���� �� �����Ϳ� ����
                if(chartName.Equals(Constants.GOODS_CHART_NAME))
                {
                    if(itemName.Equals("gold"))
                    {
                        BackendGameData.Instance.UserGameData.gold += itemCount;
                    }
                    else if(itemName.Equals("jewel"))
                    {
                        BackendGameData.Instance.UserGameData.jewel += itemCount;
                    }
                }

                Debug.Log($"{chartName} - {chartFileName}");
                Debug.Log($"[{itemId}] {itemName} : {itemInfo}, ȹ�� ���� : {itemCount}");
                Debug.Log($"�������� �����߽��ϴ�. : {itemName} - {itemCount}��");
            }
        }
        //JSON ������ �Ľ� ����
        catch(System.Exception e)
        {
            //try-catch ���� ���
            Debug.LogError(e);
        }
    }

    //��� ������ ����
    public void PostRecevieAll(PostType postType)
    {
        if(postList.Count <= 0)
        {
            Debug.LogWarning("���� �� �ִ� ������ �������� �ʽ��ϴ�. Ȥ�� ���� ����Ʈ �ҷ����⸦ ���� ȣ�����ּ���.");
            return;
        }

        Debug.Log($"{postType.ToString()} ���� ��ü ������ ��û�մϴ�.");

        Backend.UPost.ReceivePostItemAll(postType, callback =>
        {
            if(!callback.IsSuccess())
            {
                Debug.LogError($"{postType.ToString()} ���� ��ü ���� �� ������ �߻��߽��ϴ�. : {callback}");
                return;
            }

            Debug.Log($"���� ��ü ���ɿ� �����߽��ϴ�. : {callback}");

            postList.Clear(); // ��� ������ �����߱� ������ postList�� �ʱ�ȭ�Ѵ�.

            //��� ������ ������ ����
            foreach(LitJson.JsonData postItemsJson in callback.GetFlattenJSON()["postItems"]) //0�� ������ ������ = callback.GetFlattenJSON()["postItems"][0] �̴�
            {
                SavePostToLocal(postItemsJson);
            }

            //�÷��̾��� ��ȭ ������ ������ ������Ʈ
            BackendGameData.Instance.GameDataUpdate();
        });

    }
}
