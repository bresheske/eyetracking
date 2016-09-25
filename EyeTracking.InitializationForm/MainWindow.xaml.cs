using AForge.FrameProcessing.FrameProviders;
using EyeTracking.Core.Filters;
using EyeTracking.Core.Persistance;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace EyeTracking.InitializationForm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public enum StateType
        {
            CROP_POINT_1 = 0,
            CROP_POINT_2 = 1,
            IRIS = 2,
            REFERENCE_POINT_TOPLEFT = 3,
            REFERENCE_POINT_BOTTOMRIGHT = 4,
            FINISHED = 5
        }

        private StateType _state;
        private StateType State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                StatusText.Text = _state.ToString();
            }
        }

        private System.Windows.Point _croppoint1;
        private System.Windows.Point _croppoint2;
        private System.Drawing.Point _topleftreferencepoint;
        private System.Drawing.Point _bottomrightreferencepoint;

        private IFrameProvider _frameprovider;
        private CropFilter _cropfilter;
        private HueFilter _huefilter;
        private BlobCounterFilter _blobcounterfilter;
        private System.Drawing.Point _currentlefteyepos;
        private System.Drawing.Point _currentrighteyepos;

        private int _imagewidth;
        private int _imageheight;
        private Bitmap _currentframe;

        private SettingsModel _settingsmodel;

        public MainWindow()
        {
            InitializeComponent();
            State = StateType.CROP_POINT_1;

            this.Closed += MainWindow_Closed;

            _settingsmodel = new SettingsModel();

            var args = Environment.GetCommandLineArgs();
            if (args.Length == 2)
            {
                // Load up the settings from the persisted file, and skip the initialization phase.
                var file = new InitializationSettings(args[1]);
                _settingsmodel = file.LoadFromFile();

                _cropfilter = new CropFilter()
                {
                    Height = _settingsmodel.CropHeight,
                    Width = _settingsmodel.CropWidth,
                    X = _settingsmodel.CropX,
                    Y = _settingsmodel.CropY
                };

                _huefilter = new HueFilter()
                {
                    MaxDeltaHue = 35,
                    TargetHue = _settingsmodel.IrisHue
                };

                _blobcounterfilter = new BlobCounterFilter()
                {
                    MaxHeight = _cropfilter.Height / 4,
                    MinHeight = _cropfilter.Height / 15,
                    MaxWidth = _cropfilter.Width / 4,
                    MinWidth = _cropfilter.Width / 15
                };

                _topleftreferencepoint = new System.Drawing.Point(_settingsmodel.ReferencePointTopLeft.X, _settingsmodel.ReferencePointTopLeft.Y);
                _bottomrightreferencepoint = new System.Drawing.Point(_settingsmodel.ReferencePointBottomRight.X, _settingsmodel.ReferencePointBottomRight.Y);

                State = StateType.FINISHED;
            }

            _frameprovider = new CameraFrameProvider();
            _frameprovider.OnNextFrame += _frameprovider_OnNextFrame;
            _frameprovider.Start();
        }

        void MainWindow_Closed(object sender, EventArgs e)
        {
            if (_frameprovider.IsRunning)
                _frameprovider.Stop();
        }

        void _frameprovider_OnNextFrame(object sender, FrameEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                var img = e.Frame;
                if (State >= StateType.IRIS)
                    img = _cropfilter.Apply(img);

                System.Drawing.Point[] eyes = null;

                if (State >= StateType.REFERENCE_POINT_TOPLEFT)
                {
                    img = _huefilter.Apply(img);

                    eyes = _blobcounterfilter.GetBlobPositions(img)
                        .ToArray();
                    if (eyes.Count() > 0)
                        _currentlefteyepos = eyes[0];
                    if (eyes.Count() > 1)
                        _currentrighteyepos = eyes[1];

                    if (State < StateType.FINISHED)
                        img = _blobcounterfilter.Apply(img);

                }

                _imagewidth = img.Width;
                _imageheight = img.Height;
                _currentframe = img;

                if (State == StateType.FINISHED)
                {
                    // build an image of screensize, draw the eye position onto the screen.

                    var newimg = new Bitmap((int)Application.Current.MainWindow.Width, (int)Application.Current.MainWindow.Height);
                    using (var gfx = Graphics.FromImage(newimg))
                    {
                        // Fill it in with black.
                        var fullscreenrect = new System.Drawing.Rectangle(0, 0, (int)Application.Current.MainWindow.Width, (int)Application.Current.MainWindow.Height);
                        gfx.DrawRectangle(new System.Drawing.Pen(System.Drawing.Brushes.Black), fullscreenrect);


                        //gfx.DrawImage(img, 0f, 0f, (float)newimg.Width, (float)newimg.Height);

                        // Build a rectangle from the cropped rectangle.
                        var croprect = new System.Drawing.Rectangle(0, 0, _currentframe.Width, _currentframe.Height);
                        var topleftreferencepoint = new System.Windows.Point(_topleftreferencepoint.X, _topleftreferencepoint.Y);
                        var bottomrightreferencepoint = new System.Windows.Point(_bottomrightreferencepoint.X, _bottomrightreferencepoint.Y);
                        topleftreferencepoint = InverseXAxis(topleftreferencepoint, croprect);
                        bottomrightreferencepoint = InverseXAxis(bottomrightreferencepoint, croprect);

                        var refrect = new System.Drawing.Rectangle((int)topleftreferencepoint.X, (int)topleftreferencepoint.Y,
                            Math.Abs((int)bottomrightreferencepoint.X - (int)topleftreferencepoint.X),
                            Math.Abs((int)bottomrightreferencepoint.Y - (int)topleftreferencepoint.Y)
                            );

                        // Place the eye's position into the reference point.
                        var eyespos = AveragePoints(_currentlefteyepos, _currentrighteyepos);
                        var placedeyespos = PlacePoint(eyespos, refrect);

                        // Clear our bounds.  If the eyes are outside of the reference points, it doesn't really make a lot of sense.
                        if (placedeyespos.X < 0)
                            placedeyespos.X = 0;
                        if (placedeyespos.Y < 0)
                            placedeyespos.Y = 0;
                        if (placedeyespos.X > refrect.Width)
                            placedeyespos.X = refrect.Width;
                        if (placedeyespos.Y > refrect.Height)
                            placedeyespos.Y = refrect.Height;


                        // Project the eye's position onto the new image.
                        var position = new System.Windows.Point(placedeyespos.X, placedeyespos.Y);
                        var newpoint = ProjectPoint(refrect, position, fullscreenrect);

                        // Invert point over middle Axis.
                        newpoint = InverseXAxis(newpoint, fullscreenrect);

                        DebugText.Text = string.Format("EYEPoint: ({0}, {1})", newpoint.X, newpoint.Y);

                        // Finally draw the position onto the screen.
                        gfx.FillEllipse(System.Drawing.Brushes.Yellow, new RectangleF((float)newpoint.X, (float)newpoint.Y, 20f, 20f));

                        // For debugging. Show some color on the topleft of the screen for some feedback.
                        var color = eyes.Length == 0
                            ? System.Drawing.Brushes.Red
                            : eyes.Length == 1
                            ? System.Drawing.Brushes.Yellow
                            : System.Drawing.Brushes.Green;

                        gfx.FillRectangle(color, new RectangleF(0, 0, 50, 50));
                    }

                    img = newimg;
                }

                CamImage.Source = LoadBitmap(img);
            });
        }

        private System.Windows.Point InverseXAxis(System.Windows.Point p, System.Drawing.Rectangle rect)
        {
            var newpoint = new System.Windows.Point(p.X, p.Y);
            // Invert point over middle Axis.
            var delta = Math.Abs(newpoint.X - rect.Width / 2);
            if (newpoint.X < rect.Width / 2)
                newpoint.X = rect.Width / 2 + delta;
            else
                newpoint.X = rect.Width / 2 - delta;

            return newpoint;
        }

        private void CroppingGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mousepos = Mouse.GetPosition(Application.Current.MainWindow);

            if (State == StateType.CROP_POINT_1)
            {
                _croppoint1 = mousepos;
                State = StateType.CROP_POINT_2;
            }
            else if (State == StateType.CROP_POINT_2)
            {
                _croppoint2 = mousepos;

                // First, find the projection of our current points from the Screen Size to the Original Image Size.
                var screenrect = new System.Drawing.Rectangle(0, 0, (int)Application.Current.MainWindow.Width, (int)Application.Current.MainWindow.Height);
                var imagerect = new System.Drawing.Rectangle(0, 0, _imagewidth, _imageheight);
                var imagepoint1 = ProjectPoint(screenrect, _croppoint1, imagerect);
                var imagepoint2 = ProjectPoint(screenrect, _croppoint2, imagerect);

                var newwidth = imagepoint2.X - imagepoint1.X;
                var newheight = imagepoint2.Y - imagepoint1.Y;

                // Finally perform the crop of the new projection.
                _cropfilter = new CropFilter()
                {
                    Height = (int)newheight,
                    Width = (int)newwidth,
                    X = (int)imagepoint1.X,
                    Y = (int)imagepoint1.Y
                };

                _settingsmodel.CropHeight = _cropfilter.Height;
                _settingsmodel.CropWidth = _cropfilter.Width;
                _settingsmodel.CropX = _cropfilter.X;
                _settingsmodel.CropY = _cropfilter.Y;

                State = StateType.IRIS;
            }
            else if (State == StateType.IRIS)
            {
                var irislocation = mousepos;

                // Need to, again, find the projection of the iris point onto the Image Size.
                var screenrect = new System.Drawing.Rectangle(0, 0, (int)Application.Current.MainWindow.Width, (int)Application.Current.MainWindow.Height);
                var imagerect = new System.Drawing.Rectangle(0, 0, _imagewidth, _imageheight);
                var irispoint = ProjectPoint(screenrect, irislocation, imagerect);

                // Now we just need to get the color.
                var iriscolor = _currentframe.GetPixel((int)irispoint.X, (int)irispoint.Y);
                _huefilter = new HueFilter()
                {
                    MaxDeltaHue = 35,
                    TargetHue = (int)iriscolor.GetHue()
                };

                // Get the blob counter going.
                _blobcounterfilter = new BlobCounterFilter()
                {
                    MaxHeight = imagerect.Height / 4,
                    MinHeight = imagerect.Height / 15,
                    MaxWidth = imagerect.Width / 4,
                    MinWidth = imagerect.Width / 15
                };

                _settingsmodel.IrisHue = _huefilter.TargetHue;
                State = StateType.REFERENCE_POINT_TOPLEFT;
            }
            else if (State == StateType.REFERENCE_POINT_TOPLEFT)
            {
                // Already know where the eyes are, we just need to store the reference point.
                _topleftreferencepoint = AveragePoints(_currentlefteyepos, _currentrighteyepos);
                _settingsmodel.ReferencePointTopLeft = new PointModel()
                {
                    X = _topleftreferencepoint.X,
                    Y = _topleftreferencepoint.Y
                };
                State = StateType.REFERENCE_POINT_BOTTOMRIGHT;
            }
            else if (State == StateType.REFERENCE_POINT_BOTTOMRIGHT)
            {
                // Already know where the eyes are, we just need to store the reference point.
                _bottomrightreferencepoint = AveragePoints(_currentlefteyepos, _currentrighteyepos);
                _settingsmodel.ReferencePointBottomRight = new PointModel()
                {
                    X = _bottomrightreferencepoint.X,
                    Y = _bottomrightreferencepoint.Y
                };
                State = StateType.FINISHED;

                // Write out the settings to the file.
                new InitializationSettings().SaveToFile(_settingsmodel);
            }
        }

        private void DrawCroppingRectangle()
        {
            CroppingRectangle.Width = _croppoint2.X - _croppoint1.X;
            CroppingRectangle.Height = _croppoint2.Y - _croppoint1.Y;
            Canvas.SetLeft(CroppingRectangle, _croppoint1.X);
            Canvas.SetTop(CroppingRectangle, _croppoint1.Y);
            CroppingRectangle.Visibility = System.Windows.Visibility.Visible;
        }

        private System.Windows.Point ProjectPoint(System.Drawing.Rectangle originalframe, 
            System.Windows.Point originalpoint, 
            System.Drawing.Rectangle newframe)
        {
            var xratedelta = originalpoint.X / originalframe.Width;
            var yratedelta = originalpoint.Y / originalframe.Height;
            return new System.Windows.Point()
            {
                X = xratedelta * newframe.Width,
                Y = yratedelta * newframe.Height
            };
        }

        private System.Windows.Point ProjectPoint(System.Drawing.Rectangle originalframe,
            System.Drawing.Point originalpoint,
            System.Drawing.Rectangle newframe)
        {
            var p = new System.Windows.Point(originalpoint.X, originalpoint.Y);
            return ProjectPoint(originalframe, p, newframe);
        }

        private System.Windows.Point PlacePoint(System.Windows.Point originalpoint,
            System.Drawing.Rectangle newframe)
        {
            // Rather than projecting, this places the point into an overlaying rectangle.
            return new System.Windows.Point(
                originalpoint.X - newframe.X,
                originalpoint.Y - newframe.Y
                );
        }

        private System.Windows.Point PlacePoint(System.Drawing.Point originalpoint,
            System.Drawing.Rectangle newframe)
        {
            var p = new System.Windows.Point(originalpoint.X, originalpoint.Y);
            return PlacePoint(p, newframe);
        }

        private System.Windows.Point AveragePoints(System.Windows.Point p1, System.Windows.Point p2)
        {
            return new System.Windows.Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
        }

        private System.Drawing.Point AveragePoints(System.Drawing.Point p1, System.Drawing.Point p2)
        {
            return new System.Drawing.Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
        }

        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);

        public static BitmapSource LoadBitmap(System.Drawing.Bitmap source)
        {
            IntPtr ip = source.GetHbitmap();
            BitmapSource bs = null;
            try
            {
                bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip,
                   IntPtr.Zero, Int32Rect.Empty,
                   System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(ip);
            }

            return bs;
        }

    }
}
