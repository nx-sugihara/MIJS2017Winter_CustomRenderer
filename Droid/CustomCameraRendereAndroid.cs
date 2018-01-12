using System;
using System.Collections.Generic;
using Android.Content;
using Android.Hardware;
using Android.Hardware.Camera2;
using Android.Hardware.Camera2.Params;
using Android.Views;
using Android.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(MIJSWinter1.CustomCamera), typeof(MIJSWinter1.Droid.CustomCameraRenderer))]
namespace MIJSWinter1.Droid
{
    public class CustomCameraRenderer : ViewRenderer<CustomCamera, SurfaceView>
    {
        DroidCameraPreview cameraPreview;
        SurfaceView m_surfaceView;

        protected override void OnElementChanged(ElementChangedEventArgs<CustomCamera> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                m_surfaceView = new SurfaceView(Context);
                SetNativeControl(m_surfaceView);
                cameraPreview = new DroidCameraPreview(m_surfaceView, Context, e.NewElement);
            }

            if (e.OldElement != null)
            {
            }
            if (e.NewElement != null)
            {
            }
        }
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            /*
            if (this.Element == null || this.Control == null)
                return;

            // PCL側の変更をプラットフォームに反映
            if (e.PropertyName == nameof(Element.IsPreviewing))
            {
                Control.IsPreviewing = Element.IsPreviewing;
            }
            */
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }
    }

    public sealed class DroidCameraPreview
    {
        CustomCamera m_CustomCamera;
        public SurfaceView m_surfaceView;
        public CaptureRequest.Builder m_PreviewBuilder;
        public CameraCaptureSession m_backCameraSession;
        Context m_Context;

        public DroidCameraPreview(SurfaceView a_surfaceView, Context a_Context, CustomCamera a_CustomCamera)
        {
            m_surfaceView = a_surfaceView;
            m_CustomCamera = a_CustomCamera;
            m_Context = a_Context;
            Initialize();
        }

        private void Initialize()
        {
            CameraManager manager = (CameraManager)m_Context.GetSystemService(Context.CameraService);
            String backCameraId;
            foreach (String cameraId in manager.GetCameraIdList())
            {
                CameraCharacteristics chars = manager.GetCameraCharacteristics(cameraId);
                //if (chars.Get(CameraCharacteristics.LensFacing) == CameraCharacteristics.Key.LensFacing.) {
                backCameraId = cameraId;

                StreamConfigurationMap configs = (StreamConfigurationMap)manager.GetCameraCharacteristics(cameraId).Get(CameraCharacteristics.ScalerStreamConfigurationMap);
                Android.Util.Size[] size = configs.GetOutputSizes( (int)Android.Graphics.ImageFormatType.Jpeg);
                //ここに含まれるSizeを指定する

                ViewGroup.LayoutParams lp = (ViewGroup.LayoutParams)m_surfaceView.LayoutParameters;
                lp.Width = 320; //横幅
                lp.Height = 240; //縦幅
                m_surfaceView.LayoutParameters = lp;

                m_surfaceView.Holder.SetFixedSize(240, 320);//縦で持つときはwidth height が逆
                manager.OpenCamera(backCameraId, new OpenCameraCallback(this), null);
                break;
            }
        } 


        private class OpenCameraCallback : CameraDevice.StateCallback
        {
            DroidCameraPreview m_parent;
            public OpenCameraCallback(DroidCameraPreview a_parent)
            {
                m_parent = a_parent;
            }

            public override void OnOpened(CameraDevice camera)
            {
                // プレビュー用のSurfaceViewをリストに登録
                SurfaceView surfaceView = m_parent.m_surfaceView;

                ViewGroup.LayoutParams lp = (ViewGroup.LayoutParams)surfaceView.LayoutParameters;
                lp.Width = 320; //横幅
                lp.Height = 240; //縦幅
                surfaceView.LayoutParameters = lp;

                surfaceView.Holder.SetFixedSize(240, 320);//縦で持つときはwidth height が逆

                List<Surface> surfaceList = new List<Surface>();

                surfaceList.Add(surfaceView.Holder.Surface);

                try
                {
                    // プレビューリクエストの設定（SurfaceViewをターゲットに）
                    m_parent.m_PreviewBuilder = camera.CreateCaptureRequest(CameraTemplate.Preview);
                    m_parent.m_PreviewBuilder.AddTarget(surfaceView.Holder.Surface);

                    // キャプチャーセッションの開始(セッション開始後に第2引数のコールバッククラスが呼ばれる)
                    camera.CreateCaptureSession(surfaceList, new CameraCaptureSessionCallback(m_parent), null);

                }
                catch (CameraAccessException e)
                {
                    // エラー時の処理を記載
                }
            }

            public override void OnDisconnected(CameraDevice camera)
            {
                throw new NotImplementedException();
            }

             public override void OnClosed(CameraDevice camera)
            {
                base.OnClosed(camera);
            }

            public override void OnError(CameraDevice camera, Android.Hardware.Camera2.CameraError error)
            {
                throw new NotImplementedException();
            }
        }

                                                        /**
* カメラが起動し使える状態になったら呼ばれるコールバック
*/
        private class CameraCaptureSessionCallback :CameraCaptureSession.StateCallback
        {
            DroidCameraPreview m_parent;
            public CameraCaptureSessionCallback(DroidCameraPreview a_parent)
            {
                m_parent = a_parent;
            }

            public override void OnConfigured(CameraCaptureSession session)
            {
             m_parent.m_backCameraSession = session;

            try
            {
                // オートフォーカスの設定
                //m_parent.m_PreviewBuilder.Set(CaptureRequest.ControlAfTrigger, (Object)ControlAFTrigger.Start);

                // プレビューの開始(撮影時に第2引数のコールバッククラスが呼ばれる)
                session.SetRepeatingRequest(m_parent.m_PreviewBuilder.Build(), new CaptureCallback(), null);

            }
            catch (CameraAccessException e)
            {
            }
        }

        public override void OnConfigureFailed(CameraCaptureSession session)
        {
        }
    }

    /**
     * カメラ撮影時に呼ばれるコールバック関数
     */
    private class CaptureCallback : CameraCaptureSession.CaptureCallback
    {
    }

    }

}
