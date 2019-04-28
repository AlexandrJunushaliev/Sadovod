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
    [Activity(Label = "GardenBedsActivity")]
    public class BedsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GardenBeds);

            //Привязка кнопки добавления грядки
            FindViewById<Button>(Resource.Id.button1).Click += AddBedAction;
            //Инициализация всех грядок пользователя
            InitializeBeds();
        }
        //Метод действия добавления грядки
        public void AddBedAction(object sender, EventArgs eventArgs)
        {
            //Переключаюсь на окно добавления грядки
            Intent intent = new Intent(this, typeof(AddBedActivity));
            StartActivity(intent);
        }

        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        BedsAdapter mAdapter;
        //Метод инициализации грядок
        private void InitializeBeds()
        {
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            // Plug in the linear layout manager:
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            // Plug in my adapter:
            mAdapter = new BedsAdapter(UserSingleton.Instance.CurrentStead.GardenBeds);
            mRecyclerView.SetAdapter(mAdapter);
            //Привязываю нажатия на элементы адаптера
            mAdapter.ItemClick += OnBedClick;
            mAdapter.WaterClick += OnWaterClick;
            mAdapter.WeedClick += OnWeedClick;
            mAdapter.PileUpClick += OnPileUpClick;
            mAdapter.FertilizeClick += OnFertilizeClick;
            UserSingleton.Instance.CurrentStead.BedsChanged += OnCollectionChanged;
        }

        //Событие изменения коллекции грядок
        public void OnCollectionChanged(object sender, EventArgs eventArgs)
        {
            mAdapter.NotifyDataSetChanged();
        }
        //Событие нажатия на грядку
        private void OnBedClick(object sender, int position)
        {
            //Toast.MakeText(this, $"This is bed {position + 1}", ToastLength.Short).Show();
            UserSingleton.Instance.CurrentBed = UserSingleton.Instance.CurrentStead.GardenBeds[position];
            //Переключаюсь на окно редактирования грядки
            Intent intent = new Intent(this, typeof(BedActivity));
            StartActivity(intent);
        }
        //Событие нажатия на полив
        private void OnWaterClick(object sender, int position)
        {
            UserSingleton.Instance.CurrentStead.GardenBeds[position].WaterDate = DateTime.Now;
        }
        //Событие нажатия на прополку
        private void OnWeedClick(object sender, int position)
        {
            UserSingleton.Instance.CurrentStead.GardenBeds[position].WeedDate = DateTime.Now;
        }
        //Событие нажатия на окучивание
        private void OnPileUpClick(object sender, int position)
        {
            UserSingleton.Instance.CurrentStead.GardenBeds[position].PileUpDate = DateTime.Now;
        }
        //Событие нажатия на удобрение
        private void OnFertilizeClick(object sender, int position)
        {
            UserSingleton.Instance.CurrentStead.GardenBeds[position].FertilizeDate = DateTime.Now;
        }

        public class BedsAdapter : RecyclerView.Adapter
        {
            public event EventHandler<int> ItemClick;
            public event EventHandler<int> WaterClick;
            public event EventHandler<int> WeedClick;
            public event EventHandler<int> PileUpClick;
            public event EventHandler<int> FertilizeClick;
            public ReadOnlyCollection<GardenBed> Beds;
            public BedsAdapter(ReadOnlyCollection<GardenBed> steads)
            {
                Beds = steads;
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                View itemView = LayoutInflater.From(parent.Context).
                            Inflate(Resource.Layout.BedView, parent, false);
                SteadViewHolder vh = new SteadViewHolder(itemView, OnClick, OnWaterClick, OnWeedClick, OnPileUpClick, OnFertilizeClick);
                return vh;
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                SteadViewHolder vh = holder as SteadViewHolder;
                vh.TypeName.Text = Beds[position].Plant.TypeName;
                vh.SortName.Text = Beds[position].Plant.SortName;
            }

            public override int ItemCount
            {
                get { return Beds.Count; }
            }

            void OnClick(int position)
            {
                ItemClick?.Invoke(this, position);
            }

            void OnWaterClick(int position)
            {
                WaterClick?.Invoke(this, position);
            }
            void OnWeedClick(int position)
            {
                WeedClick?.Invoke(this, position);
            }
            void OnPileUpClick(int position)
            {
                PileUpClick?.Invoke(this, position);
            }
            void OnFertilizeClick(int position)
            {
                FertilizeClick?.Invoke(this, position);
            }
        }
        public class SteadViewHolder : RecyclerView.ViewHolder
        {
            public LinearLayout MainBedLayout { get; private set; }
            public TextView TypeName { get; private set; }
            public TextView SortName { get; private set; }

            public SteadViewHolder(View itemView, Action<int> listener,
                Action<int> waterListener, Action<int> weedListener, Action<int> pileUpListener, Action<int> fertilizeListener) : base(itemView)
            {
                // Locate and cache view references:
                MainBedLayout = itemView.FindViewById<LinearLayout>(Resource.Id.bedLinLayout);
                MainBedLayout.Click += (sender, e) => listener(LayoutPosition);

                TypeName = itemView.FindViewById<TextView>(Resource.Id.textView1);
                SortName = itemView.FindViewById<TextView>(Resource.Id.textView2);

                itemView.FindViewById<ImageButton>(Resource.Id.imageButton1).Click += (sender, r) => waterListener(LayoutPosition);
                itemView.FindViewById<ImageButton>(Resource.Id.imageButton2).Click += (sender, r) => weedListener(LayoutPosition);
                itemView.FindViewById<ImageButton>(Resource.Id.imageButton3).Click += (sender, r) => pileUpListener(LayoutPosition);
                itemView.FindViewById<ImageButton>(Resource.Id.imageButton4).Click += (sender, r) => fertilizeListener(LayoutPosition);
            }
        }
    }
}