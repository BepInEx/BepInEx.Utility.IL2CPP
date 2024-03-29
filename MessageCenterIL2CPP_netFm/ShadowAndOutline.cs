﻿using UnityEngine;

namespace BepInEx
{
    /// <summary>
    /// Utilities for drawing IMGUI elements like labels with outlines around them.
    /// </summary>
    internal static class ShadowAndOutline
    {
        /// <summary>
        /// Draw a label with an outline
        /// </summary>
        /// <param name="rect">Size of the control</param>
        /// <param name="text">Text of the label</param>
        /// <param name="style">Style to be applied to the label</param>
        /// <param name="txtColor">Color of the text</param>
        /// <param name="outlineColor">Color of the outline</param>
        /// <param name="outlineThickness">Thickness of the outline in pixels</param>
        public static void DrawOutline(Rect rect, string text, GUIStyle style, Color txtColor, Color outlineColor, int outlineThickness)
        {
            var backupColor = style.normal.textColor;
            //Color backupGuiColor = GUI.color;

            style.normal.textColor = outlineColor;
            GUI.color = outlineColor;

            var baseRect = rect;

            rect.x -= outlineThickness;
            rect.y -= outlineThickness;

            while (rect.x++ < baseRect.x + outlineThickness)
                GUI.Label(rect, text, style);
            rect.x--;

            while (rect.y++ < baseRect.y + outlineThickness)
                GUI.Label(rect, text, style);
            rect.y--;

            while (rect.x-- > baseRect.x - outlineThickness)
                GUI.Label(rect, text, style);
            rect.x++;

            while (rect.y-- > baseRect.y - outlineThickness)
                GUI.Label(rect, text, style);

            style.normal.textColor = txtColor;
            //GUI.color = backupGuiColor;
            GUI.Label(baseRect, text, style);

            style.normal.textColor = backupColor;
        }

        /// <summary>
        /// Draw a label with a shadow
        /// </summary>        
        /// <param name="rect">Size of the control</param>
        /// <param name="content">Contents of the label</param>
        /// <param name="style">Style to be applied to the label</param>
        /// <param name="txtColor">Color of the outline</param>
        /// <param name="shadowColor">Color of the text</param>
        /// <param name="shadowOffset">Offset of the shadow in pixels</param>
        public static void DrawShadow(Rect rect, GUIContent content, GUIStyle style, Color txtColor, Color shadowColor, Vector2 shadowOffset)
        {
            var backupColor = style.normal.textColor;

            style.normal.textColor = shadowColor;
            rect.x += shadowOffset.x;
            rect.y += shadowOffset.y;
            GUI.Label(rect, content, style);

            style.normal.textColor = txtColor;
            rect.x -= shadowOffset.x;
            rect.y -= shadowOffset.y;
            GUI.Label(rect, content, style);

            style.normal.textColor = backupColor;
        }
        public static void DrawLayoutShadow(GUIContent content, GUIStyle style, Color txtColor, Color shadowColor, Vector2 direction, params GUILayoutOption[] options) => DrawShadow(GUILayoutUtility.GetRect(content, style, options), content, style, txtColor, shadowColor, direction);
    }
}
