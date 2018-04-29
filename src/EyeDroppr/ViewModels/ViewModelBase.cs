using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EyeDroppr.ViewModels
{
    /// <summary>
    /// The view model base.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// The set value.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        protected void SetValue<T>(ref T property, T value, [CallerMemberName] string propertyName = null) where T : class
        {
            if (property == value)
            {
                return;
            }
            property = value;
            this.OnPropertyChanged(propertyName);
        }
    }
}