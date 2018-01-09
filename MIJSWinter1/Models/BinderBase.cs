using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace MIJSWinter1
{
    public class BindableBase : INotifyPropertyChanged
    {
        protected BindableBase()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(field, value)) { return false; }
            field = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }
    }

    public class MyPageViewModel : BindableBase
    {
        //格納
        private double m_fSliderValue;
        private string m_sOSName;

        //入出力用
        public double SliderValue
        {
            get { return this.m_fSliderValue; }
            set { this.SetProperty(ref this.m_fSliderValue, value); this.OnPropertyChanged(nameof(LabelValue)); }
        }

        public string LabelValue => string.Format("This is slider value '{0:000}'", this.SliderValue);

        public string OSName
        {
            get { return this.m_sOSName; }
            set { this.SetProperty(ref this.m_sOSName, value);  }
        }

    }
}