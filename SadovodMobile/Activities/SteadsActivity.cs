﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

using SadovodClasses;

namespace SadovodMobile.Activities
{
    [Activity(Label = "SteadsActivity")]
    public class SteadsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Steads);

            //Привязка кнопки добавления участка
            FindViewById<Button>(Resource.Id.button1).Click += AddSteadAction;
            //Инициализация всех участков пользователя
            InitializeSteads();
        }

        //Метод действия добавления участка
        public void AddSteadAction(object sender, EventArgs eventArgs)
        {
            //Переключаюсь на окно добавления участка
            Intent intent = new Intent(this, typeof(AddSteadActivity));
            StartActivity(intent);
        }

        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        SteadsAdapter mAdapter;
        //Метод инициализации участков
        private void InitializeSteads()
        {
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            // Plug in the linear layout manager:
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            // Plug in my adapter:
            mAdapter = new SteadsAdapter(UserSingleton.Instance.Steads);
            mRecyclerView.SetAdapter(mAdapter);
            //Привязываю нажатия на элементы адаптера
            mAdapter.ItemClick += OnSteadClick;
            UserSingleton.Instance.SteadsChanged += OnCollectionChanged;
        }
        //Событие изменения коллекции участков
        public void OnCollectionChanged(object sender, EventArgs eventArgs)
        {
            mAdapter.NotifyDataSetChanged();
        }
        //Событие нажатия на участок
        private void OnSteadClick(object sender, int position)
        {
            //Toast.MakeText(this, $"This is stead {position + 1}", ToastLength.Short).Show();
            UserSingleton.Instance.CurrentStead = UserSingleton.Instance.Steads[position];
            //Переключаюсь на окно грядок
            Intent intent = new Intent(this, typeof(BedsActivity));
            StartActivity(intent);
        }

        public class SteadsAdapter : RecyclerView.Adapter
        {
            public event EventHandler<int> ItemClick;
            public ReadOnlyCollection<Stead> Steads;
            public SteadsAdapter(ReadOnlyCollection<Stead> steads)
            {
                Steads = steads;
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                View itemView = LayoutInflater.From(parent.Context).
                            Inflate(Resource.Layout.SteadView, parent, false);
                SteadViewHolder vh = new SteadViewHolder(itemView, OnClick);
                return vh;
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                SteadViewHolder vh = holder as SteadViewHolder;
                vh.Button.Text = Steads[position].Name;
            }

            public override int ItemCount
            {
                get { return Steads.Count; }
            }

            void OnClick(int position)
            {
                ItemClick?.Invoke(this, position);
            }
        }
        public class SteadViewHolder : RecyclerView.ViewHolder
        {
            public Button Button { get; private set; }

            public SteadViewHolder(View itemView, Action<int> listener) : base(itemView)
            {
                // Locate and cache view references:
                Button = itemView.FindViewById<Button>(Resource.Id.button);
                Button.Click += (sender, e) => listener(base.LayoutPosition);
            }
        }
    }
}