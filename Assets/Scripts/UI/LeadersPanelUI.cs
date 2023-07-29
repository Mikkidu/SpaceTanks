using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AlexDev.SpaceTanks
{
    public class LeadersPanelUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private TextMeshProUGUI _BodyTextText;

        public void Initialization(string headerText, string bodyText)
        {
            _headerText.SetText(headerText);
            _BodyTextText.SetText(bodyText);
        }

    }
}
