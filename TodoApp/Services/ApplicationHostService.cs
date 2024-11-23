using Microsoft.Extensions.Hosting;
using MvvmEssentials.Core.Dialog;
using System.Windows;
using TodoApp.Views;

namespace TodoApp.Services
{
    public class ApplicationHostService : IHostedService
    {
        public ApplicationHostService(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        private IDialogService dialogService;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await HandleActivationAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Creates main window during activation.
        /// </summary>
        private async Task HandleActivationAsync()
        {
            await Task.CompletedTask;

            if (!Application.Current.Windows.OfType<MainWindow>().Any())
            {
                dialogService.Show(typeof(MainWindow));
            }

            await Task.CompletedTask;
        }
    }
}