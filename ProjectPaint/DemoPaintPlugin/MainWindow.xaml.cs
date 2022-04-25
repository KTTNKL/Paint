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
        List<IShapeEntity> _drawnList = new List<IShapeEntity>();
        int currentIndex = -1;

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
                shapesStackPanel.Children.Add(button);
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


                    var shape = painter.Draw(item);                  
                    canvas.Children.Add(shape);
                    ++i;
                }

                var previewPainter = _painterPrototypes[_preview.Name];
                previewPainter.setColor(_preview, color);
                previewPainter.setThickness(_preview, thickness);
                previewPainter.setStrokeType(_preview, stroke_type);
                var previewElement = previewPainter.Draw(_preview);
                canvas.Children.Add(previewElement);
            }
        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isDrawing = false;

            var end = e.GetPosition(canvas); // Điểm kết thúc

            _preview.HandleEnd(end);
            _drawnShapes.Add(_preview.Clone() as IShapeEntity);


            for (int i = currentIndex + 1; i < _drawnList.Count(); i++)
            {
                _drawnList.RemoveAt(i);
                i--;
            }
            _drawnList.Add(_preview.Clone() as IShapeEntity);
            currentIndex++;

        }

        public void readData()
        {
            BinaryReader br;
            //reading from the file
            try
            {
                br = new BinaryReader(new FileStream("mydata", FileMode.Open));
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message + "\n Cannot open file.");
                return;
            }

            try
            {
                int num = br.ReadInt32();
                for(int i =0; i < num; i++)
                {
                    string name = br.ReadString();

                    Point startPoint = new Point();
                    Point endPoint = new Point();

                    startPoint.X = br.ReadDouble();
                    endPoint.X = br.ReadDouble();

                    startPoint.Y = br.ReadDouble();
                    endPoint.Y = br.ReadDouble();

                    int color_read = br.ReadInt32();
                    int thickness_read = br.ReadInt32();
                    int stroketype_read = br.ReadInt32();

                    _preview = (_shapesPrototypes[name].Clone() as IShapeEntity)!;

                    _preview.HandleStart(startPoint);
                    _preview.HandleEnd(endPoint);
                    _preview.color = color_read;
                    _preview.thickness = thickness_read;
                    _preview.stroke_type = stroketype_read;
                    _drawnShapes.Add(_preview.Clone() as IShapeEntity);

                }
                foreach (var item in _drawnShapes)
                {
                    var painter = _painterPrototypes[item.Name];
                    var shape = painter.Draw(item);

                    canvas.Children.Add(shape);
                }

            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message + "\n Cannot read from file.");
                return;
            }
            br.Close();
        }
        public void WriteData()
        {
            BinaryWriter bw;
            


            //create the file
            try
            {
                bw = new BinaryWriter(new FileStream("mydata", FileMode.Create));
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message + "\n Cannot create file.");
                return;
            }

            //writing into the file
            try
            {
                bw.Write(_drawnShapes.Count());

                foreach (var item in _drawnShapes)
                {
                    bw.Write(item.Name);
                    var painter = _painterPrototypes[item.Name];
                    bw.Write(painter.getX1(item));
                    bw.Write(painter.getX2(item));
                    bw.Write(painter.getY1(item));
                    bw.Write(painter.getY2(item));
                    bw.Write(painter.getColor(item));
                    bw.Write(painter.getThickness(item));
                    bw.Write(painter.getStrokeType(item));
                }
                MessageBox.Show("Save Successfully");
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message + "\n Cannot write to file.");
                return;
            }
            bw.Close();

           
        }

        private void load_Click(object sender, RoutedEventArgs e)
        {

            readData();
        }
        private void save_Click(object sender, RoutedEventArgs e)
        {
            WriteData();
        }

        private void loadImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dl1 = new Microsoft.Win32.OpenFileDialog();
            dl1.FileName = "MYFileSave";
            dl1.DefaultExt = ".png";
            dl1.Filter = "Image documents (.png)|*.png";
            Nullable<bool> result = dl1.ShowDialog();

            if (result == true)
            {
                string filename = dl1.FileName;
                ImageBrush brush = new ImageBrush();
                Uri uri = new Uri(@filename, UriKind.Relative);
                brush.ImageSource = new BitmapImage(uri);
                canvas.Background = brush;
                canvas.Children.Clear();
            }

        }

        private void saveImage_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap targetBitmap =
            new RenderTargetBitmap((int)canvas.ActualWidth,
                           (int)canvas.ActualHeight,
                           96d, 96d,
                           PixelFormats.Default);
            targetBitmap.Render(canvas);

            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(targetBitmap));
            var num = 1;
            FileStream fs;
            fs = File.Open("RESULT.png", FileMode.OpenOrCreate);
            encoder.Save(fs);
            fs.Close();

        }

        private void undo_Click(object sender, RoutedEventArgs e)
        {
            if (_drawnShapes.Count() >= 0)
            {
                canvas.Children.Clear();
                _drawnShapes.RemoveAt(_drawnShapes.Count() - 1);
                currentIndex--;
                foreach (var item in _drawnShapes)
                {
                    var painter = _painterPrototypes[item.Name];
                    var shape = painter.Draw(item);

                    canvas.Children.Add(shape);
                }
            }

        }

        private void redo_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex < (_drawnList.Count() - 1))
            {
                currentIndex++;
                _drawnShapes.Add(_drawnList[currentIndex].Clone() as IShapeEntity);
                canvas.Children.Clear();
                foreach (var item in _drawnShapes)
                {
                    var painter = _painterPrototypes[item.Name];
                    var shape = painter.Draw(item);

                    canvas.Children.Add(shape);
                }
            }

        }
    }
}
