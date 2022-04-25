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
        bool _isDrawing = false;
        string _currentType = "";
        IShapeEntity _preview = null;
        Point _start;
        List<IShapeEntity> _drawnShapes = new List<IShapeEntity>();

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

                // Vẽ lại những hình đã vẽ trước đó
                foreach (var item in _drawnShapes)
                {
                    var painter = _painterPrototypes[item.Name];
                    var shape = painter.Draw(item);

                    canvas.Children.Add(shape);
                }

                var previewPainter = _painterPrototypes[_preview.Name];
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

                    string color = br.ReadString();
                    int thickness = br.ReadInt32();

                    _preview = (_shapesPrototypes[name].Clone() as IShapeEntity)!;

                    _preview.HandleStart(startPoint);
                    _preview.HandleEnd(endPoint);
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
    }
}
