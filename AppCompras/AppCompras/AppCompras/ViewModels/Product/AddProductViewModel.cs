using AppCompras.Models.Product;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using static System.Net.Mime.MediaTypeNames;

namespace AppCompras.ViewModels.Product
{
    public class AddProductViewModel: ProductPageViewModel
    {
        #region Fields
        private Products _product;
        private bool _isEdit;
        private bool _isSelectCamFolder;
        private ObservableCollection<CategoryProduct> _categories;

        #endregion

        #region Ctor
        public AddProductViewModel()
        {
            if(ProductSelect != null)
            {
                IsEdit = true;
            }
            InicializeProperties();
        }
        

        #endregion

        #region Properties
        public bool IsEdit
        {
            get => _isEdit;
            set => SetProperty(ref _isEdit, value);
        }
        public Products GetProduct
        {
            get { 
                if(_product ==  null)
                    _product = new Products()
                    {
                        Base64Image = new List<string>(),
                        Categories = new List<CategoryProduct>(),
                        Taxs = new List<Tax>()
                    };
                return _product;
                }
            set => SetProperty(ref _product, value);

        }
        public bool IsSelectCamFolder
        {
            get => _isSelectCamFolder;
            set => SetProperty(ref _isSelectCamFolder, value);
        }
        public ObservableCollection<CategoryProduct> GetCategory
        {
            get => _categories;
            set=> SetProperty(ref _categories, value);
        }
        #endregion

        #region Command
        public Command<object> CreateCommand { get; set; }

        public Command<object> UpdateCommand { get; set; }

        public Command SelectImageCommand { get; set; }

        public Command<object> ShowImageCommand { get; set; }

        public Command<ImageX> DeleteImageCommand { get; set; }

        #endregion

        #region Methods
        private async void InicializeProperties()
        {
            var result = await _serviceFireBase.GetCategories();
            GetCategory = new ObservableCollection<CategoryProduct>(result.Success == true ? (IEnumerable<CategoryProduct>)result.Object: new List<CategoryProduct>());
            

            SelectImageCommand = new Command(() => IsSelectCamFolder = !IsSelectCamFolder);

            ShowImageCommand = new Command<object>(AddImageAsync);

            DeleteImageCommand = new Command<ImageX>(DeleteImage);
        }

        private void DeleteImage(ImageX image)
        {
            
            if (image != null)
            {
                GetProduct.Base64Image.RemoveAt(image.Id);
                _displayAlert.Toast("Se elimino correctamente una imagen.");
                NotifyPropertyChanged(nameof(GetProduct));
            }
        }

        private async void AddImageAsync(object obj)
        {
            var value = bool.Parse(obj.ToString());
            try
            {

                if (value)
                {
                    var options = new MediaPickerOptions
                    {
                        Title = "Capture una imagen para el producto"
                    };
                    var fileResult = await MediaPicker.CapturePhotoAsync(options);
                    var PhotoBytes = StreamToByteArray(await fileResult?.OpenReadAsync());

                    GetProduct.Base64Image.Add(Convert.ToBase64String(PhotoBytes));
                }
                else
                {
                    var options = new PickOptions
                    {
                        PickerTitle = "Selecciones una imagen para la categoría",
                        FileTypes = FilePickerFileType.Images

                    };
                    var fileResult = await FilePicker.PickMultipleAsync(options);
                    foreach(var item in fileResult) 
                    {
                        var PhotoBytes = StreamToByteArray(await item?.OpenReadAsync());

                        GetProduct.Base64Image.Add(Convert.ToBase64String(PhotoBytes));
                    }


                }
                
                NotifyPropertyChanged(nameof(GetProduct));

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await _displayAlert.Show($"El dispositivo no soporta esta ación.\nDetalles:{fnsEx.Message}");
            }
            catch (PermissionException pEx)
            {
                pEx.Message.Contains("Denied");
                {
                    await CheckAndRequestStorageRead();

                    await CheckAndRequestStorageWrite();

                    await CheckAndRequestCamera();
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("is denied."))
                {
                    await _displayAlert.Show("La app no tiene accesos a esta carpeta");
                    return;
                }
                await _displayAlert.Show($"Se produjo una excepcion no controlada. \nDetales: {ex.Message}");
            }
        }
        #endregion

    }
}
