using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using AVFoundation;
using CoreGraphics;
using CoreFoundation;
using CoreMedia;
using CoreVideo;
using Foundation;

[assembly: Xamarin.Forms.ExportRenderer(typeof(MIJSWinter1.CustomCamera), typeof(MIJSWinter1.iOS.CustomCameraRenderer))]
namespace MIJSWinter1.iOS
{
    public class CustomCameraRenderer : ViewRenderer<CustomCamera, UICameraPreview>
    {
        UICameraPreview m_uiCameraPreview;

        protected override void OnElementChanged(ElementChangedEventArgs<CustomCamera> e)
        {
            if (Control == null)
            {

                AVAuthorizationStatus status = AVCaptureDevice.GetAuthorizationStatus(AVAuthorizationMediaType.Video);
                if (status == AVAuthorizationStatus.Authorized)
                { // プライバシー設定でカメラ使用が許可されている

                }
                else if (status == AVAuthorizationStatus.Denied)
                { // 　不許可になっている

                }
                else if (status == AVAuthorizationStatus.Restricted)
                { // 制限されている

                }
                else if (status == AVAuthorizationStatus.NotDetermined)
                { // アプリで初めてカメラ機能を使用する場合

                    AVCaptureDevice.RequestAccessForMediaTypeAsync(AVAuthorizationMediaType.Video);
                }
                m_uiCameraPreview = new UICameraPreview(e.NewElement);
                SetNativeControl(m_uiCameraPreview);

                if (e.OldElement != null)
                {
                }
                if (e.NewElement != null)
                {
                }
            }
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            /*
            if (e.PropertyName == CustomButton.TextProperty.PropertyName)
            {
                this.UpdateText();

            }
            */
        }
        /*
        private void UpdateText()
        {
            this.Control.SetTitle(this.Element.Text, UIControlState.Normal);
        }
        private void OnTouchDown(object sender, EventArgs e)
        {
            this.Element.OnClicked();
        }
        */
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }
    }

    public class UICameraPreview : UIView
    {
        CustomCamera m_CustomCamera;

        AVCaptureVideoPreviewLayer m_prevAVLayer;

        public AVCaptureSession m_AVCapSession;

        AVCaptureDevice m_AVCapDevice;
        AVCaptureDeviceInput m_AVInput;

        AVCaptureVideoDataOutput m_AVVideoOutput;

        OutputRecorder m_OutputRecorder;

        private const int mc_iPreviewWidth = 512;
        private const int mc_iPreviewHeight = 384;

        public UICameraPreview(CustomCamera camera)
        {
            m_CustomCamera = camera;

            Initialize();
        }

        public void Initialize()
        {
            this.Frame = new CGRect(new CGPoint(0, 0), new CGSize(mc_iPreviewWidth, mc_iPreviewHeight));
            m_AVCapSession = new AVCaptureSession();

            //m_AVCapDevice = AVCaptureDevice.GetDefaultDevice(AVMediaTypes.Video);

            var arCamDevice = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);
            if(arCamDevice.Length != 0)
            {
                m_AVCapDevice = arCamDevice[0];
                //フロントカメラを取得
                foreach (AVCaptureDevice camDevice in arCamDevice)
                {
                    if (camDevice.Position == AVCaptureDevicePosition.Back)
                    {
                        m_AVCapDevice = camDevice;
                    }
                    /*
                    if (camDevice.Position == AVCaptureDevicePosition.Back && m_iCameraDevice == 1)
                    {
                        m_AVCapDevice = camDevice;
                    }
                    */
                }

                if(m_AVCapDevice == null)
                {
                    m_AVCapDevice = arCamDevice[0];
                }
            }


            NSError device_error;
            m_AVCapDevice.LockForConfiguration(out device_error);
            if (device_error != null)
            {
                Console.WriteLine($"Error: {device_error.LocalizedDescription}");
                m_AVCapDevice.UnlockForConfiguration();
                return;
            }
            //フレームレート設定
            m_AVCapDevice.ActiveVideoMinFrameDuration = new CMTime(1, 24);
            m_AVCapDevice.UnlockForConfiguration();

            if (m_AVCapDevice == null)
            {
                return;
            }





            NSError error = null;
            try{
                //m_AVInput = new AVCaptureDeviceInput(m_AVCapDevice, out error);
                m_AVInput = AVCaptureDeviceInput.FromDevice(m_AVCapDevice);
                if(error != null)
                {
                    Console.WriteLine(error.ToString());
                }else{

                    m_AVCapSession.AddInput(m_AVInput);
                    m_AVCapSession.BeginConfiguration();

                    m_AVCapSession.CanSetSessionPreset(AVCaptureSession.PresetHigh);

                    m_AVCapSession.CommitConfiguration();

                    m_AVVideoOutput = new AVCaptureVideoDataOutput();

                    m_OutputRecorder = new OutputRecorder() { m_CustomCamera = m_CustomCamera };
                    var Queue = new DispatchQueue("myQueue");
                    m_AVVideoOutput.SetSampleBufferDelegateQueue(m_OutputRecorder, Queue);

                    m_AVCapSession.AddOutput(m_AVVideoOutput);
                }

                m_prevAVLayer = new AVCaptureVideoPreviewLayer(m_AVCapSession)
                {
                    Frame = new CGRect(new CGPoint(0, 0), new CGSize(mc_iPreviewWidth, mc_iPreviewHeight)),
                    VideoGravity = AVLayerVideoGravity.ResizeAspectFill
                };
                Layer.AddSublayer(m_prevAVLayer);

                m_AVCapSession.StartRunning();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return;
        }
    }

    //Capture
    public class OutputRecorder : AVCaptureVideoDataOutputSampleBufferDelegate
    {
        public CustomCamera m_CustomCamera { get; set; }//共有のObject

        private UIImage GetImageFromSampleBuffer(CMSampleBuffer sampleBuffer)
        {
            using (var pixelBuffer = sampleBuffer.GetImageBuffer() as CVPixelBuffer)
            {
                // Lock the base address

                pixelBuffer.Lock(CVPixelBufferLock.None);

                // Prepare to decode buffer
                var flags = CGBitmapFlags.PremultipliedFirst | CGBitmapFlags.ByteOrder32Little;

                // Decode buffer - Create a new colorspace
                using (var cs = CGColorSpace.CreateDeviceRGB())
                {

                    // Create new context from buffer
                    using (var context = new CGBitmapContext(pixelBuffer.BaseAddress,
                                             pixelBuffer.Width,
                                             pixelBuffer.Height,
                                             8,
                                             pixelBuffer.BytesPerRow,
                                             cs,
                                             (CGImageAlphaInfo)flags))
                    {

                        // Get the image from the context
                        using (var cgImage = context.ToImage())
                        {

                            // Unlock and return image
                            pixelBuffer.Unlock(CVPixelBufferLock.None);
                            return UIImage.FromImage(cgImage);
                        }
                    }
                }
            }
        }

        public override void DidOutputSampleBuffer(
            AVCaptureOutput captureOutput,
            CMSampleBuffer sampleBuffer,
            AVCaptureConnection connection)
        {

            try
            {
                Console.WriteLine("test");
                // ここでフレーム画像を取得していろいろしたり
                //var image = GetImageFromSampleBuffer (sampleBuffer);

                //PCLプロジェクトとのやりとりやら
    
                //変更した画像をプレビューに反映させたりする

                //これがないと"Received memory warning." で落ちたり、画面の更新が止まったりする
                GC.Collect();  //  "Received memory warning." 回避

            }
            catch (Exception e)
            {
                Console.WriteLine("Error sampling buffer: {0}", e.Message);
            }
        }
    }
}