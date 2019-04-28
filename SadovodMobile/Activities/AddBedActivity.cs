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
    [Activity(Label = "BedActivity")]
    public class AddBedActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddGardenBed);

            //Привязка кнопки добавления записки
            FindViewById<Button>(Resource.Id.button1).Click += AddNoteAction;
            //Привязка кнопки добавления грядки
            FindViewById<Button>(Resource.Id.button2).Click += AddBedAction;

            //Инициализация всех полей
            Initialize();
        }
        //Экземпляр грядки, который будет добавлен
        private GardenBed bed;
        //Поле вида растения
        private EditText typeName;
        //Поле сорта растения
        private EditText sortName;
        //Поле даты полива
        private EditText waterDate;
        //Поле периодичности полива
        private EditText waterPeriod;
        //Поле даты прополки
        private EditText weedDate;
        //Поле периодичности прополки
        private EditText weedPeriod;
        //Поле даты окучивания
        private EditText pileUpDate;
        //Поле периодичности окучивания
        private EditText pileUpPeriod;
        //Поле даты удобрения
        private EditText fertilizeDate;
        //Поле периодичности удобрения
        private EditText fertilizePeriod;
        //Инициализация всех полей
        private void Initialize()
        {
            //Поле вида растения
            typeName = FindViewById<EditText>(Resource.Id.editText1);
            //Поле сорта растения
            sortName = FindViewById<EditText>(Resource.Id.editText2);
            //Поле даты полива
            waterDate = FindViewById<EditText>(Resource.Id.editText3);
            //Поле периодичности полива
            waterPeriod = FindViewById<EditText>(Resource.Id.editText4);
            //Поле даты прополки
            weedDate = FindViewById<EditText>(Resource.Id.editText5);
            //Поле периодичности прополки
            weedPeriod = FindViewById<EditText>(Resource.Id.editText6);
            //Поле даты окучивания
            pileUpDate = FindViewById<EditText>(Resource.Id.editText7);
            //Поле периодичности окучивания
            pileUpPeriod = FindViewById<EditText>(Resource.Id.editText8);
            //Поле даты удобрения
            fertilizeDate = FindViewById<EditText>(Resource.Id.editText9);
            //Поле периодичности удобрения
            fertilizePeriod = FindViewById<EditText>(Resource.Id.editText10);

            bed = new GardenBed(new PlantType("Вид", "Сорт"));
            typeName.Text = bed.Plant.TypeName;
            sortName.Text = bed.Plant.SortName;
            //Полив
            waterDate.Text = bed.WaterDate.ToString("dd/MM/yyyy hh:mm");
            waterPeriod.Text = bed.WaterPeriod.ToString();
            //Прополка
            weedDate.Text = bed.WeedDate.ToString("dd/MM/yyyy hh:mm");
            weedPeriod.Text = bed.WeedPeriod.ToString();
            //Окучивание
            pileUpDate.Text = bed.PileUpDate.ToString("dd/MM/yyyy hh:mm");
            pileUpPeriod.Text = bed.PileUpPeriod.ToString();
            //Удобрение
            fertilizeDate.Text = bed.FertilizeDate.ToString("dd/MM/yyyy hh:mm");
            fertilizePeriod.Text = bed.FertilizePeriod.ToString();


            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            // Plug in the linear layout manager:
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            // Plug in my adapter:
            mAdapter = new NotesAdapter(bed.Notes);
            mRecyclerView.SetAdapter(mAdapter);
            //Привязываю нажатия на элементы адаптера
            mAdapter.NoteChanged += OnNoteChanged;
        }

        private void OnNoteChanged(object sender, NotesArgs args)
        {
            bed.Notes[args.Position] = args.Text;
        }

        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private NotesAdapter mAdapter;

        //Действие добавления грядки
        public void AddBedAction(object sender, EventArgs eventArgs)
        {

            //FIXME:: Беру все эти поля нахуй и еще записки в придачу и хуярю все в грядку

            bed.Plant.TypeName = typeName.Text;
            bed.Plant.SortName = sortName.Text;

            int parseResult;
            bed.WaterDate = Utilities.DateTimeFormat(waterDate.Text);
            if (int.TryParse(waterPeriod.Text, out parseResult))
            {
                bed.WaterPeriod = parseResult;
            }
            bed.WeedDate = Utilities.DateTimeFormat(weedDate.Text);
            if (int.TryParse(weedPeriod.Text, out parseResult))
            {
                bed.WeedPeriod = parseResult;
            }
            bed.PileUpDate = Utilities.DateTimeFormat(pileUpDate.Text);
            if (int.TryParse(pileUpPeriod.Text, out parseResult))
            {
                bed.PileUpPeriod = parseResult;
            }
            bed.FertilizeDate = Utilities.DateTimeFormat(fertilizeDate.Text);
            if (int.TryParse(fertilizePeriod.Text, out parseResult))
            {
                bed.FertilizePeriod = parseResult;
            }

            UserSingleton.Instance.CurrentStead.AddBed(bed);
            Finish();
        }

        //Действие добавления записки
        public void AddNoteAction(object sender, EventArgs eventArgs)
        {
            bed.AddNote("");
            mAdapter.NotifyDataSetChanged();
        }

        public class NotesArgs : EventArgs
        {
            public int Position;
            public string Text;

            public NotesArgs(int pos, string text)
            {
                Position = pos;
                Text = text;
            }
        }

        public class NotesAdapter : RecyclerView.Adapter
        {
            public event EventHandler<NotesArgs> NoteChanged;
            public List<string> Notes;
            public NotesAdapter(List<string> notes)
            {
                Notes = notes;
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                View itemView = LayoutInflater.From(parent.Context).
                            Inflate(Resource.Layout.GardenNoteView, parent, false);
                SteadViewHolder vh = new SteadViewHolder(itemView, OnNoteChanged);
                return vh;
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                SteadViewHolder vh = holder as SteadViewHolder;
                vh.NoteName.Text = $"Записка {position}";
                vh.Note.Text = Notes[position];
            }

            public override int ItemCount
            {
                get { return Notes.Count; }
            }

            void OnNoteChanged(int position, string text)
            {
                NoteChanged?.Invoke(this, new NotesArgs(position, text));
            }
        }
        public class SteadViewHolder : RecyclerView.ViewHolder
        {
            public TextView NoteName { get; private set; }
            public EditText Note { get; private set; }

            public SteadViewHolder(View itemView, Action<int, string> listener) : base(itemView)
            {
                // Locate and cache view references:
                NoteName = itemView.FindViewById<TextView>(Resource.Id.textView1);

                Note = itemView.FindViewById<EditText>(Resource.Id.editText1);
                Note.TextChanged += (sender, e) => listener(LayoutPosition, Note.Text);
            }
        }
    }
}