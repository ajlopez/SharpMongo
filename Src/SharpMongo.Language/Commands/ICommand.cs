﻿namespace SharpMongo.Language.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface ICommand
    {
        object Execute(Context context);
    }
}
