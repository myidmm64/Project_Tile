using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSkillModule : PlayerModule
{
    [SerializeField]
    private Slider _mpSlider = null;
    [SerializeField]
    private Slider _ultimateSlider = null;

    protected override void Awake()
    {
        base.Awake();
        if(_mpSlider != null)
        {
            _mpSlider.minValue = 0;
            _mpSlider.maxValue = 100;
        }
        if (_ultimateSlider != null)
        {
            _ultimateSlider.minValue = 0;
            _ultimateSlider.maxValue = 100;
        }
    }

    public void IncreaseMP(int amount)
    {
        _mpSlider.value += _mpSlider.value + amount;
    }

    public void IncreaseUP(int amount)
    {
        _ultimateSlider.value += amount;
    }

    public void SkillInput(InputAction.CallbackContext context)
    {

    }
}
