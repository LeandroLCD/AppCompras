using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppCompras.Services
{
    public class MessageService : IMessageService
    {
        private readonly IToast _toast;
        public MessageService()
        {
            _toast = DependencyService.Get<IToast>();
        }
        public async Task<bool> QuestionAsync(string messagestring, string aceptar = "Aceptar", string cancelar = "Cancelar")
        {
            return await App.Current.MainPage.DisplayAlert("¿Consulta?", messagestring, aceptar, cancelar);

        }

        public async Task Show(string message)
        {
            await App.Current.MainPage.DisplayAlert("Notificacón!", message, "Ok");

        }

        public async Task<string> ShowAsync(string[] message, string title = "Compras")
        {
            return await App.Current.MainPage.DisplayActionSheet(title, "", "Cancelar", message);

        }

        public void Toast(string messagestring)
        {
            _toast.Show(messagestring);
        }
    }
}
