using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Library// : MonoBehaviour
{
    /// <summary>
    /// int配列の中で一番初めのnumberのindexを返します
    /// </summary>
    /// <param name="ints"></param>
    /// <param name="number"></param>
    /// <returns></returns>
    public static int SearchNumberIndex(int[] ints, int number)
    {
        for (int i = 0; i < ints.Length; i++) if (ints[i] == number) return i;
        return -1;
    }

    /// <summary>
    /// 特性関数、bool配列に対してTrueの数を返します
    /// </summary>
    /// <param name="bools"></param>
    /// <returns></returns>
    public static int CharacteristicFanction(bool[] bools)
    {
        int count = 0;
        for (int i = 0; i < bools.Length; i++) if (bools[i]) count++;
        return count;
    }
}
