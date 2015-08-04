using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JQWidgetsSugar
{
    public class ActionResultModel<T>
    {
        public bool isSuccess { get; set; }
        public T respnseInfo { get; set; }
    }
}
