using AppCompras.Font;
using AppCompras.Models.Product;
using AppCompras.Views.Home;
using Plugin.SharedTransitions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCompras.ViewModels.Product
{
    public class CategoryPageViewModel: BaseViewModel
    {
        
        #region Fields
        private bool _isVisibleModal;
        private bool _isSelectCamFolder;
        private CategoryProduct _category;
        private string _name;
        private ImageSource _image;
        private byte[] PhotoBytes;
        private StackLayout _categories;
        ResourceDictionary staticResources = Application.Current.Resources;
        #endregion

        #region Ctor
        public CategoryPageViewModel()
        {
            InicializeProperties();
        }

        
        #endregion

        #region Command
        public Command ShowModalCommand {get; set; }

        public Command SelectImageCommand { get; set; }

        public Command<object> ShowImageCommand { get; set; }

        public Command CreateCategoryCommand { get; set; }
        #endregion

        #region Properties
        public bool IsVicibleModal
        {
            get => _isVisibleModal;
            set => SetProperty(ref _isVisibleModal, value);
        }

        private Grid Container;
        public bool IsSelectCamFolder
        {
            get => _isSelectCamFolder;
            set => SetProperty(ref _isSelectCamFolder, value);
        }
        public CategoryProduct Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }
        public StackLayout CategoryProducts
        {
            get => _categories;
            set=> SetProperty(ref _categories, value);
        }
        public string Name
        {
            get => _name;
            set=> SetProperty(ref _name, value);
        }
        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }
        public List<CategoryProduct> CategoryList { get; private set; }
        #endregion

        #region Methods

        private async void InicializeProperties()
        {
            
            IsVicibleModal = false;

            ShowModalCommand = new Command(() => IsVicibleModal = !IsVicibleModal);

            SelectImageCommand = new Command(() => IsSelectCamFolder = !IsSelectCamFolder);

            ShowImageCommand = new Command<object>(AddImageAsync);

            CreateCategoryCommand = new Command(CrearCategory);

            //cargar lista de categories
            var result = await _serviceFireBase.GetCategories();
            if(result.Success)
            {
                CategoryList = (List<CategoryProduct>)result.Object;
                CollectionCategories(CategoryList);
                
            }
            else
            {
                
                await _displayAlert.Show($"{result.Message}");
            }

            
        }

        private async void CrearCategory(object obj)
        {
            #region Validations
            if(string.IsNullOrEmpty(Name) || string.IsNullOrWhiteSpace(Name))
            {
                _displayAlert.Toast("El campo nombre es requerido");
            }

            #endregion
            try
            {
                Category = new CategoryProduct
                {
                    Name = Name.ToUpper(),
                    Base64Image = PhotoBytes == null ? null : Convert.ToBase64String(PhotoBytes)
                };

                if( await _serviceFireBase.CheckConnection())
                {
                  var result = await _serviceFireBase.InsertCategory(Category);
                    switch(result.Status) 
                    {
                        case 200:
                            PhotoBytes = null;
                            Image = Name =  null;
                            IsVicibleModal= false;
                            _displayAlert.Toast($"{result.Message}");
                            var res = await _serviceFireBase.GetCategories();
                            if (result.Success)
                            {
                                CollectionCategories((List<CategoryProduct>)res.Object);

                            }
                            break;
                        case 400:
                            await _displayAlert.Show($"{result.Message}\nDetalles: {result.Object}");
                            break;
                    
                    }
                }
                else
                {
                    await _displayAlert.Show($"El dispositivo no cuenta con conexión a internet.");
                    IsVicibleModal = false;
                }
            }
            catch(Exception ex) 
            {
                await _displayAlert.Show($"Se produjo una excepción no controlada.\nDetalles:{ex.Message}");
            }
            
        }

        private async void AddImageAsync(object obj)
        {
            var value = bool.Parse(obj.ToString());
            try
            {
                FileResult fileResult;
                if(value)
                {
                    var options = new MediaPickerOptions
                    {
                        Title = "Capture una imagen para la categoría"
                    };
                    fileResult = await MediaPicker.CapturePhotoAsync(options);
                }
                else
                {
                    var options = new MediaPickerOptions
                    {
                        Title = "Selecciones una imagen para la categoría"
                    };
                    fileResult = await MediaPicker.PickPhotoAsync(options);

                    
                }
                PhotoBytes = StreamToByteArray(await fileResult?.OpenReadAsync());

                    Image = ImageSource.FromStream(()=> new MemoryStream(PhotoBytes));
                
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

        public void CollectionCategory(Grid grid)
        {
            grid.HorizontalOptions = LayoutOptions.Center;
            grid.Children.Add(new StackLayout
            {

                Children = { new Label
                    {
                        Text = $"Cargando Datos...",
                        FontSize = 15,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.CenterAndExpand

                    } }

            });
            Container = grid;
        }

        private void CollectionCategories(List<CategoryProduct> Models)
        {
            
            StackLayout Left = new StackLayout() { Padding = new Thickness(10) };

            StackLayout Rigth = new StackLayout() { Padding = new Thickness(10), Margin = new Thickness(0, 80, 0, 0) };

            foreach (var item in Models)
            {
                CategoryPlot(item, Models.IndexOf(item), Left, Rigth);
            }
            Container.Children.Clear();
            Container.Children.Add( new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { Left, Rigth }

            });
        }

        private void CategoryPlot(CategoryProduct item, int index, StackLayout Left, StackLayout Rigth)
        {
            var _islayout = Convert.ToBoolean(index % 2);

            var _layout = _islayout ? Rigth: Left;

            var frame = new Frame
            {
                HasShadow= false,
                BorderColor = Color.Black,
                CornerRadius = 20,
                HorizontalOptions= LayoutOptions.Center,
                Content = new StackLayout
                {
                Children =  {
                    new Frame
                    {
                        CornerRadius=50,
                        HeightRequest=100,
                        WidthRequest=100,
                        HorizontalOptions=LayoutOptions.Center,
                        Padding=0,
                        IsClippedToBounds= true,
                        Content = new Image
                        {
                        Source = item.Image,
                        HeightRequest = 60,
                        WidthRequest=60,
                        Aspect = Aspect.AspectFill,
                        Margin = new Thickness(0, 5)
                        }
                    },
                     new Label
                     {
                        Text = item.Name,
                        FontSize = 16,
                        TextColor = Color.Black,
                        CharacterSpacing = 1,
                        HorizontalOptions= LayoutOptions.Center,
                     }

                }
                }
                
               
            };
            
            var tap = new TapGestureRecognizer();

            
            staticResources.TryGetValue("IconLabel", out var myStyle);

            var grid = new Grid
            {
                Children = {
                    frame,
                    new Label
                    {
                        Text = IconFont.Trash,
                        Style = (Style)myStyle,
                        TextColor= Color.Red,
                        HorizontalOptions= LayoutOptions.EndAndExpand,
                        VerticalOptions= LayoutOptions.Start,
                        Margin= new Thickness(8, 5)
                    }
                }
            };
            tap.Tapped += async (object sender, EventArgs e) =>
            {
                var resp = await _serviceFireBase.DeleteCategory(item);
                if (resp.Success)
                {
                    CategoryList.Remove((CategoryProduct)resp.Object);
                    CollectionCategories(CategoryList);
                }

            };
            grid.GestureRecognizers.Add(tap);
            _layout.Children.Add(grid);

        }

        #endregion
    }
}
