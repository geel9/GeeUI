using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeeUI.Managers;
namespace GeeUI.Structs
{
    public class CodeBoundMouse
    {
        public Action lambda;
        public MouseButton boundMouseButton;
        public bool press;
        public bool constant;


        public CodeBoundMouse(Action a, MouseButton button, bool pressing = true, bool constant = false)
        {
            lambda = a;
            boundMouseButton = button;
            press = pressing;
            this.constant = (pressing) ? constant : false;
        }
    }
}
