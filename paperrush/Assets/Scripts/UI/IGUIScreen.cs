using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Assets.Class
{
    public interface IGUIScreen
    {
        bool IsVisible { get; }
        void ShowScreen();
        void HideScreen();
    }
}
