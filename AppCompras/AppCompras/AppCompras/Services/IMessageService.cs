using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppCompras.Services
{
    public interface IMessageService
    {
        Task Show(string message);

        Task<string> ShowAsync(string[] message, string title = "Compras");

        Task<bool> QuestionAsync(string messagestring, string aceptar = "Aceptar", string cancelar = "Cancelar");

        void Toast(string messagestring);
    }
}
