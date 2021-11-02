﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.DB.Entities
{
    public record ContentData(
        string Content,
        DateTimeOffset Time,
        string Template,
        string[] Parameters);
}
