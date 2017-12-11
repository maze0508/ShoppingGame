/**********************************************************
// Author   : K.(k79k06k02k)
// FileName : Utility.cs
**********************************************************/
using UnityEngine;
using System.Text;
using System.IO;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Reflection;
using System.Security.Cryptography;

public class Utility
{

    public struct SystemRelate
    {
        private const string LOG_COLOR_001 = "fuchsia";
        private const string LOG_COLOR_002 = "lime";
        private const string LOG_COLOR_003 = "orange";

        //http://docs.unity3d.com/Manual/StyledText.html
        //Debug.Log(Utility.SystemRelate.LogServer("Msg"));
        public static string LogServer(object _Msg)
        {
            return "<color=" + LOG_COLOR_001 + ">" + _Msg + "</color>";
        }
        //Debug.Log(Utility.SystemRelate.LogSystem("Msg"));
        public static string LogSystem(object _Msg)
        {
            return "<color=" + LOG_COLOR_002 + ">" + _Msg + "</color>";
        }
        //Debug.Log(Utility.SystemRelate.LogColor("Msg"));
        public static string LogColor(object _Msg)
        {
            return "<color=" + LOG_COLOR_003 + ">" + _Msg + "</color>";
        }

        /// <summary>
        /// ���o�t�θ�T
        /// </summary>
        public static string GetSystemInfo()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("CPU�����G" + SystemInfo.processorType + "\n");
            sb.Append("cores�֤߼ơG" + SystemInfo.processorCount + "\n");
            sb.Append("RAM���s(MB)�G" + SystemInfo.systemMemorySize + "\n");
            sb.Append("��d�����G" + SystemInfo.graphicsDeviceName + "\n");
            sb.Append("�e���e�G" + Screen.width + "\n");
            sb.Append("�e�����G" + Screen.height + "\n");
            sb.Append("�e����s�v�G" + Screen.currentResolution.refreshRate + "\n");
            sb.Append("VRAM��s�G�G" + SystemInfo.graphicsMemorySize + "\n");

