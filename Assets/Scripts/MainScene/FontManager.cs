using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FontManager : MonoBehaviour
{
    public Font font;
    
    public enum Font
    {
        //1からに修正したい
        Gothic = 0,
        Gothic_Bold = 1,
        Gothic_Rounded = 2,
        Mincho = 3,
        Gyosho = 4,
        Pop = 5,
        Textbook = 6,
        Textbook_Rounded = 7,
        Kaisho = 8,
        Chark = 9,
        Dot = 10,
        Handwritten = 11,
        Maka = 12,
        Scary = 13,
        Shake = 14,
        Sloppy = 15,
        Strong = 16,
        Thin = 17,
        Unknown = 18,
        Weak = 19,
    }

}
