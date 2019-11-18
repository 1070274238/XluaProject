//xiaowangzi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class TestXLua : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LuaEnv _lua = new LuaEnv();
        _lua.DoString("CS.UnityEngine.Debug.Log('Hello World')");
        _lua.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
