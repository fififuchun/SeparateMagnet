using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Kento", menuName = "Create RPGSO")]
public class RPGSO : ScriptableObject
{
    //ここにセーブしたいデータ
    public int[] angerLimitCoinTable = { 100, 1000, 10000 };

    public int[] angerTimeCoinTable = { /*1,*/ 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };

    public int[] lateAngerTimeCoinTable = { 10, 30, 50, 100, 300, 500, 1000, 3000, 5000, 10000 };

    public int[] holdTimeCoinTable = { 10, 50, 100, 500, 1000 };

    // public int[] angerRateCoinTable= {}

    public int[] nextKentoTimeCoinTable = { 10, 50, 100, 500, 1000 };

    // public int[] rareRate= {}
}
