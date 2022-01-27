using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SplitView : TwoPaneSplitView
{
    public new class UXMLFactory: UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> { }
}
