﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using System.Collections.Generic;
using Schedule.Models;
using Schedule.Views;
using Newtonsoft.Json;
using System.Reflection;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Schedule
{
    public partial class App : Application
    {
        //переменная, чтобы проверять обновление только один раз
        public static bool updateWasChecked = false;
        //переменная, чтобы проверять была ли авторизация только что и подгружать расписание
        public static bool justLogged = false; 

        public static List<Faculty> facultiesMain;
        public static List<Faculty> facultiesRait;
        public static List<ExamFaculty> facultiesExam;

        public static List<Specialty> timetable;
        public static List<Day> myTimetable;

        public int MyProperty { get; set; }

        public App()
        {
            facultiesMain = new List<Faculty>();
            facultiesRait = new List<Faculty>();
            facultiesExam = new List<ExamFaculty>();
            timetable = new List<Specialty>();
            myTimetable = new List<Day>();

            InitializeComponent();

            //параметры пользователя
            if (!Current.Properties.ContainsKey("isTeacher")) //проверка на авторизованность
            {
                MainPage = new Login();
            }
            else
            {
                //проверка на существование важных даннах
                if (!Current.Properties.ContainsKey("scheduleMain") ||
                    !Current.Properties.ContainsKey("scheduleRait") ||
                    !Current.Properties.ContainsKey("scheduleExam") ||
                    !Current.Properties.ContainsKey("timetable") ||
                    !Current.Properties.ContainsKey("myTimetable"))
                {
                    justLogged = true;
                    MainPage = new MasterDetailPage1();
                }
                else
                {
                    SchedulesLoad();
                    TimetableLoad();
                    MainPage = new MasterDetailPage1();
                } 
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        /*public void TimetableLoad() //загрузка файла с графиком
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(DayMonday)).Assembly;
            timetable = new List<Specialty>();
            Stream stream2 = assembly.GetManifestResourceStream("Schedule.timetable.json");
            using (var reader = new System.IO.StreamReader(stream2))
            {
                string json = reader.ReadToEnd();
                timetable = JsonConvert.DeserializeObject<List<Specialty>>(json);
            }
        }

        public void SchedulesLoad() //загрузка расписаний каждого факультета
        {
            facultiesJSON = new List<Faculty>();
            facultiesJSONRaiting = new List<Faculty>();
            facultiesJSONExams = new List<ExamFaculty>();

            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(DayMonday)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("Schedule.math_b.json");
            using (var reader = new System.IO.StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                facultiesJSON.Add(JsonConvert.DeserializeObject<Faculty>(json));
            }
            Stream stream1 = assembly.GetManifestResourceStream("Schedule.math_m.json");
            using (var reader = new System.IO.StreamReader(stream1))
            {
                string json = reader.ReadToEnd();
                facultiesJSON.Add(JsonConvert.DeserializeObject<Faculty>(json));
            }

            Stream stream2 = assembly.GetManifestResourceStream("Schedule.math_r.json");
            using (var reader = new System.IO.StreamReader(stream2))
            {
                string json = reader.ReadToEnd();
                facultiesJSONRaiting.Add(JsonConvert.DeserializeObject<Faculty>(json));
            }

            Stream streamExams = assembly.GetManifestResourceStream("Schedule.math_e.json");
            using (var reader = new System.IO.StreamReader(streamExams))
            {
                string json = reader.ReadToEnd();
                facultiesJSONExams.Add(JsonConvert.DeserializeObject<ExamFaculty>(json));
            }

            Stream streamExamsM = assembly.GetManifestResourceStream("Schedule.math_me.json");
            using (var reader = new System.IO.StreamReader(streamExamsM))
            {
                string json = reader.ReadToEnd();
                facultiesJSONExams.Add(JsonConvert.DeserializeObject<ExamFaculty>(json));
            }
        }*/

        
        public void SchedulesLoad() 
        {
            if ((bool)Current.Properties["isTeacher"])
            {
                //загрузка расписаний каждого факультета

            }
            else
            {
                facultiesMain.Add(JsonConvert.DeserializeObject<Faculty>((string)Current.Properties["scheduleMain"]));
                facultiesRait.Add(JsonConvert.DeserializeObject<Faculty>((string)Current.Properties["scheduleRait"]));
                facultiesExam.Add(JsonConvert.DeserializeObject<ExamFaculty>((string)Current.Properties["scheduleExam"]));
            }
        }

        //загрузка графика и определение номера недели
        public void TimetableLoad()
        {
            timetable = JsonConvert.DeserializeObject<List<Specialty>>((string)Current.Properties["timetable"]);

            string table = "";
            if (App.Current.Properties.TryGetValue("myTimetable", out object tableFrom))
            {
                table = (string)tableFrom;
                myTimetable = JsonConvert.DeserializeObject<List<Day>>(table);
            }

            DateTime now = DateTime.Now;
            int day = now.Day;
            int month = now.Month;
            foreach (var item in myTimetable)
            {
                if (item.ThisDay == day && item.ThisMonth == month)
                {
                    if (item.ThisWeek % 2 == 0)
                    {
                        App.Current.Properties["numOfWeek"] = "2";
                    }
                    else
                        App.Current.Properties["numOfWeek"] = "1";
                    break;
                }
            }
        }

    }
}
