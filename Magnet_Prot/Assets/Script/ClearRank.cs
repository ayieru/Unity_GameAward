using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ClearRank : MonoBehaviour
{
    [Header("ステージの総数")]
    public static  int TortalStage = 7;

    string filePath;
    ClearData Clear;

    [System.Serializable]
    public class ClearData
    {
        public static int[] Rank = new int[TortalStage];
    }

    void Awake()
    {

        // Assets/Json/rank.json 
        filePath = Application.dataPath + "/Json/rank.json";
    }

    //ランクをセーブする
    //引数：StageNum..保存するステージのIndex
    //      RankNum ..ランクに対応し値（S..1,）
    public void Save(int StageNum,int RankNum)
    {
        if(StageNum > TortalStage - 1)
        {
            return;
        }

        //クリアランクを保存
        ClearData.Rank[StageNum] = RankNum;

        string json = JsonUtility.ToJson(ClearData.Rank[StageNum], true);
        StreamWriter streamWriter = new StreamWriter(filePath);
        streamWriter.Write(json);
        streamWriter.Flush();
        streamWriter.Close();
    }

    public int Load(int StageNum)
    {
        if (File.Exists(filePath))
        {
            StreamReader streamReader;
            streamReader = new StreamReader(filePath);
            string data = streamReader.ReadToEnd();
            streamReader.Close();
            Clear = JsonUtility.FromJson<ClearData>(data);

            return ClearData.Rank[StageNum];
        }
        else
        {
            return 0;
        }
    }
}
