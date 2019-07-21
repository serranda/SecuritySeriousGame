using UnityEngine;
using plib.Util;
using plib.Util.Helper;
using CommandInterpreter.Calculator.Container;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;

namespace GraphPlotter
{
    public static class MathPlotter
    {
        /// <summary>
        /// Checks if a formula can be computed
        /// </summary>
        /// <param name="_formula"></param>
        /// <param name="_variables">can be null</param>
        public static bool CheckIfFormulaIsValid(string _formula, float[] _variables)
        {
            try
            {
                ContainerList cl = AContainer.Read(_formula);
                fi_fillCommonConstants(cl);

                if (cl == null)
                {
                    return false;
                }

                if (_variables != null)
                {

                    cl.SetConstants(Enumerable.Range(0, _variables.Length).
                                    Select(o => new Constant(((char)(65 + o)).ToString(),
                                                        _variables[o])).ToArray());
                }

                cl.Calculate();
            }
            catch (System.Exception _ex)
            {
                Debug.LogWarning(_ex.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if a formula can be computed
        /// </summary>
        /// <param name="_formula"></param>
        /// <returns></returns>
        public static bool CheckIfFormulaIsValid(string _formula)
        {
            return CheckIfFormulaIsValid(_formula, null);
        }

        /// <summary>
        /// Returns an animationcurve with a single graph 
        /// </summary>
        /// <param name="_formula"></param>
        /// <param name="_variables">can be null</param>
        /// <param name="_plottetVarIndex">should be -1 if _variables is null</param>
        /// <param name="_plotStartX"></param>
        /// <param name="_plotEndX"></param>
        /// <param name="_plotDensity"></param>
        /// <returns></returns>
        public static AnimationCurve PlotAnimationCurve(string _formula, float[] _variables, int _plottetVarIndex, float _plotStartX, float _plotEndX, float _plotDensity)
        {
            float value;
            ContainerList cl = AContainer.Read(_formula);

            if (_variables != null)
            {
                cl.SetConstants(Enumerable.Range(0, _variables.Length).
                                Select(o => new Constant(((char)(65 + o)).ToString(),
                                                    _variables[o])).ToArray());
            }

            AnimationCurve curve = new AnimationCurve();
            for (float f = _plotStartX; f <= _plotEndX; f += _plotDensity)
            {
                if (_variables != null)
                {
                    if (_plottetVarIndex != -1)
                    {
                        cl.SetConstants(new Constant(((char)(65 + _plottetVarIndex)).ToString(), f));
                    }

                }
                value = (float)((ContainerList)cl.Clone()).Calculate();
                if (!(float.IsNaN(value) || float.IsNegativeInfinity(value) || float.IsPositiveInfinity(value)))
                {
                    curve.AddKey(f, value);
                }
            }
            return curve;
        }

        /// <summary>
        /// Returns an animationcurve with a single graph 
        /// </summary>
        /// <param name="_formula"></param>
        /// <param name="_plotStartX"></param>
        /// <param name="_plotEndX"></param>
        /// <param name="_plotDensity"></param>
        /// <returns></returns>
        public static AnimationCurve PlotAnimationCurve(string _formula, float _plotStartX, float _plotEndX, float _plotDensity)
        {
            return PlotAnimationCurve(_formula, null, -1, _plotStartX, _plotEndX, _plotDensity);
        }

        /// <summary>
        /// Returns a texture with a graph and axis
        /// </summary>
        /// <param name="_formula"></param>
        /// <param name="_variables">can be null</param>
        /// <param name="_plottetVarIndex">should be -1 if _variables is null</param>
        /// <param name="_plotStart"></param>
        /// <param name="_plotEnd"></param>
        /// <returns></returns>
        public static Texture2D PlotTexture(IEnumerable<ColorFormula> _formulas, float[] _variables, Vector2 _plotStart, Vector2 _plotEnd, int _textureWitdth, Color _backgroundColor, Color _axisColor, Color _gridColor, Vector2? _grid = null)
        {
            if (_plotStart.x == _plotEnd.x || _plotStart.y == _plotEnd.y)
            {
                Debug.LogError("there is no space for plotting, check plot starts and end");
                return null;
            }

            int y;
            Color32[,] colors = new Color32[_textureWitdth, _textureWitdth];

            for (int x = 0; x < _textureWitdth; x++)
            {
                for (y = 0; y < _textureWitdth; y++)
                {
                    colors[x, y] = _backgroundColor;
                }
            }

            Vector2 relativeZero = -new Vector2(_plotStart.x, _plotStart.y);
            Vector2 size = (_plotStart - _plotEnd).Abs();
            relativeZero /= relativeZero + _plotEnd;
            
            // Grid
            if (_grid != null)
            {
                Vector2 step = _grid.Value / size;
                if (step.y > 0.01)
                {
                    for (float f = relativeZero.y; f <= 1; f += step.y)
                    {
                        TextureHelper.DrawLine(colors, new Vector2(0f, f), new Vector2(1f, f), _gridColor);
                    }
                    for (float f = relativeZero.y - step.y; f >= 0; f -= step.y)
                    {
                        TextureHelper.DrawLine(colors, new Vector2(0f, f), new Vector2(1f, f), _gridColor);
                    }
                }
                if (step.x > 0.01)
                {

                    for (float f = relativeZero.x; f <= 1; f += step.x)
                    {
                        TextureHelper.DrawLine(colors, new Vector2(f, 0f), new Vector2(f, 1f), _gridColor);
                    }
                    for (float f = relativeZero.x - step.x; f >= 0; f -= step.x)
                    {
                        TextureHelper.DrawLine(colors, new Vector2(f, 0f), new Vector2(f, 1f), _gridColor);
                    }
                }
            }

            // X Axis
            TextureHelper.DrawLine(colors, new Vector2(0f, relativeZero.y), new Vector2(1f, relativeZero.y), _axisColor);
            // Y Axis
            TextureHelper.DrawLine(colors, new Vector2(relativeZero.x, 0f), new Vector2(relativeZero.x, 1f), _axisColor);


            // Draw Graph
            foreach (ColorFormula formula in _formulas)
            {
                DrawGraph(formula.mu_formula, colors, _variables, formula.mu_selectedVariableIndex, size, relativeZero, _textureWitdth, formula.mu_color);
            }

            // apply array on texture
            Texture2D t = new Texture2D(_textureWitdth, _textureWitdth, TextureFormat.ARGB32, true);
            t.SetPixels32(colors.TwoToOneD());
            t.Apply();

            return t;
        }

        /// <summary>
        /// Returns a texture with a graph and axis
        /// </summary>
        /// <param name="_formula"></param>
        /// <param name="_plotStart"></param>
        /// <param name="_plotEnd"></param>
        /// <returns></returns>
        public static Texture2D PlotTexture(IEnumerable<ColorFormula> _formulas, Vector2 _plotStart, Vector2 _plotEnd, int _textureWidth, Color _backgroundColor, Color _axisColor, Color _gridColor, Vector2? _grid = null)
        {
            return PlotTexture(_formulas, null, _plotStart, _plotEnd, _textureWidth, _backgroundColor, _axisColor, _gridColor, _grid);
        }

        /// <summary>
        /// helper function to draw a graph
        /// </summary>
        /// <param name="_formula"></param>
        /// <param name="_colors"></param>
        /// <param name="_variables">can be null</param>
        /// <param name="_plottetVarIndex">should be -1 if _variables is null</param>
        /// <param name="_size"></param>
        /// <param name="_relativeZero"></param>
        /// <param name="_tWidth"></param>
        /// <param name="_color"></param>
        private static void DrawGraph(string _formula, Color32[,] _colors, float[] _variables, int _plottetVarIndex, Vector2 _size, Vector2 _relativeZero, int _tWidth, Color _color)
        {
            // use threads to speed calculations up
            System.Threading.Thread[] ciThreads = new System.Threading.Thread[16];
            double[] values = new double[_tWidth];
            for (int i = 0; i < ciThreads.Length; i++)
            {
                ciThreads[i] = new System.Threading.Thread(CIThread);
                ciThreads[i].Start(new CIThreadObject(_formula, i, _tWidth / ciThreads.Length, values, _size, _relativeZero, _tWidth, _plottetVarIndex, _variables));
            }

            // wait for the threads to finish their work
            ciThreads.ForEach(o => o.Join());

            // draw lines between 2 plotpoints to close gaps in the graph
            Vector2Int last = new Vector2Int(-1, -1);
            int y;
            for (int x = 0; x < _tWidth; x++)
            {
                y = (int)System.Math.Round(values[x]);

                if (!(y < 0 || y >= _tWidth))
                {
                    _colors[x, y] = Color.green;
                }

                if (x != 0 && x - last.x <= 1 && (Mathf.Abs(y - last.y) < _tWidth || Mathf.Abs(Mathf.Sign(y) - Mathf.Sign(last.y)) <= 1))      // Minimaler stetigkeitscheck: Vorzeichen soll nicht extrem wechseln
                {
                    TextureHelper.DrawLine(_colors, new Vector2Int(x, y), last, _color);
                }
                last = new Vector2Int(x, y);
            }
        }

        public static Rect WindowPosToPlotPos(Rect _rect, Rect _textureRect, float _textureWidth, Vector2 _plotStart, Vector2 _plotEnd)
        {
            Rect r = new Rect();
            r.min = WindowPosToPlotPos(_rect.min, _textureRect, _textureWidth, _plotStart, _plotEnd);
            r.max = WindowPosToPlotPos(_rect.max, _textureRect, _textureWidth, _plotStart, _plotEnd);

            return r;
        }

        public static Vector2 WindowPosToPlotPos(Vector2 _pos, Rect _textureRect, float _textureWidth, Vector2 _plotStart, Vector2 _plotEnd)
        {
            Vector2 relpos = _pos - _textureRect.position;
            relpos /= new Vector2(Mathf.Min(_textureRect.width, _textureWidth),
                                Mathf.Min(_textureRect.width, _textureWidth));

            relpos.y = 1 - relpos.y;

            relpos = relpos * (_plotEnd - _plotStart);
            relpos += _plotStart;

            return relpos;
        }

        public static string PlotRectToString(Rect _rect, Rect _textureRect, int _textureWidth, Vector2 _plotStart, Vector2 _plotEnd)
        {
            return "Start: " + WindowPosToPlotPos(_rect.min, _textureRect, _textureWidth, _plotStart, _plotEnd).ToString("F4") +
                    "End: " + WindowPosToPlotPos(_rect.max, _textureRect, _textureWidth, _plotStart, _plotEnd).ToString("F4");
        }

        static void fi_fillCommonConstants(ContainerList _containerList)
        {
            _containerList.SetConstants(new Constant("PI", Mathf.PI));
            _containerList.SetConstants(new Constant("DTOR", Mathf.Deg2Rad));
            _containerList.SetConstants(new Constant("RTOD", Mathf.Rad2Deg));
        }

        /// <summary>
        /// multithreaded calculator
        /// </summary>
        /// <param name="_o">must be a CIThreadObject</param>
        static void CIThread(object _o)
        {
            CIThreadObject co = (CIThreadObject)_o;
            float xValue;
            double value;

            float xSlice = co.mu_size.x / co.mu_tWidth;

            int start = co.mu_index * co.mu_count;
            int end = (co.mu_index + 1) * co.mu_count;

            ContainerList cl = AContainer.Read(co.mu_text);
            fi_fillCommonConstants(cl);

            if (co.mu_variables != null)
            {
                cl.SetConstants(Enumerable.Range(0, co.mu_variables.Length).
                                Select(o => new Constant(((char)(65 + o)).ToString(),
                                                    co.mu_variables[o])).ToArray());
            }

            for (int x = start; x < end; x++)
            {
                xValue = x * xSlice - co.mu_relativeZero.x * co.mu_size.x;
                if (co.mu_plottetVarIndex != -1)
                {
                    cl.SetConstants(new Constant(((char)(65 + co.mu_plottetVarIndex)).ToString(), xValue));
                }
                value = ((ContainerList)cl.Clone()).Calculate();

                value /= co.mu_size.y;
                value += co.mu_relativeZero.y;
                value *= co.mu_tWidth;

                co.mu_values[x] = value;
            }
        }

        /// <summary>
        /// helper object for transfering data to calculation threads
        /// </summary>
        class CIThreadObject
        {
            public string mu_text;
            public int mu_index;
            public int mu_count;
            public double[] mu_values;
            public Vector2 mu_size;
            public Vector2 mu_relativeZero;
            public int mu_tWidth;
            public int mu_plottetVarIndex;
            public float[] mu_variables;

            public CIThreadObject(string _text, int _index, int _count, double[] _values, Vector2 _size, Vector2 _relativeZero, int _tWidth, int _plottetVarIndex, float[] _variables)
            {
                mu_text = _text;
                mu_index = _index;
                mu_count = _count;
                mu_values = _values;
                mu_size = _size;
                mu_relativeZero = _relativeZero;
                mu_tWidth = _tWidth;
                mu_plottetVarIndex = _plottetVarIndex;
                mu_variables = _variables;
            }
        }

        public class ColorFormula
        {
            public Color mu_color;
            public string mu_formula;
            public int mu_selectedVariableIndex;

            public ColorFormula(Color _color, string _formula, int _selectedVariableIndex)
            {
                mu_color = _color;
                mu_formula = _formula;
                mu_selectedVariableIndex = _selectedVariableIndex;
            }
        }
    }
}
