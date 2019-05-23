﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Reflection;
using System.IO;
using Schedule.Models;
using System.Collections.ObjectModel;
using Schedule.ViewModels;

namespace Schedule.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimelinePage : ContentPage
    {
        public TimelinePage()
        {
            InitializeComponent();
            TimelineViewModel bind = new TimelineViewModel();
            BindingContext = bind;

            //проверяется студент или преподаватель
            if (App.Current.Properties.TryGetValue("isTeacher", out object isTeacher))
                if ((bool)isTeacher)
                {
                    couplesList.ItemsSource = bind.ItemsForTeacher;
                }
                else
                {
                    couplesList.ItemsSource = bind.ItemsForStudents;
                }
        }
    }

    
}