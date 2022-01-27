using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System;

public class RuleList : VisualElement
{
   public Button plusbutton;
    public Button subtractbutton;
    public Button UpArrow;
    public Button DownArrow;
    public ListView list;
    //List<string> items;
    public new class UxmlFactory : UxmlFactory<RuleList, RuleList.UxmlTraits> { };

    public RuleList()
    {
        DownArrow = this.Query<Button>("ScrollDownButton");
        UpArrow = this.Query<Button>("ScrollUpButton");
        subtractbutton = this.Query<Button>("DeleteButton");
        plusbutton = this.Query<Button>("AddButton");
        list = this.Query<ListView>("RuleList");

        
        
       List<string> items = new List<string>();

        Func<VisualElement> MakeItem = () => new Label();
        Action<VisualElement, int> bindItem = (e, i) => (e as Label).text = items[i];
        const int ItemHeight = 16;

        list.itemHeight = ItemHeight;
        list.itemsSource = items;
        list.makeItem = MakeItem;
        list.bindItem = bindItem;


    }

    private void AddItem()
    {
       
    }

}
