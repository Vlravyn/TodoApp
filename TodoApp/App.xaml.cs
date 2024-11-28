﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvvmEssentials.Navigation.WPF.Dialog;
using System.IO;
using System.Reflection;
using System.Windows;
using TodoApp.Core.EntityFramework;
using TodoApp.Core.Services;
using TodoApp.Services;
using TodoApp.ViewModels;
using TodoApp.Views;
using MvvmEssentials.Core.Navigation;
using MvvmEssentials.Navigation.WPF.Navigation;
using MvvmEssentials.Core.Dialog;

namespace TodoApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
        }

        private static readonly IHost _host =
            Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c =>
            {
                var entryAssembly = Assembly.GetEntryAssembly();
                if (entryAssembly is null)
                    throw new Exception("Entry assembly is null. Occured while cofigurating the application host.");

                var assemblyPath = Path.GetDirectoryName(entryAssembly.Location);
                if (string.IsNullOrEmpty(assemblyPath))
                    throw new Exception("Directory path of EntryAssembly is null");

                c.SetBasePath(assemblyPath);
            })
            .ConfigureAppConfiguration(d1 =>
            {
            })
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<UserTasksDbContext>();
                services.AddSingleton<IDialogService, DialogService>();
                services.AddSingleton<IUserTaskService, UserTaskService>();
                services.AddSingleton<INavigationService, NavigationService>();

                services.AddSingleton<ApplicationHostService>();
                services.AddSingleton<ReminderService>();

                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<AllTasksViewModel>();
                services.AddSingleton<TaskDetailViewModel>();
                services.AddSingleton<CompletedTasksViewModel>();
                services.AddSingleton<TaskListViewModel>();

                services.AddSingleton<MainWindow>();
                services.AddSingleton<AllTasksPage>();
                services.AddSingleton<CompletedTasksPage>();
                services.AddSingleton<TaskDetailPage>();
                services.AddSingleton<TaskListPage>();
                services.AddTransient<TaskCollectionCardControlViewModel>();
            })
            .Build();

        private async void OnStartup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();
            await GetService<ApplicationHostService>().StartAsync(new CancellationToken());
        }

        /// <summary>
        /// Gets registered service.
        /// </summary>
        /// <typeparam name="T">Type of the service to get.</typeparam>
        /// <returns>Instance of the service or <see langword="null"/>.</returns>
        public static T GetService<T>() where T : class
        {
            var result = _host.Services.GetService(typeof(T));
            if (result is T converted)
                return converted;

            throw new Exception($"Could not find a registered service of type {typeof(T).Name}");
        }
    }
}