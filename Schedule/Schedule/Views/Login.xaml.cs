﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Schedule.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Schedule.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{
		public Login ()
		{
			InitializeComponent ();
            loadFaculties();
            loadTeachers();
        }

        //переменные хранения выбранных значений
        string selectedFaculty = "";
        string selectedGroup = "";
        string selectedGroupName = "";
        string selectedSubgroup = "";
        string selectedTeacher = "";
        bool isTeacher = false;

        //Загрузка факультетов, для отображения в списке факультетов
        List<string> faculties = new List<string>();
        void loadFaculties()
        {
            foreach (var faculty in App.facultiesJSON)
            {
                if (!groups.Contains(faculty.FacultyName))
                {
                    groups.Add(faculty.FacultyName);
                }
            }
        }
        //Загрузка групп, для отображения в списке групп
        List<string> groups = new List<string>();
        void loadGroups()
        {
            foreach (var f in App.facultiesJSON)
            {
                if (f.FacultyName == selectedFaculty)
                {
                    foreach (var item in f.Groups)
                    {
                        groups.Add(item.GroupId);
                    }
                    break;
                }
            }
        }
        //Загрузка подгрупп, для отображения в списке групп
        List<string> subgroups = new List<string>();
        void loadSubgroups()
        {
            foreach (var f in App.facultiesJSON)
            {
                if (f.FacultyName == selectedFaculty)
                {
                    foreach (var g in f.Groups)
                    {
                        if (g.GroupId == selectedGroup)
                        {
                            foreach (var s in g.Couples)
                            {
                                if (s.SubgroupName == "null")
                                    break;
                                if (!subgroups.Contains(s.SubgroupName))
                                {
                                    subgroups.Add(s.SubgroupName);
                                }
                            }
                            break;
                        }
                    }
                    break;
                }
            }
        }
        //Загрузка преподавателей, для отображения в списке преподавателей
        List<string> teachers = new List<string>();
        void loadTeachers()
        {
            foreach (var f in App.facultiesJSON)
            {
                if (f.FacultyName == selectedFaculty)
                {
                    foreach (var g in f.Groups)
                    {
                        foreach (var s in g.Couples)
                        {
                            if (!teachers.Contains(s.CoupleTeacher))
                            {
                                subgroups.Add(s.CoupleTeacher);
                            }
                        }

                    }
                    break;
                }
            }

            teachers.Sort();
        }


        Label headerForPicker;
        Picker picker;

        //изменение поля с выбором типа пользователя
        void pickerUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedGroup = ""; //обнуляем переменные хранения значений
            selectedTeacher = "";
            Picker workPicker = (Picker)sender;
            if (workPicker.SelectedIndex == 0) //Если выбран студент
            {
                isTeacher = false;
            }
            else isTeacher = true;

            headerForPicker = new Label
            {
                Text = "Выберите факультет:",
                TextColor = Color.FromRgb(38, 38, 38),
                Margin = new Thickness(10, 0, 0, 0),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
            };

            picker = new Picker
            {
                Title = ""

            };
            foreach (var item in groups)
            {
                picker.Items.Add(item);
            }

            picker.SelectedIndexChanged += pickerFaculty_SelectedIndexChanged;

            SelectFacultyStackLoyaout.Children.Clear();
            SelectGroupStackLoyaout.Children.Clear();
            SelectSubgroupStackLoyaout.Children.Clear();

            SelectFacultyStackLoyaout.Children.Add(headerForPicker);
            SelectFacultyStackLoyaout.Children.Add(picker);

        }
        //изменение поля с выбором факультета
        void pickerFaculty_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker pic = (Picker)sender;
            selectedFaculty = pic.SelectedItem.ToString(); //сохранение группы

            loadGroups();

            if (!isTeacher) //Если выбран студент
            {
                headerForPicker = new Label
                {
                    Text = "Выберите группу:",
                    TextColor = Color.FromRgb(38, 38, 38),
                    Margin = new Thickness(10, 0, 0, 0),
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                };

                picker = new Picker
                {
                    Title = ""

                };
                foreach (var item in groups)
                {
                    picker.Items.Add(item);
                }

                picker.SelectedIndexChanged += pickerGroup_SelectedIndexChanged;


            }
            else //Если выбран преподаватель
            {
                headerForPicker = new Label
                {
                    Text = "Выберите преподавателя:",
                    TextColor = Color.FromRgb(38, 38, 38),
                    Margin = new Thickness(10, 0, 0, 0),
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                };

                picker = new Picker
                {
                    Title = ""

                };
                foreach (var item in teachers)
                {
                    picker.Items.Add(item);
                }

                picker.SelectedIndexChanged += pickerTeacher_SelectedIndexChanged;

            }

            SelectGroupStackLoyaout.Children.Clear();
            SelectSubgroupStackLoyaout.Children.Clear();

            SelectGroupStackLoyaout.Children.Add(headerForPicker);
            SelectGroupStackLoyaout.Children.Add(picker);

        }
        //изменение поля с выбором группы
        void pickerGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker pic = (Picker)sender;
            selectedGroup = pic.SelectedItem.ToString(); //сохранение группы
            string selectedGroupId = pic.SelectedItem.ToString();

            loadSubgroups();

            if (subgroups.Count > 0)
            {
                headerForPicker = new Label
                {
                    Text = "Выберите подгруппу:",
                    TextColor = Color.FromRgb(38, 38, 38),
                    Margin = new Thickness(10, 0, 0, 0),
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                };

                picker = new Picker
                {
                    Title = "Подгруппа"

                };

                foreach (var item in subgroups)
                {
                    picker.Items.Add(item);
                }

                picker.SelectedIndexChanged += pickerSubgroup_SelectedIndexChanged;

                SelectSubgroupStackLoyaout.Children.Clear();
                SelectSubgroupStackLoyaout.Children.Add(headerForPicker);
                SelectSubgroupStackLoyaout.Children.Add(picker);
            }
            else
            {
                SelectSubgroupStackLoyaout.Children.Clear();
                //Если нет подгруппы показываем кнопку сохранения
                saveButton.Clicked += onSavebuttonClick;
                StackLoyaoutForSavebutton.Children.Add(saveButton);
            }


        }

        //Кнопка Сохранить
        Button saveButton = new Button
        {
            Text = "Сохранить",
            TextColor = Color.FromRgb(38, 38, 38),
            Margin = new Thickness(10),
            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
            BackgroundColor = Color.FromRgb(235, 179, 13)
        };

        //изменение поля с выбором преподавателя
        void pickerTeacher_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker pic = (Picker)sender;
            selectedTeacher = pic.SelectedItem.ToString();
            saveButton.Clicked += onSavebuttonClick;
            StackLoyaoutForSavebutton.Children.Add(saveButton);
        }

        //изменение поля с выбором подгруппы
        void pickerSubgroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker pic = (Picker)sender;
            selectedSubgroup = pic.SelectedItem.ToString();
            saveButton.Clicked += onSavebuttonClick;
            StackLoyaoutForSavebutton.Children.Add(saveButton);
        }


        //Нажатие на кнопку Сохранить
        void onSavebuttonClick(object sender, EventArgs e)
        {
            if (selectedFaculty != "")
            {
                App.Current.Properties.Add("facultyName", selectedFaculty);

                if (selectedGroup != "")
                {
                    //Сохранение данных в словарь App.Current.Properties
                    App.Current.Properties.Add("groupId", selectedGroup);
                    //получение наименования группы (специальность) (нужна для вывода в меню)
                    foreach (var f in App.facultiesJSON)
                    {
                        if (f.FacultyName == selectedFaculty)
                        {
                            foreach (var g in f.Groups)
                            {
                                if (g.GroupId == selectedGroup)
                                {
                                    App.Current.Properties.Add("groupName", selectedGroup + " | " + g.GroupName);
                                }
                            }
                        }
                    }

                    if (selectedSubgroup != "")
                    {
                        App.Current.Properties.Add("subgroup", selectedSubgroup);
                    }

                    App.Current.Properties.Add("isTeacher", false);
                }
                else
                {
                    App.Current.Properties.Add("isTeacher", true);
                    App.Current.Properties.Add("teacherName", selectedTeacher);
                }
            }

            App.Current.Properties.Add("numOfWeek", "1");
            App.Current.MainPage = new MasterDetailPage1();
        }
    }
}