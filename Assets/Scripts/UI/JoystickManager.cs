using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexDev.SpaceTanks
{
    public class JoystickManager : MonoBehaviour
    {
        [SerializeField] private Joystick _leftJoystick;
        [SerializeField] private Joystick _rightJoystick;

        public Joystick GetLeftJoystick => _leftJoystick;
        public Joystick GetRightJoystick => _rightJoystick;
    }
}
