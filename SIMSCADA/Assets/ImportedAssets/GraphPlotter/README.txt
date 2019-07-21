You can find the editor window at "Window/Graph Editor" in the main Unity menu.
This window can be used for displaying function graphs, contextsensitive textfields will help at your inputs by marking errors red.
To draw the graphs you have to first use the "Plot" button, this redraw will also happen if you use the zoom-mode.
The zoom-button will appear after your first plot.
To add more functions use the "+" button.
Remember: your varaibles start with A, so you have to transform something like "x*x" into "A*A"


Variables
	Variables start with A, try somthing like 'A*A'.
	You have to create a variable before you can use it.
	For every formula you can choose against whichs variable you wannt to plot it.
	You can set default values for other ones.

Animationcurve:
	Plot density is used only on the animation curve.
	Only the first formula can be converted into an animation curve.

Functions:
    pow		pow(arg1, arg2)		arg1 power to arg2
    log		log(arg1, arg2)		log arg1 to base arg2
    abs		abs(arg)			returns the absolute of arg 
    min		min(arg1, arg2)		returns the smaller arg
    max		max(arg1, arg2)		returns the bigger arg
    sign	sign(arg)			returns the sign of arg, eg -5 => -1, 0 => 0, 45 => 1
    ceil	ceil(arg)			rounds arg to next integer up
    floor	floor(arg)			rounds arg to next integer down
    sin		sin(arg)			returns the sinus of arg
    cos		cos(arg)			returns the cosinus of arg
    root	root(arg1, arg2)	arg2 root of arg1

Constants:
    PI		3.14159274F
    DTOR	0.0174532924F		convertes degree into radians
    RTOD	57.29578F			convertes radians into degree

Controlls in zoom mode:
	Hold left mouse button to draw a rect.
	Left click in a rect will zoom into this rect, outside will destroy the rect.

	Right click in a rect will zoom out, outside will destroy the rect.

	Drag middle mousebutton to pan the view.
	Caution: this can be slow when having multiple complex formulas.

For Programmers:
	to use graph plotter while runtime you have to pass a colorfomula-array, which is used to define the function, color and the index of the plottet variable starting with 0 for A
	then you have to pass an array of floats, to cover your used variable indices
	look for PlotTestUI as an example

	If you want to use the calculator in a diffrent environment keep in mind, that ContainerLists will shrink to one element if you use Calculate
	so if you want to use it for more than one calculation use somthing like this, to clone it before:

	ContainerList cl = AContainer.Read("A");

    for (int i = 0; i <= 100; i++)
    {
        cl.SetConstants(new Constant("A", i));
        Assert.AreEqual(((ContainerList)cl.Clone()).Calculate(), (double) (i));
    }