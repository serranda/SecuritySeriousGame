using GraphPlotter;
using UnityEngine;
using UnityEngine.UI;

using ColorFormula = GraphPlotter.MathPlotter.ColorFormula;

public class PlotTestUI : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        RawImage ri = GetComponent<RawImage>();
        ColorFormula[] formulas = new[] { new ColorFormula(Color.green, "-abs(0.1*A*A+sin(A*5)-5)", 0),	// curve color, function, plottetvariable index
                        new ColorFormula(new Color(0, 1, 0.9130526f), "0.1*A*A+sin(A*5+PI)-5", 0),
                        new ColorFormula(new Color(1,0, 0.6009512f),"sin(A*5)-(1/(A*A))", 0),
                        new ColorFormula(new Color(1,0, 0.04724216f),"sin(A*5 + PI)-(1/(A*A))", 0),
                        new ColorFormula(new Color(0, 0.1934497f, 1),"-(1/(A*A)*5) * 10", 0),
                        new ColorFormula(new Color(0, 0.1934497f, 1),"(1/(A*A)*5) * 10", 0)};

        float[] variables = new[] { 0f };		// just one variable, biggest index is 0, everything is ok

        ri.texture = MathPlotter.PlotTexture(formulas, variables, new Vector2(-15, -15),	// plottet area min
                                                    new Vector2(15, 15),					// plottet area max
													1024,									// Texture width  and hight
                                                    Color.black,							// Background color
													Color.white,							// Axis color
													Color.gray,								// grid color
                                                    Vector2.one);							// grid size, you can pass null, if you want no grid
    }
	
}
