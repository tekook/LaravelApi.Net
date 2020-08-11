using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Tekook.LaravelApi.Resources.Validation
{
    /// <summary>
    /// A Model which implements <see cref="INotifyPropertyChanged"/> and provided a simple method to fire the event (<see cref="OnPropertyChanged(string)"/>).
    /// </summary>
    public class PropertyChangingModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        /// <summary>
        /// Implementierung von <see cref="INotifyPropertyChanged"/>.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invoke for <see cref="INotifyPropertyChanged"/>
        /// </summary>
        /// <param name="propertyName">Name of the Property which changed</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged
    }
}