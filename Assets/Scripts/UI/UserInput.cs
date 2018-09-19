using System;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    public Action TapUiEvent;
    public Action UpTouchUiEvent;

    private bool _isHold;

    private void OnMouseDown()
    {
        _isHold = (!_isHold);

        if (_isHold && TapUiEvent != null)
            TapUiEvent();
    }

    private void OnMouseUp()
    {
        if (_isHold && UpTouchUiEvent != null)
            UpTouchUiEvent();

        _isHold = false;
    }
}
