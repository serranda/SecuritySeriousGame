using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabNavigation : MonoBehaviour
{
    public bool findFirstSelectable;

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Tab)) return;
        if (EventSystem.current == null) return;
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        //try and find the first selectable if there isn't one currently selected
        //only do it if the findFirstSelectable is true
        //you may not always want this feature and thus
        //it is disabled by default
        if (selected == null && findFirstSelectable)
        {
            List<Selectable> allSelectables = Selectable.allSelectables;
            Selectable found = (allSelectables.Count > 0) ? allSelectables[0] : null;

            if (found != null)
            {
                //simple reference so that selected isn't null and will proceed
                //past the next if statement
                selected = found.gameObject;
            }
        }

        if (selected == null) return;
        Selectable current = selected.GetComponent<Selectable>();

        if (current == null) return;
        Selectable nextDown = current.FindSelectableOnDown();
        Selectable nextUp = current.FindSelectableOnUp();
        Selectable nextRight = current.FindSelectableOnRight();
        Selectable nextLeft = current.FindSelectableOnLeft();

        if (nextDown != null)
        {
            nextDown.Select();
        }
        else if (nextRight != null)
        {
            nextRight.Select();
        }
        else if (nextUp != null)
        {
            nextUp.Select();
        }
        else if (nextLeft != null)
        {
            nextLeft.Select();
        }
    }
}