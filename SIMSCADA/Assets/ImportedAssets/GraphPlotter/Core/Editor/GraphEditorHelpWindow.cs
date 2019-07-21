using UnityEditor;
using UnityEngine;

namespace GraphPlotter.Editor
{
    public class GraphEditorHelpWindow : EditorWindow
    {
        private void OnGUI()
        {
			GUIStyle gs = GUI.skin.GetStyle("Label");
			gs.richText = true;
            GUILayout.Label(@"<size=13><b>Variables</b></size>
	Variables start with A, try somthing like 'A*A'.
	You have to create a variable before you can use it.
	For every formula you can choose against whichs variable you wannt to plot it.
	You can set default values for other ones.

<size=13><b>Animationcurve:</b></size>
	Plot density is used only on the animation curve.
	Only the first formula can be converted into an animation curve.

<size=13><b>Functions:</b></size>
    pow	pow(arg1, arg2)	arg1 power to arg2
    log	log(arg1, arg2)	log arg1 to base arg2
    abs	abs(arg)		returns the absolute of arg 
    min	min(arg1, arg2)	returns the smaller arg
    max	max(arg1, arg2)	returns the bigger arg
    sign	sign(arg)		returns the sign of arg, eg -5 => -1, 0 => 0, 45 => 1
    ceil	ceil(arg)		rounds arg to next integer up
    floor	floor(arg)		rounds arg to next integer down
    sin	sin(arg)		returns the sinus of arg
    cos	cos(arg)		returns the cosinus of arg
    root	root(arg1, arg2)	arg2 root of arg1

<size=13><b>Constants:</b></size>
    PI	3.14159274F
    DTOR	0.0174532924F	convertes degree into radians
    RTOD	57.29578F		convertes radians into degree

<size=13><b>Controlls in zoom mode:</b></size>
	Hold left mouse button to draw a rect.
	Left click in a rect will zoom into this rect, outside will destroy the rect.

	Right click in a rect will zoom out, outside will destroy the rect.

	Drag middle mousebutton to pan the view.
	Caution: this can be slow when having multiple complex formulas.
");
        }
    }
}