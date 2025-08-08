using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Itens
{
      public class ItemLayout : MonoBehaviour
      {
           public ItemManager.ItemSetup _CurrSetup;
           public Image uiIcon;
           public TextMeshProUGUI uiValue;

            public void Load(ItemManager.ItemSetup setup)
           {
               _CurrSetup = setup;
               UpdateUI();
           }

           private void UpdateUI()
           {
                uiIcon.sprite = _CurrSetup.icon;
           }

           private void Update()
           {
                uiValue.text = _CurrSetup.soInt.value.ToString();
           }
           
      }
}