using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;
using UnityEngine.UI;

namespace Runtime.Utilities
{
    public static class ThaiHelperUtility
    {
        public static void DestroyChildren(this Transform t)
        {
            foreach (Transform child in t) Object.Destroy(child.gameObject);
        }

        public static void Wait(this MonoBehaviour mono, float delay, Action action)
        {
            mono.StartCoroutine(ExecuteAction(delay, action));
        }

        private static IEnumerator ExecuteAction(float delay, Action action)
        {
            yield return new WaitForSecondsRealtime(delay);
            action?.Invoke();
            yield break;
        }
        public static void GetComponentAtPath<T>(
                this Transform transform,
                string path,
                out T foundComponent) where T : Component
        {
            Transform t = null;
            if (path == null)
            {
                // Return the component of the first child that have that type of component
                foreach (Transform child in transform)
                {
                    T comp = child.GetComponent<T>();
                    if (comp != null)
                    {
                        foundComponent = comp;
                        return;
                    }
                }
            }
            else
                t = transform.Find(path);

            if (t == null)
                foundComponent = default(T);
            else
                foundComponent = t.GetComponent<T>();
        }

        public static T GetComponentAtPath<T>(
            this Transform transform,
            string path) where T : Component
        {
            T foundComponent;
            transform.GetComponentAtPath(path, out foundComponent);

            return foundComponent;
        }


        public static bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        public static Vector3 GetMousePosWorld(Camera camera)
        {
            var pos=camera.ScreenToWorldPoint(GetMousePosScreen());
            pos.z = 0;
            return pos;
        }

        public static Vector3 GetMousePosScreen()
        {
#if UNITY_EDITOR
            return Input.mousePosition;
#else
            return Input.touches[0].position;
#endif
        }

        public static float RadianToDegree(float radian)
        {
            return radian * 180f / (float)Math.PI;
        }

        public static float AngleDir(Vector2 A, Vector2 B)
        {
            return (0f - A.x) * B.y + A.y * B.x;
        }

        public static float AngleBetweenVector2(Vector2 vec1, Vector2 vec2, Vector2 dir)
        {
            Vector2 diference = vec2 - vec1;
            float sign = (vec2.y < vec1.y)? -1.0f : 1.0f;
            return Vector2.Angle(dir, diference) * sign;
        }



        private static Vector2 boundSizeHorizontal;
        private static Vector2 boundSizeVerticle;

        public static Vector2 GetBoundSizeHorizontal(Camera camera)
        {
            //if (boundSizeHorizontal == Vector2.zero)
            {
                var botLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
                var topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
                boundSizeHorizontal = new Vector2(botLeft.x, topRight.x);
            }

            return boundSizeHorizontal;
        }

        public static Vector2 GetBoundSizeVertical(Camera camera)
        {
            // if (boundSizeVerticle == Vector2.zero)
            {
                var botLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
                var topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
                boundSizeVerticle = new Vector4(botLeft.y, topRight.y);
            }

            return boundSizeVerticle;
        }


        private static void Complete(UnityAction method)
        {
            if (method != null)
                method.Invoke();
        }

        public static void ChangeColor(this SpriteRenderer sprite, Color newColor)
        {
            if (sprite == null)
                return;
            var color = sprite.color;
            color = newColor;
            sprite.color = color;
        }

        public static void ChangeColor(this Text txt, Color newColor)
        {
            if (txt == null)
                return;
            var color = txt.color;
            color = newColor;
            txt.color = color;
        }

        // public static void ChangeColor(this TextMeshPro txt, float a)
        // {
        //     var color = txt.color;
        //     color.a = a;
        //     txt.color = color;
        // }
        //
        // public static void ChangeColor(this TextMeshPro txt, Color newColor)
        // {
        //     var color = txt.color;
        //     color = newColor;
        //     txt.color = color;
        // }

        public static void ChangeColor(this TextMesh txt, Color newColor)
        {
            var color = txt.color;
            color = newColor;
            txt.color = color;
        }

        public static void ChangeColor(this Image txt, Color newColor)
        {
            var color = txt.color;
            color = newColor;
            txt.color = color;
        }

        public static void ChangeColor(this Renderer render, Color newColor)
        {
            var color = render.material.color;
            color = newColor;
            render.material.color = color;
        }


        public static IEnumerator Delay(float time, Action callBack)
        {
            yield return new WaitForSeconds(time);
            if (callBack != null)
                callBack.Invoke();
        }

        public static IEnumerator ChangeSpeed(this float speed, float v_start, float v_end, float duration)
        {
            float elapsed = 0.0f;
            while (elapsed < duration)
            {
                speed = Mathf.Lerp(v_start, v_end, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            speed = v_end;
        }

        public static Color HexToColor(this string hex)
        {
            Color color;
            if (ColorUtility.TryParseHtmlString(hex, out color))
            {
                return color;
            }
            else
            {
                return Color.white;
            }
        }
    }
  
}
