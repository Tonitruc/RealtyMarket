using Syncfusion.Maui.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyMarket.Controls
{
    public class GrSfSwitch : SfSwitch
    {
        public GrSfSwitch()
        {
            SwitchSettings onStyle = new SwitchSettings();
            onStyle.TrackBackground = new SolidColorBrush(Color.FromRgba("#e0aaff"));
            onStyle.ThumbBackground = new SolidColorBrush(Color.FromRgba("#8900f2"));
            onStyle.TrackStroke = Color.FromRgba("#8900f2");
            onStyle.ThumbStroke = Color.FromRgba("#8900f2");

            SwitchSettings offStyle = new SwitchSettings();
            offStyle.TrackBackground = new SolidColorBrush(Color.FromRgba("#E1E1E1"));
            offStyle.ThumbBackground = new SolidColorBrush(Color.FromRgba("#ACACAC"));
            offStyle.TrackStroke = Color.FromRgba("#ACACAC");
            offStyle.ThumbStroke = Color.FromRgba("#ACACAC");

            VisualStateGroupList visualStateGroupList = new VisualStateGroupList();
            VisualStateGroup commonStateGroup = new VisualStateGroup();

            VisualState onState = new VisualState { Name = "On" };
            onState.Setters.Add(new Setter { Property = SfSwitch.SwitchSettingsProperty, Value = onStyle });

            VisualState offState = new VisualState { Name = "Off" };
            offState.Setters.Add(new Setter { Property = SfSwitch.SwitchSettingsProperty, Value = offStyle });

            commonStateGroup.States.Add(onState);
            commonStateGroup.States.Add(offState);

            visualStateGroupList.Add(commonStateGroup);
            VisualStateManager.SetVisualStateGroups(this, visualStateGroupList);
        }
    }
}
