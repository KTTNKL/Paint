using IContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace DemoPaintPlugin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // State
       
        int color = 0;
        int thickness = 1;
        int stroke_type = 0;
        bool _isDrawing = false;
        string _currentType = "";
        IShapeEntity _preview = null;
        Point _start;
        List<IShapeEntity> _drawnShapes = new List<IShapeEntity>();
        List<int> _colors = new List<int>();
        List<int> _thicknesses = new List<int>();
        List<int> _stroke_types = new List<int>();

        // Cấu hình
        Dictionary<string, IPaintBusiness> _painterPrototypes = new Dictionary<string, IPaintBusiness>();
        Dictionary<string, IShapeEntity> _shapesPrototypes = new Dictionary<string, IShapeEntity>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            /* Nạp tất cả dll, tìm kiếm entity và business */
            var exeFolder = AppDomain.CurrentDomain.BaseDirectory;
            var folderInfo = new DirectoryInfo(exeFolder);
            var dllFiles = folderInfo.GetFiles("*.dll");

            foreach (var dll in dllFiles)
            {
                Assembly assembly = Assembly.LoadFrom(dll.FullName);

                Type[] types = assembly.GetTypes();

                // Giả định: 1 dll chỉ có 1 entity và 1 business tương ứng
                IShapeEntity? entity = null;
                IPaintBusiness? business = null;

                foreach (Type type in types)
                {
                    if (type.IsClass)
                    {
                        if (typeof(IShapeEntity).IsAssignableFrom(type))
                        {
                            entity = (Activator.CreateInstance(type) as IShapeEntity)!;
                        }

                        if (typeof(IPaintBusiness).IsAssignableFrom(type))
                        {
                            business = (Activator.CreateInstance(type) as IPaintBusiness)!;
                        }
                    }
                }
                
                //var img = new Bitmap
                if (entity != null)
                {
                    _shapesPrototypes.Add(entity!.Name, entity);
                    _painterPrototypes.Add(entity!.Name, business!);
                }
            }

            Title = $"Tìm thấy {_shapesPrototypes.Count} hình";

            // Tạo ra các nút bấm tương ứng
            foreach(var (name, entity) in _shapesPrototypes)
            {
                var button = new Button();
                button.Content = name;
                button.Tag = entity;
                button.Width = 80;
                button.Height = 35;
                button.Click += Button_Click;

                //TODO: thêm các nút bấm vào giao diện
                actionsStackPanel.Children.Add(button);
            }

            if (_shapesPrototypes.Count > 0)
            {
                //Lựa chọn nút bấm đầu tiên
                var (key, shape) = _shapesPrototypes.ElementAt(0);
                _currentType = key;
                _preview = (shape.Clone() as IShapeEntity)!;
            }

            
            
        }

        // Đổi lựa chọn
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var entity = button!.Tag as IShapeEntity;

            _currentType = entity!.Name;
            _preview = (_shapesPrototypes[entity.Name].Clone() as IShapeEntity)!;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _isDrawing = true;
            _start = e.GetPosition(canvas);

            _preview.HandleStart(_start);
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {
                var end = e.GetPosition(canvas);
                _preview.HandleEnd(end);

                // Xóa đi tất cả bản vẽ cũ và vẽ lại những đường thẳng trước đó
                canvas.Children.Clear(); // Xóa đi toàn bộ

                if(color_ComboBox.SelectedIndex >=0)
                {
                    color = color_ComboBox.SelectedIndex;
                }
                if(thickness_ComboBox.SelectedIndex >=0)
                {
                    thickness = thickness_ComboBox.SelectedIndex + 1;
                }
                if(stroke_type_ComboBox.SelectedIndex >=0)
                {
                    stroke_type = stroke_type_ComboBox.SelectedIndex;
                }

                // Vẽ lại những hình đã vẽ trước đó
                int i = 0;
                foreach (var item in _drawnShapes)
                {
                    
                    var painter = _painterPrototypes[item.Name];
                    var shape = painter.Draw(item, _colors[i], _thicknesses[i], _stroke_types[i]);                  
                    canvas.Children.Add(shape);
                    ++i;
                }

                var previewPainter = _painterPrototypes[_preview.Name];
                var previewElement = previewPainter.Draw(_preview, color, thickness, stroke_type);
                canvas.Children.Add(previewElement);
            }
        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isDrawing = false;

            var end = e.GetPosition(canvas); // Điểm kết thúc

            _preview.HandleEnd(end);

            _drawnShapes.Add(_preview.Clone() as IShapeEntity);
            _colors.Add(color);
            _thicknesses.Add(thickness);
            _stroke_types.Add(stroke_type);
        }
    }
}
