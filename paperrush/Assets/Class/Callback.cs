﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Class
{
    public delegate void Callback();
    public delegate void Callback<T>(T arg1);
    public delegate void Callback<T, U>(T arg1, U arg2);
    public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);
}
