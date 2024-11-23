using PresentationLayer.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;
using System.Linq.Expressions;
using System.Reflection;

namespace PresentationLayer.ApplicationTheme
{
    public static class AppAnimations
    {
        public static void AnimateBrushColor<T>(T target, Expression<Func<T, object>> brush, Color from, Color to, int duration = 500)
        {
            var memberSelectorExpression = brush.Body as MemberExpression;
            if (memberSelectorExpression != null)
            {
                var property = memberSelectorExpression.Member as PropertyInfo;

                if (property != null)
                {
                    SolidColorBrush solidColorBrush = new SolidColorBrush(from);
                    property.SetValue(target, solidColorBrush); 
                    AnimationTimeline animation =
                        new ColorAnimation(
                            to,
                            new Duration(TimeSpan.FromMilliseconds(duration))
                        );
                    (property.GetValue(target) as Brush).BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
            }
        }
    }
}
