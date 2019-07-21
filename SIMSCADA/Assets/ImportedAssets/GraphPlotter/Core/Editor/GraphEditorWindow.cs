using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using plib.Util;

// TODO:
// - (logarithmische Axen)
// - elliptische Kurven
// - exportierbare Texturen
// - Inanwendungsdoku

namespace GraphPlotter.Editor
{
    public class GraphEditorWindow : EditorWindow
    {
        private AnimationCurve mi_curve = new AnimationCurve();

        private List<GraphEditorFormula> mi_formulas = new List<GraphEditorFormula>() { new GraphEditorFormula("", Color.green)};
        private bool mi_formulaValid = true;
        private bool mi_plotRectValid = true;
        private bool pi_gridValid { get { return !mi_showGrid || (mi_showGrid && mi_grid.x > 0 && mi_grid.y > 0); } }
        private bool pi_allValid { get { return mi_formulaValid && mi_plotRectValid && pi_gridValid; } }

        private Texture2D mi_plottetTex;
        private Vector2 mi_plotStart = new Vector2(-100, -100);
        private Vector2 mi_plotEnd = new Vector2(100, 100);
        private float mi_plotDensity = 1f;

        #region -- grid --
        private bool mi_showGrid = true;
        private Vector2 mi_grid = new Vector2(10,10);
        private bool mi_lockGrid;
        #endregion

        #region -- variables --
        public int mu_variableCount = 1;
        private float[] mi_variables;
        private bool mi_variablesFoldOut = true;
        #endregion

        #region -- selection --
        private Vector2 mi_selectionRectStart;
        private Rect? mi_selectionRect;
        private bool mi_inZoomMode;
        private bool mi_squareSelection;
        private bool mi_isDraggingRect;
        private string mi_selectionRectText = "";
        private Vector2 mi_lastPanPosition;
        private string mi_mousePosText;
        #endregion

        private Vector2 mi_oldWindowSize;
        private int mi_textureWidth = 1024;

        private static GUIStyle mi_toggleButtonStyleNormal = null;
        private static GUIStyle mi_toggleButtonStyleToggled = null;

        private Color mi_backGroundColor = Color.black;
        private Color mi_axisColor = Color.white;
        private Color mi_gridColor = Color.gray;


        [MenuItem("Window/Graph Editor")]
        public static void ShowWindow()
        {
            GetWindow<GraphEditorWindow>();
        }

        private void Awake()
        {
            position.Set(0, 0, 1000, 1000);
            fi_regenerateTexture();
            titleContent = new GUIContent("Graph Editor");
			mi_variables = new float[mu_variableCount];


		}

