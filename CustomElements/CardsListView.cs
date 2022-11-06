using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.CustomElements
{
    [Serializable]
    public class CardModel
    {
        public string id;
        public string first_name;
        public string last_name;
        public string email;
        public string gender;
        public string ip_address;
        public string address;
    }

    [Serializable]
    public class CardsArrayWrapper { public CardModel[] items; }

    public class CardsListView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<CardsListView, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits { }

        private ListView _listView;
        private VisualTreeAsset _cardUiAsset;

        public CardsListView()
        {
            _cardUiAsset = Resources.Load<VisualTreeAsset>("Templates/CardTemplate");

            var entries = Resources.Load("test_data") as TextAsset;
            IList cardsArray = JsonUtility.FromJson<CardsArrayWrapper>("{\"items\":" + entries.text + "}").items;

            _listView = new ListView(cardsArray, 350, MakeItem, BindItem)
            {
                selectionType = SelectionType.None,

            };
            
            _listView.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
            _listView.Q<ScrollView>().touchScrollBehavior = ScrollView.TouchScrollBehavior.Elastic;
            Add(_listView);
        }


        private void BindItem(VisualElement element, int index)
        {
            if (_listView.itemsSource[index] is CardModel item)
            {
                element.Q<Label>("nameLabel").text = item.last_name + " " + item.last_name;
                element.Q<Label>("emailLabel").text = item.email;
                element.Q<Label>("addressLabel").text = item.address;
                element.Q<Label>("genderLabel").text = item.gender;
                element.Q<Label>("ipLabel").text = item.ip_address;
                element.Q<Label>("idLabel").text = item.id;
            }
        }

        private VisualElement MakeItem()
        {
            VisualElement cardElement = _cardUiAsset.CloneTree();
            return cardElement;
        }
    }
}
