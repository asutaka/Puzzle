using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ExtensionMethod 
{
    public static void SetTransparency(this Image p_image, float p_transparency)
    {
        if (p_image != null)
        {
            UnityEngine.Color __alpha = p_image.color;
            __alpha.a = p_transparency;
            p_image.color = __alpha;
        }
    }
    public static void SetTransparency(this GameObject obj, float p_transparency)
    {
        if (obj != null)
        {
            foreach (var item in obj.GetAllChilds())
            {
                item.GetComponent<Image>().SetTransparency(p_transparency);
            }
        }
    }
    public static List<GameObject> GetAllChilds(this GameObject Go)
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < Go.transform.childCount; i++)
        {
            list.Add(Go.transform.GetChild(i).gameObject);
        }
        return list;
    }
}
