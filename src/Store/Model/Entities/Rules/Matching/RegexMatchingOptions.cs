using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.DB.Entities
{
    public record RegexMatchingOptions(string Expression, RegexOptions[] Options);
}
