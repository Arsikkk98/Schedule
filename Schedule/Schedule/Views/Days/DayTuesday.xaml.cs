﻿using Schedule.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Schedule.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DayTuesday : ContentPage
	{
		public DayTuesday()
		{
			InitializeComponent ();
            //BindingContext = new FillTuesday();

        }
	}
}