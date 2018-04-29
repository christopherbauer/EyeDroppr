using System;
using System.Collections.Generic;
using Core;

namespace EyeDroppr.ViewModels
{
    public class PhotoMetaDataViewModel : ViewModelBase
    {
        private string _make;
        private string _model;
        private string _exposureTime;
        private string _isoSpeed;
        private string _fStop;
        private string _exposureBias;
        private string _aperture;
        public PhotoMetaDataViewModel(IDictionary<SystemProperty, string> metaData)
        {

            Make = DisplayProperty<string>(metaData, SystemProperty.CameraManufacturer);
            Model = DisplayProperty<string>(metaData, SystemProperty.CameraModel);
            ExposureTime = $"1/{1 / DisplayProperty<double>(metaData, SystemProperty.ExposureTime)}";
            ISOSpeed = DisplayProperty<string>(metaData, SystemProperty.ISOSpeed);
            FStop = $"f/{DisplayProperty<string>(metaData, SystemProperty.FStop)}";
            ExposureBias = DisplayProperty<string>(metaData, SystemProperty.ExposureBias);
            Aperture = DisplayProperty<string>(metaData, SystemProperty.Aperture);
        }
        private static T DisplayProperty<T>(IDictionary<SystemProperty, string> metaData, SystemProperty systemProperty)
        {
            if (metaData.ContainsKey(systemProperty))
            {
                var s = metaData[systemProperty];
                return (T)Convert.ChangeType(s, typeof(T));
            }
            return default(T);
        }
        public string HasAperture => string.IsNullOrEmpty(Aperture) ? "Collapsed" : "Visible";
        public string Make
        {
            get { return _make; }
            set { SetValue(ref _make, value); }
        }

        public string Model
        {
            get { return _model; }
            set { SetValue(ref _model, value); }
        }

        public string ExposureTime
        {
            get { return _exposureTime; }
            set { SetValue(ref _exposureTime, value); }
        }

        public string ISOSpeed
        {
            get { return _isoSpeed; }
            set { SetValue(ref _isoSpeed, value); }
        }

        public string FStop
        {
            get { return _fStop; }
            set { SetValue(ref _fStop, value); }
        }
        public string ExposureBias
        {
            get { return _exposureBias; }
            set { SetValue(ref _exposureBias, value); }
        }
        public string Aperture
        {
            get { return _aperture; }
            set
            {
                SetValue(ref _aperture, value);
                OnPropertyChanged(nameof(HasAperture));
            }
        }

    }
}