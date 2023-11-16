using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Kento", menuName = "Create RPGSO")]
public class RPGSO : ScriptableObject
{
    //ここにセーブしたいデータ
    public static int[] angerLimitCoinTable = { 100, 1000, 10000 };

    public static int[] angerTimeCoinTable = { /*1,*/ 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };

    public static int[] lateAngerTimeCoinTable = { 10, 30, 50, 100, 300, 500, 1000, 3000, 5000, 10000 };

    public static int[] holdTimeCoinTable = { 10, 50, 100, 500, 1000 };

    public static int[] angerRateCoinTable = { };

    public static int[] nextKentoTimeCoinTable = { 10, 50, 100, 500, 1000 };

    public static int[] rareRate = { };


    public List<int[]> tables = new List<int[]>() { angerLimitCoinTable, angerTimeCoinTable, lateAngerTimeCoinTable, holdTimeCoinTable, angerRateCoinTable, nextKentoTimeCoinTable, rareRate };

    public int AcquireCoin(int tableNum, int level)
    {
        if (tableNum == 4) return (level + 2) * 100;
        if (tableNum == 6) return (level + 1) * 10;
        if (0 <= tableNum && tableNum < 6 && level < tables[tableNum].Count()) return tables[tableNum][level];

        return 999;
    }
}
