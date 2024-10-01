using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace RealtyMarket
{
    public class EffectButton : Button
    {
        private Color _startColor;

        public EffectButton()
        {
            Pressed += OnButtonPressed;
            Released += OnButtonReleased;
        }

        private async void OnButtonPressed(object sender, EventArgs e)
        {
            _startColor = BackgroundColor;

            await ColorTo(Colors.White, Colors.Gray, c => this.BackgroundColor = c, 250);
        }

        private async void OnButtonReleased(object sender, EventArgs e)
        {
            await ColorTo(Colors.Gray, Colors.White, c => this.BackgroundColor = c, 250);
        }

        private Task<bool> ColorTo(Color fromColor, Color toColor, Action<Color> callback, uint length = 250, Easing easing = null)
        {
            Func<double, Color> transform = t =>
                Color.FromRgba(
                    fromColor.Red + t * (toColor.Red - fromColor.Red),
                    fromColor.Green + t * (toColor.Green - fromColor.Green),
                    fromColor.Blue + t * (toColor.Blue - fromColor.Blue),
                    fromColor.Alpha + t * (toColor.Alpha - fromColor.Alpha));

            return ColorAnimation("ColorTo", transform, callback, length, easing);
        }

        private Task<bool> ColorAnimation(string name, Func<double, Color> transform, Action<Color> callback, uint length, Easing easing)
        {
            easing ??= Easing.Linear;  
            var taskCompletionSource = new TaskCompletionSource<bool>();

            this.Animate(name, transform, callback: callback, length: length, easing: easing, finished: (v, c) => taskCompletionSource.SetResult(c));

            return taskCompletionSource.Task;
        }
    }
}
