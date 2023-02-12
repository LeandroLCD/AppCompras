using AppCompras.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppCompras.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Fields

        private string _menssage;
        private bool _isLook;
        private bool _isShow;
        private string _titleSubMenu;
        private Dictionary<string, string> _subMenuOptions;


        private static bool isCompletet { get; set; }

        #endregion

        public BaseViewModel()
        {
            _displayAlert = DependencyService.Get<IMessageService>();

            _serviceFireBase = DependencyService.Get<IApiFireBase>();
        }

        #region Event handler

        /// <summary>
        /// Ocurre cuando un propiedad cambia.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties
        public IMessageService _displayAlert {get; set;}

        public IApiFireBase _serviceFireBase { get; set; }  
        /// <summary>
        /// Notifica que una tarea en cursa ha sido completada
        /// </summary>
        public static bool IsCompletet
        {
            get
            {
                return isCompletet;
            }

            set
            {
                if (isCompletet == value)
                {
                    return;
                }

                isCompletet = value;
            }
        }
        /// <summary>
        /// Bloquea la actividad principal del usuario, con un activity indicator.
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return _isLook;
            }

            set
            {
                this.SetProperty(ref _isLook, value);
            }
        }
        /// <summary>
        /// Controla la visibilidad del submenu en una vista.
        /// </summary>
        public bool IsShowSubMenu
        {
            get
            {
                return _isShow;
            }

            set
            {
                this.SetProperty(ref _isShow, value);
            }
        }
        /// <summary>
        /// Muestra un mensaje durando la ejecución de una tarea. 
        /// </summary>
        public string StatusMessage
        {
            get
            {
                return _menssage;
            }

            set
            {
                this.SetProperty(ref _menssage, value);
            }
        }
        public string TitleSubMenu
        {
            get
            {
                return _titleSubMenu;
            }

            set
            {
                this.SetProperty(ref _titleSubMenu, value);
            }
        }
        /// <summary>
        /// Carga la lista de opciones en el menu, Key = icon y Value = la opción.
        /// </summary>
        public Dictionary<string, string> SubMenuOptions
        {
            get => _subMenuOptions;
            set => SetProperty(ref _subMenuOptions, value);
        }

        #endregion

        #region Command
        /// <summary>
        /// Muestra el Shell menu.
        /// </summary>
        public Command ShowMenuCommand => new Command( ()=>
        {
            Shell.Current.FlyoutIsPresented = true;
        });
        /// <summary>
        /// Muestra o oculta el submenu.
        /// </summary>
        public Command ShowSubMenuCommand => new Command(() =>
        {
            IsShowSubMenu = !IsShowSubMenu;
        });
        /// <summary>
        /// Retorna ol objeto seleccionado en el submenu. Debe implementarse como new public Command<object> OptionSelectCommand, en el viewmodel donde se use un submenu.
        /// </summary>
        public Command<object> OptionSelectCommand => new Command<object>((obj) => {

            throw new NotImplementedException();
        });

        #endregion

        #region Methods
        /// <summary>
        /// Combierte un Stream en un Byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public byte[] StreamToByteArray(Stream stream)
        {
            stream.Position = 0L;
            byte[] array = new byte[stream.Length];
            stream.Read(array, 0, array.Length);
            return array;
        }
        public async Task<PermissionStatus> CheckAndRequestStorageWrite()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && Device.RuntimePlatform == Device.Android)
            {
                await _displayAlert.Show("El Usuario denego el permiso para guardar archivos en el dispostivo.");
                return status;
            }

            if (Permissions.ShouldShowRationale<Permissions.StorageWrite>())
            {
                await _displayAlert.Show("Se requiere su permiso para almacenar los archivos en el dispositivo, por favor conceda el permiso.");
            }

            status = await Permissions.RequestAsync<Permissions.StorageWrite>();

            return status;
        }

        public async Task<PermissionStatus> CheckAndRequestStorageRead()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && Device.RuntimePlatform == Device.Android)
            {
                await _displayAlert.Show("El Usuario denego el permiso para guardar archivos en el dispostivo.");
                return status;
            }

            if (Permissions.ShouldShowRationale<Permissions.StorageRead>())
            {
                await _displayAlert.Show("Se requiere su permiso para almacenar los archivos en el dispositivo, por favor conceda el permiso.");
            }

            status = await Permissions.RequestAsync<Permissions.StorageRead>();

            return status;
        }

        public async Task<PermissionStatus> CheckAndRequestCamera()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && Device.RuntimePlatform == Device.Android)
            {
                await _displayAlert.Show("El Usuario denego el permiso para usar la camaradel dispostivo, debe ingresar al administrador de aplicaciones a conseder manualmente el permiso.");
                return status;
            }

            if (Permissions.ShouldShowRationale<Permissions.Camera>())
            {
                await _displayAlert.Show("Se requiere su permiso para capturar fotos, por favor conceda el permiso.");
            }

            status = await Permissions.RequestAsync<Permissions.Camera>();

            return status;
        }

        /// <summary>
        /// Notifica a la vista cuando una propiedad ha cambiado.
        /// </summary>
        /// <param name="propertyName">The PropertyName</param>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Establece el nuevo valor de una propiedad ej: SetProperty(ref _property, value)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            this.NotifyPropertyChanged(propertyName);

            return true;
        }

        #endregion
    }
}
