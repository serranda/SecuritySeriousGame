using plib.Util;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GraphPlotter.Editor
{
    [System.Serializable]
    public class GraphEditorFormula
    {
        public string mu_formula;
        public bool mu_formulaValid;
        public Color mu_color;
        public int mu_selectedVariableIndex;
        public bool mu_validationFlag = false;

        public GraphEditorFormula()
        {
        }

        public GraphEditorFormula(string _formula)
            : this(_formula, UnityHelper.GetNiceRandomColor())
        {
        }

        public GraphEditorFormula(string _formula, Color _color)
            :this(_formula, _color, 0)
        {
            
        }

        public GraphEditorFormula(string _formula, Color _color, int _selectedVariableIndex)
        {
            mu_formula = _formula;
            mu_color = _color;
            mu_formulaValid = true;
            mu_selectedVariableIndex = _selectedVariableIndex;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_variables"></param>
        /// <returns></returns>
        public void DrawField(string _text, float[] _variables)
        {
            string oldFormula = mu_formula;
            Color c = GUI.contentColor;
            GUILayout.BeginHorizontal();
            if (mu_selectedVariableIndex >= _variables.Length + 1)
            {
                mu_selectedVariableIndex = _variables.Length - 1;
            }
            mu_selectedVariableIndex = EditorGUILayout.Popup(
                                            mu_selectedVariableIndex,
                                            Enumerable.Range(0, _variables.Length)
                                            .Select(o => ((char)(o + 65)).ToString())
                                            .ToArray(), 
                                        GUILayout.Width(30));

            GUI.backgroundColor = mu_formulaValid ? Color.green : Color.red;
            float lbw = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 50;
            mu_formula = EditorGUILayout.TextField(_text, mu_formula);
            EditorGUIUtility.labelWidth = lbw;
            GUI.backgroundColor = c;
            if (oldFormula != mu_formula || mu_validationFlag)
            {
                mu_formulaValid = MathPlotter.CheckIfFormulaIsValid(mu_formula, _variables);
                mu_validationFlag = false;
            }
            mu_color = EditorGUILayout.ColorField(mu_color, GUILayout.Width(50));
            GUILayout.EndHorizontal();
        }

        public static explicit operator MathPlotter.ColorFormula(GraphEditorFormula _formula)
        {
            return new MathPlotter.ColorFormula(_formula.mu_color, _formula.mu_formula, _formula.mu_selectedVariableIndex);
        }
    }
}