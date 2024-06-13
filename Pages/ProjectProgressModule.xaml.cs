﻿using LiveCharts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TeamHubServiceProjects.DTOs;
using TeamsHubDesktopClient.DTOs;
using TeamsHubDesktopClient.Gateways.Provider;
using TeamsHubDesktopClient.SinglentonClasses;

namespace TeamsHubDesktopClient.Pages
{
    /// <summary>
    /// Lógica de interacción para ProjectProgressModule.xaml
    /// </summary>
    public partial class ProjectProgressModule : Page
    {

        public ProjectProgressModule()
        {
            InitializeComponent();
            lblProyectName.Content = ProjectSinglenton.projectDTO.Name;
            _TaskManagement = new TaskManagementRESTProvider();
            _tasks = _TaskManagement.GetAllTaskByProject(ProjectSinglenton.projectDTO.IdProject);
            InitializeGraph();
            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public Func<ChartPoint, string> PointLabel { get; set; }
        public ChartValues<double> Values1 { get; set; }
        public ChartValues<double> Values2 { get; set; }
        public ChartValues<double> Values3 { get; set; }
        private List<TaskDTO> _tasks;
        private TaskManagementRESTProvider _TaskManagement;

        private void InitializeGraph()
        {
            var totalTasks = _tasks.Count;
            var pendingTasks = _tasks.Count(t => t.Status == "Actividad Pendiente");
            var inProgressTasks = _tasks.Count(t => t.Status == "Actividad en proceso");
            var finishedTasks = _tasks.Count(t => t.Status == "Actividad Finalizada");

            lblTaskNum.Content = totalTasks;

            PointLabel = chartPoint =>
            {
                switch (chartPoint.SeriesView.Title)
                {
                    case "Actividades Pendientes":
                        return string.Format("Actividades Pendientes ({0:P})", chartPoint.Participation);
                    case "Actividades en proceso":
                        return string.Format("Actividades en proceso ({0:P})", chartPoint.Participation);
                    case "Actividades Terminadas":
                        return string.Format("Actividades Finalizadas ({0:P})", chartPoint.Participation);
                    default:
                        return string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
                }
            };

            Values1 = new ChartValues<double> { pendingTasks };
            Values2 = new ChartValues<double> { inProgressTasks };
            Values3 = new ChartValues<double> { finishedTasks };

            SeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Actividades Pendientes",
                    Values = Values1,
                    DataLabels = true,
                    LabelPoint = PointLabel
                },
                new PieSeries
                {
                    Title = "Actividades en proceso",
                    Values = Values2,
                    DataLabels = true,
                    LabelPoint = PointLabel
                },
                new PieSeries
                {
                    Title = "Actividades Terminadas",
                    Values = Values3,
                    DataLabels = true,
                    LabelPoint = PointLabel
                }
            };
        }

        private void PieChart_DataClick(object sender, LiveCharts.ChartPoint chartPoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartPoint.ChartView;
            foreach (PieSeries series in chart.Series)
            {
                series.PushOut = 0;
            }

            var selectedSeries = (PieSeries)chartPoint.SeriesView;
            selectedSeries.PushOut = 8;
        }
    }
}