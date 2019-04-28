﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SadovodClasses;

namespace SadovodMobile.Activities
{
    [Activity(Label = "AddSteadActivity")]
    public class AddSteadActivity : Activity
    {
        private EditText text;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddStead);

            //Привязка кнопки добавления участка
            FindViewById<Button>(Resource.Id.button1).Click += AddSteadAction;
            //Привязка текста с именем участка
            text = FindViewById<EditText>(Resource.Id.editText1);
        }

        public void AddSteadAction(object sender, EventArgs eventArgs)
        {
            string name = text.Text;
            if(name == null || name.Length == 0 /*|| !Utilities.IsFromLatinOrNums(name)*/)
            {
                //если название говно
                Utilities.ShowMessage(sender, "Название не должно быть пустым");
            }
            else
            {
                //если все ок, то добавляем участок
                UserSingleton.Instance.AddStead(new Stead(name));
                Finish();
            }
        }
    }
}