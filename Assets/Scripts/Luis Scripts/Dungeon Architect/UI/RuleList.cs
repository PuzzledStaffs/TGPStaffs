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
    Rule[] rules;
    //Rule CurrentSelectedRule;
    //List<string> items;
    public new class UxmlFactory : UxmlFactory<RuleList, RuleList.UxmlTraits> { };

     

    public RuleList()
    {
        GetAllRules(out rules);

    

    }

    public void SetupList()
    {
        VisualElement ButtonGroup = this.Q<VisualElement>("Control-Button-Group");
        DownArrow = ButtonGroup.Query<Button>("ScrollDownButton");
        UpArrow = ButtonGroup.Query<Button>("ScrollUpButton");
        subtractbutton = ButtonGroup.Query<Button>("DeleteButton");
        plusbutton = ButtonGroup.Query<Button>("AddButton");
        list = this.Query<ListView>("RuleList");





        plusbutton.clicked += AddItem;

        subtractbutton.clicked += SubtractItem;
        Func<VisualElement> MakeItem = () => new Label();
        Action<VisualElement, int> bindItem = (e, i) => (e as Label).text = rules[i].name;
        const int ItemHeight = 16;
    }

    private void AddItem()
    {
       Rule ScripObject = ScriptableObject.CreateInstance<Rule>();
        AssetDatabase.CreateAsset(ScripObject, "Assets/ScriptableObjects/RulesFolder");
        AssetDatabase.SaveAssets();
        GetAllRules(out rules);
    }

    private void SubtractItem()
    {
        Rule currentRule = list.selectedItem as Rule;
        string path = AssetDatabase.GetAssetPath(currentRule);
        AssetDatabase.DeleteAsset(path);
        GetAllRules(out rules);
    }

    public void GetAllRules(out Rule[] rules)
    {
        string[] GUIDS = AssetDatabase.FindAssets($"t:{typeof(Rule)}", new[] { "Assets/ScriptableObjects/RulesFolder" });

        rules = new Rule[GUIDS.Length];
        for (int i = 0; i < GUIDS.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(GUIDS[i]);
            rules[i] = AssetDatabase.LoadAssetAtPath<Rule>(path);
        }
    }

}
