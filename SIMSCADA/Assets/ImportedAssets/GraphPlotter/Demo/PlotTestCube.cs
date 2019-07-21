using GraphPlotter;
using UnityEngine;

using ColorFormula = GraphPlotter.MathPlotter.ColorFormula;

public class PlotTestCube : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Renderer ri = GetComponent<Renderer>();
        ColorFormula[] formulas = new[] { new ColorFormula(Color.green, "-abs(0.1*A*A+sin(A*5)-5)", 0),
                        new ColorFormula(new Color(0, 1, 0.9130526f), "0.1*A*A+sin(A*5+PI)-5", 0),
                        new ColorFormula(new Color(1,0, 0.6009512f),"sin(A*5)-(1/(A*A))", 0),
                        new ColorFormula(new Color(1,0, 0.04724216f),"sin(A*5 + PI)-(1/(A*A))", 0),
                        new ColorFormula(new Color(0, 0.1934497f, 1),"-(1/(A*A)*5) * 10", 0),
                        new ColorFormula(new Color(0, 0.1934497f, 1),"(1/(A*A)*5) * 10", 0)};

        float[] variables = new[] { 0f };

        ri.material.mainTexture = MathPlotter.PlotTexture(formulas, variables, new Vector2(-15, -15), 
                                                    new Vector2(15, 15), 1024,
                                                    Color.black, Color.white, Color.gray,
                                                    Vector2.one);
    }

	private void Update()
	{
		transform.Rotate(Vector3.one, 50 * Time.deltaTime);
	}

}
