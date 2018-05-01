using System.Collections.Generic;
using Core;

namespace EyeDroppr.ViewModels.Designer
{
    public class DesignPhotoMetaDataViewModel : PhotoMetaDataViewModel
    {
        public DesignPhotoMetaDataViewModel() : base(new Dictionary<SystemProperty, string>
        {
            { SystemProperty.CameraManufacturer, "Canon" },
            { SystemProperty.CameraModel, "Canon Powershot G12"},
            { SystemProperty.ExposureTime, "1/250 sec" },
            { SystemProperty.ISOSpeed, "SIO-80" },
            { SystemProperty.FStop, "4" }
        })
        {
            
        }
    }
}