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
}
