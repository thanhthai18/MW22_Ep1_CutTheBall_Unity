﻿using UnityEngine;

namespace UnityScreenNavigator.Runtime.Foundation
{
    internal static class RectTransformExtensions
    {
        public static void FillParent(this RectTransform self, RectTransform parent)
        {
            self.SetParent(parent);
            self.localPosition = Vector3.zero;
            self.anchorMin = Vector2.zero;
            self.anchorMax = Vector2.one;
            self.offsetMin = Vector2.zero;
            self.offsetMax = Vector2.zero;
            self.pivot = new Vector2(0.5f, 0.5f);
            self.rotation = Quaternion.identity;
            self.localScale = Vector3.one;
        }

        public static void FillParentAndScale(this RectTransform self, RectTransform parent, float bonusScale)
        {
            FillParent(self, parent);
            self.localScale = Vector3.one * (1 + bonusScale);
        }

        public static void RemoveChild(this Transform transform, Transform child, bool worldPositionStays = false)
        {
            if (child == false || !transform.Equals(child.parent))
                return;

            child.SetParent(null, worldPositionStays);
        }

        public static void AddChild(this Transform transform, Transform child, bool worldPositionStays = false)
        {
            if (child == false || transform.Equals(child.parent))
                return;

            child.gameObject.layer = transform.gameObject.layer;
            child.SetParent(transform, worldPositionStays);
            child.SetAsLastSibling();
        }
    }
}