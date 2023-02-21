using AppCompras.Models.Product;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using static System.Net.Mime.MediaTypeNames;

namespace AppCompras.ViewModels.Product
{
    public class AddProductViewModel: BaseViewModel
    {
        #region Fields
        private Products _product;
        private bool _isEdit;
        private bool _isSelectCamFolder;
        private ObservableCollection<CategoryProduct> _categories;
        private bool _isAddCategory;
        private Command<object> _addCategoryCommand;
        private Command<CategoryProduct> _deletCategoryCommand;
        private Command<object> _addTaxesCommand;
        private bool _isAddTaxes;
        private Command<Tax> _deleteTaxesCommand;
        private string _name;
        private Command _getProductChangedCommand;
        private Command _modalTaxesCommand;
        private Command _modalCategoryCommand;
        private Command<object> _showImageCommand;
        private Command<ImageX> _deleteImageCommand;
        private Command<object> _createCommand;
        private ObservableCollection<CategoryProduct> _getProductCategories;
        private ObservableCollection<Tax> _getProductTaxes;

        #endregion

        #region Ctor
        public AddProductViewModel()
        {
            if(EditProduct != null)
            {
                IsEdit = true;
            }
            InicializeProperties();
        }
        

        #endregion

        #region Properties

        public static Products EditProduct { get; set; }
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
        public bool IsAddCategory 
        {
            get => _isAddCategory;
            set => SetProperty(ref _isAddCategory, value);
        }

        public bool IsAddTaxes
        {
            get => _isAddTaxes;
            set => SetProperty(ref _isAddTaxes, value);
        }

        public string Name
        {
            get => _name;
            set=> SetProperty(ref _name, value);
        }
        public ObservableCollection<CategoryProduct> GetCategory
        {
            get 
            {
                if(_categories == null)
                    _categories = new ObservableCollection<CategoryProduct>();
                return _categories;
            }
            set=> SetProperty(ref _categories, value);
        }
        public ObservableCollection<CategoryProduct> GetProductCategories
        {
            get
            {
                if (_getProductCategories == null)
                    _getProductCategories = new ObservableCollection<CategoryProduct>();
                return _getProductCategories;
            }
            set => SetProperty(ref _getProductCategories, value);
        }
        #endregion

        #region Command
        public Command<object> CreateCommand
        {
            get => _createCommand;
            set => SetProperty(ref _createCommand, value);
        }

        public Command<object> UpdateCommand { get; set; }

        public Command SelectImageCommand { get; set; }
        public Command ModalCategoryCommand
        {
            get => _modalCategoryCommand;
            set => SetProperty(ref _modalCategoryCommand, value);
        }
        public Command<object> AddCategoryCommand 
        {
            get => _addCategoryCommand;
            set => SetProperty(ref _addCategoryCommand, value);
        }
        public Command<CategoryProduct> DeleteCategoryCommand
        {
            get => _deletCategoryCommand;
            set => SetProperty(ref _deletCategoryCommand, value);
        }
        public Command<Tax> DeleteTaxesCommand
        {
            get => _deleteTaxesCommand;
            set => SetProperty(ref _deleteTaxesCommand, value);
        }
        public Command<object> AddTaxesCommand
        {
            get => _addTaxesCommand;
            set => SetProperty(ref _addTaxesCommand, value);
        }
        public Command ModalTaxesCommand
        {
            get => _modalTaxesCommand;
            set => SetProperty(ref _modalTaxesCommand, value);
        }
        public Command<object> ShowImageCommand
        {
            get => _showImageCommand;
            set=> SetProperty(ref _showImageCommand, value);
        }

        public Command<ImageX> DeleteImageCommand
        {
            get => _deleteImageCommand;
            set=> SetProperty(ref _deleteImageCommand, value);
    }

    public Command GetProductChangedCommand
        {
            get => _getProductChangedCommand;
            set => SetProperty(ref _getProductChangedCommand, value);
        }

        #endregion

        #region Methods
        private async void InicializeProperties()
        {
            var result = await _serviceFireBase.GetCategories();
            GetCategory = new ObservableCollection<CategoryProduct>(result.Success == true ? (IEnumerable<CategoryProduct>)result.Object: new List<CategoryProduct>());

            ModalCategoryCommand = new Command(() => IsAddCategory = !IsAddCategory);

            ModalTaxesCommand = new Command(() => IsAddTaxes = !IsAddTaxes);

            DeleteCategoryCommand = new Command<CategoryProduct>(DeleteCategory);

            DeleteTaxesCommand = new Command<Tax>(DeleteTax);

            SelectImageCommand = new Command(() => IsSelectCamFolder = !IsSelectCamFolder);

            GetProductChangedCommand = new Command(() =>
            {
                NotifyPropertyChanged(nameof(GetProduct));
            });

            ShowImageCommand = new Command<object>(AddImageAsync);

            DeleteImageCommand = new Command<ImageX>(DeleteImage);

            AddCategoryCommand = new Command<object>(AddCategory);

            AddTaxesCommand = new Command<object>(AddTaxes);

            CreateCommand = new Command<object>(CreateProduct);



        }

        private async void CreateProduct(object obj)
        {
            try
            {
                #region Validate

                if(string.IsNullOrEmpty(GetProduct.Name))
                {
                   await _displayAlert.Show("Debes ingresar un nombre valido para el producto");
                    return;
                }
                if(GetProduct.GrossPrice == 0) 
                {
                    await _displayAlert.Show("Debes ingresar un precio valido para el producto");
                    return;
                }
                if(string.IsNullOrEmpty(GetProduct.Sku) && GetProduct.BarCode < 1000)
                {
                    await _displayAlert.Show("Debes ingresar un código de barras o Sku para el producto");
                    return;
                }

                #endregion

                #region insert product
                var result = await _serviceFireBase.InsertProduct(GetProduct);
                if (result.Success) 
                {
                    _displayAlert.Toast("Se creo correctamente el producto");
                    GetProduct = null;
                }
                #endregion

            }
            catch (Exception ex)
            {
                await _displayAlert.Show($"Se produjo un error al intentar crear el producto. código:{ex.GetHashCode()}.\nDetalles: {ex.Message}");
            }
        }

        private void AddTaxes(object obj)
        {
            throw new NotImplementedException();
        }

        private void AddCategory(object obj)
        {
            if (obj == null) return;
            var list = (obj as CollectionView).SelectedItems.ToList();
            if(list.Count > 0)
            {
                
                foreach (var item in list)
                {
                    var category = (CategoryProduct)item;
                    if(!GetProduct.Categories.Exists(c => c.Id == category.Id))
                    {
                        GetProduct.Categories.Add(category);
                        GetProductCategories.Add(category);
                    }
                }

            }
            (obj as CollectionView).SelectedItems = null;
            IsAddCategory = !IsAddCategory;
        }

        private void DeleteTax(Tax obj)
        {
            var p = GetProduct;
        }

        private void DeleteCategory(CategoryProduct obj)
        {
            var category = (CategoryProduct)obj;
            if(category != null)
            {
                GetProduct.Categories.Remove(category);
                GetProductCategories.Remove(category);
            }
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