        public void OnGUI()
        {
            if (mi_oldWindowSize != position.size)
            {
                mi_selectionRect = null;
                mi_isDraggingRect = false;
                mi_oldWindowSize = position.size;
            }

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();

            if (mi_toggleButtonStyleNormal == null)
            {
                mi_toggleButtonStyleNormal = "Button";
                mi_toggleButtonStyleToggled = new GUIStyle(mi_toggleButtonStyleNormal);
                mi_toggleButtonStyleToggled.normal.background = mi_toggleButtonStyleToggled.active.background;
            }

            if (GUILayout.Button("? help ?"))
            {
                GraphEditorHelpWindow help = GetWindow<GraphEditorHelpWindow>();
                help.titleContent = new GUIContent("manual");
                help.minSize = new Vector2(550, 600);
            }

            // draw formulas and animationcurve
            fi_drawFormulas();

            // draw selection for plotrect
            fi_drawPlotRect();

            // optionally you can draw a grid
            fi_drawGrid();

            // draw variables, here you can insert default values
            fi_drawVariables();

            fi_drawColor();

            GUILayout.EndVertical();
            GUILayout.BeginVertical();

            GUI.enabled = pi_allValid;
            GUILayoutOption buttonWidth = GUILayout.Width(Mathf.Min(mi_textureWidth,
                                                            position.width * 0.6f));
            if (GUILayout.Button("Plot animation curve", buttonWidth))
            {
                mi_curve = MathPlotter.PlotAnimationCurve(mi_formulas[0].mu_formula, mi_variables, 0, mi_plotStart.x, mi_plotEnd.x, mi_plotDensity);
            }

            if (GUILayout.Button("Plot", buttonWidth))
            {
                fi_regenerateTexture();
            }
            GUI.enabled = true;

            if (mi_plottetTex != null)
            {
                fi_drawPlotTexture();
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Editorfunction for drawing formulas
        /// </summary>
        void fi_drawFormulas()
        {
            GUILayoutOption fieldWidth = GUILayout.Width(Mathf.Max(300, 
                                                            position.width * 0.4f - 20));
            EditorGUILayout.CurveField(mi_curve, GUILayout.MinHeight(100), fieldWidth);

            int i = 0;
            for (char c = 'f'; c != '{' && i < mi_formulas.Count; c++, i++)
            {
                mi_formulas[i].DrawField(c + "(" + ((char)(mi_formulas[i].mu_selectedVariableIndex +65)) + "):", mi_variables);
            }
            mi_formulaValid = mi_formulas.All(o => o.mu_formulaValid);
            if (!mi_formulaValid)
            {
                mi_inZoomMode = false;
            }

            GUILayout.BeginHorizontal();
            if (mi_formulas.Count < 21 && GUILayout.Button("+", GUILayout.Width(20)))
            {
                mi_formulas.Add(new GraphEditorFormula(""));
            }
            if (mi_formulas.Count > 1 && GUILayout.Button("-", GUILayout.Width(20)))
            {
                mi_formulas.RemoveAt(mi_formulas.Count - 1);
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Editorfunction for drawing the plot rect
        /// </summary>
        private void fi_drawPlotRect()
        {
            Color c = GUI.contentColor;

            mi_plotRectValid = mi_plotStart.x < mi_plotEnd.x && mi_plotStart.y < mi_plotEnd.y && mi_plotDensity > 0;
            if (!mi_plotRectValid)
            {
                mi_inZoomMode = false;
            }
            GUI.backgroundColor = mi_plotStart.x < mi_plotEnd.x && mi_plotStart.y < mi_plotEnd.y
                        ? Color.green : Color.red;
            mi_plotStart = EditorGUILayout.Vector2Field("Plot start", Vector2.Min(mi_plotStart, mi_plotEnd));
            mi_plotEnd = EditorGUILayout.Vector2Field("Plot end", Vector2.Max(mi_plotEnd, mi_plotStart));
            GUI.backgroundColor = mi_plotDensity > 0 ? Color.green : Color.red;
            mi_plotDensity = EditorGUILayout.FloatField("Plot density", mi_plotDensity);
            GUI.backgroundColor = c;
        }

        /// <summary>
        /// Draws a grid
        /// </summary>
        void fi_drawGrid()
        {
            GUILayout.BeginHorizontal();
            mi_showGrid = EditorGUILayout.Toggle("Draw grid", mi_showGrid);
            if (mi_showGrid)
            {
                Color c = GUI.contentColor;

                GUI.backgroundColor = pi_gridValid ? Color.green : Color.red;
                mi_grid = EditorGUILayout.Vector2Field("",mi_grid);
                mi_grid = Vector2.Max(mi_grid, mi_lockGrid ? (mi_plotEnd - mi_plotStart).Abs() / 10 : Vector2.zero);
                GUI.backgroundColor = c;

                float f = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 50;
                mi_lockGrid = EditorGUILayout.Toggle("lock",mi_lockGrid, GUILayout.Width(70));
                EditorGUIUtility.labelWidth = f;
            }
            GUILayout.EndHorizontal();

        }

        /// <summary>
        /// draws alls variables
        /// </summary>
        void fi_drawVariables()
        {
            int oldCount = mu_variableCount;
            mu_variableCount = Mathf.Clamp(EditorGUILayout.IntField("Variable count", mu_variableCount), 0, 26);
            if (oldCount != mu_variableCount)
            {
                float[] newVars = new float[mu_variableCount];
                System.Array.Copy(mi_variables, newVars, Mathf.Min(mu_variableCount, mi_variables.Length));
                mi_variables = newVars;
                mi_formulas.ForEach(o => o.mu_validationFlag = true);
            }

            if (mu_variableCount > 0)
            {
                mi_variablesFoldOut = EditorGUILayout.Foldout(mi_variablesFoldOut, "Variables");
                if (mi_variablesFoldOut)
                {
                    for (int i = 0; i < mi_variables.Length; i++)
                    {
                        mi_variables[i] = EditorGUILayout.FloatField(((char)(65 + i)).ToString(), mi_variables[i]);
                    }
                }
            }
        }

        /// <summary>
        /// draws options for changing background, axis and grid colors
        /// </summary>
        void fi_drawColor()
        {
            mi_backGroundColor = EditorGUILayout.ColorField("Background color", mi_backGroundColor);
            mi_axisColor = EditorGUILayout.ColorField("Axis color", mi_axisColor);
            mi_gridColor = EditorGUILayout.ColorField("Grid color", mi_gridColor);
        }

        /// <summary>
        /// draws the texture und does mousestuff for the selectionrect and paning
        /// </summary>
        void fi_drawPlotTexture()
        {
            GUILayoutOption buttonWidth = GUILayout.Width(Mathf.Min(mi_textureWidth,
                                                            position.width * 0.6f));

            GUILayout.BeginHorizontal(buttonWidth);
            GUI.enabled = pi_allValid;
            if (GUILayout.Button("Zoom", mi_inZoomMode 
                                            ? mi_toggleButtonStyleToggled 
                                            : mi_toggleButtonStyleNormal, 
                                        GUILayout.MaxWidth(100)))
            {
                mi_inZoomMode = !mi_inZoomMode;
                if (!mi_inZoomMode)
                {
                    mi_selectionRect = null;
                    mi_isDraggingRect = false;
                    mi_squareSelection = false;
                }
            }
            if (mi_inZoomMode)
            {
                float f = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 50;
                mi_squareSelection = EditorGUILayout.Toggle("square", mi_squareSelection, GUILayout.Width(70));
                EditorGUIUtility.labelWidth = f;
            }
            if (GUILayout.Button("reset"))
            {
                mi_plotStart = new Vector2(-100, -100);
                mi_plotEnd = new Vector2(100, 100);
                fi_regenerateTexture();
            }
            GUI.enabled = true;
            if (GUILayout.Button("save texture"))
            {
                string filename = EditorUtility.SaveFilePanel("save plottet texture","", System.DateTime.Now.ToString("yyyy-MM-dd"), "png");
                byte[] bytes = mi_plottetTex.EncodeToPNG();
                System.IO.File.WriteAllBytes(filename, bytes);
            }
            GUILayout.FlexibleSpace();
            if (mi_inZoomMode && mi_selectionRect != null)
            {
                GUILayout.Label(mi_selectionRectText);
            }
            GUILayout.FlexibleSpace();
            GUILayout.Label(mi_mousePosText);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label(mi_plottetTex, buttonWidth);
            Rect tRect = GUILayoutUtility.GetLastRect();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            Vector2 mp = Event.current.mousePosition;
            if (tRect.Contains(mp))
            {
                Vector2 relMousePos = MathPlotter.WindowPosToPlotPos(mp, tRect, mi_textureWidth, mi_plotStart, mi_plotEnd);
                mi_mousePosText = relMousePos.ToString("F4");

                Repaint();
            }

            if (mi_inZoomMode)
            {
                var e = Event.current;
                if (tRect.Contains(mp))
                {
                    if (e.button == 0)  // left mouse button for selectionrect und zooming in
                    {
                        if (e.type == EventType.MouseDown)
                        {
                            if (mi_selectionRect != null
                                && mi_selectionRect.Value.Contains(mp))
                            {
                                Rect r = MathPlotter.WindowPosToPlotPos(mi_selectionRect.Value, tRect, mi_textureWidth, mi_plotStart, mi_plotEnd);
                                mi_plotStart = new Vector2(r.min.x, r.max.y);
                                mi_plotEnd = new Vector2(r.max.x, r.min.y);

                                fi_regenerateTexture();
                            }

                            mi_selectionRectStart = mp;
                            mi_selectionRect = null;
                            mi_isDraggingRect = true;
                        }
                        else if (e.type == EventType.MouseUp)
                        {
                            // Only use large enough rects
                            if (Vector3.Distance(mi_selectionRectStart, mp) > 16)
                            {
                                Vector2 min = Vector2.Min(mi_selectionRectStart, mp);
                                Vector2 max = Vector2.Max(mi_selectionRectStart, mp);
                                mi_selectionRect = new Rect(min, max - min);

                                mi_selectionRectText = MathPlotter.PlotRectToString(mi_selectionRect.Value, tRect, mi_textureWidth, mi_plotStart, mi_plotEnd);
                            }
                            else
                            {
                                mi_selectionRect = null;
                            }
                            mi_isDraggingRect = false;
                        }
                    }
                    else if (e.button == 1) // left mouse button for deselection und zooming out
                    {
                        if (e.type == EventType.MouseDown)
                        {
                            if (mi_selectionRect != null
                                && mi_selectionRect.Value.Contains(mp))
                            {
                                Vector2 center = MathPlotter.WindowPosToPlotPos(mi_selectionRect.Value.center, tRect, mi_textureWidth, mi_plotStart, mi_plotEnd);

                                mi_plotStart = center + (mi_plotStart - center) * 2;
                                mi_plotEnd = center + (mi_plotEnd - center) * 2;

                                fi_regenerateTexture();
                            }

                            mi_selectionRectStart = mp;
                            mi_selectionRect = null;
                            mi_isDraggingRect = false;
                        }
                    }
                }
                else if (e.button == 2) // draw middle mouse button for paning
                {
                    if (e.type == EventType.MouseDown)
                    {
                        mi_lastPanPosition = mp;
                    }

                    Vector2 pan = mp - mi_lastPanPosition;
                    pan.y = -pan.y;
                    mi_lastPanPosition = mp;

                    if (pan.magnitude < 64)
                    {

                        pan *= mi_plotEnd - mi_plotStart;
                        pan /= mi_textureWidth;
                        mi_plotStart -= pan;
                        mi_plotEnd -= pan;

                        fi_regenerateTexture();
                    }
                }

                if (mi_isDraggingRect || mi_selectionRect != null)      // draw selection rect
                {
                    Vector2 min = mi_isDraggingRect ? Vector2.Min(mi_selectionRectStart, mp) : mi_selectionRect.Value.min;
                    Vector2 max = mi_isDraggingRect ? Vector2.Max(mi_selectionRectStart, mp) : mi_selectionRect.Value.max;
                    Vector2 tr = new Vector2(max.x, min.y);
                    Vector2 bl = new Vector2(min.x, max.y);

                    Handles.color = Color.blue;
                    Handles.DrawLine(min, tr);
                    Handles.DrawLine(tr, max);
                    Handles.DrawLine(max, bl);
                    Handles.DrawLine(bl, min);
                    Repaint();
                }
            }
        }

        /// <summary>
        /// this will replot the texture
        /// </summary>
        private void fi_regenerateTexture()
        {
            mi_plottetTex = MathPlotter.PlotTexture(mi_formulas.Select(o => (MathPlotter.ColorFormula)o), 
                                                    mi_variables, mi_plotStart, mi_plotEnd, mi_textureWidth, 
                                                    mi_backGroundColor, mi_axisColor, mi_gridColor,
                                                    (pi_gridValid && mi_showGrid) ? (Vector2?)mi_grid : null);
        }
    }
}