            return sb.ToString();

        }
    }



    public struct Program
    {
        public class KeyData
        {
            public KeyData(string from, string to)
            {
                this.from = from;
                this.to = to;
            }

            public string from;
            public string to;
        }
        public static void RenameKey<TKey, TValue>(IDictionary<TKey, TValue> dic, TKey fromKey, TKey toKey)
        {
            TValue value = dic[fromKey];
            dic.Remove(fromKey);
            dic[toKey] = value;
        }
    }



    public struct TypeRelate
    {
        public static T GetDefaultValue<T>()
        {
            return default(T);
        }

        public static bool StringToBool(string value)
        {
            if (value == "T")
                return true;
            else if (value == "F")
                return false;
            else
            {
                Debug.LogError(string.Format("Unable to convert value:[{0}]", value));
                return false;
            }
        }
        public static Vector2 StringToVector2(string value)
        {
            string[] str = value.Split(',');
            return new Vector2(float.Parse(str[0]), float.Parse(str[1]));
        }
        public static Vector3 StringToVector3(string value)
        {
            string[] str = value.Split(',');
            return new Vector3(float.Parse(str[0]), float.Parse(str[1]), float.Parse(str[2]));
        }
    }



    public struct SecureRelate
    {
        /// <summary>
        /// ���oMD5�[�K�r��
        /// </summary>
        /// <param name="ConvertString">�n�[�K�r��</param>
        /// <param name="isShort">���  True:16��  False:32��</param>
        /// <param name="isToUpper">�j�p�g True:�j�g  False:�p�g</param>
        public static string MD5Type(string ConvertString, bool isShort = false, bool isToUpper = false)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string md5Str = string.Empty;

            if (isShort)
                md5Str = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);
            else
                md5Str = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)));

            md5Str = md5Str.Replace("-", "");

            if (isToUpper)
                return md5Str.ToUpper();
            else
                return md5Str.ToLower();
        }
    }



    public struct UGUIRelate
    {
        /// <summary>
        ///  �P�_�O�_�I��UI
        /// </summary>
        /// <returns>True:��  False:�L</returns>
        public static bool IsPointerOverUIObject()
        {
            // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
            // the ray cast appears to require only eventData.position.
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            return results.Count > 0;
        }
        public static bool IsPointerOverUIObject(Canvas canvas, Vector2 screenPosition)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = screenPosition;

            GraphicRaycaster uiRaycaster = canvas.gameObject.GetComponent<GraphicRaycaster>();
            List<RaycastResult> results = new List<RaycastResult>();
            uiRaycaster.Raycast(eventDataCurrentPosition, results);

            return results.Count > 0;
        }

        /// <summary>
        /// UI �g�u�˴�
        /// </summary>
        public static List<RaycastResult> UIRaycast(Canvas canvas, Vector2 screenPosition)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = screenPosition;

            GraphicRaycaster uiRaycaster = canvas.gameObject.GetComponent<GraphicRaycaster>();
            List<RaycastResult> results = new List<RaycastResult>();
            uiRaycaster.Raycast(eventDataCurrentPosition, results);

            return results;
        }
    }



    public struct PlayerPrefab
    {
        public static bool GetBool(string tag, bool def = false)
        {
            return bool.Parse(PlayerPrefs.GetString(tag, def.ToString()));
        }
        public static void SetBool(string tag, bool def)
        {
            PlayerPrefs.SetString(tag, def.ToString());
        }
    }



    public struct GameObjectRelate
    {
        /// <summary>
        /// �M����������U�l����
        /// </summary>
        public static void ClearChildren(Transform Obj)
        {
            for (int i = Obj.childCount - 1; i >= 0; --i)
            {
                Transform Item = Obj.GetChild(i);
                Item.SetParent(null);
                MonoBehaviour.DestroyImmediate(Item.gameObject);
            }
        }

        /// <summary>
        /// �b������U�إߤl����(�ϥΦW�٫إߤ@�ӷs����)
        /// </summary>
        public static GameObject InstantiateGameObject(GameObject parent, string name)
        {
            GameObject go = new GameObject(name);

            if (parent != null)
            {
                Transform t = go.transform;
                t.SetParent(parent.transform);
                t.localPosition = Vector3.zero;
                t.localRotation = Quaternion.identity;
                t.localScale = Vector3.one;

                RectTransform rect = go.transform as RectTransform;
                if (rect != null)
                {
                    rect.anchoredPosition = Vector3.zero;
                    rect.localRotation = Quaternion.identity;
                    rect.localScale = Vector3.one;

                    //�P�_anchor�O�_�b�P�@�I
                    if (rect.anchorMin.x != rect.anchorMax.x && rect.anchorMin.y != rect.anchorMax.y)
                    {
                        rect.offsetMin = Vector2.zero;
                        rect.offsetMax = Vector2.zero;
                    }
                }

                go.layer = parent.layer;
            }
            return go;
        }

        /// <summary>
        /// �b������U�إߤl����
        /// </summary>
        public static GameObject InstantiateGameObject(GameObject parent, GameObject prefab)
        {

            GameObject go = GameObject.Instantiate(prefab) as GameObject;

            if (go != null && parent != null)
            {
                Transform t = go.transform;
                t.SetParent(parent.transform);
                t.localPosition = Vector3.zero;
                t.localRotation = Quaternion.identity;
                t.localScale = Vector3.one;

                RectTransform rect = go.transform as RectTransform;
                if (rect != null)
                {
                    rect.anchoredPosition = Vector3.zero;
                    rect.localRotation = Quaternion.identity;
                    rect.localScale = Vector3.one;

                    //�P�_anchor�O�_�b�P�@�I
                    if (rect.anchorMin.x != rect.anchorMax.x && rect.anchorMin.y != rect.anchorMax.y)
                    {
                        rect.offsetMin = Vector2.zero;
                        rect.offsetMax = Vector2.zero;
                    }
                }

                go.layer = parent.layer;
            }
            return go;
        }

        /// <summary>
        /// �d�ߤl����
        /// </summary>
        public static Transform SearchChild(Transform target, string name)
        {
            if (target.name == name) return target;

            for (int i = 0; i < target.childCount; ++i)
            {
                var result = SearchChild(target.GetChild(i), name);

                if (result != null) return result;
            }

            return null;
        }
        /// <summary>
        /// �d�ߦh�Ӥl����
        /// </summary>
        public static List<Transform> SearchChildsPartName(Transform target, string name)
        {
            List<Transform> objs = new List<Transform>();
            Transform child = null;

            for (int i = 0; i < target.childCount; ++i)
            {
                child = target.GetChild(i);

                if (child != null)
                {
                    if (child.name.IndexOf(name, 0) >= 0)
                        objs.Add(child);
                }
            }

            return objs;
        }

        /// <summary>
        /// �ϥ�GetInstance���GameObject
        /// </summary>
        public static bool CompareGameObject(GameObject A, GameObject B)
        {
            return A.GetInstanceID() == B.GetInstanceID() ? true : false;
        }

        /// <summary>
        /// GameObject Array ���}/����
        /// </summary>
        public static void SetObjectArrayActive(GameObject[] gos, bool isActive)
        {
            for (int i = 0; i < gos.Length; i++)
                gos[i].SetActive(isActive);
        }

        /// <summary>
        /// GameObject �}��
        /// </summary>
        public static void SetObjectActiveToggle(GameObject go)
        {
            go.SetActive(!go.activeSelf);
        }


        public delegate void SmallTabHandler();
        /// <summary>
        /// GameObject Array ���@��Active�A��LInActive
        /// </summary>
        /// <param name="gos">GameObject Array<</param>
        /// <param name="id">�ĴX��index Active</param>
        /// <param name="callback">���槹callback</param>
        public static GameObject SetObjectArrayOneActive(GameObject[] gos, int id, SmallTabHandler callback = null)
        {
            foreach (GameObject go in gos)
            {
                if (go != null)
                    go.SetActive(false);
            }


            if (callback != null)
            {
                callback();
            }


            if (id == -1)
                return null;

            gos[id].SetActive(true);

            return gos[id];
        }

        /// <summary>
        /// GameObject Array �Ƨ�
        /// </summary>
        public static void SortGameObjectArray(ref GameObject[] gos)
        {
            System.Array.Sort(gos, (a, b) => a.name.CompareTo(b.name));
        }

        /// <summary>
        /// GameObject Child �Ƨ�
        /// </summary>
        public static void SortHierarchyObjectChildByName(Transform parent)
        {
            List<Transform> children = new List<Transform>();
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                {
                    Transform child = parent.GetChild(i);
                    children.Add(child);
                    child.parent = null;
                }
            }

            children.Sort((Transform t1, Transform t2) => { return t1.name.CompareTo(t2.name); });
            foreach (Transform child in children)
            {
                child.parent = parent;
            }
        }

        /// <summary>
        /// �ϥΤw�s�b�� Compoent �[�J GameObject
        /// </summary>
        public static T AddComponent<T>(GameObject go, T toAdd) where T : Component
        {
            return GetCopyOf(go.AddComponent<T>(), toAdd) as T;
        }
        public static T GetCopyOf<T>(Component comp, T other) where T : Component
        {
            Type type = comp.GetType();
            if (type != other.GetType()) return null; // type mis-match
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            PropertyInfo[] pinfos = type.GetProperties(flags);
            foreach (var pinfo in pinfos)
            {
                if (pinfo.CanWrite)
                {
                    try
                    {
                        pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                    }
                    catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
                }
            }
            FieldInfo[] finfos = type.GetFields(flags);
            foreach (var finfo in finfos)
            {
                finfo.SetValue(comp, finfo.GetValue(other));
            }
            return comp as T;
        }

        /// <summary>
        /// ���ܪ���layer (�]�t�Ҧ��l����)
        /// </summary>
        public static void SetLayerRecursively(GameObject go, int layerNumber)
        {
            if (go == null) return;
            foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
            {
                trans.gameObject.layer = layerNumber;
            }
        }
    }



    public struct TransformRelate
    {
        /// <summary>
        /// �N�@�ɮy���ରCanvas�y��
        /// </summary>
        /// <param name="canvas">�ؼ�Canvas</param>
        /// <param name="world_position">�@�ɮy�Ц�m</param>
        /// <param name="camera">�ؼ�camera</param>
        /// <returns></returns>
        public static Vector2 WorldToCanvas(Canvas canvas, Vector3 world_position, Camera camera = null)
        {
            if (camera == null)
            {
                camera = Camera.main;
            }

            var viewport_position = camera.WorldToViewportPoint(world_position);
            var canvas_rect = canvas.GetComponent<RectTransform>();

            return new Vector2((viewport_position.x * canvas_rect.sizeDelta.x) - (canvas_rect.sizeDelta.x * 0.5f),
                               (viewport_position.y * canvas_rect.sizeDelta.y) - (canvas_rect.sizeDelta.y * 0.5f));
        }

        /// <summary>
        /// ����Vector3
        /// </summary>
        public static Vector3 ChangeVector3(Vector3 org, object x = null, object y = null, object z = null)
        {
            return new Vector3((x == null ? org.x : (float)x), (y == null ? org.y : (float)y), (z == null ? org.z : (float)z));
        }
        /// <summary>
        /// Vector3 �����
        /// </summary>
        public static Vector3 Vector3Abs(Vector3 org)
        {
            return new Vector3(Mathf.Abs(org.x), Mathf.Abs(org.y), Mathf.Abs(org.z));
        }
        /// <summary>
        /// Vector3 ���H value
        /// </summary>
        public static Vector3 Vector3Division(Vector3 org, float value)
        {
            return new Vector3(org.x / value, org.y / value, org.z / value);
        }

        /// <summary>
        /// Transform ���mPosition�BRotation�BScale
        /// </summary>
        public static void ResetTransform(Transform trans)
        {
            trans.localRotation = Quaternion.identity;
            trans.localPosition = Vector3.zero;
            trans.localScale = Vector3.one;
        }

        /// <summary>
        /// RectTransform ���m AnchoredPosition�BAnchor�BPivot�BRotation�BScale
        /// </summary>
        public static void ResetRectTransform(RectTransform rectTrans)
        {
            rectTrans.anchoredPosition = Vector3.zero;
            rectTrans.anchorMin = Vector2.one * 0.5f;
            rectTrans.anchorMax = Vector2.one * 0.5f;
            rectTrans.pivot = Vector2.one * 0.5f;
            rectTrans.localRotation = Quaternion.identity;
            rectTrans.localScale = Vector3.one;
        }

        public enum FacingDirection
        {
            UP = 270,
            DOWN = 90,
            LEFT = 180,
            RIGHT = 0
        }
        /// <summary>
        /// ���o���I Quaternion
        /// </summary>
        /// <param name="startingPosition">�}�l��m</param>
        /// <param name="targetPosition">�ؼЦ�m</param>
        /// <param name="facing">��V</param>
        public static Quaternion LookAt2D(Vector2 startingPosition, Vector2 targetPosition, FacingDirection facing)
        {
            Vector2 direction = targetPosition - startingPosition;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle -= (float)facing;

            return Quaternion.AngleAxis(angle, Vector3.forward);
        }

        /// <summary>
        /// ���o���I ����
        /// </summary>
        /// <param name="startingPosition">�}�l��m</param>
        /// <param name="targetPosition">�ؼЦ�m</param>
        /// <param name="facing">��V</param>
        public static Vector3 LookAt2DAngle(Vector2 startingPosition, Vector2 targetPosition, FacingDirection facing)
        {
            return LookAt2D(startingPosition, targetPosition, facing).eulerAngles;
        }
    }



    public struct SpriteRelate
    {
        /// <summary> 
        /// �]�wsprite sortingOrder
        /// </summary>
        public static void SetDepth(SpriteRenderer sprite, int depth)
        {
            if (sprite == null)
            {
                Debug.LogError("SpriteRenderer is Null!!");
                return;
            }

            sprite.sortingOrder = depth;
        }
    }



    public struct TimeRelate
    {
        /// <summary>
        /// ��ɶ��t�Z�A�����ɶ�(�ǤJ�ɶ�) - �{�b�ɶ�
        /// </summary>
        /// <returns>�p�G "�{�b�ɶ�" �w�g�W�L "�����ɶ�" �^�� TimeSpan.Zero</returns>
        public static TimeSpan TimeGap(DateTime timeComplete)
        {
            if (DateTime.Compare(DateTime.Now, timeComplete) > 0)
                return TimeSpan.Zero;
            else
                return timeComplete.Subtract(DateTime.Now);
        }

        /// <summary>
        /// �z�L�ɶ��]�w Text�BSlider
        /// </summary>
        /// <param name="_slider">UI Slider</param>
        /// <param name="_text">UI Text</param>
        /// <param name="completeMsg">��������ܤ�r</param>
        /// <param name="completeTime">�����ɶ�</param>
        /// <param name="totalTime">�`�@�ɶ�(��)</param>
        /// <returns>�p�G "�{�b�ɶ�" �w�g�W�L "�����ɶ�" �^�� TimeSpan.Zero</returns>
        public static TimeSpan SetTimeUI(Slider _slider, Text _text, string completeMsg, DateTime completeTime, float totalTime)
        {
            //��Ѿl�ɶ�
            TimeSpan lastTime = TimeRelate.TimeGap(completeTime);

            //�ɶ���
            if (lastTime == TimeSpan.Zero)
            {
                if (_text != null)
                    _text.text = completeMsg;

                if (_slider != null)
                    _slider.value = 1;
            }
            else
            {
                if (_text != null)
                    _text.text = FormatRelate.TimeSpanToString(lastTime);

                if (_slider != null)
                    _slider.value = 1 - ((float)lastTime.TotalSeconds / (totalTime * 60));
            }

            return lastTime;
        }
    }



    public struct FormatRelate
    {
        /// <summary>
        /// �ھڼƭȦ^�� B M K �榡�r��
        /// EX: 5.23B�B18.7M�B152K
        /// </summary>
        public static string ValueToString(int value)
        {
            float fValue = 0.0f;
            int iValue = 0;
            if (Mathf.Abs(value) < 1000)
            {
                return value.ToString();
            }
            else if (Mathf.Abs(value) < 10000)
            {
                iValue = (value * 100) / 1000;
                fValue = iValue / 100.0f;
                return string.Format("{0:0.00}K", fValue);
            }
            else if (Mathf.Abs(value) < 100000)
            {
                iValue = (value * 10) / 1000;
                fValue = iValue / 10.0f;
                return string.Format("{0:00.0}K", fValue);
            }
            else if (Mathf.Abs(value) < 1000000)
            {
                iValue = value / 1000;
                return string.Format("{0}K", iValue);
            }
            else if (Mathf.Abs(value) < 10000000)
            {
                iValue = (value * 100) / 1000000;
                fValue = iValue / 100.0f;
                return string.Format("{0:0.00}M", fValue);
            }
            else if (Mathf.Abs(value) < 100000000)
            {
                iValue = (value * 10) / 1000000;
                fValue = iValue / 10.0f;
                return string.Format("{0:00.0}M", fValue);
            }
            else if (Mathf.Abs(value) < 1000000000)
            {
                iValue = value / 1000000;
                return string.Format("{0}M", iValue);
            }
            else if (Mathf.Abs((long)value) < 10000000000)
            {
                iValue = (int)((value * 100) / 1000000000);
                fValue = iValue / 100.0f;
                return string.Format("{0:0.00}B", fValue);
            }


            return value.ToString();
        }


        /// <summary>
        /// �NTimeSpan�ର �ɡB���B��A������줣���
        /// EX�G 50:18:29
        /// </summary>
        public static string TimeSpanToString(TimeSpan span)
        {
            if (span.TotalHours > 1)
                return string.Format("{0:00}:{1:00}:{2:00}", span.Hours, span.Minutes, span.Seconds);
            else if (span.TotalMinutes > 1)
                return string.Format("{0:00}:{1:00}", span.Minutes, span.Seconds);
            else
                return string.Format("{0:00}", span.Seconds);
        }
        /// <summary>
        /// �NTimeSpan�ର �ѡB�ɡB���B��A������줣���
        /// EX�G 5d 6h 30m 45s
        /// </summary>
        public static string TimeSpanToStringLetter(TimeSpan span)
        {
            if (span.TotalDays > 1)
                return string.Format("{0}d {1}h {2}m {3}s", span.Days, span.Hours, span.Minutes, span.Seconds);
            else if (span.TotalHours > 1)
                return string.Format("{0}h {1}m {2}s", span.Hours, span.Minutes, span.Seconds);
            else if (span.TotalMinutes > 1)
                return string.Format("{0}m {1}s", span.Minutes, span.Seconds);
            else
                return string.Format("{0}s", span.Seconds);
        }
    }



    public struct AssetRelate
    {
        /// <summary>
        /// Resources.Load ���ˬd�O�_null
        /// </summary>
        public static T ResourcesLoadCheckNull<T>(string name) where T : UnityEngine.Object
        {
            T loadGo = Resources.Load<T>(name);

            if (loadGo == null)
            {
                Debug.LogError("Resources.Load [ " + name + " ] is Null !!");
                return default(T);
            }

            return loadGo;
        }

        /// <summary>
        /// Resources.Load Sprite
        /// </summary>
        public static Sprite ResourcesLoadSprite(string name)
        {
            return ResourcesLoadCheckNull<Sprite>("Sprites/" + name);
        }

        /// <summary>
        /// ŪTXT��
        /// </summary>
        public static void LoadFile(string path)
        {
            string strTemp;
            TextAsset data = null;
            TextReader reader = null;

            data = Resources.Load(path, typeof(TextAsset)) as TextAsset;

            if (data != null)
                reader = new StringReader(data.text);

            if (reader != null)
            {
                while ((strTemp = reader.ReadLine()) != null)
                {
                    Debug.Log(strTemp);
                }

                reader.Close();
            }
        }
    }



    public struct OtherRelate
    {
        /// <summary>
        /// �P�_�O�_�OURL
        /// </summary>
        public static bool IsUrl(string url)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(url, RegularExp.Url);
        }
        public struct RegularExp
        {
            public const string Url = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
        }


        /// <summary>
        /// ���ð}�C������
        /// </summary>
        public static void Shuffle<T>(T[] Source)
        {
            if (Source == null) return;

            int len = Source.Length;

            int r;

            //�Ȧs��
            T tmp;

            for (int i = 0; i < len - 1; i++)
            {
                //���üơA�d��]�t�̤p�ȡA���]�t�̤j��
                r = UnityEngine.Random.Range(i, len);

                //�p�G�@�˫h����            
                if (i == r) continue;

                //���üƫ᪺���޻P��Ӫ��洫
                tmp = Source[i];
                Source[i] = Source[r];
                Source[r] = tmp;
            }
        }
    }


}