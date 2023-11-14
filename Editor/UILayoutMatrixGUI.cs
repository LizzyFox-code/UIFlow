namespace UIFlow.Editor
{
    using System;
    using System.Reflection;
    using Runtime.Layouts;
    using UnityEditor;
    using UnityEngine;

    public sealed class UILayoutMatrixGUI
    {
        public delegate bool GetValueFunc(int a, int b);
        public delegate void SetValueFunc(int a, int b, bool value);
        
        private static readonly Color m_TransparentColor = new Color(1f, 1f, 1f, 0.0f);
        private static readonly Color m_HighlightColor = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.2f) : new Color(0.0f, 0.0f, 0.0f, 0.2f);
        
        private static GUIStyle m_RightLabel;
        private static GUIStyle m_HoverStyle;
        
        private static readonly Type m_GUIClipType = Type.GetType("UnityEngine.GUIClip, UnityEngine");

        private static readonly PropertyInfo m_TopmostRectPropertyInfo = m_GUIClipType.GetProperty("topmostRect", BindingFlags.Default | BindingFlags.NonPublic | BindingFlags.Static);

        private static readonly MethodInfo m_UnclipMethodInfo =
            m_GUIClipType.GetMethod("Unclip", new[] {typeof(Vector2)});
        
        public void Draw(GetValueFunc getValueFunc, SetValueFunc setValueFunc, int maxLayoutCount, int defaultCount)
        {
            InitializeStylesIfNeed();
            
            var layoutCount = 0;
            for (var layout = 0; layout < maxLayoutCount; layout++)
            {
                if (UILayoutId.LayoutToName((UILayoutId) layout) != string.Empty)
                    layoutCount++;
            }

            var offset = 80;
            for (var layout = 0; layout < maxLayoutCount; layout++)
            {
                var size = GUI.skin.label.CalcSize(new GUIContent(UILayoutId.LayoutToName((UILayoutId) layout)));
                if (size.x > offset)
                    offset = (int)size.x;
            }

            GUILayout.BeginScrollView(new Vector2(0.0f, 0.0f), GUILayout.Height(offset + 15));
            var rect = GUILayoutUtility.GetRect(16 * layoutCount + offset, offset);
            var topmostRect = GetTopmostRect();
            var unclip = Unclip(new Vector2(rect.x - 10.0f, rect.y));

            var currentLayoutIndex = 0;
            for (var layout = 0; layout < maxLayoutCount; layout++)
            {
                var name = UILayoutId.LayoutToName((UILayoutId) layout);
                if (name == string.Empty)
                    continue;

                var flag1 = false;
                var flag2 = false;
                var minWidth = 311;
                var minCount = 10;
                var num6 = offset + 16 * layoutCount + 16;
                if (unclip.x + (num6 - 16 * currentLayoutIndex) <= 0.0)
                    flag1 = true;
                if (rect.height > topmostRect.height)
                    flag2 = true;
                else if (rect.width != topmostRect.width || rect.width != topmostRect.width - unclip.x)
                {
                    if (rect.width > minWidth)
                    {
                        var num7 = rect.width - topmostRect.width;
                        if (num7 > 1.0)
                        {
                            if (unclip.x < 0.0)
                                num7 += unclip.x;
                            if (num7 / 16.0 > currentLayoutIndex)
                                flag2 = true;
                        }
                    }
                    else
                    {
                        var num7 = minWidth;
                        if (layoutCount < minCount)
                            num7 -= 16 * (minCount - layoutCount);
                        if (topmostRect.width + currentLayoutIndex * 16 + 16.0 <= num7)
                            flag2 = true;
                        if (unclip.x < 0.0)
                        {
                            if (rect.width == topmostRect.width - unclip.x)
                                flag2 = false;
                            if (layoutCount <= minCount / 2)
                            {
                                if (num7 - (topmostRect.width - (unclip.x - 10.0) * (currentLayoutIndex + 1)) < 0.0)
                                    flag2 = false;
                            }
                            else if ((num7 - topmostRect.width) / 16 - (unclip.x * -1.0 + 12.0) / 16 < currentLayoutIndex)
                                flag2 = false;
                        }
                    }
                }
                
                GUI.matrix = Matrix4x4.TRS(new Vector3(offset + 30 + 16 * (layoutCount - currentLayoutIndex) + unclip.y + unclip.x + 10.0f, unclip.y, 0.0f), 
                    Quaternion.identity, Vector3.one) * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0.0f, 0.0f, 90f), Vector3.one);
                var labelRect = new Rect(2f - unclip.x, 0.0f, offset, 21f);
                if (flag1 | flag2)
                {
                    GUI.Label(labelRect, GUIContent.none, m_RightLabel);
                }
                else
                {
                    GUI.Label(labelRect, name, m_RightLabel);
                    var hoverRect = new Rect(2f - unclip.x, 1f, offset + 4 + layoutCount * 16, 16f);
                    GUI.Label(hoverRect, GUIContent.none, m_HoverStyle);
                    ref var local = ref hoverRect;
                    var matrix = GUI.matrix;
                    Vector2 position = matrix.MultiplyPoint(new Vector3(hoverRect.position.x, hoverRect.position.y + 200f, 0.0f));
                    matrix = GUI.matrix;
                    Vector2 size = matrix.MultiplyPoint(new Vector3(hoverRect.size.x, hoverRect.size.y, 0.0f));
                    local = new Rect(position, size);
                }
                ++currentLayoutIndex;
            }
            
            GUILayout.EndScrollView();
            GUI.matrix = Matrix4x4.identity;
            var rowCount = 0;
            for (var layout = 0; layout < maxLayoutCount; ++layout)
            {
                var name = UILayoutId.LayoutToName((UILayoutId) layout);
                if (name == string.Empty) 
                    continue;
                
                var columnCount = 0;
                var elementRect = GUILayoutUtility.GetRect(30 + 16 * layoutCount + offset, 16f);
                var labelRect = new Rect(elementRect.x + 30f, elementRect.y, offset, 21f);
                GUI.Label(labelRect, name, m_RightLabel);
                var hoverRect = new Rect(elementRect.x + 30f, elementRect.y, offset + layoutCount * 16, 16f);
                GUI.Label(hoverRect, GUIContent.none, m_HoverStyle);
                for (var inverseLayout = maxLayoutCount-1; inverseLayout >= 0; --inverseLayout)
                {
                    var inverseName = UILayoutId.LayoutToName((UILayoutId) inverseLayout);
                    if (inverseName == string.Empty) 
                        continue;
                        
                    var content = new GUIContent("", $"{name}/{inverseName}");
                    var flag = getValueFunc(layout, inverseLayout);

                    GUI.enabled = layout >= defaultCount;
                    var value = GUI.Toggle(new Rect(offset + 30 + elementRect.x + columnCount * 16, elementRect.y, 16f, 16f), flag, content);
                    if (value != flag)
                    {
                        setValueFunc(layout, inverseLayout, value);
                    }
                    GUI.enabled = true;
                    ++columnCount;
                }
                ++rowCount;
            }
            EditorGUILayout.Space(8f);
        }

        private static void InitializeStylesIfNeed()
        {
            if (m_RightLabel == null)
                m_RightLabel = new GUIStyle("RightLabel");

            if (m_HoverStyle == null)
                m_HoverStyle = CreateHoverStyle();
        }
        
        private static GUIStyle CreateHoverStyle()
        {
            var guiStyle = new GUIStyle(EditorStyles.label);
            var transparentTexture = new Texture2D(1, 1)
            {
                alphaIsTransparency = true
            };
            transparentTexture.SetPixel(1, 1, m_TransparentColor);
            transparentTexture.Apply();
            
            var highlightTexture = new Texture2D(1, 1)
            {
                alphaIsTransparency = true
            };
            
            highlightTexture.SetPixel(1, 1, m_HighlightColor);
            highlightTexture.Apply();
            
            guiStyle.normal.background = transparentTexture;
            guiStyle.hover.background = highlightTexture;
            
            return guiStyle;
        }

        private static Rect GetTopmostRect()
        {
            return (Rect)m_TopmostRectPropertyInfo.GetValue(null, null);
        }

        private static Vector2 Unclip(Vector2 position)
        {
            return (Vector2)m_UnclipMethodInfo.Invoke(null, new object[] {position});
        }
    }
}