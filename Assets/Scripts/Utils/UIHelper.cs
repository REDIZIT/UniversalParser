using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace InGame
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Type Safety", "UNT0014:Invalid type for call to GetComponent", Justification = "<��������>")]
    public static class UIHelper
    {
        public static void ClearChildren(Transform content)
        {
            foreach (Transform child in content)
            {
                Object.Destroy(child.gameObject);
            }
        }






        public static void FillContent<TUIItem, TList>(Transform content, GameObject prefab, IEnumerable<TList> list, Action<TUIItem, TList> forEachFunc)
        {
            if (list == null) return;

            foreach (TList listItem in list)
            {
                GameObject inst = Object.Instantiate(prefab, content);
                forEachFunc(inst.GetComponent<TUIItem>(), listItem);
            }
        }

        public static void UpdateContent<TUIItem, TList>(Transform content, GameObject prefab, IEnumerable<TList> list, Action<TUIItem, TList> forEachFunc)
        {
            if (list == null)
            {
                ClearChildren(content);
                return;
            }

            int childCountBefore = content.childCount;

            // If in list more elements than UIItems spawned in content
            // Spawn new onces
            if (childCountBefore < list.Count())
            {
                for (int i = 0; i <= list.Count() - childCountBefore; i++)
                {
                    Object.Instantiate(prefab, content);
                }
            }
            // If list has deleted some elements, we need to remove UIItems in content
            if (childCountBefore > list.Count())
            {
                for (int i = 0; i < childCountBefore - list.Count(); i++)
                {
                    Object.Destroy(content.GetChild(childCountBefore - 1).gameObject);
                }
            }


            // Update UIItems. Now list elements and UIItems count are same.
            int li = -1;
            foreach (TList listItem in list)
            {
                li++;
                forEachFunc(content.GetChild(li).GetComponent<TUIItem>(), listItem);
            }
        }
    }
}