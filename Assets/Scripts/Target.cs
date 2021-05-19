using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    internal bool onSearchMode { get; private set; }

    private string _desiredTarget;
    private int _range;
    private int _aoe;

    private Transform _actionMaker;
    public GameObject targetUnit = null;
    public Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SearchMode(bool boo)
    {
        onSearchMode = boo;
    }

    //Single Target
    public void StartSearchMode(bool boo, int range, Transform actionMaker, string desiredTarget)
    {
        onSearchMode = boo;
        _range = range;
        _actionMaker = actionMaker;
        _desiredTarget = desiredTarget;
    }
    //AOE
    public void StartSearchMode(bool boo, int range, Transform actionMaker, int aoe, string desiredTarget)
    {
        onSearchMode = boo;
        _range = range;
        _actionMaker = actionMaker;
        _desiredTarget = desiredTarget;
        _aoe = aoe;
    }
    //Multiple Targets??

    void TargetWithMouse()
    {

    }
}
