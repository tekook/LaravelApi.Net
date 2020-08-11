using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Tekook.LaravelApi.Resources.Validation
{
    /// <summary>
    /// A validatable Model which can be used by WPF.
    /// </summary>
    public abstract class ValidatableModel : PropertyChangingModel, INotifyDataErrorInfo, INotifyPropertyChanged
    {
        #region INotifyDataErrorInfo

        /// <summary>
        /// Gibt an, ob das Model einen Fehler hat.
        /// </summary>
        public bool HasErrors
        {
            get
            {
                return this.PropErrors.Values.Any(x => x != null && x.Count > 0);
            }
        }

        /// <summary>
        /// Dictionary für die enthaltenen Errors der Properties.
        /// </summary>
        protected ConcurrentDictionary<string, List<string>> PropErrors { get; set; } = new ConcurrentDictionary<string, List<string>>();

        /// <summary>
        /// EventHandler für <see cref="INotifyDataErrorInfo"/>.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Ruft die Validierungsfehler für eine angegebene Eigenschaft oder für die ganze
        /// Entität ab.
        /// </summary>
        /// <param name="propertyName">Der Name der Eigenschaft, für die Validierungsfehler abgerufen werden sollen,
        /// oder null oder System.String.Empty, um Fehler auf Entitätsebene abzurufen.</param>
        /// <returns>Die Validierungsfehler für die Eigenschaft oder die Entität.</returns>
        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName == null)
            {
                return null;
            }
            lock (_lock)
            {
                return this.PropErrors.ContainsKey(propertyName) ? this.PropErrors[propertyName] : null;
            }
        }

        /// <summary>
        /// Invoke for <see cref="INotifyDataErrorInfo"/>
        /// </summary>
        /// <param name="propertyName">Name of the Property which changed</param>
        protected virtual void OnPropertyErrorsChanged([CallerMemberName] string propertyName = null)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged("HasErrors");
        }

        #endregion INotifyDataErrorInfo

        #region INotifyPropertyChanged

        /// <summary>
        /// Invoke for <see cref="INotifyPropertyChanged"/>
        /// </summary>
        /// <param name="propertyName">Name of the Property which changed</param>
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName != "HasErrors")
            {
                this.ValidateAsync(this.RaiseEventsForAllPropertiesAtOnce ? null : propertyName);
            }
        }

        #endregion INotifyPropertyChanged

        #region Validation

        /// <summary>
        /// Lock Objekt für <see cref="Validate"/>.
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// Gibt an, ob alle Properties gleichzeitig angezeigt werden sollen.
        /// </summary>
        protected bool RaiseEventsForAllPropertiesAtOnce { get; set; } = true;

        /// <summary>
        /// Startet die Validierung des Models via <see cref="System.ComponentModel.DataAnnotations"/>.
        /// </summary>
        public void Validate(string propertyName = null)
        {
            lock (_lock)
            {
                ValidationContext validationContext = new ValidationContext(this, null, null);
                List<ValidationResult> validationResults = new List<ValidationResult>();
                Validator.TryValidateObject(this, validationContext, validationResults, true);

                foreach (var kv in this.PropErrors.ToList())
                {
                    if (propertyName != null && kv.Key != propertyName)
                    { // Temp for Raising only Events for propertyName if set
                        continue;
                    }
                    if (validationResults.All(r => r.MemberNames.All(m => m != kv.Key)))
                    {
                        this.PropErrors.TryRemove(kv.Key, out List<string> outLi);
                        OnPropertyErrorsChanged(kv.Key);
                    }
                }

                var q = from r in validationResults
                        from m in r.MemberNames
                        group r by m into g
                        select g;

                foreach (var prop in q)
                {
                    if (propertyName != null && prop.Key != propertyName)
                    { // Temp for Raising only Events for propertyName if set
                        continue;
                    }
                    var messages = prop.Select(r => r.ErrorMessage).ToList();

                    if (this.PropErrors.ContainsKey(prop.Key))
                    {
                        this.PropErrors.TryRemove(prop.Key, out List<string> outLi);
                    }
                    this.PropErrors.TryAdd(prop.Key, messages);
                    OnPropertyErrorsChanged(prop.Key);
                }
            }
        }

        /// <summary>
        /// Startet die asynchrone <see cref="Validate">Validierung</see> des Models.
        /// </summary>
        /// <returns>Asynchroner <see cref="Task"/> der Validierung.</returns>
        public Task ValidateAsync(string propertyName = null)
        {
            return Task.Run(() => Validate(propertyName));
        }

        #endregion Validation
    }
}